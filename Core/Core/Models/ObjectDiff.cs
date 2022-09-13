using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Api;
using Speckle.Core.Logging;
using Speckle.Core.Models.Extensions;

namespace Speckle.Core.Models
{
  public class ObjectDiff : Base
  {
    public Base A { get; set; }
    public Base B { get; set; }

    public Base Diff { get; set; } = null;

    public ObjectDiff(Base a, Base b)
    {
      this.A = a;
      this.B = b;
      CalculateDiff();
    }

    public void CalculateDiff()
    {
      Diff = new Base();

      var AMembers = A.GetMemberNames();
      var BMembers = B.GetMemberNames();

      var deleted = AMembers.Except(BMembers);
      var added = BMembers.Except(AMembers);
      
      //todo
      
      var potentiallyChange = A.GetDynamicMemberNames().Intersect(B.GetDynamicMemberNames());

      foreach (var prop in potentiallyChange)
      {
        var valA = A[prop];
        var valB = B[prop];
        
        if (IsDiff(valA, valB))
        {
          Diff[prop] = (valA, valB);
        }
      }
    }

    private bool IsDiff(object? valA, object? valB)
    {
      if (valA is null && valB is null) return false;
      
      if (valA == null || valB == null) return true;
      
      if (valB.GetType() != valA.GetType()) return true;
      
      switch (valA)
      {
        case Base valABase:
          Base valBBase = (Base) valB;

          if (valABase.id != valBBase.id)
          {
            return true;
          }

          break;
        case IList valAList:
          var valBList = (IList) valB;

          if (valAList.Count != valBList.Count)
          {
            return true;
          }

          for (int i = 0; i < valAList.Count; i++)
          {
            if (IsDiff(valAList[i], valBList[i])) return true;
          }

          break;
        default:
          return valA == valB;
        
      }
      return false;
    }


    public Base TakeA()
    {

      var copy = A.ShallowCopy();
      foreach (var member in Diff.GetDynamicMemberNames() )
      {
        object old = ((ValueTuple<object, object>) Diff[member]).Item2;
        if (old == null) ((ValueTuple<object, object>) Diff[member]).Item1;
        copy[member] = ;
      }

      copy.id = copy.GetId();
      return copy;
    }
    
    
    public Base Merge()
    {
      var myDuplicate = (Base)Activator.CreateInstance(A.GetType());

      var properties = A.GetDynamicMemberNames().Intersect(b)
      foreach (var prop in GetDynamicMemberNames())
      {
        var p = GetType().GetProperty(prop);
        if (p != null && !p.CanWrite)
        {
          continue;
        }

        try
        {
          myDuplicate[prop] = this[prop];
        }
        catch
        {
          // avoids any last ditch unsettable or strange props.
        }
      }

      return myDuplicate;
    }
  }
}

enum Resolution
{
  Ignore,
  Take
}
interface IConflict
{
  Resolution res;
  bool IsResolved();

}

class AddedConflict
{
  private object added;
}

class ModifiedConflict
{
  
  public ModifiedConflict()
  {
    Resolution.
  }
}
