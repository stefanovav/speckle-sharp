﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class RevitFaceWall_3d8854f3_c238_2097_3c42_fc2433a3072d: CreateSchemaObjectBase {
        
        static RevitFaceWall_3d8854f3_c238_2097_3c42_fc2433a3072d() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitFaceWall_3d8854f3_c238_2097_3c42_fc2433a3072d(): base("RevitWall by face", "RevitWall by face", "Creates a Revit wall from a surface.", "Revit", "Architecture"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("3d8854f3-c238-2097-3c42-fc2433a3072d");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitFaceWall.ctor(System.String,System.String,Objects.Geometry.Brep,Objects.BuiltElements.Level,Objects.BuiltElements.Revit.LocationLine,System.Collections.Generic.List`1[Speckle.Core.Models.Base],System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.RevitFaceWall");
          base.AddedToDocument(document);
        }
    }
    
    public class RevitFaceWall_3d8854f3_c238_2097_3c42_fc2433a3072dUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("e8d7f0c1-d76b-7197-efa2-f394602952b1");
        public Guid UpgradeTo => new("3d8854f3-c238-2097-3c42-fc2433a3072d");
    }

}
