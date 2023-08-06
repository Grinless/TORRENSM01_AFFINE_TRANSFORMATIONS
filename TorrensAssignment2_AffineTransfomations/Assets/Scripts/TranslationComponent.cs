using MatrixLib;
using static MatrixLib.Matrix2X2;

[System.Serializable]
public class TranslationComponent
{
    public Vector2D translation = new Vector2D(0, 0);

    public Matrix2X2 Matrix =>
        Matrix2X2.CreateTranslationMatrix(translation.x, translation.y);

    public TranslationComponent()
    {
    }

    public TranslationComponent(float x, float y)
    {
        this.translation = new Vector2D(x, y);
    }
}
