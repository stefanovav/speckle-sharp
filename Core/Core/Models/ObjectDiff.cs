using System;
using System.Collections;
using System.Linq;

#nullable enable
namespace Speckle.Core.Models
{
  public class ObjectDiff : Base
  {
    public Base A { get; set; }
    public Base B { get; set; }

    public Base Diff { get; set; }

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
      foreach (var d in deleted)
      {
        Diff[d] = new DeletedConflict(A[d]);
      }
      
      var added = BMembers.Except(AMembers);
      foreach (var a in added)
      {
        Diff[a] = new AddedConflict(B[a]);
      }

      var potentiallyChange = A.GetDynamicMemberNames().Intersect(B.GetDynamicMemberNames());

      foreach (var propName in potentiallyChange)
      {
        object? valA = A[propName];
        object? valB = B[propName];
        
        if (IsDiff(valA, valB))
        {
          Diff[propName] = new ModifiedConflict(valA, valB);
        }
        else
        {
          Diff[propName] = new UnmodifiedConflict(valA);
        }
      }
      
      
    }

    private static bool IsDiff(object? valA, object? valB)
    {
      if (valA == null && valB == null) return false;
      
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


    public Base Merge()
    {
      var c = (Base)Activator.CreateInstance(A.GetType()); //todo: what happens if the type has changed?
      
      foreach (var member in Diff.GetDynamicMemberNames() )
      {
        if(!(Diff[member] is IConflict conflict)) continue;

        if (conflict.res == null) throw new Exception("Not resolved");

        conflict.Resolve(c, member);
      }

      c.id = c.GetId();
      return c;
    }

    public Base TakeAMerge()
    {
      foreach (var member in Diff.GetDynamicMemberNames())
      {
        if(Diff[member] is AddedConflict a) a.res = Resolution.Accept;
        if(Diff[member] is DeletedConflict d) d.res = Resolution.Ignore;
        if(Diff[member] is ModifiedConflict m) m.res = Resolution.Ignore;
      }
      return Merge();
    }
    
    public Base TakeBMerge()
    {
      foreach (var member in Diff.GetDynamicMemberNames())
      {
        if(Diff[member] is AddedConflict a) a.res = Resolution.Accept;
        if(Diff[member] is DeletedConflict d) d.res = Resolution.Accept;
        if(Diff[member] is ModifiedConflict m) m.res = Resolution.Accept;
      }

      return Merge();
    }
    
  }

  enum Resolution
  {
    Ignore,
    Accept
  }
  interface IConflict
  {
    public Resolution? res { get; set; }
    public void Resolve(Base obj, string key);

  }

  class AddedConflict: IConflict
  {
    private object? value;

    public AddedConflict(object value)
    {
      this.value = value;
    }

    public Resolution? res { get; set; }
    void IConflict.Resolve(Base obj, string key)
    {
      if (res == Resolution.Accept)
        obj[key] = value;
    }
  }
  class DeletedConflict: IConflict
  {
    private object? value;

    public DeletedConflict(object value)
    {
      this.value = value;
    }

    public Resolution? res { get; set; }
    void IConflict.Resolve(Base obj, string key)
    {
      // TODO: This may not be the best choice, assuming the object is empty.
      if (res == Resolution.Ignore)
        obj[key] = value;
    }
  }
  class ModifiedConflict: IConflict
  {
    public object? A, B;

    public ModifiedConflict(object? a, object? b)
    {
      A = a;
      B = b;
    }

    public Resolution? res { get; set; }
    public void Resolve(Base obj, string key)
    {
      if (res == Resolution.Ignore)
        obj[key] = A;
      else
        obj[key] = B;
    }
  }
  
  class UnmodifiedConflict: IConflict
  {
    public object? A;

    public UnmodifiedConflict(object? a)
    {
      A = a;
    }

    public Resolution? res { get; set; }
    public void Resolve(Base obj, string key)
    {
      obj[key] = A;
    }
  }
}
