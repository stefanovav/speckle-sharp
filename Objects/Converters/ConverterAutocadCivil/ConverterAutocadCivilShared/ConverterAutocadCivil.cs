using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Objects.BuiltElements;
using Objects.BuiltElements.Revit.Curve;
using Objects.Geometry;
using Objects.Other;
using Speckle.Core.Kits;
using Speckle.Core.Models;
using Acad = Autodesk.AutoCAD;
using AcadDB = Autodesk.AutoCAD.DatabaseServices;
using Plane = Autodesk.AutoCAD.Geometry.Plane;
#if CIVIL2021 || CIVIL2022 || CIVIL2023
using CivilDB = Autodesk.Civil.DatabaseServices;
#endif

namespace Objects.Converter.AutocadCivil
{
  public class ConverterAutocadCivil : ISpeckleConverter
  {
#if AUTOCAD2021
    public static string AutocadAppName = HostApplications.AutoCAD.GetVersion(HostAppVersion.v2021);
#elif AUTOCAD2022
    public static string AutocadAppName = HostApplications.AutoCAD.GetVersion(HostAppVersion.v2022);
#elif AUTOCAD2023
    public static string AutocadAppName = HostApplications.AutoCAD.GetVersion(HostAppVersion.v2023);
#elif CIVIL2021
    public static string AutocadAppName = HostApplications.Civil.GetVersion(HostAppVersion.v2021);
#elif CIVIL2022
    public static string AutocadAppName = HostApplications.Civil.GetVersion(HostAppVersion.v2022);
#elif CIVIL2023
    public static string AutocadAppName = HostApplications.Civil.GetVersion(HostAppVersion.v2023);
#elif ADVANCESTEEL2023
    public static string AutocadAppName = HostApplications.AdvanceSteel.GetVersion(HostAppVersion.v2023);
#endif

    public ConverterAutocadCivil()
    {
      var ver = Assembly.GetAssembly(typeof(ConverterAutocadCivil)).GetName().Version;
    }

    #region ISpeckleConverter props
    public string Description => "Default Speckle Kit for AutoCAD";
    public string Name => nameof(ConverterAutocadCivil);
    public string Author => "Speckle";
    public string WebsiteOrEmail => "https://speckle.systems";
    public ProgressReport Report { get; private set; } = new();

    public IEnumerable<string> GetServicedApplications()
    {
      return new[] { AutocadAppName };
    }

    public Document Doc { get; private set; }
    public AcadDB.Transaction Trans { get; private set; } // TODO: evaluate if this should be here
    public Dictionary<string, string> Settings { get; private set; } = new();
    #endregion ISpeckleConverter props

    public ReceiveMode ReceiveMode { get; set; }

    public List<ApplicationObject> ContextObjects { get; set; } = new();

    public void SetContextObjects(List<ApplicationObject> objects)
    {
      ContextObjects = objects;
    }

    public void SetPreviousContextObjects(List<ApplicationObject> objects)
    {
      throw new NotImplementedException();
    }

    public void SetConverterSettings(object settings)
    {
      Settings = settings as Dictionary<string, string>;
    }

    public void SetContextDocument(object doc)
    {
      Doc = (Document)doc;
      Trans = Doc.TransactionManager.TopTransaction; // set the stream transaction here! make sure it is the top level transaction
    }

    private string ApplicationIdKey = "applicationId";

