using MatrixLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    public TranslationComponent translation = new TranslationComponent();
    public ScaleComponent scale = new ScaleComponent();
    public RotationComponent rotation = new RotationComponent();
    public PointObj point;

    public void Start() => point.SetInitalState(); 

    public void Update()
    {
        //Update the objects position.
        point.SetAffineTransformations(translation, rotation, scale);
    }
}
