using System;

namespace Objects.Primitive
{
  public struct Matrix4x4 : IEquatable<Matrix4x4>
  {
    // layout:
    //
    //                colum no (=vert)
    //               |  0   1   2   3
    //            ---+----------------
    //            0  | m00 m01 m02 m03
    // row no     1  | m10 m11 m12 m13
    // (=horiz)   2  | m20 m21 m22 m23
    //            3  | m30 m31 m32 m33

    public double m00;
    public double m01;
    public double m02;
    public double m03;

    public double m10;
    public double m11;
    public double m12;
    public double m13;

    public double m20;
    public double m21;
    public double m22;
    public double m23;

    public double m30;
    public double m31;
    public double m32;
    public double m33;

    #region index
    public double this[int row, int column]
    {
      get
      {
        return this[row + column * 4];
      }
      set
      {
        this[row + column * 4] = value;
      }
    }

    /// <summary>
    /// Access element at row-based sequential index [0..15] inclusive
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
          case 0: return m00;
          case 1: return m01;
          case 2: return m02;
          case 3: return m03;
          case 4: return m10;
          case 5: return m11;
          case 6: return m12;
          case 7: return m13;
          case 8: return m20;
          case 9: return m21;
          case 10: return m22;
          case 11: return m23;
          case 12: return m30;
          case 13: return m31;
          case 14: return m32;
          case 15: return m33;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }

      set
      {
        switch (index)
        {
          case 0: m00 = value; break;
          case 1: m01 = value; break;
          case 2: m02 = value; break;
          case 3: m03 = value; break;
          case 4: m10 = value; break;
          case 5: m11 = value; break;
          case 6: m12 = value; break;
          case 7: m13 = value; break;
          case 8: m20 = value; break;
          case 9: m21 = value; break;
          case 10: m22 = value; break;
          case 11: m23 = value; break;
          case 12: m30 = value; break;
          case 13: m31 = value; break;
          case 14: m32 = value; break;
          case 15: m33 = value; break;

          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
    }
    #endregion

    #region constructors
    public Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
      this.m00 = column0.x; this.m01 = column1.x; this.m02 = column2.x; this.m03 = column3.x;
      this.m10 = column0.y; this.m11 = column1.y; this.m12 = column2.y; this.m13 = column3.y;
      this.m20 = column0.z; this.m21 = column1.z; this.m22 = column2.z; this.m23 = column3.z;
      this.m30 = column0.w; this.m31 = column1.w; this.m32 = column2.w; this.m33 = column3.w;
    }
    public Matrix4x4(double[] value)
    {
      if (value.Length != 16)
        throw new ArgumentException("Double array length is not 16");

      this.m00 = value[0]; this.m01 = value[1]; this.m02 = value[2]; this.m03 = value[3];
      this.m10 = value[4]; this.m11 = value[5]; this.m12 = value[6]; this.m13 = value[7];
      this.m20 = value[8]; this.m21 = value[9]; this.m22 = value[10]; this.m23 = value[11];
      this.m30 = value[12]; this.m31 = value[13]; this.m32 = value[14]; this.m33 = value[15];
    }
    #endregion

