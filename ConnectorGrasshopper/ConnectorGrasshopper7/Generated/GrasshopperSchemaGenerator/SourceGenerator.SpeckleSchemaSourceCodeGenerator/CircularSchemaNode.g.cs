﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class Circular_6a6301b0_a723_665c_5383_f45fd3e2ebb0: CreateSchemaObjectBase {
        
        static Circular_6a6301b0_a723_665c_5383_f45fd3e2ebb0() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public Circular_6a6301b0_a723_665c_5383_f45fd3e2ebb0(): base("Circular", "Circular", "Creates a Speckle structural circular section profile", "Structural", "Section Profile"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("6a6301b0-a723-665c-5383-f45fd3e2ebb0");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Properties.Profiles.Circular.ctor(System.String,System.Double,System.Double)", "Objects.Structural.Properties.Profiles.Circular");
          base.AddedToDocument(document);
        }
    }
    
    public class Circular_6a6301b0_a723_665c_5383_f45fd3e2ebb0UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("75bce7d6-0e5e-0a3d-40f2-b84f63e91965");
        public Guid UpgradeTo => new("6a6301b0-a723-665c-5383-f45fd3e2ebb0");
    }

}
