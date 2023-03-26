﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class Ceiling_66c3682d_d168_00ea_bfaf_64b4cc5a3eb1: CreateSchemaObjectBase {
        
        static Ceiling_66c3682d_d168_00ea_bfaf_64b4cc5a3eb1() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public Ceiling_66c3682d_d168_00ea_bfaf_64b4cc5a3eb1(): base("Ceiling", "Ceiling", "Creates a Speckle ceiling", "BIM", "Architecture"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("66c3682d-d168-00ea-bfaf-64b4cc5a3eb1");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Ceiling.ctor(Objects.ICurve,System.Collections.Generic.List`1[Objects.ICurve],System.Collections.Generic.List`1[Speckle.Core.Models.Base])", "Objects.BuiltElements.Ceiling");
          base.AddedToDocument(document);
        }
    }
    
    public class Ceiling_66c3682d_d168_00ea_bfaf_64b4cc5a3eb1UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("cbadd02e-fe35-11d8-dabe-5c5436ed0828");
        public Guid UpgradeTo => new("66c3682d-d168-00ea-bfaf-64b4cc5a3eb1");
    }

}