    public Base ConvertToSpeckle(object @object)
    {
      Base @base = null;
      ApplicationObject reportObj = null;
      DisplayStyle style = null;
      List<string> notes = new();

      switch (@object)
      {
        case AcadDB.DBObject obj:
          /*
          // check for speckle schema xdata
          string schema = GetSpeckleSchema(o.XData);
          if (schema != null)
            return ObjectToSpeckleBuiltElement(o);
          */
          var appId = obj.ObjectId.ToString(); // TODO: UPDATE THIS WITH STORED APP ID IF IT EXISTS

          //Use the Handle object to update progressReport object.
          //In an AutoCAD session, you can get the Handle of a DBObject from its ObjectId using the ObjectId.Handle or Handle property.
          reportObj = new ApplicationObject(obj.Handle.ToString(), obj.GetType().Name) { applicationId = appId };
          style = DisplayStyleToSpeckle(obj as AcadDB.Entity);

          switch (obj)
          {
            case AcadDB.DBPoint o:
              @base = PointToSpeckle(o);
              break;
            case AcadDB.Line o:
              @base = LineToSpeckle(o);
              break;
            case AcadDB.Arc o:
              @base = ArcToSpeckle(o);
              break;
            case AcadDB.Circle o:
              @base = CircleToSpeckle(o);
              break;
            case AcadDB.Ellipse o:
              @base = EllipseToSpeckle(o);
              break;
            case AcadDB.Hatch o:
              @base = HatchToSpeckle(o);
              break;
            case AcadDB.Spline o:
              @base = SplineToSpeckle(o);
              break;
            case AcadDB.Polyline o:
              if (o.IsOnlyLines) // db polylines can have arc segments, decide between polycurve or polyline conversion
                @base = PolylineToSpeckle(o);
              else
                @base = PolycurveToSpeckle(o);
              break;
            case AcadDB.Polyline3d o:
              @base = PolylineToSpeckle(o);
              break;
            case AcadDB.Polyline2d o:
              @base = PolycurveToSpeckle(o);
              break;
            case AcadDB.Region o:
              @base = RegionToSpeckle(o, out notes);
              break;
            case AcadDB.Surface o:
              @base = SurfaceToSpeckle(o, out notes);
              break;
            case AcadDB.PolyFaceMesh o:
              @base = MeshToSpeckle(o);
              break;
            case AcadDB.ProxyEntity o:
              @base = ProxyEntityToSpeckle(o);
              break;
            case AcadDB.SubDMesh o:
              @base = MeshToSpeckle(o);
              break;
            case AcadDB.Solid3d o:
              if (o.IsNull)
                notes.Add("Solid was null");
              else
                @base = SolidToSpeckle(o, out notes);
              break;
            case AcadDB.Dimension o:
              @base = DimensionToSpeckle(o);
              break;
            case AcadDB.BlockReference o:
              @base = BlockReferenceToSpeckle(o);
              break;
            case AcadDB.BlockTableRecord o:
              @base = BlockRecordToSpeckle(o);
              break;
            case AcadDB.DBText o:
              @base = TextToSpeckle(o);
              break;
            case AcadDB.MText o:
              @base = TextToSpeckle(o);
              break;
#if CIVIL2021 || CIVIL2022 || CIVIL2023
            case CivilDB.Alignment o:
              @base = AlignmentToSpeckle(o);
              break;
            case CivilDB.Corridor o:
              @base = CorridorToSpeckle(o);
              break;
            case CivilDB.FeatureLine o:
              @base = FeatureLineToSpeckle(o);
              break;
            case CivilDB.Structure o:
              @base = StructureToSpeckle(o);
              break;
            case CivilDB.Pipe o:
              @base = PipeToSpeckle(o);
              break;
            case CivilDB.PressurePipe o:
              @base = PipeToSpeckle(o);
              break;
            case CivilDB.Profile o:
              @base = ProfileToSpeckle(o);
              break;
            case CivilDB.TinSurface o:
              @base = SurfaceToSpeckle(o);
              break;

#elif ADVANCESTEEL2023

            default:
              try
              {
                @base = ConvertASToSpeckle(obj, reportObj, notes);
              }
              catch (Exception ex)
              {
                //Update report because AS object type
                Report.UpdateReportObject(reportObj);
                throw ex;
              }

              break;
#endif
          }
          break;
        case Point3d o:
          @base = PointToSpeckle(o);
          break;
        case Vector3d o:
          @base = VectorToSpeckle(o);
          break;
        case Line3d o:
          @base = LineToSpeckle(o);
          break;
        case LineSegment3d o:
          @base = LineToSpeckle(o);
          break;
        case CircularArc3d o:
          @base = ArcToSpeckle(o);
          break;
        case Plane o:
          @base = PlaneToSpeckle(o);
          break;
        case Curve3d o:
          @base = CurveToSpeckle(o) as Base;
          break;
        default:
          if (reportObj != null)
          {
            reportObj.Update(
              status: ApplicationObject.State.Skipped,
              logItem: $"{@object.GetType()} type not supported"
            );
            Report.UpdateReportObject(reportObj);
          }
          return @base;
      }

      if (@base is null)
        return @base;

      if (style != null)
        @base["displayStyle"] = style;

      if (reportObj != null)
      {
        reportObj.Update(log: notes);
        Report.UpdateReportObject(reportObj);
      }
      return @base;
    }

    private Base ObjectToSpeckleBuiltElement(AcadDB.DBObject o)
    {
      throw new NotImplementedException();
    }

    public List<Base> ConvertToSpeckle(List<object> objects)
    {
      return objects.Select(x => ConvertToSpeckle(x)).ToList<>();
    }

