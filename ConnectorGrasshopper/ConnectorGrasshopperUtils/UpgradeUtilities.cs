using System;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace ConnectorGrasshopperUtils;

public static class UpgradeUtils
{
  public static void SwapGroups(GH_Document document, IGH_Component component, IGH_Component upgradedComponent)
  {
    var groups = document
      .Objects
      .OfType<GH_Group>()
      .Where(gr => gr.ObjectIDs.Contains(component.InstanceGuid))
      .ToList();
    groups.ForEach(g => g.AddObject(upgradedComponent.InstanceGuid));
  }

  public static void SwapParameter(IGH_Component component, int index, IGH_Param target)
  {
    var source = component.Params.Input[index];
    GH_UpgradeUtil.MigrateSources(source, target);
    component.Params.Input.Remove(source);
    component.Params.Input.Insert(index, target);
  }
  
  public static Guid ToGuid(string src)
  {
    byte[] stringbytes = Encoding.UTF8.GetBytes(src);
    byte[] hashedBytes = new System.Security.Cryptography
        .SHA1CryptoServiceProvider()
      .ComputeHash(stringbytes);
    Array.Resize(ref hashedBytes, 16);
    return new Guid(hashedBytes);
  }
}
