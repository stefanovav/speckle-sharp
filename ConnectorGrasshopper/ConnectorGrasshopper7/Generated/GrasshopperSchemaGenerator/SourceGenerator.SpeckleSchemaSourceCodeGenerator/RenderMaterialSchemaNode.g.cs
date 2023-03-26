﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class RenderMaterial_58aecd4b_3fe2_470e_dffc_ddfcbcadb5e3: CreateSchemaObjectBase {
        
        static RenderMaterial_58aecd4b_3fe2_470e_dffc_ddfcbcadb5e3() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RenderMaterial_58aecd4b_3fe2_470e_dffc_ddfcbcadb5e3(): base("RenderMaterial", "RenderMaterial", "Creates a render material.", "BIM", "Other"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("58aecd4b-3fe2-470e-dffc-ddfcbcadb5e3");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Other.RenderMaterial.ctor(System.Double,System.Double,System.Double,System.Nullable`1[System.Drawing.Color],System.Nullable`1[System.Drawing.Color])", "Objects.Other.RenderMaterial");
          base.AddedToDocument(document);
        }
    }
    
    public class RenderMaterial_58aecd4b_3fe2_470e_dffc_ddfcbcadb5e3UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("a5b516dc-2047-07a8-cb86-e6df24046866");
        public Guid UpgradeTo => new("58aecd4b-3fe2-470e-dffc-ddfcbcadb5e3");
    }

}
