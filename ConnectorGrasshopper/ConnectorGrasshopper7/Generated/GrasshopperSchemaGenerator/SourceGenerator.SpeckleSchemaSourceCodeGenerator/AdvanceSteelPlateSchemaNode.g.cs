﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class AdvanceSteelPlate_ed612d45_5289_6f9c_a10b_bb2227597c02: CreateSchemaObjectBase {
        
        static AdvanceSteelPlate_ed612d45_5289_6f9c_a10b_bb2227597c02() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public AdvanceSteelPlate_ed612d45_5289_6f9c_a10b_bb2227597c02(): base("AdvanceSteelPlate", "AdvanceSteelPlate", "Creates a Advance Steel plate.", "Advance Steel", "Structure"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("ed612d45-5289-6f9c-a10b-bb2227597c02");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.AdvanceSteel.AdvanceSteelPlate.ctor(Objects.Structural.Properties.Profiles.SectionProfile,Objects.Geometry.Polyline,System.String,Objects.Structural.Materials.StructuralMaterial)", "Objects.BuiltElements.AdvanceSteel.AdvanceSteelPlate");
          base.AddedToDocument(document);
        }
    }
    
    public class AdvanceSteelPlate_ed612d45_5289_6f9c_a10b_bb2227597c02UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("d75c06fe-697a-4d8d-a801-5ad78a7a95c7");
        public Guid UpgradeTo => new("ed612d45-5289-6f9c-a10b-bb2227597c02");
    }

}
