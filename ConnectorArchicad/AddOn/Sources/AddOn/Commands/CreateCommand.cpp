#include "ObjectState.hpp"
#include "Utility.hpp"
#include "CreateCommand.hpp"
#include "LibpartImportManager.hpp"
#include "ClassificationImportManager.hpp"
#include "APIHelper.hpp"
#include "FieldNames.hpp"
#include "OnExit.hpp"
#include "ExchangeManager.hpp"
#include "Database.hpp"
#include "Objects/Level.hpp"


using namespace FieldNames;

namespace AddOnCommands {


GSErrCode CreateCommand::CreateNewElement (API_Element& element,
	API_ElementMemo& elementMemo,
	API_SubElement* marker /*= nullptr*/) const
{
	if (marker != nullptr)
		return ACAPI_Element_CreateExt (&element, &elementMemo, 1UL, marker);

	return ACAPI_Element_Create (&element, &elementMemo);
}


GSErrCode CreateCommand::ModifyExistingElement (API_Element& element,
	API_Element& elementMask,
	API_ElementMemo& memo,
	GS::UInt64 memoMask) const
{
	return ACAPI_Element_Change (&element, &elementMask, &memo, memoMask, true);
}


void CreateCommand::GetStoryFromObjectState (const GS::ObjectState& os, const double& elementLevel, short& floorIndex, double& relativeLevel) const
{
	Objects::Level level;
	os.Get (ElementBase::Level, level);
	Utility::SetStoryLevelAndFloor (elementLevel, level.floorIndex, relativeLevel);
	floorIndex = level.floorIndex;
}


GS::ObjectState CreateCommand::Execute (const GS::ObjectState& parameters, GS::ProcessControl& /*processControl*/) const
{
	GS::ObjectState result;

	GS::Array<GS::ObjectState> objectStates;
	parameters.Get (GetFieldName (), objectStates);

	ACAPI_CallUndoableCommand (GetUndoableCommandName (), [&] () -> GSErrCode {
		LibraryHelper helper (false);

		GS::Array<GS::ObjectState> applicationObjects;

		AttributeManager* attributeManager = AttributeManager::GetInstance ();
		LibpartImportManager* libpartImportManager = LibpartImportManager::GetInstance ();

		Utility::Database db;
		db.SwitchToFloorPlan ();

		for (const GS::ObjectState& objectState : objectStates) {
			API_Element element{};
			API_Element elementMask{};
			API_ElementMemo memo{};
			GS::UInt64 memoMask = 0;
			API_SubElement* marker = nullptr;
			GS::OnExit memoDisposer ([&memo, &marker] {
				ACAPI_DisposeElemMemoHdls (&memo);

				if (marker != nullptr)
					ACAPI_DisposeElemMemoHdls (&marker->memo);
			});

			GSErrCode err = NoError;

			GS::String speckleId;
			{
				objectState.Get (ElementBase::Id, speckleId);

				if (speckleId.IsEmpty ())
					err = Error;
			}

			bool elementExists = false;
			GS::Array<GS::UniString> log;

			if (err == NoError) {
				bool isConverted = false;
				API_Guid convertedArchicadId;
				ExchangeManager::GetInstance ().GetState (speckleId, isConverted, convertedArchicadId);

				elementExists = isConverted && Utility::ElementExists (convertedArchicadId);

				{
					// if already converted and element exists, use that
					if (elementExists) {
						element.header.guid = convertedArchicadId;
					}
					// otherwise try to use applicationId
					else {
						GS::UniString applicationId;
						objectState.Get (ElementBase::ApplicationId, applicationId);
						element.header.guid = APIGuidFromString (applicationId.ToCStr ());
					}
				}

				err = GetElementFromObjectState (objectState, element, elementMask, memo, memoMask, &marker, *attributeManager, *libpartImportManager, log);
				if (err == NoError) {
					if (elementExists) {
						err = ModifyExistingElement (element, elementMask, memo, memoMask);
					} else {
						err = CreateNewElement (element, memo, marker);
					}
				}

				if (err == NoError)
					err = ImportClassificationsAndProperties (objectState, element.header.guid);
			}

			GS::ObjectState applicationObject;
			applicationObject.Add (ApplicationObject::OriginalId, speckleId);

			if (err == NoError) {
				GS::UniString applicationId = APIGuidToString (element.header.guid);
				applicationObject.Add (ElementBase::ApplicationId, applicationId);
				GS::Array<GS::UniString> createdIds;
				createdIds.Push (applicationId);
				applicationObject.Add (ApplicationObject::CreatedIds, createdIds);

				if (elementExists)
					applicationObject.Add (ApplicationObject::Status, ApplicationObject::StateUpdated);
				else
					applicationObject.Add (ApplicationObject::Status, ApplicationObject::StateCreated);

				ExchangeManager::GetInstance ().UpdateState (speckleId, element.header.guid);
			} else {
				applicationObject.Add (ApplicationObject::Status, ApplicationObject::StateFailed);
				applicationObject.Add (ApplicationObject::Log, log);
			}

			applicationObjects.Push (applicationObject);
		}

		result.Add (ApplicationObject::ApplicationObjects, applicationObjects);

		return NoError;
	});

	return result;
}


GS::ErrCode CreateCommand::ImportClassificationsAndProperties (const GS::ObjectState& os, API_Guid& elemGuid) const
{
	GSErrCode err = NoError;
	GS::Array<GS::ObjectState> classifications;
	os.Get (FieldNames::ElementBase::Classifications, classifications);

	// store guids of the Archicad classification items in a set
	GS::HashSet<API_Guid> classificationItemsInArchicad;
	{
		GS::Array<GS::Pair<API_Guid, API_Guid>> systemItemPairs;
		err = ACAPI_Element_GetClassificationItems (elemGuid, systemItemPairs);
		if (err != NoError)
			return err;

		for (auto& systemItemPair : systemItemPairs) {
			classificationItemsInArchicad.Add (systemItemPair.second);
		}
	}

	// store guids of the Speckle classification items in a set
	GS::HashSet<API_Guid> classificationItemsInSpekcle;
	{
		GS::UniString code;
		GS::UniString system;
		GS::UniString name;
		for (auto& classification : classifications) {
			API_Guid itemGuid;
			classification.Get (FieldNames::ElementBase::Classification::System, system);
			classification.Get (FieldNames::ElementBase::Classification::Code, code);
			classification.Get (FieldNames::ElementBase::Classification::Name, name); // for later usage

			err = ClassificationImportManager::GetInstance ()->GetItem (system, code, itemGuid);
			if (err != NoError)
				return err;
			classificationItemsInSpekcle.Add (itemGuid);
		}
	}

	// in Archicad but not in Speckle
	for (auto& itemGuid : classificationItemsInArchicad.Where ([&] (const API_Guid& guid) { return !classificationItemsInSpekcle.Contains(guid); })) {
		err = ACAPI_Element_RemoveClassificationItem (elemGuid, itemGuid);
		if (err != NoError)
			return err;
	}

	// in Speckle but not in Archicad
	for (auto& itemGuid : classificationItemsInSpekcle.Where ([&] (const API_Guid& guid) { return !classificationItemsInArchicad.Contains(guid); })) {
		err = ACAPI_Element_AddClassificationItem (elemGuid, itemGuid);
		if (err != NoError)
			return err;
	}

	return err;
}


} // namespace AddOnCommands
