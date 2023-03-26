﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class ModelUnits_01dc8b20_2289_3d3c_da67_9a4cd3aa3c32: CreateSchemaObjectBase {
        
        static ModelUnits_01dc8b20_2289_3d3c_da67_9a4cd3aa3c32() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public ModelUnits_01dc8b20_2289_3d3c_da67_9a4cd3aa3c32(): base("ModelUnits", "ModelUnits", "Creates a Speckle object which specifies the units associated with the model", "Structural", "Analysis"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("01dc8b20-2289-3d3c-da67-9a4cd3aa3c32");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Analysis.ModelUnits.ctor(Objects.Structural.Analysis.UnitsType)", "Objects.Structural.Analysis.ModelUnits");
          base.AddedToDocument(document);
        }
    }

    public class ModelUnits_50092c28_30a1_0ab3_a088_9f13db624705: CreateSchemaObjectBase {
        
        static ModelUnits_50092c28_30a1_0ab3_a088_9f13db624705() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public ModelUnits_50092c28_30a1_0ab3_a088_9f13db624705(): base("ModelUnits (custom)", "ModelUnits (custom)", "Creates a Speckle object which specifies the units associated with the model", "Structural", "Analysis"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("50092c28-30a1-0ab3-a088-9f13db624705");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Analysis.ModelUnits.ctor(System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String)", "Objects.Structural.Analysis.ModelUnits");
          base.AddedToDocument(document);
        }
    }
    
    public class ModelUnits_01dc8b20_2289_3d3c_da67_9a4cd3aa3c32UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("9727bc6e-a49d-88b8-9f03-9698451e90d7");
        public Guid UpgradeTo => new("01dc8b20-2289-3d3c-da67-9a4cd3aa3c32");
    }


    public class ModelUnits_50092c28_30a1_0ab3_a088_9f13db624705UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("4f0af487-9ce0-e5b7-fcdb-f1c366bfabcb");
        public Guid UpgradeTo => new("50092c28-30a1-0ab3-a088-9f13db624705");
    }

}
