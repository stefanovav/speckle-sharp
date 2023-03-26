﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class Column_35804900_7357_f7a4_7b59_9172c4d49963: CreateSchemaObjectBase {
        
        static Column_35804900_7357_f7a4_7b59_9172c4d49963() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public Column_35804900_7357_f7a4_7b59_9172c4d49963(): base("Column", "Column", "Creates a Speckle column", "BIM", "Structure"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("35804900-7357-f7a4-7b59-9172c4d49963");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Column.ctor(Objects.ICurve)", "Objects.BuiltElements.Column");
          base.AddedToDocument(document);
        }
    }
    
    public class Column_35804900_7357_f7a4_7b59_9172c4d49963UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("c2d0cac4-d1c2-6a4d-e96e-ad99f6077012");
        public Guid UpgradeTo => new("35804900-7357-f7a4-7b59-9172c4d49963");
    }

}
