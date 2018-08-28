using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour {

    public int radius;
    public int core_Diameter;
    private int core_radius;
    public int chunkSize;
    public int nLat;
    public int nLong;
    private int faceCount = 0;
    public GameObject planet;
    private GameObject core;

    private GameObject Chunk;
    private List<GameObject> Chunks;
    private List<Vector3> newVerts = new List<Vector3>();
    private List<int> newTris = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();
    //private List<Vector3> GeoCoord = new List<Vector3>();
    private Vector3[,,] GeoCoord;
    


    private byte[,,] data;

    private Mesh mesh;
    

    void Core()
    {
        core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        core.transform.parent = planet.transform;
        core.transform.position = new Vector3(planet.transform.position.x, planet.transform.position.y, planet.transform.position.z);
        core.transform.localScale = new Vector3(core_Diameter, core_Diameter, core_Diameter);
        core.name = "core";
    }

    void MeshBottom()
    {
        Debug.Log(GeoCoord.GetLength(2));

        for(int k = 0; k < GeoCoord.GetLength(2) - 1; k++)
        {
            for (int j = 0; j < GeoCoord.GetLength(1); j++)
            {
                for (int i = 0; i < GeoCoord.GetLength(0); i++)
                {
                    if (i < GeoCoord.GetLength(0) - 1 && j < GeoCoord.GetLength(1) - 1)
                    {
                        newVerts.Add(GeoCoord[i, j, k]);
                        newVerts.Add(GeoCoord[i + 1, j, k]);
                        newVerts.Add(GeoCoord[i + 1, j + 1, k]);
                        newVerts.Add(GeoCoord[i, j + 1, k]);
                    }
                    else if (i < GeoCoord.GetLength(0) - 1 && j == GeoCoord.GetLength(1) - 1)
                    {
                        newVerts.Add(GeoCoord[i, j, k]);
                        newVerts.Add(GeoCoord[i + 1, j, k]);
                        newVerts.Add(GeoCoord[i + 1, 0, k]);
                        newVerts.Add(GeoCoord[i, 0, k]);
                    }

                    else if (i == GeoCoord.GetLength(0) && j != GeoCoord.GetLength(1) - 1)
                    {
                        newVerts.Add(GeoCoord[i, j, k]);
                        newVerts.Add(GeoCoord[0, j, k]);
                        newVerts.Add(GeoCoord[0, j + 1, k]);
                        newVerts.Add(GeoCoord[i, j + 1, k]);
                    }

                    else
                    {
                        newVerts.Add(GeoCoord[i, j, k]);
                        newVerts.Add(GeoCoord[0, j, k]);
                        newVerts.Add(GeoCoord[0, 0, k]);
                        newVerts.Add(GeoCoord[i, 0, k]);
                    }


                    if (i < GeoCoord.GetLength(0) / 2)
                    {
                        newTris.Add(faceCount * 4);
                        newTris.Add(faceCount * 4 + 1);
                        newTris.Add(faceCount * 4 + 2);
                        newTris.Add(faceCount * 4);
                        newTris.Add(faceCount * 4 + 2);
                        newTris.Add(faceCount * 4 + 3);
                    }



                    else
                    {
                        newTris.Add(i * 4 + 2);
                        newTris.Add(i * 4 + 1);
                        newTris.Add(i * 4);
                        newTris.Add(i * 4 + 3);
                        newTris.Add(i * 4 + 2);
                        newTris.Add(i * 4);
                    }
                    faceCount++;




                }
            }
        }
    }

    void GeodesicCoordinates()
    {
        float _2Pi = Mathf.PI * 2;
        
        GeoCoord = new Vector3[nLat, nLong, radius - core_radius];
        for (int Rad = 0; Rad < GeoCoord.GetLength(2); Rad++)
        {
           for (int Long = 0; Long < GeoCoord.GetLength(1); Long++)
          {
            for (int Lat = 0; Lat < GeoCoord.GetLength(0); Lat++)
            {
              GeoCoord[Lat, Long, Rad] = new Vector3((core.transform.position.x + Rad + core_radius) *  Mathf.Cos(Long * (_2Pi / nLong)) * Mathf.Sin(Lat * (_2Pi / nLat)),
                (core.transform.position.y + Rad + core_radius) * Mathf.Cos(Lat * (_2Pi / nLat)),
                (core.transform.position.z + Rad + core_radius) * Mathf.Sin(Long * (_2Pi / nLong)) * Mathf.Sin(Lat * (_2Pi / nLat)));

                    GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    test.transform.position = GeoCoord[Lat, Long, Rad];
                    test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    

            }
        }
         }






         
         /*
        for (int Rad = 0; Rad < radius - core_radius; Rad++)
        {
            for (int Long = 0; Long < nLong; Long++)
            {
                for (int Lat = 0; Lat < nLat; Lat++)
                {
                    GeoCoord.Add(new Vector3((core.transform.position.x + Rad + core_radius) * Mathf.Cos(Long * (_2Pi / nLong)) * Mathf.Sin(Lat * (_2Pi / nLat)),
                 (core.transform.position.y + Rad + core_radius) * Mathf.Sin(Long * (_2Pi / nLong)) * Mathf.Sin(Lat * (_2Pi / nLat)),
                (core.transform.position.z + Rad + core_radius) * Mathf.Cos(Lat * (_2Pi / nLat))));
                }
            }
        }
        */

     



    }


    void UpdateMesh()
    {

        mesh.Clear();
        mesh.vertices = newVerts.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTris.ToArray();
        
        mesh.RecalculateNormals();

        //col.sharedMesh=null;
        //col.sharedMesh=mesh;

        newVerts.Clear();
        newUV.Clear();
        newTris.Clear();

        faceCount = 0;

        //faceCount = 0; //Fixed: Added this thanks to a bug pointed out by ratnushock!

    }


    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        core_radius = core_Diameter / 2;
    data = new byte[nLat, nLong, radius - core_radius];
    GeoCoord = new Vector3[nLat, nLong, radius - core_radius];
    Core();
    GeodesicCoordinates();

        MeshBottom();
        UpdateMesh();
}

// Update is called once per frame
void Update () {
		
	}
}

