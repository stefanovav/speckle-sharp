﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class RevitRailing_69ba2ccc_ba1e_2d25_6f21_ce49b1a2b034: CreateSchemaObjectBase {
        
        static RevitRailing_69ba2ccc_ba1e_2d25_6f21_ce49b1a2b034() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitRailing_69ba2ccc_ba1e_2d25_6f21_ce49b1a2b034(): base("Railing", "Railing", "Creates a Revit railing by base curve.", "Revit", "Architecture"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("69ba2ccc-ba1e-2d25-6f21-ce49b1a2b034");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitRailing.ctor(System.String,Objects.Geometry.Polycurve,Objects.BuiltElements.Level,System.Boolean)", "Objects.BuiltElements.Revit.RevitRailing");
          base.AddedToDocument(document);
        }
    }
    
    public class RevitRailing_69ba2ccc_ba1e_2d25_6f21_ce49b1a2b034UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("768f4cec-24fd-8f33-8893-6434985c8770");
        public Guid UpgradeTo => new("69ba2ccc-ba1e-2d25-6f21-ce49b1a2b034");
    }

}
