using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixLib
{
    /// <summary>
    /// Class used to represent a 2D Coordinate point.
    /// </summary>
    [System.Serializable]
    public class Point
    {
        public GameObject pointObject;

        /// <summary>
        /// The inital position of the object. 
        /// </summary>
        public Vector3 initalPos;

        /// <summary>
        /// The transformed position of the object. 
        /// </summary>
        public Vector3 transformedPos;

        /// <summary>
        /// Set the inital state of the object. 
        /// </summary>
        public virtual void SetInitalState() => initalPos = pointObject.transform.position;

        /// <summary>
        /// Set the affine transformation to the point. 
        /// </summary>
        /// <param name="tComp"> The translation component. </param>
        /// <param name="rComp"> The rotation component. </param>
        /// <param name="sComp"> The scale component. </param>
        public virtual void SetAffineTransformations(TranslationComponent tComp,
            RotationComponent rComp, ScaleComponent sComp)
        {
            Matrix2X2 modelMatrix =
                (Matrix2X2.CreateTranslationMatrix(tComp.translation.x, tComp.translation.y) *
                Matrix2X2.CreateRotationMatrix(rComp.orientation)) *
                Matrix2X2.CreateScaleMatrix(sComp.scale.x, sComp.scale.y);
            if(CameraCont.Instance != null )
            {
                Matrix2X2 worldM = CameraCont.WorldMatrix * modelMatrix;
                pointObject.transform.position = transformedPos =
                    (Vector2)(worldM * new Matrix2X2.Vector3D(initalPos.x, initalPos.y, 1));
            }
            else
            {
                pointObject.transform.position = transformedPos = 
                    (Vector2)(modelMatrix * new Matrix2X2.Vector3D(initalPos.x, initalPos.y, 1));
            }

        }
    }

    /// <summary>
    /// Class used to represent a 2D cordiantate point object. 
    /// </summary>
    [System.Serializable]
    public class PointObj : Point
    {
        /// <summary>
        /// The inital scale of the object. 
        /// </summary>
        public Vector3 initalScale;

        /// <summary>
        /// The transformed scale of the object. 
        /// </summary>
        public Vector3 transformedScale;

        /// <summary>
        /// Set the inital state of the point. 
        /// </summary>
        public override void SetInitalState()
        {
            initalScale = pointObject.transform.localScale;
            base.SetInitalState();
        }

        /// <summary>
        /// Set Affine Transformations to the point.
        /// </summary>
        /// <param name="tComp"> The translation component. </param>
        /// <param name="rComp"> The rotation component. </param>
        /// <param name="sComp"> The scale component. </param>
        public override void SetAffineTransformations(TranslationComponent tComp, RotationComponent rComp,
            ScaleComponent sComp)
        {
            pointObject.transform.position = tComp.Matrix * base.initalPos;
            pointObject.transform.localScale = rComp.Matrix * initalScale;
        }
    }

    /// <summary>
    /// Class used to represent a Matrix with X and Y coordinates. 
    /// Provides functionality for affine transformations.
    /// </summary>
    [System.Serializable]
    public class Matrix2X2
    {
        #region Vector Data Structs.
        /// <summary>
        /// Struct. Representation for 2 dimensional vectors. 
        /// </summary>
        [System.Serializable]
        public struct Vector2D
        {
            /// <summary>
            /// The X component of the vector. 
            /// </summary>
            public float x;

            /// <summary>
            /// The Y component of the vector. 
            /// </summary>
            public float y;

            /// <summary>
            /// CTOR: 
            /// </summary>
            /// <param name="x"> The x component of the vector. </param>
            /// <param name="y"> The y component of the vector. </param>
            public Vector2D(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            #region Operator Definitions.
            /// <summary>
            /// Operator defining conversion from a Vector3D to a Vector2D.
            /// </summary>
            /// <param name="v"> The Vector3D to convert. </param>
            public static explicit operator Vector2D(Vector3D v) => new(v.x, v.y);

            /// <summary>
            /// Operator defining conversion from a Vector2D to a Vector3D. 
            /// </summary>
            /// <param name="v"> The Vector2D to convert. </param>
            public static explicit operator Vector3D(Vector2D v) => new(v.x, v.y, 0);

            /// <summary>
            /// Operator defining conversion from a Vector2D to a Vector2. 
            /// </summary>
            /// <param name="v"> The Vector2D to convert. </param>
            public static explicit operator Vector2(Vector2D v) => new(v.x, v.y);
            #endregion
        }

        /// <summary>
        /// Struct. Representation for 3 dimensional vectors. 
        /// </summary>
        [System.Serializable]
        public struct Vector3D
        {
            /// <summary>
            /// The X component of the vector. 
            /// </summary>
            public float x;

            /// <summary>
            /// The X component of the vector. 
            /// </summary>
            public float y;

            /// <summary>
            /// The X component of the vector. 
            /// </summary>
            public float z;

            /// <summary>
            /// CTOR:
            /// </summary>
            /// <param name="x"> The X component of the vector. </param>
            /// <param name="y"> The Y compinent of the vector. </param>
            public Vector3D(float x, float y)
            {
                this.x = x;
                this.y = y;
                z = 1;
            }

            /// <summary>
            /// CTOR:
            /// </summary>
            /// <param name="x"> The X component of the vector. </param>
            /// <param name="y"> The Y component of the vector. </param>
            /// <param name="z"> The z component of the vector. </param>
            public Vector3D(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            /// <summary>
            /// Get a Vector3D where components are set to zero. 
            /// </summary>
            public static Vector3D Zero => new Vector3D(0, 0, 0);

            public Vector3D Unitized
            {
                get
                {
                    float magnitude = Mathf.Sqrt((x * x) + (y * y) + (z * z));
                    return new Vector3D(x/magnitude, y/magnitude, z/magnitude);
                }
            }

            /// <summary>
            /// Debug.log the vectors components. 
            /// </summary>
            public void DebugVector() => Debug.Log(this.ToString());

            /// <summary>
            /// Debug.log the vectors components. 
            /// </summary>
            public void DebugVector(string message) =>
                Debug.Log(message + this.ToString());

            #region Conversion Operators. 

            /// <summary>
            /// Get the elements of the vector as a string. 
            /// </summary>
            /// <returns></returns>
            public override string ToString() =>
                "Vector: (" + x + ", " + y + ", " + z + ").";

            /// <summary>
            /// Typecast (Vector3D -> UnityEngine.Vector2). 
            /// </summary>
            /// <param name="v"> The Vector3D to typecast. </param>
            public static explicit operator UnityEngine.Vector2(Vector3D v) =>
                new Vector2(v.x, v.y);

            /// <summary>
            /// Typecast (Vector3D -> UnityEngine.Vector3). 
            /// </summary>
            /// <param name="v"> The Vector3D to typecast to UnityEngine.Vector3. </param>
            public static explicit operator Vector3(Vector3D v) =>
                new Vector3(v.x, v.y, v.z);

            public static float operator *(Vector3D lhs, Vector3D rhs) =>
                (lhs.x * rhs.x) + (lhs.y * rhs.y) + (lhs.z * rhs.z);

            #endregion
        }

        #endregion

        #region Constants/Readonly Fields. 

        private static readonly Matrix2X2 IDENTITY = new(
                new Vector3D(1, 0, 0),
                new Vector3D(0, 1, 0),
                new Vector3D(0, 0, 1));

        private static readonly Matrix2X2 ZERO = 
            new Matrix2X2(Vector3D.Zero, Vector3D.Zero, Vector3D.Zero);
        #endregion

        /// <summary>
        /// The matrix's x axis <x,y,z>. 
        /// </summary>
        public Vector3D xAxis;

        /// <summary>
        /// The matrix's y axis <x,y,z>.
        /// </summary>
        public Vector3D yAxis;

        /// <summary>
        /// The matrix's t axis <x,y,z>. 
        /// </summary>
        public Vector3D tAxis;

        #region Properties. 
        /// <summary>
        /// Shorthand for the Identity matrix. 
        /// </summary>
        public static Matrix2X2 Identity => IDENTITY;

        /// <summary>
        /// Shorthand for the zero vector. 
        /// </summary>
        public static Matrix2X2 Zero => ZERO;

        #region Row/Column Accesors. 
        public Vector3D R0 => new Vector3D(xAxis.x, yAxis.x, tAxis.x);
        public Vector3D R1 => new Vector3D(xAxis.y, yAxis.y, tAxis.y);
        public Vector3D R2 => new Vector3D(xAxis.z, yAxis.z, tAxis.z);

        public Vector3D C0 => xAxis;
        public Vector3D C1 => yAxis;
        public Vector3D C2 => tAxis;
        #endregion
        #endregion

        #region CTORS. 
        /// <summary>
        /// CTOR. 
        /// </summary>
        public Matrix2X2() => xAxis = yAxis = tAxis = Vector3D.Zero;

        /// <summary>
        /// CTOR. 
        /// </summary>
        /// <param name="xAxis"> The x axis of the matrix. </param>
        /// <param name="yAxis"> The y axis of the matrix. </param>
        /// <param name="tAxis"> The t axis of the matrix. </param>
        public Matrix2X2(Vector3D xAxis, Vector3D yAxis, Vector3D tAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.tAxis = tAxis;
        }
        #endregion

        #region Multiplication Functions. 

        /// <summary>
        /// Multiply Vector3D by the 3x3 matrix. 
        /// </summary>
        /// <param name="v"> The vector to multiply. </param>
        /// <returns> The product of this 3x3 matrix and v. </returns>
        public Vector2D Multiply(Vector3D v) =>
            (Vector2D) new Vector3D( R0 * v, R1 * v, R2 * v);

        /// <summary>
        /// Multiply Matrix2x2 LHS and Matrix2x2 RHS.
        /// </summary>
        /// <param name="lhs">The left hand side matrix to multiply.</param>
        /// <param name="rhs">The right hand side matrix to multiply.</param>
        /// <returns></returns>
        internal static Matrix2X2 Multiply(Matrix2X2 lhs, Matrix2X2 rhs)
        {

            return new Matrix2X2(
                new Vector3D(lhs.R0 * rhs.C0, lhs.R1 * rhs.C0, lhs.R2 * rhs.C0),
                new Vector3D(lhs.R0 * rhs.C1, lhs.R1 * rhs.C1, lhs.R2 * rhs.C1),
                new Vector3D(lhs.R0 * rhs.C2, lhs.R1 * rhs.C2, lhs.R2 * rhs.C2)
                );
        }

        #endregion

        #region Matrix Setters. 
        /// <summary>
        /// Generate a new translation matrix.
        /// </summary>
        /// <param name="transX"> The x-axis translation. </param>
        /// <param name="transY"> The y-axis translation. </param>
        /// <returns> New translation Matrix2x2. </returns>
        public static Matrix2X2 CreateTranslationMatrix(float transX, float transY)
        {
            Matrix2X2 mat = Identity;
            mat.tAxis.x = transX;
            mat.tAxis.y = transY;
            return mat;
        }

        /// <summary>
        /// Generate a new scale matrix. 
        /// </summary>
        /// <param name="scaleX"> The x scale value. </param>
        /// <param name="scaleY"> The y scale value. </param>
        /// <returns> Scale Matrix2X2 with provided values preset.</returns>
        public static Matrix2X2 CreateScaleMatrix(float scaleX, float scaleY)
        {
            Matrix2X2 mat = ZERO;
            mat.xAxis.x = scaleX;
            mat.yAxis.y = scaleY;
            mat.tAxis.z = 1; 
            return mat;
        }

        /// <summary>
        /// Generate a new rotation matrix (Z-axis rotation). 
        /// </summary>
        /// <param name="angle"> The angle to rotate by. </param>
        /// <returns> New rotation Matrix2X2 with preset values. </returns>
        public static Matrix2X2 CreateRotationMatrix(float angle)
        {
            Matrix2X2 mat = Zero;
            mat.xAxis = new Vector3D(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0).Unitized;
            mat.yAxis = new Vector3D(
                -Mathf.Sin(angle * Mathf.Deg2Rad),
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0).Unitized;
            mat.tAxis.z = 1; 
            return mat;
        }

        /// <summary>
        /// Set this matrix to the shear matrix. 
        /// </summary>
        /// <param name="xShear"> The amount to shear the x-axis by. </param>
        /// <param name="yShear"> The amount to shear the y-axis by. </param>
        public static Matrix2X2 CreateShearMatrix(float xShear, float yShear)
        {
            Matrix2X2 m = Identity;
            m.xAxis.y = xShear;
            m.yAxis.x = xShear;
            return m;
        }

        /// <summary>
        /// Set this matrix to the reflection matrix. 
        /// </summary>
        /// <param name="xAxis"> Flag to say if the x axis should be flipped. </param>
        /// <param name="yAxis"> Flag to say if the y axis should be flipped. </param>
        public void SetReflection(bool xAxis, bool yAxis)
        {
            Matrix2X2 m = Identity;
            m.xAxis.x = (xAxis ? -1 : 0);
            m.yAxis.y = (yAxis ? -1 : 0);
        }

        #endregion

        #region Debugging Functions.

        private const string DEBUG_HEADER = "--------------- \n";
        private const string DEBUG_SPACER = " \n";
        private const string DEBUG_ROW_FORMAT = "[  {0}, {1}, {2} ] ";

        public void DebugMatrix() =>
            Debug.Log(ToString());

        public void DebugMatrix(string name) =>
            Debug.Log( name + "  \n" + ToString());


        public void DebugMatrixPair(Matrix2X2 b)
        {
            Debug.Log(
                DEBUG_HEADER +
                RowSTR(xAxis.x, yAxis.x, tAxis.x) + RowSTR(b.xAxis.x, b.yAxis.x, b.tAxis.x) + DEBUG_SPACER +
                RowSTR(xAxis.y, yAxis.y, tAxis.y) + RowSTR(b.xAxis.y, b.yAxis.y, b.tAxis.y) + DEBUG_SPACER +
                RowSTR(xAxis.z, yAxis.z, tAxis.z) + RowSTR(b.xAxis.z, b.yAxis.z, b.tAxis.z) + DEBUG_SPACER +
                DEBUG_HEADER
                );
        }

        public override string ToString()
        {
            return
                DEBUG_HEADER +
                RowSTR(xAxis.x, yAxis.x, tAxis.x) + DEBUG_SPACER +
                RowSTR(xAxis.y, yAxis.y, tAxis.y) + DEBUG_SPACER +
                RowSTR(xAxis.z, yAxis.z, tAxis.z) + DEBUG_SPACER +
                DEBUG_HEADER;
        }

        string RowSTR(float x, float y, float z) =>
            String.Format(DEBUG_ROW_FORMAT, x, y, z);
        #endregion

        #region Operator Definitions. 

        public override bool Equals(object obj)
        {
            if ((Matrix2X2)obj == this)
                return true;
            if (!(obj is Matrix2X2))
                return false;
            Matrix2X2 m = obj as Matrix2X2;
            return ((Vector3)xAxis == (Vector3)m.xAxis &&
                (Vector3)yAxis == (Vector3)m.yAxis
                && (Vector3)tAxis == (Vector3)m.tAxis);
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + (int)(xAxis.x + xAxis.y + xAxis.z);
            result = 31 * result + (int)(yAxis.x + yAxis.y + yAxis.z);
            result = 31 * result + (int)(tAxis.x + tAxis.y + tAxis.z);
            return result;
        }

        public static Matrix2X2 operator *(Matrix2X2 lhs, Matrix2X2 rhs) =>
            Matrix2X2.Multiply(lhs, rhs);

        public static Vector2D operator *(Matrix2X2 m, Vector3D v) =>
            m.Multiply(new Vector3D(v.x, v.y, 1));

        public static Vector2 operator *(Matrix2X2 m, Vector2 v) =>
            (Vector2)m.Multiply(new Vector3D(v.x, v.y, 1));

        public static bool operator ==(Matrix2X2 a, Matrix2X2 b) =>
            ((Vector3)a.xAxis == (Vector3)b.xAxis &&
             (Vector3)a.yAxis == (Vector3)b.yAxis &&
             (Vector3)a.tAxis == (Vector3)b.tAxis);

        public static bool operator !=(Matrix2X2 a, Matrix2X2 b) =>
            !(a == b);
        #endregion
    }

    /// <summary>
    /// Class defining 4x4 Matrix structure. 
    /// </summary>
    public class Matrix4X4
    {
        /// <summary>
        /// Struct defining 4 dimensional vectors. 
        /// </summary>
        [System.Serializable]
        public struct Vector4D
        {
            /// <summary>
            /// The x component of the vector. 
            /// </summary>
            public float x;

            /// <summary>
            /// The y component of the vector. 
            /// </summary>
            public float y;

            /// <summary>
            /// The z component of the vector. 
            /// </summary>
            public float z;

            /// <summary>
            /// The w component of the vector. 
            /// </summary>
            public float w;

            #region CTORs. 
            /// <summary>
            /// CTOR. 
            /// </summary>
            /// <param name="x"> The x component of the vector. </param>
            /// <param name="y"> The y component of the vector. </param>
            /// <param name="z"> The z component of the vector. </param>
            /// <param name="w"> The w component of the vector. </param>
            public Vector4D(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            /// <summary>
            /// CTOR. 
            /// </summary>
            /// <param name="vec"> The vector to convert to Vector4D. </param>
            public Vector4D(Vector4 vec) : this(vec.x, vec.y, vec.z, vec.w) { }
            #endregion

            #region Operators. 

            /// <summary>
            /// Typecast Operator (Vector4 -> Vector4D). 
            /// </summary>
            /// <param name="v"> The vector4 to cast. </param>
            public static explicit operator Vector4D(Vector4 v) =>
                new Vector4D(v.x, v.y, v.z, v.w);

            /// <summary>
            /// Typecast Operator (Vector4D -> Vector4). 
            /// </summary>
            /// <param name="v"> The Vector4D to cast. </param>
            public static explicit operator Vector4(Vector4D v) =>
                new Vector4(v.x, v.y, v.z, v.w);

            /// <summary>
            /// Typecast Operator (Vector4D -> Vector3). 
            /// </summary>
            /// <param name="v"> The Vector4D to cast. </param>
            public static explicit operator Vector3(Vector4D v) =>
                new Vector3(v.x, v.y, v.z);
            #endregion

        }

        /// <summary>
        /// The x axis of the matrix. 
        /// </summary>
        public Vector4D xAxis;

        /// <summary>
        /// The y axis of the matrix. 
        /// </summary>
        public Vector4D yAxis;

        /// <summary>
        /// The z axis of the matrix. 
        /// </summary>
        public Vector4D zAxis;

        /// <summary>
        /// The w axis of the matrix. 
        /// </summary>
        public Vector4D wAxis;

        #region Properties. 
        /// <summary>
        /// Shorthand for the 3x3 Identity Matrix. 
        /// </summary>
        public static Matrix4X4 Identity =>
            new Matrix4X4(
                new Vector4D(1, 0, 0, 0),
                new Vector4D(0, 1, 0, 0),
                new Vector4D(0, 0, 1, 0),
                new Vector4D(0, 0, 0, 1));
        #endregion

        #region CTORS. 
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="xAxis"> The x-axis as a Vector4D. </param>
        /// <param name="yAxis"> The y-axis as a Vector4D. </param>
        /// <param name="zAxis"> The z-axis as a Vector4D. </param>
        /// <param name="wAxis"> The w-axis as a Vector4D. </param>
        public Matrix4X4(Vector4D xAxis, Vector4D yAxis,
            Vector4D zAxis, Vector4D wAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.zAxis = zAxis;
            this.wAxis = wAxis;
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="xAxis"> The x-axis as a Vector4. </param>
        /// <param name="yAxis"> The y-axis as a Vector4. </param>
        /// <param name="zAxis"> The z-axis as a Vector4. </param>
        /// <param name="wAxis"> The w-axis as a Vector4. </param>
        public Matrix4X4(Vector4 xAxis, Vector4 yAxis, Vector4 zAxis, Vector4 wAxis) :
            this((Vector4D)xAxis, (Vector4D)yAxis, (Vector4D)zAxis, (Vector4D)wAxis)
        {
        }
        #endregion

        #region Affine Transformation Setting Functions.
        /// <summary>
        /// Enum value type used to specify scale matrix typing. 
        /// </summary>
        public enum RotationType
        {
            X,
            Y,
            Z
        }

        /// <summary>
        /// Degree to Radian conversion value. 
        /// </summary>
        const float D2R = Mathf.Deg2Rad;

        /// <summary>
        /// Stored function reference to MathF.Cos(). 
        /// </summary>
        static readonly Func<float, float> COS = MathF.Cos;

        /// <summary>
        /// Stored function reference to MathF.Sin(). 
        /// </summary>
        static readonly Func<float, float> SIN = MathF.Sin;

        /// <summary>
        /// Generate a translation matrix based on provided values.
        /// </summary>
        /// <param name="X"> The x translation amount. </param>
        /// <param name="Y"> The y translation amount. </param>
        /// <param name="Z"> The y translation amount. </param>
        /// <returns> Matrix4X4 translation matrix. </returns>
        public static Matrix4X4 CreateTranslationMatrix(float X, float Y, float Z)
        {
            Matrix4X4 m = Matrix4X4.Identity;
            m.wAxis.x = X;
            m.wAxis.y = Y;
            m.wAxis.z = Z;
            m.wAxis.y = 1;
            return m;
        }

        /// <summary>
        /// Generate a scale matrix based on provided values. 
        /// </summary>
        /// <param name="X"> The x scale component. </param>
        /// <param name="Y"> The y scale component. </param>
        /// <returns> A scale matrix with the provided component values. </returns>
        public static Matrix4X4 CreateScaleMatrix(float X, float Y, float Z)
        {
            Matrix4X4 m = Matrix4X4.Identity;
            m.xAxis.x = X;
            m.xAxis.y = Y;
            m.xAxis.z = Z;
            m.xAxis.w = 1;
            return m;
        }

        /// <summary>
        /// Generate a rotation matrix. 
        /// </summary>
        /// <param name="type"> The type of rotation matrix to generate (x, y, or z). </param>
        /// <param name="angle"> The angle of the rotation. </param>
        /// <returns> New rotation matrix of the sspecified type prefilled with the angular values. </returns>
        public static Matrix4X4 GenerateRotationMatrix(RotationType type, float angle)
        {
            Matrix4X4 m = Matrix4X4.Identity;

            switch (type)
            {
                case RotationType.X:
                    m.yAxis.y = COS(angle * D2R);
                    m.yAxis.z = SIN(angle * D2R);
                    m.zAxis.y = -SIN(angle * D2R);
                    m.zAxis.z = COS(angle * D2R);
                    return m;

                case RotationType.Y:
                    m.xAxis.x = COS(angle * D2R);
                    m.xAxis.z = -SIN(angle * D2R);
                    m.zAxis.x = SIN(angle * D2R);
                    m.zAxis.z = COS(angle * D2R);
                    return m;

                case RotationType.Z:
                    m.xAxis.x = COS(angle * D2R);
                    m.xAxis.y = SIN(angle * D2R);
                    m.yAxis.x = -SIN(angle * D2R);
                    m.yAxis.y = COS(angle * D2R);
                    return m;
                default:
                    return m;
            }
        }
        #endregion

        #region Multiplication Functions. 

        /// <summary>
        /// Multiply Vector3 by the matrix.  
        /// </summary>
        /// <param name="vec"> The vector to compute the product of. </param>
        /// <returns> The vector product of the matrix and vec.</returns>
        public Vector4 Multiply(Vector3 vec) =>
            Multiply(vec);

        /// <summary>
        /// Multiply a four dimensional vector by a Matrix4X4. 
        /// </summary>
        /// <param name="vec"> The 4 dimensional vector to multiply. </param>
        /// <returns> The product of the referenced matrix and vec as Vector4D. </returns>
        public Vector4D Multiply(Vector4D vec) =>
            new Vector4D(
                MultiplyRowByColumn(new Vector4D(xAxis.x, yAxis.x, zAxis.x, wAxis.x), vec),
                MultiplyRowByColumn(new Vector4D(xAxis.y, yAxis.y, zAxis.y, wAxis.y), vec),
                MultiplyRowByColumn(new Vector4D(xAxis.x, yAxis.x, zAxis.x, wAxis.z), vec),
                MultiplyRowByColumn(new Vector4D(xAxis.w, yAxis.w, zAxis.w, wAxis.w), vec)
                );

        /// <summary>
        /// Multiply matrix a by matrix b. 
        /// </summary>
        /// <param name="a"> Matrix4x4 value. </param>
        /// <param name="b"> Matrix4x4 value. </param>
        /// <returns> The product of a * b. </returns>
        internal static Matrix4X4 Multiply(Matrix4X4 a, Matrix4X4 b)
        {
            ///Generate the columns. 
            Vector4D col1 = new Vector4D(b.xAxis.x, b.xAxis.y, b.xAxis.z, b.xAxis.w),
                     col2 = new Vector4D(b.yAxis.x, b.yAxis.y, b.yAxis.z, b.yAxis.w),
                     col3 = new Vector4D(b.zAxis.x, b.zAxis.y, b.zAxis.z, b.zAxis.w),
                     col4 = new Vector4D(b.wAxis.x, b.wAxis.y, b.wAxis.z, b.wAxis.w);

            //Compute the new matrix. 
            Matrix4X4 n = new Matrix4X4(
                    MultiplyRC(new(a.xAxis.x, a.yAxis.x, a.zAxis.x, a.wAxis.x), col1, col2, col3, col4),
                    MultiplyRC(new(a.xAxis.y, a.yAxis.y, a.zAxis.y, a.wAxis.y), col1, col2, col3, col4),
                    MultiplyRC(new(a.xAxis.z, a.yAxis.z, a.zAxis.z, a.wAxis.z), col1, col2, col3, col4),
                    MultiplyRC(new(a.xAxis.w, a.yAxis.w, a.zAxis.w, a.wAxis.w), col1, col2, col3, col4)
                );

            //Func multiplies a row by all four columns. 
            static Vector4D MultiplyRC(Vector4D row,
                Vector4D col1, Vector4D col2, Vector4D col3, Vector4D col4)
            {
                return new Vector4D(
                    MultiplyRowByColumn(row, col1),
                    MultiplyRowByColumn(row, col2),
                    MultiplyRowByColumn(row, col3),
                    MultiplyRowByColumn(row, col4)
                    );
            }
            return n;
        }

        /// <summary>
        /// Mulitplies a given row by a given column. 
        /// </summary>
        /// <param name="row"> The row to multiply. </param>
        /// <param name="column"> The column to multiply. </param>
        /// <returns> float: sum of the products of row * vector. </returns>
        static float MultiplyRowByColumn(Vector4D row, Vector4D column) =>
                row.x * column.x + row.y * column.y + row.z * column.z + row.w * column.w;
        #endregion

        #region Projection Matrix Functions. 
        public static Matrix4X4 GetOthographicProjection(ProjectionData proData)
        {
            float r_m_l = proData.scale.right - proData.scale.left;
            float t_m_b = proData.scale.top - proData.scale.bottom;
            float f_m_n = proData.scale.far - proData.scale.near;
            float r_p_l = proData.scale.right + proData.scale.left;
            float t_p_b = proData.scale.top + proData.scale.bottom;
            float f_p_n = proData.scale.far + proData.scale.near;

            Matrix4X4 m = new Matrix4X4(
                new Vector4D(2 / (r_m_l), 0, 0, 0),
                new Vector4D(0, 2 / (t_m_b), 0, 0),
                new Vector4D(0, 0, -2 / f_m_n, 0),
                new Vector4D(-(r_p_l / r_m_l), -(t_p_b / t_m_b), -(f_p_n / f_m_n), 1)
                );
            Matrix4X4 xRot =
                Matrix4X4.GenerateRotationMatrix(RotationType.X, proData.orientation.x);
            Matrix4X4 yRot =
                Matrix4X4.GenerateRotationMatrix(RotationType.Y, proData.orientation.y);
            Matrix4X4 zRot =
                Matrix4X4.GenerateRotationMatrix(RotationType.Z, proData.orientation.z);
            Matrix4X4 translation =
                Matrix4X4.CreateTranslationMatrix(
                    proData.position.x,
                    proData.position.y,
                    proData.position.z
                    );

            return translation * xRot * yRot * zRot * m;
        }
        #endregion

        #region Debugging / STR Conversion Functions.
        private const string DEBUG_HEADER = "--------------- \n";
        private const string DEBUG_SPACER = " \n";
        private const string DEBUG_ROW_FORMAT = "{  {0}, {1}, {2}, {3} } ";

        /// <summary>
        /// Get the matrix represented as a string. 
        /// </summary>
        /// <returns> String: --------------- \n
        ///                   {xx, yx, zx, wx} \n
        ///                   {xy, yy, zy, wy} \n
        ///                   {xz, yz, zz, wz} \n
        ///                   {xw, yw, zw, ww} \n
        ///                   --------------- \n
        ///                   </returns>
        public override string ToString() =>
                DEBUG_HEADER +
                RowSTR(xAxis.x, yAxis.x, zAxis.x, wAxis.x) + DEBUG_SPACER +
                RowSTR(xAxis.y, yAxis.y, zAxis.y, wAxis.y) + DEBUG_SPACER +
                RowSTR(xAxis.z, yAxis.z, zAxis.z, wAxis.z) + DEBUG_SPACER +
                RowSTR(xAxis.w, yAxis.w, zAxis.w, wAxis.w) + DEBUG_SPACER +
                DEBUG_HEADER;

        /// <summary>
        /// Debugs a pair of matrices to the Unity Engine console. 
        /// </summary>
        /// <param name="b"> The second matrix to write to the display.</param>
        public void DebugMatrixPair(Matrix4X4 b)
        {
            Debug.Log(
                DEBUG_HEADER +
                RowSTR(xAxis.x, yAxis.x, zAxis.x, wAxis.x) + RowSTR(b.xAxis.x, b.yAxis.x, b.zAxis.x, b.wAxis.x) + DEBUG_SPACER +
                RowSTR(xAxis.y, yAxis.y, zAxis.y, wAxis.y) + RowSTR(b.xAxis.y, b.yAxis.y, b.zAxis.y, b.wAxis.y) + DEBUG_SPACER +
                RowSTR(xAxis.z, yAxis.z, zAxis.z, wAxis.z) + RowSTR(b.xAxis.z, b.yAxis.z, b.zAxis.z, b.wAxis.z) + DEBUG_SPACER +
                RowSTR(xAxis.w, yAxis.w, zAxis.w, wAxis.w) + RowSTR(b.xAxis.w, b.yAxis.w, b.zAxis.w, b.wAxis.w) + DEBUG_SPACER +
                DEBUG_HEADER
                );
        }

        /// <summary>
        /// Debug the referenced matrix to the UnityEngine console as a string. 
        /// </summary>
        public void DebugMatrix() =>
            Debug.Log(ToString());

        /// <summary>
        /// Convert a set of 4 floating point numbers to a pre-formated string for display. 
        /// </summary>
        /// <param name="x"> The X component to display. </param>
        /// <param name="y"> The Y component to display. </param>
        /// <param name="z"> The Z component to display. </param>
        /// <param name="w"> The W component to display. </param>
        /// <returns> String in the format "{  x, y, z, w } "</returns>
        string RowSTR(float x, float y, float z, float w) =>
           String.Format(DEBUG_ROW_FORMAT, new object[] { x, y, z, w });

        #endregion

        #region Operator Definitions. 

        public override bool Equals(object obj)
        {
            if ((Matrix4X4)obj == this)
                return true;
            if (obj is not Matrix4X4)
                return false;
            Matrix4X4 m = obj as Matrix4X4;
            return ((Vector4)xAxis == (Vector4)m.xAxis &&
                (Vector4)yAxis == (Vector4)m.yAxis &&
                (Vector4)zAxis == (Vector4)m.zAxis &&
                (Vector4)wAxis == (Vector4)m.wAxis);
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + (int)(xAxis.x + xAxis.y + xAxis.z + xAxis.w);
            result = 31 * result + (int)(yAxis.x + yAxis.y + yAxis.z + yAxis.w);
            result = 31 * result + (int)(zAxis.x + zAxis.y + zAxis.z + zAxis.w);
            result = 31 * result + (int)(wAxis.x + wAxis.y + wAxis.z + wAxis.w);
            return result;
        }

        public static Matrix4X4 operator *(Matrix4X4 lhs, Matrix4X4 rhs) =>
            Multiply(lhs, rhs);

        public static Vector4D operator *(Matrix4X4 m, Vector4 v) =>
            m.Multiply(new Vector4D(v.x, v.y, v.z, 1));

        public static Vector4 operator *(Matrix4X4 m, Vector4D v) =>
            (Vector4)m.Multiply(new Vector4D(v.x, v.y, v.z, v.w));

        public static bool operator ==(Matrix4X4 a, Matrix4X4 b) =>
            Equals(a, b);

        public static bool operator !=(Matrix4X4 a, Matrix4X4 b) =>
            !(a == b);
        #endregion
    }
}