    public object ConvertToNative(Base @object)
    {
      // determine if this object has autocad props
      bool isFromAutoCAD = @object[AutocadPropName] != null ? true : false;
      bool isFromCivil = @object[CivilPropName] != null ? true : false;
      object acadObj = null;
      var reportObj = Report.ReportObjects.ContainsKey(@object.id)
        ? new ApplicationObject(@object.id, @object.speckle_type)
        : null;
      List<string> notes = new();
      switch (@object)
      {
        case Point o:
          acadObj = PointToNativeDB(o);
          break;

        case Line o:
          acadObj = LineToNativeDB(o);
          break;

        case Arc o:
          acadObj = ArcToNativeDB(o);
          break;

        case Circle o:
          acadObj = CircleToNativeDB(o);
          break;

        case Ellipse o:
          acadObj = EllipseToNativeDB(o);
          break;

        case Spiral o:
          acadObj = PolylineToNativeDB(o.displayValue);
          break;

        case Hatch o:
          acadObj = HatchToNativeDB(o);
          break;

        case Polyline o:
          acadObj = PolylineToNativeDB(o);
          break;

        case Polycurve o:
          bool convertAsSpline = o.segments.Where(s => !(s is Line) && !(s is Arc)).Count() > 0 ? true : false;
          if (convertAsSpline || !IsPolycurvePlanar(o))
            acadObj = PolycurveSplineToNativeDB(o);
          else
            acadObj = PolycurveToNativeDB(o);
          break;

        case Curve o:
          acadObj = CurveToNativeDB(o);
          break;

        /*
        case Surface o:
          return SurfaceToNative(o);

        */

        case Mesh o:
#if CIVIL2021 || CIVIL2022 || CIVIL2023
          acadObj = isFromCivil ? CivilSurfaceToNative(o) : MeshToNativeDB(o);
#else
          acadObj = MeshToNativeDB(o);
#endif
          break;

        case Dimension o:
          acadObj = isFromAutoCAD ? AcadDimensionToNative(o) : DimensionToNative(o);
          break;

        case Instance o:
          acadObj = InstanceToNativeDB(o);
          break;

        case BlockDefinition o:
          acadObj = DefinitionToNativeDB(o, out notes);
          break;

        case Text o:
          acadObj = isFromAutoCAD ? AcadTextToNative(o) : TextToNative(o);
          break;

#if CIVIL2021 || CIVIL2022 || CIVIL2023
        case Alignment o:
          acadObj = AlignmentToNative(o);
          break;
#endif

        case ModelCurve o:
          acadObj = CurveToNativeDB(o.baseCurve);
          break;

        default:
          if (reportObj != null)
          {
            reportObj.Update(
              status: ApplicationObject.State.Skipped,
              logItem: $"{@object.GetType()} type not supported"
            );
            Report.UpdateReportObject(reportObj);
          }
          throw new NotSupportedException();
      }

      switch (acadObj)
      {
        case ApplicationObject o: // some to native methods return an application object (if object is baked to doc during conv)
          acadObj = o.Converted.Any() ? o.Converted : null;
          if (reportObj != null)
            reportObj.Update(
              status: o.Status,
              createdIds: o.CreatedIds,
              converted: o.Converted,
              container: o.Container,
              log: o.Log
            );
          break;
        default:
          if (reportObj != null)
            reportObj.Update(log: notes);
          break;
      }
      if (reportObj != null)
        Report.UpdateReportObject(reportObj);
      return acadObj;
    }

    public List<object> ConvertToNative(List<Base> objects)
    {
      return objects.Select(x => ConvertToNative(x)).ToList<>();
    }

    public bool CanConvertToSpeckle(object @object)
    {
      switch (@object)
      {
        case AcadDB.DBObject o:
          switch (o)
          {
            case AcadDB.DBPoint _:
            case AcadDB.Line _:
            case AcadDB.Arc _:
            case AcadDB.Circle _:
            case AcadDB.Dimension _:
            case AcadDB.Ellipse _:
            case AcadDB.Hatch _:
            case AcadDB.Spline _:
            case AcadDB.Polyline _:
            case AcadDB.Polyline2d _:
            case AcadDB.Polyline3d _:
            case AcadDB.Surface _:
            case AcadDB.PolyFaceMesh _:
            case AcadDB.ProxyEntity _:
            case AcadDB.Region _:
            case AcadDB.SubDMesh _:
            case AcadDB.Solid3d _:
              return true;

            case AcadDB.BlockReference _:
            case AcadDB.BlockTableRecord _:
            case AcadDB.DBText _:
            case AcadDB.MText _:
              return true;

#if CIVIL2021 || CIVIL2022 || CIVIL2023
            // NOTE: C3D pressure pipes and pressure fittings API under development
            case CivilDB.FeatureLine _:
            case CivilDB.Corridor _:
            case CivilDB.Structure _:
            case CivilDB.Alignment _:
            case CivilDB.Pipe _:
            case CivilDB.PressurePipe _:
            case CivilDB.Profile _:
            case CivilDB.TinSurface _:
              return true;
#endif

            default:
            {
#if ADVANCESTEEL2023
                return CanConvertASToSpeckle(o);
#else
              return false;
#endif
            }
          }

        case Point3d _:
        case Vector3d _:
        case Plane _:
        case Line3d _:
        case LineSegment3d _:
        case CircularArc3d _:
        case Curve3d _:
          return true;

        default:
          return false;
      }
    }

    public bool CanConvertToNative(Base @object)
    {
      switch (@object)
      {
        case Point _:
        case Line _:
        case Arc _:
        case Circle _:
        case Ellipse _:
        case Spiral _:
        case Hatch _:
        case Polyline _:
        case Polycurve _:
        case Curve _:
        //case Brep _:
        case Mesh _:

        case Dimension _:
        case BlockDefinition _:
        case Instance _:
        case Text _:

        case Alignment _:
        case ModelCurve _:
          return true;

        default:
          return false;
      }
    }
  }
}
