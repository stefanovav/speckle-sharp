using System;
using System.Collections.Generic;
using NUnit.Framework;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;

namespace Tests.Models
{
  [TestFixture, TestOf(typeof(BaseExtensions))]
  public class DiffTests
  {
    
    static Base MakeObject(Action<Base> initDelegate)
    {
      Base obj = new Base();
      initDelegate.Invoke(obj);
      obj.id = obj.GetId();
      return obj;
    }

    [Test]
    public void TestUnchanged()
    {
      Base objA = MakeObject(b => b.applicationId = "myObject");
      Base objAA = MakeObject(b => b.applicationId = "v1");
      Base commitObjA = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{objA, objAA};
      });
      
      Base objB = MakeObject(b => b.applicationId = "myObject");
      Base objAB = MakeObject(b => b.applicationId = "v2");
      Base commitObjB = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{objB, objAB};
      });
      
      var diff = BaseExtensions.PerformObjectDiff(commitObjA, commitObjB);
      Assert.That(diff.Added, Has.Member(objAB));
      Assert.That(diff.Deleted, Has.Member(objAA));
      Assert.IsEmpty(diff.Modified);
      Assert.That(diff.Unchanged, Is.EquivalentTo(new []{ objB }));
    }
    
    [Test]
    public void TestDeleted()
    {
      Base obj = MakeObject(b => b.applicationId = "myObject");
      
      Base commitObjA = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{obj};
      });

      Base commitObjB = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{};
      });
      
      
      var diff = BaseExtensions.PerformObjectDiff(commitObjA, commitObjB);
      Assert.IsEmpty(diff.Added);
      Assert.That(diff.Deleted, Is.EquivalentTo(new []{ obj }) );
      Assert.IsEmpty(diff.Modified);
      Assert.IsEmpty(diff.Unchanged);
    }
    
    
    [Test]
    public void TestModified()
    {
      Base objA = MakeObject(b =>
      {
        b.applicationId = "myObject";
        b["value"] = "v1";
      });
      Base objB = MakeObject(b =>
      {
        b.applicationId = "myObject";
        b["value"] = "v2;";
      });
      
      Base commitObjA = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{objA};
      });

      Base commitObjB = MakeObject(b =>
      {
        b.applicationId = "commit";
        b["objects"] = new List<Base>{objB};
      });
      
      var diff = BaseExtensions.PerformObjectDiff(commitObjA, commitObjB);
      
      
      Assert.IsEmpty(diff.Added, "added");
      Assert.IsEmpty(diff.Deleted, "deleted");
      Assert.That(diff.Modified, Is.EquivalentTo(new[] {(objA, objB)}), "modified");
      Assert.IsEmpty(diff.Unchanged, "unchanged");
    }
    
    
  }
}
