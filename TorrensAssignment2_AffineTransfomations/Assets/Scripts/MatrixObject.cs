using MatrixLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MatrixObject : MonoBehaviour
{
    public TranslationComponent translation = new TranslationComponent();
    public ScaleComponent scale = new ScaleComponent();
    public RotationComponent rotation = new RotationComponent();
    public List<Point> pointList;

    public void Start()
    {
        //Set the inital points for each object. 
        foreach(Point point in pointList)
        {
            point.SetInitalState();
        }
    }

    private void Update()
    {
        //Update the points transformations. 
        foreach (Point item in pointList)
        {
            item.SetAffineTransformations(translation, rotation, scale);
        }
    }
}
