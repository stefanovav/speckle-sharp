﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class FreeformElement_43519ebe_8616_412f_1361_836dcc8f14df: CreateSchemaObjectBase {
        
        static FreeformElement_43519ebe_8616_412f_1361_836dcc8f14df() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public FreeformElement_43519ebe_8616_412f_1361_836dcc8f14df(): base("Freeform element", "Freeform element", "Creates a Revit Freeform element using a list of Brep or Meshes. Category defaults to Generic Models", "Revit", "Families"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("43519ebe-8616-412f-1361-836dcc8f14df");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.FreeformElement.ctor(System.Collections.Generic.List`1[Speckle.Core.Models.Base],System.String,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.FreeformElement");
          base.AddedToDocument(document);
        }
    }

    public class FreeformElement_39a0f6fc_4013_3762_06f8_f94f26080f32: CreateSchemaObjectBase {
        
        static FreeformElement_39a0f6fc_4013_3762_06f8_f94f26080f32() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public FreeformElement_39a0f6fc_4013_3762_06f8_f94f26080f32(): base("Freeform element", "Freeform element", "Creates a Revit Freeform element using a list of Brep or Meshes.", "Revit", "Families"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("39a0f6fc-4013-3762-06f8-f94f26080f32");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.FreeformElement.ctor(Speckle.Core.Models.Base,System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.FreeformElement");
          base.AddedToDocument(document);
        }
    }

    public class FreeformElement_3c3acfc8_1cd8_db67_c85c_4f9c9179ba7f: CreateSchemaObjectBase {
        
        static FreeformElement_3c3acfc8_1cd8_db67_c85c_4f9c9179ba7f() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public FreeformElement_3c3acfc8_1cd8_db67_c85c_4f9c9179ba7f(): base("Freeform element", "Freeform element", "Creates a Revit Freeform element using a list of Brep or Meshes.", "Revit", "Families"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("3c3acfc8-1cd8-db67-c85c-4f9c9179ba7f");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.BuiltElements.Revit.FreeformElement.ctor(System.Collections.Generic.List`1[Speckle.Core.Models.Base],System.Collections.Generic.List`1[Objects.BuiltElements.Revit.Parameter])", "Objects.BuiltElements.Revit.FreeformElement");
          base.AddedToDocument(document);
        }
    }
    
    public class FreeformElement_43519ebe_8616_412f_1361_836dcc8f14dfUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("63878609-5eb8-837d-7a7f-5836dd97670e");
        public Guid UpgradeTo => new("43519ebe-8616-412f-1361-836dcc8f14df");
    }


    public class FreeformElement_39a0f6fc_4013_3762_06f8_f94f26080f32UpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("11939d5e-c5a4-f49c-5586-6571b87fccb2");
        public Guid UpgradeTo => new("39a0f6fc-4013-3762-06f8-f94f26080f32");
    }


    public class FreeformElement_3c3acfc8_1cd8_db67_c85c_4f9c9179ba7fUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("e0941d35-46d6-9fea-a7dc-b9d38727eb59");
        public Guid UpgradeTo => new("3c3acfc8-1cd8-db67-c85c-4f9c9179ba7f");
    }

}
