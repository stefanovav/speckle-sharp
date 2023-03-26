﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class Result3D_37e46daf_e22c_7d85_b168_fef3227c2796: CreateSchemaObjectBase {
        
        static Result3D_37e46daf_e22c_7d85_b168_fef3227c2796() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public Result3D_37e46daf_e22c_7d85_b168_fef3227c2796(): base("Result3D (load case)", "Result3D (load case)", "Creates a Speckle 3D element result object (for load case)", "Structural", "Results"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("37e46daf-e22c-7d85-b168-fef3227c2796");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Results.Result3D.ctor(Objects.Structural.Geometry.Element3D,Objects.Structural.Loading.LoadCase,System.Collections.Generic.List`1[System.Double],System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)", "Objects.Structural.Results.Result3D");
          base.AddedToDocument(document);
        }
    }

    public class Result3D_10769e1d_8e7a_2ffc_5985_7e36820a1730: CreateSchemaObjectBase {
        
        static Result3D_10769e1d_8e7a_2ffc_5985_7e36820a1730() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public Result3D_10769e1d_8e7a_2ffc_5985_7e36820a1730(): base("Result3D (load combination)", "Result3D (load combination)", "Creates a Speckle 3D element result object (for load combination)", "Structural", "Results"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("10769e1d-8e7a-2ffc-5985-7e36820a1730");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Results.Result3D.ctor(Objects.Structural.Geometry.Element3D,Objects.Structural.Loading.LoadCombination,System.Collections.Generic.List`1[System.Double],System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)", "Objects.Structural.Results.Result3D");
          base.AddedToDocument(document);
        }
    }
    
    public class Result3D_37e46daf_e22c_7d85_b168_fef3227c2796UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("41832127-d23b-5c23-c07b-0357328096f3");
        public Guid UpgradeTo => new("37e46daf-e22c-7d85-b168-fef3227c2796");
    }


    public class Result3D_10769e1d_8e7a_2ffc_5985_7e36820a1730UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("e590c244-ad93-4494-99ef-ba00be1fc775");
        public Guid UpgradeTo => new("10769e1d-8e7a-2ffc-5985-7e36820a1730");
    }

}
