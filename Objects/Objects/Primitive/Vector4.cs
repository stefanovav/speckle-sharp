using System;

namespace Objects.Primitive
{
  public struct Vector4 : IEquatable<Vector4>
  {
    public double x;
    public double y;
    public double z;
    public double w;

    public double magnitude => Math.Sqrt(magnitudeSquared);
    public double magnitudeSquared => x * x + y * y + z * z + w * w;
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
          case 3: return w;
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
          case 3: w = value; break;

          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
    }

    public Vector4(double x, double y, double z, double w)
    {
      this.x = x; this.y = y; this.z = z; this.w = w;
    }

    public override bool Equals(object other)
    {
      if (!(other is Vector4)) return false;

      return Equals((Vector4)other);
    }

    public bool Equals(Vector4 other)
    {
      return x.Equals(other.x)
          && y.Equals(other.y)
          && z.Equals(other.z)
          && w.Equals(other.w);
    }

  }
}
