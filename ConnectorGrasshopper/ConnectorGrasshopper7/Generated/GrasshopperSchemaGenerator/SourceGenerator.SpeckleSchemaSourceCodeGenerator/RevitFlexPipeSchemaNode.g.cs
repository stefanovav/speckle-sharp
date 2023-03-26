﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class RevitFlexPipe_84f3ac93_5bdd_2420_d656_0f8acf88bfb7: CreateSchemaObjectBase {
        
        static RevitFlexPipe_84f3ac93_5bdd_2420_d656_0f8acf88bfb7() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitFlexPipe_84f3ac93_5bdd_2420_d656_0f8acf88bfb7(): base("RevitFlexPipe", "RevitFlexPipe", "Creates a Revit flex pipe", "Revit", "MEP"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("84f3ac93-5bdd-2420-d656-0f8acf88bfb7");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitFlexPipe.ctor(System.String,System.String,Objects.ICurve,System.Double,Objects.BuiltElements.Level,Objects.Geometry.Vector,Objects.Geometry.Vector,System.String,System.String,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.RevitFlexPipe");
          base.AddedToDocument(document);
        }
    }
    
    public class RevitFlexPipe_84f3ac93_5bdd_2420_d656_0f8acf88bfb7UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("679b6bcf-5a70-cd8c-75c5-0e3d79e62e62");
        public Guid UpgradeTo => new("84f3ac93-5bdd-2420-d656-0f8acf88bfb7");
    }

}
