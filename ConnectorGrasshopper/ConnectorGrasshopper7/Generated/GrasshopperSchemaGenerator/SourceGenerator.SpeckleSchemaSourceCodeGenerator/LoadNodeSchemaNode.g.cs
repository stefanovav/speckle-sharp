﻿//<auto-generated/>
using System;
using System.Linq;
using Grasshopper.Kernel;
using ConnectorGrasshopperUtils;
using ConnectorGrasshopper;

namespace ConnectorGrasshopper.SchemaNodes.AutoGenerated {
    
    public class LoadNode_0d38cce3_391f_5474_bdfe_7e1c70ee2f8e: CreateSchemaObjectBase {
        
        static LoadNode_0d38cce3_391f_5474_bdfe_7e1c70ee2f8e() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public LoadNode_0d38cce3_391f_5474_bdfe_7e1c70ee2f8e(): base("Node Load", "Node Load", "Creates a Speckle node load", "Structural", "Loading"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("0d38cce3-391f-5474-bdfe-7e1c70ee2f8e");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Loading.LoadNode.ctor(Objects.Structural.Loading.LoadCase,System.Collections.Generic.List`1[Objects.Structural.Geometry.Node],Objects.Structural.Loading.LoadDirection,System.Double,System.String)", "Objects.Structural.Loading.LoadNode");
          base.AddedToDocument(document);
        }
    }

    public class LoadNode_b06a533c_d68f_d797_853f_db51427aa62a: CreateSchemaObjectBase {
        
        static LoadNode_b06a533c_d68f_d797_853f_db51427aa62a() {
          SpeckleGHSettings.SettingsChanged += (_, args) =>
          {
            if (!args.Key.StartsWith("Speckle2:tabs.")) return;
            var proxy = Grasshopper.Instances.ComponentServer.ObjectProxies.FirstOrDefault(p => p.Guid == internalGuid);
            if (proxy == null) return;
            proxy.Exposure = internalExposure;
          };
        }
        
        public LoadNode_b06a533c_d68f_d797_853f_db51427aa62a(): base("Node Load (user-defined axis)", "Node Load (user-defined axis)", "Creates a Speckle node load (specifed using a user-defined axis)", "Structural", "Loading"){}
        
        internal static string internalCategory => "Speckle 2 Autogenerated";
        internal static Guid internalGuid => new Guid("b06a533c-d68f-d797-853f-db51427aa62a");
        internal static GH_Exposure internalExposure => GH_Exposure.tertiary;

        public override GH_Exposure Exposure => internalExposure;
        public override Guid ComponentGuid => internalGuid;

        public override void AddedToDocument(GH_Document document){
          SelectedConstructor = CSOUtils.FindConstructor("Objects.Structural.Loading.LoadNode.ctor(Objects.Structural.Loading.LoadCase,System.Collections.Generic.List`1[Objects.Structural.Geometry.Node],Objects.Structural.Geometry.Axis,Objects.Structural.Loading.LoadDirection,System.Double,System.String)", "Objects.Structural.Loading.LoadNode");
          base.AddedToDocument(document);
        }
    }
    
    public class LoadNode_0d38cce3_391f_5474_bdfe_7e1c70ee2f8eUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("68ef4833-5aaf-ab0a-2535-26368b647de6");
        public Guid UpgradeTo => new("0d38cce3-391f-5474-bdfe-7e1c70ee2f8e");
    }


    public class LoadNode_b06a533c_d68f_d797_853f_db51427aa62aUpgraderObject: IGH_UpgradeObject
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

        public Guid UpgradeFrom => new Guid("103466bc-f3e6-4812-a0b8-3acaa16d3f7d");
        public Guid UpgradeTo => new("b06a533c-d68f-d797-853f-db51427aa62a");
    }

}
