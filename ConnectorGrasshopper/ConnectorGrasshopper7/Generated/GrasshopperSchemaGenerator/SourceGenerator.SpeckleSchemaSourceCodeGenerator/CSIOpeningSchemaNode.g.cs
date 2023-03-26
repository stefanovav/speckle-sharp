﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class CSIOpening_1630fe5d_5c57_8590_edf8_fbae372691dd: CreateSchemaObjectBase {
        
        static CSIOpening_1630fe5d_5c57_8590_edf8_fbae372691dd() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public CSIOpening_1630fe5d_5c57_8590_edf8_fbae372691dd(): base("Opening", "Opening", "Create an CSI Opening", "CSI", "Properties"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("1630fe5d-5c57-8590-edf8-fbae372691dd");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.CSI.Properties.CSIOpening.ctor(System.Boolean)", "Objects.Structural.CSI.Properties.CSIOpening");
          base.AddedToDocument(document);
        }
    }
    
    public class CSIOpening_1630fe5d_5c57_8590_edf8_fbae372691ddUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("502e8487-b321-1865-1c7c-e1b00eabb384");
        public Guid UpgradeTo => new("1630fe5d-5c57-8590-edf8-fbae372691dd");
    }

}
