using System;
using System.Runtime.CompilerServices;

namespace Objects.Primitive
{
  public struct Vector3 : IEquatable<Vector3>
  {
    public double x;
    public double y;
    public double z;

    public double magnitude => Math.Sqrt(magnitudeSquared);
    public double magnitudeSquared => x * x + y * y + z * z;

    /// <summary>
    /// Access element at sequential index [0..3] inclusive
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public double this[int index]
    {
      get
      {
        switch (index)
        {
          case 0: return x;
          case 1: return y;
          case 2: return z;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }

      set
      {
        switch (index)
        {
          case 0: x = value; break;
          case 1: y = value; break;
          case 2: z = value; break;

          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
    }

    public Vector3(double x, double y, double z)
    {
      this.x = x; this.y = y; this.z = z;
    }

    public override bool Equals(object other)
    {
      if (!(other is Vector3)) return false;

      return Equals((Vector3)other);
    }

    public bool Equals(Vector3 other)
    {
      return x.Equals(other.x)
          && y.Equals(other.y)
          && z.Equals(other.z);
    }

    public Vector3 Unitize()
    {
      var mag = this.magnitude;
      return mag != 0 ? new Vector3(x / mag, y / mag, z / mag) : this;
    }

    public Vector3 Cross(Vector3 second)
    {
      var x = this.y * second.z - second.y * this.z;
      var y = (this.x * second.z - second.x * this.z) * -1;
      var z = this.x * second.y - second.x * this.y;

      var cross = new Vector3(x, y, z);
      return cross.Unitize();
    }
  }
}
