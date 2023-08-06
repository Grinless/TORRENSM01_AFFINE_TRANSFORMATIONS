using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixLib;
using System;

[System.Serializable]
public struct ProjectionScale
{
    public float near; 
    public float far;
    public float left; 
    public float right;
    public float top;
    public float bottom;
}

[System.Serializable]
public struct ProjectionData
{
    public Vector3 position;
    public ProjectionScale scale;
    public Vector3 orientation; 
}

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public ProjectionData data;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Matrix4X4 projMat =
            Matrix4X4.GetOthographicProjection(data);
        Matrix4x4 translatedProjMat = new Matrix4x4(
            (Vector4)projMat.xAxis,
            (Vector4)projMat.yAxis,
            (Vector4)projMat.zAxis,
            (Vector4)projMat.wAxis
            );
        _camera.projectionMatrix = translatedProjMat;
    }
}
