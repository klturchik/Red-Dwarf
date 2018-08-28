using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour

{
    public GameObject planet;
    private GameObject core;
    private List<Vector3> newVerts = new List<Vector3>();
    private List<int> newTris = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(1, 0);
    private Vector2 tGrass = new Vector2(0, 1);

    private Mesh mesh;
    private MeshCollider col;

    public int nLat = 64;
    public int nLong = 64;
    public int radius = 64;
    public int core_radius = 16;


    const float _2PI = 2 * Mathf.PI;

    private int faceCount;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        //CubeTop(0, 0, 0, 0);
        UpdateMesh();


    }

    void Core()
    {
        core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        core.transform.parent = planet.transform;
        core.transform.position = new Vector3(planet.transform.position.x, planet.transform.position.y, planet.transform.position.z);
        core.transform.localScale = new Vector3(core_radius, core_radius, core_radius);
        core.name = "core";
    }

    

    void UpdateMesh()
    { 

    } 
}