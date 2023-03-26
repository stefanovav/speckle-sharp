﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class ModelInfo_3b60fc53_917c_4f28_ae98_af3fa17667e3: CreateSchemaObjectBase {
        
        static ModelInfo_3b60fc53_917c_4f28_ae98_af3fa17667e3() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public ModelInfo_3b60fc53_917c_4f28_ae98_af3fa17667e3(): base("ModelInfo", "ModelInfo", "Creates a Speckle object which describes basic model and project information for a structural model", "Structural", "Analysis"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("3b60fc53-917c-4f28-ae98-af3fa17667e3");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Analysis.ModelInfo.ctor(System.String,System.String,System.String,System.String,Objects.Structural.Analysis.ModelSettings,System.String,System.String)", "Objects.Structural.Analysis.ModelInfo");
          base.AddedToDocument(document);
        }
    }
    
    public class ModelInfo_3b60fc53_917c_4f28_ae98_af3fa17667e3UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("47c4d271-1e3b-f2e3-9baf-74698d4e7714");
        public Guid UpgradeTo => new("3b60fc53-917c-4f28-ae98-af3fa17667e3");
    }

}
