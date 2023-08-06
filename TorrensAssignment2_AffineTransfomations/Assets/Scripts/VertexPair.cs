using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VertexPair : MonoBehaviour
{
    private const float _lineWidth = 0.1f;
    private LineRenderer _lRenderer;
    public Transform transformB;
    public Color lineColor;

    private Vector3 Pos1 => transform.position;

    private Vector3 Pos2 => transformB.position;

    public void Start() => SetUp();

    private void OnValidate() => SetUp(); //Called here to assist with in-engine setup. 

    public void Update() => SetPositions();

    private void SetUp()
    {
        //Set the LineRenderer up. 
        if (_lRenderer == null)
        {
            _lRenderer = GetComponent<LineRenderer>();
            _lRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lRenderer.startColor = _lRenderer.endColor = lineColor;
            _lRenderer.startWidth = _lRenderer.endWidth = _lineWidth;
            _lRenderer.positionCount = 2;
        }
        SetPositions(); //Set up vertex positions. 
    }

    //Set the positions of each object to the LineRenderer.
    private void SetPositions() => 
        _lRenderer.SetPositions(new Vector3[] { Pos1, Pos2 });
}
