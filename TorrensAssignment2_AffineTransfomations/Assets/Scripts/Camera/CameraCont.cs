using MatrixLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCont : MonoBehaviour
{
    public float zoom;
    public Vector2 position;
    public float rotation;

    private static CameraCont instance;

    public static CameraCont Instance => instance;
    public static Matrix2X2 WorldMatrix => instance.TranslationMatrix();

    public void Start()
    {
        instance = this;
    }

    public Matrix2X2 TranslationMatrix()
    {
        //S -> t
        return 
            (Matrix2X2.CreateTranslationMatrix(-position.x, -position.y) *
            Matrix2X2.CreateScaleMatrix(zoom, zoom)) * 
            Matrix2X2.CreateRotationMatrix(rotation); 
    }
}
