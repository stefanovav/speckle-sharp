#include "DUI3Palette.hpp"
#include "Utility.hpp"

// API
#include "APIEnvir.h"
#include "ACAPinc.h"

// DG
#include "DGModule.hpp"
#include "DGShortcutManager.hpp"
#include "DGCommandDescriptor.hpp"


GS::Ref<DUI3::Palette>	DUI3::Palette::instance;

GS::Guid DUI3::Palette::paletteGuid = GS::Guid ("{8DD2BBDF-B8B7-4E46-82ED-A7941AE30595}");


DUI3::Palette::Palette ():
	DG::Palette (ACAPI_GetOwnResModule (), 32500, ACAPI_GetOwnResModule (), paletteGuid),
	browser (GetReference (), BrowserId),
    savedShortcutScheme (nullptr)
{
	Attach (*this);

	enterHotKey = RegisterHotKey (DG::Key::Enter);
	returnHotKey = RegisterHotKey (DG::Key::Return);

	BeginEventProcessing ();

	//DUI3::RegisterCallbackJSObject (browser);
}


DUI3::Palette::~Palette ()
{
	EndEventProcessing ();

	UnregisterHotKey (enterHotKey);
	UnregisterHotKey (returnHotKey);

	Detach (*this);
}


bool DUI3::Palette::HasInstance ()
{
	return instance != nullptr;
}


void DUI3::Palette::CreateInstance ()
{
	DBASSERT (!HasInstance ());
	instance = new Palette ();
	ACAPI_KeepInMemory (true);
}


DUI3::Palette&	DUI3::Palette::GetInstance ()
{
	if (!HasInstance ())
		CreateInstance ();

	return *instance;
}

struct ShortcutDescriptor {
    DG::Shortcut* shortcut;
    DG::CommandDescriptor* cmdDesc;
    GS::Guid context;
};


GSErrCode MoveShortcuts (DG::ShortcutScheme* sourceScheme, DG::ShortcutScheme* targetScheme, std::function<bool (const DG::Shortcut*)> const & filter)
{
    if (sourceScheme == nullptr || targetScheme == nullptr)
        return Error;

    GS::Array<ShortcutDescriptor> shortcutsToBeDeleted;
    
    for (DG::ShortcutIterator iter = sourceScheme->GetShortcutIterator (nullptr, nullptr); iter != nullptr; ++iter) {
       DG::Shortcut* shortcut = iter.GetCurrentShortcut ();
       DG::CommandDescriptor* cmdDesc = iter.GetCurrentCmdDesc ();
       GS::Guid context = iter.GetCurrentContextGuid ();
       
        if (!shortcut->IsSystem () &&
            (filter == nullptr || filter (shortcut))) {
            targetScheme->CreateShortcut (shortcut->GetKey (), shortcut->GetModifierFlags(), cmdDesc->GetCommand(), context);
            
            ShortcutDescriptor shortcutDescriptor;
            shortcutDescriptor.shortcut = shortcut;
            shortcutDescriptor.cmdDesc = cmdDesc;
            shortcutDescriptor.context = context;
            shortcutsToBeDeleted.Push (shortcutDescriptor);
       }
    }

    shortcutsToBeDeleted.Enumerate([&] (ShortcutDescriptor shortcutDescriptor) {
        sourceScheme->DeleteShortcut (shortcutDescriptor.shortcut, shortcutDescriptor.cmdDesc, shortcutDescriptor.context);
    });
    
    return NoError;
}


void DUI3::Palette::Show ()
{
	DG::Palette::Show ();
	Utility::SetMenuItemCheckedState (PaletteMenuResourceId, PaletteMenuItemId, true);

    savedShortcutScheme = new DG::ShortcutScheme ();
    MoveShortcuts (DG::shortcutManager.GetShortcutScheme(), savedShortcutScheme, [] (const DG::Shortcut * shortcut) {
            if (shortcut->GetModifierKeyCount() == 0 &&
                !shortcut->GetKey ().IsSpecial ())    // does not solve Tracker special shortcuts (',' '.')
                return true;

            return false;
    } );
}


void DUI3::Palette::Hide ()
{
	DG::Palette::Hide ();
	Utility::SetMenuItemCheckedState (PaletteMenuResourceId, PaletteMenuItemId, false);
    
    MoveShortcuts (savedShortcutScheme, DG::shortcutManager.GetShortcutScheme(), nullptr);
    delete savedShortcutScheme;
    savedShortcutScheme = nullptr;
}


void DUI3::Palette::PanelOpened (const DG::PanelOpenEvent& /*ev*/)
{
	GS::UniString url ("https://localhost:8082");

	browser.LoadURL (url);
}


void DUI3::Palette::PanelCloseRequested (const DG::PanelCloseRequestEvent& /*ev*/, bool* accepted)
{
	Hide ();
	*accepted = true;
}


void DUI3::Palette::PanelResized (const DG::PanelResizeEvent& ev)
{
	short dh = ev.GetHorizontalChange ();
	short dv = ev.GetVerticalChange ();

	BeginMoveResizeItems ();

	browser.Resize (dh, dv);

	EndMoveResizeItems ();
}


void DUI3::Palette::PanelHotkeyPressed (const DG::PanelHotKeyEvent& ev, bool* processed)
{
	if (ev.GetKeyId () == enterHotKey || ev.GetKeyId () == returnHotKey) {
		*processed = true;
   }
}
