using Codice.Client.Commands.TransformerRule;
using MatrixLib;

[System.Serializable]
public class RotationComponent
{
    public float orientation;

    public Matrix2X2 Matrix
    {
        get
        {
            CapRotation();
            return Matrix2X2.CreateRotationMatrix(orientation);
        }
    }

    public RotationComponent()
    {
        orientation = 0;
    }

    public RotationComponent(float angle)
    {
        orientation = angle;
    }

    private void CapRotation()
    {
        if (orientation > 360)
            orientation -= 360;
        if (orientation < 0)
            orientation += 360;
    }
}
