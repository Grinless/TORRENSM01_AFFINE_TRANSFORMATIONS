using MatrixLib;
using System.Runtime.InteropServices;
using static MatrixLib.Matrix2X2;

[System.Serializable]
public class ScaleComponent
{
    public Vector2D scale = new Vector2D(1, 1);

    public Matrix2X2 Matrix => GetScaleMat();

    private Matrix2X2 GetScaleMat() => Matrix2X2.CreateScaleMatrix(scale.x, scale.y);

    public ScaleComponent()
    {

    }

    public ScaleComponent(float x, float y)
    {
        this.scale.x = x;
        this.scale.y = y;
    }
}
