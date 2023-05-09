using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using ConverterRevitShared.Utils;
using Microsoft.Extensions.Caching.Memory;
using Objects.Geometry;

namespace ConverterRevitShared.Extensions.RevitObjects
{
  internal static class DocumentExtensions
  {
    ////private Dictionary<string, Transform> _docTransforms = new Dictionary<string, Transform>();
    public static Transform GetDocReferencePointTransform(this Document doc, Dictionary<string, string> settings, IMemoryCache cache)
    {
      //linked files are always saved to disc and will have a path name
      //if the current doc is unsaved it will not, but then it'll be the only one :)
      var id = doc.PathName;

      return cache.GetOrCreate(id, entry =>
      {
        var referencePointSetting = settings.TryGetValue("reference-point", out string value) ? value : string.Empty;
        return doc.GetReferencePointTransform(referencePointSetting, cache);
      });
    }

    ////////////////////////////////////////////////
    /// NOTE
    ////////////////////////////////////////////////
    /// The BasePoint shared properties in Revit are based off of the survey point.
    /// The BasePoint non-shared properties are based off of the internal origin.
    /// Also, survey point does NOT have an rotation parameter.
    ////////////////////////////////////////////////
    public static Transform GetReferencePointTransform(this Document doc, string type, IMemoryCache cache)
    {
      // first get the main doc base points and reference setting transform
      var referencePointTransform = Transform.Identity;
      var points = new FilteredElementCollector(doc).OfClass(typeof(BasePoint)).Cast<BasePoint>().ToList();
      var projectPoint = points.FirstOrDefault(o => o.IsShared == false);
      var surveyPoint = points.FirstOrDefault(o => o.IsShared == true);
      switch (type)
      {
        case GlobalUtils.ProjectBase: // note that the project base (ui) rotation is registered on the survey pt, not on the base point
          referencePointTransform = Transform.CreateTranslation(projectPoint.Position);
          break;
        case GlobalUtils.Survey:
          // note that the project base (ui) rotation is registered on the survey pt, not on the base point
          // retrieve the survey point rotation from the project point
          var angle = projectPoint.get_Parameter(BuiltInParameter.BASEPOINT_ANGLETON_PARAM)?.AsDouble() ?? 0;
          referencePointTransform = Transform.CreateTranslation(surveyPoint.Position).Multiply(Transform.CreateRotation(XYZ.BasisZ, angle));
          break;
        case GlobalUtils.InternalOrigin:
          break;
      }

      // Second, if this is a linked doc get the transform and adjust
      if (doc.IsLinked)
      {
        // get the linked doc instance transform
        var linkInstances = cache.GetOrCreate("linkedInstances", entry =>
        {
          return new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).ToElements().Cast<RevitLinkInstance>().ToList();
        });
        var instance = linkInstances.FirstOrDefault(x => x.GetLinkDocument().PathName == doc.PathName);
        if (instance != null)
        {
          var linkInstanceTransform = instance.GetTotalTransform();
          referencePointTransform = linkInstanceTransform.Inverse.Multiply(referencePointTransform);
        }
      }

      return referencePointTransform;
    }
  }
}
