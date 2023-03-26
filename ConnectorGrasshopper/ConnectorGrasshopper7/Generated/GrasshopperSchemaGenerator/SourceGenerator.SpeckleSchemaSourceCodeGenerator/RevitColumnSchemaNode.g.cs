﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class RevitColumn_76c1c30b_6c81_b308_7b3e_ff39febe2a80: CreateSchemaObjectBase {
        
        static RevitColumn_76c1c30b_6c81_b308_7b3e_ff39febe2a80() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitColumn_76c1c30b_6c81_b308_7b3e_ff39febe2a80(): base("RevitColumn Vertical", "RevitColumn Vertical", "Creates a vertical Revit Column by point and levels.", "Revit", "Architecture"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("76c1c30b-6c81-b308-7b3e-ff39febe2a80");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitColumn.ctor(System.String,System.String,Objects.ICurve,Objects.BuiltElements.Level,Objects.BuiltElements.Level,System.Double,System.Double,System.Boolean,System.Double,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.RevitColumn");
          base.AddedToDocument(document);
        }
    }

    public class RevitColumn_da5513a4_9f7d_f2a5_5771_0b60cd62a5d7: CreateSchemaObjectBase {
        
        static RevitColumn_da5513a4_9f7d_f2a5_5771_0b60cd62a5d7() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitColumn_da5513a4_9f7d_f2a5_5771_0b60cd62a5d7(): base("RevitColumn Slanted (old)", "RevitColumn Slanted (old)", "Creates a slanted Revit Column by curve.", "Revit", "Structure"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("da5513a4-9f7d-f2a5-5771-0b60cd62a5d7");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitColumn.ctor(System.String,System.String,Objects.ICurve,Objects.BuiltElements.Level,System.Boolean,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.RevitColumn");
          base.AddedToDocument(document);
        }
    }

    public class RevitColumn_7a7e65bf_c653_95fe_8b07_86465d81ee1c: CreateSchemaObjectBase {
        
        static RevitColumn_7a7e65bf_c653_95fe_8b07_86465d81ee1c() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public RevitColumn_7a7e65bf_c653_95fe_8b07_86465d81ee1c(): base("RevitColumn Slanted", "RevitColumn Slanted", "Creates a slanted Revit Column by curve.", "Revit", "Structure"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("7a7e65bf-c653-95fe-8b07-86465d81ee1c");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.RevitColumn.ctor(System.String,System.String,Objects.ICurve,Objects.BuiltElements.Level,Objects.BuiltElements.Level,System.Boolean,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.RevitColumn");
          base.AddedToDocument(document);
        }
    }
    
    public class RevitColumn_76c1c30b_6c81_b308_7b3e_ff39febe2a80UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("643578da-dc5c-3576-cb14-63f817f78943");
        public Guid UpgradeTo => new("76c1c30b-6c81-b308-7b3e-ff39febe2a80");
    }


    public class RevitColumn_da5513a4_9f7d_f2a5_5771_0b60cd62a5d7UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("88ae9456-c5a6-cd62-f3b0-7df9030a7cb9");
        public Guid UpgradeTo => new("da5513a4-9f7d-f2a5-5771-0b60cd62a5d7");
    }


    public class RevitColumn_7a7e65bf_c653_95fe_8b07_86465d81ee1cUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("dae1866c-0ebc-fd2d-edf4-31528f22a669");
        public Guid UpgradeTo => new("7a7e65bf-c653-95fe-8b07-86465d81ee1c");
    }

}
