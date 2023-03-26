﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class SpaceSeparationLine_3cead981_fd85_f2f4_dd9f_0d42a8eed9df: CreateSchemaObjectBase {
        
        static SpaceSeparationLine_3cead981_fd85_f2f4_dd9f_0d42a8eed9df() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public SpaceSeparationLine_3cead981_fd85_f2f4_dd9f_0d42a8eed9df(): base("SpaceSeparationLine", "SpaceSeparationLine", "Creates a Revit space separation line", "Revit", "Curves"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("3cead981-fd85-f2f4-dd9f-0d42a8eed9df");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.Curve.SpaceSeparationLine.ctor(Objects.ICurve,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.Curve.SpaceSeparationLine");
          base.AddedToDocument(document);
        }
    }
    
    public class SpaceSeparationLine_3cead981_fd85_f2f4_dd9f_0d42a8eed9dfUpgraderObject: IGH_UpgradeObject
    {
        public IGH_DocumentObject Upgrade(IGH_DocumentObject target, GH_Document document)
        {
          var component = target as IGH_Component;
          if (component == null)
            return null;

          var upgradedComponent = GH_UpgradeUtil.SwapComponents(component, UpgradeTo);
          UpgradeUtils.SwapGroups(document, component, upgradedComponent);
          return upgradedComponent;
        }

        public DateTime Version => new DateTime(2023, 3, 1);

        public Guid UpgradeFrom => new Guid("f186d3ad-3eee-e49f-b700-2a352c10d17b");
        public Guid UpgradeTo => new("3cead981-fd85-f2f4-dd9f-0d42a8eed9df");
    }

}