    #region operations
    // used to allow Matrix4x4s to be used as keys in hash tables
    public override int GetHashCode()
    {
      return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2) ^ (GetColumn(3).GetHashCode() >> 1);
    }

    public override bool Equals(object other)
    {
      if (!(other is Matrix4x4)) return false;

      return Equals((Matrix4x4)other);
    }

    public bool Equals(Matrix4x4 other)
    {
      return GetColumn(0).Equals(other.GetColumn(0))
          && GetColumn(1).Equals(other.GetColumn(1))
          && GetColumn(2).Equals(other.GetColumn(2))
          && GetColumn(3).Equals(other.GetColumn(3));
    }
    #endregion

    #region methods
    static readonly Matrix4x4 identityMatrix =  new Matrix4x4(
      new Vector4(1, 0, 0, 0),
      new Vector4(0, 1, 0, 0),
      new Vector4(0, 0, 1, 0),
      new Vector4(0, 0, 0, 1));

    public static Matrix4x4 identity { get { return identityMatrix; } }

    public Vector4 GetColumn(int index)
    {
      switch (index)
      {
        case 0: return new Vector4(m00, m10, m20, m30);
        case 1: return new Vector4(m01, m11, m21, m31);
        case 2: return new Vector4(m02, m12, m22, m32);
        case 3: return new Vector4(m03, m13, m23, m33);
        default:
          throw new IndexOutOfRangeException("Invalid column index!");
      }
    }

    public double determinant =>
        m03 * m12 * m21 * m30 - m02 * m13 * m21 * m30 -
        m03 * m11 * m22 * m30 + m01 * m13 * m22 * m30 +
        m02 * m11 * m23 * m30 - m01 * m12 * m23 * m30 -
        m03 * m12 * m20 * m31 + m02 * m13 * m20 * m31 +
        m03 * m10 * m22 * m31 - m00 * m13 * m22 * m31 -
        m02 * m10 * m23 * m31 + m00 * m12 * m23 * m31 +
        m03 * m11 * m20 * m32 - m01 * m13 * m20 * m32 -
        m03 * m10 * m21 * m32 + m00 * m13 * m21 * m32 +
        m01 * m10 * m23 * m32 - m00 * m11 * m23 * m32 -
        m02 * m11 * m20 * m33 + m01 * m12 * m20 * m33 +
        m02 * m10 * m21 * m33 - m00 * m12 * m21 * m33 -
        m01 * m10 * m22 * m33 + m00 * m11 * m22 * m33;
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="rotation"></param>
    /// <param name="translation"></param>
    /// <returns></returns>
    public void Decompose(out Vector3 scale, out System.Numerics.Quaternion rotation, out Vector4 translation)
    {
      // translation
      translation = new Vector4(m03, m13, m23, m33);

      // scale
      // this should account for non-uniform scaling
      double scaleX = new Vector4(m00, m10, m20, m30).magnitude;
      double scaleY = new Vector4(m01, m11, m21, m31).magnitude;
      double scaleZ = new Vector4(m02, m12, m22, m32).magnitude;
      scale = new Vector3(scaleX, scaleY, scaleZ);

      // rotation
      var forward = new Vector3(m02, m12, m22);
      var up = new Vector3(m01, m11, m21);
      rotation = LookRotation(forward, up);
    }

    private static System.Numerics.Quaternion LookRotation(Vector3 forward, Vector3 up)
    {
      Vector3 vector = forward.Unitize();
      Vector3 vector2 = up.Cross(forward);
      Vector3 vector3 = vector.Cross(vector2);
      var m00 = vector2.x;
      var m01 = vector2.y;
      var m02 = vector2.z;
      var m10 = vector3.x;
      var m11 = vector3.y;
      var m12 = vector3.z;
      var m20 = vector.x;
      var m21 = vector.y;
      var m22 = vector.z;

      var num8 = (m00 + m11) + m22;
      if (num8 > 0d)
      {
        var num = Math.Sqrt(num8 + 1d);
        num = 0.5 / num;
        return new System.Numerics.Quaternion(
          (float)((m12 - m21) * num), 
          (float)((m20 - m02) * num), 
          (float)((m01 - m10) * num), 
          (float)(num * 0.5));
      }
      if ((m00 >= m11) && (m00 >= m22))
      {
        var num7 = Math.Sqrt(1d + m00 - m11 - m22);
        var num4 = 0.5 / num7;
        return new System.Numerics.Quaternion(
          (float)(0.5 * num7),
          (float)((m01 + m10) * num4),
          (float)((m02 + m20) * num4),
          (float)((m12 - m21) * num4));
      }
      if (m11 > m22)
      {
        var num6 = Math.Sqrt(1d + m11 - m00 - m22);
        var num3 = 0.5 / num6;
        return new System.Numerics.Quaternion(
          (float)((m10 + m01) * num3),
          (float)(0.5 * num6),
          (float)((m21 + m12) * num3),
          (float)((m20 - m02) * num3));
      }
      var num5 = Math.Sqrt(1d + m22 - m00 - m11);
      var num2 = 0.5 / num5;
      return new System.Numerics.Quaternion(
          (float)((m20 + m02) * num2),
          (float)((m21 + m12) * num2),
          (float)(0.5 * num5),
          (float)((m01 - m10) * num2));
    }

    #endregion
  }
}
