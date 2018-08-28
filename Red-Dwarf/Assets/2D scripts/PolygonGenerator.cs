using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour {

    //mesh stuff
    public List<Vector3> newVerts = new List<Vector3>();
    public List<int> newTris = new List<int>();
    public List<Vector2> newUVs = new List<Vector2>();

    public List<Vector3> colVerts = new List<Vector3>();
    public List<int> colTris = new List<int>();
        
    private Mesh mesh;
    private MeshCollider col;

    private int squareCount;
    private int colCount;

    public byte[,] blocks;

    //texture stuff
    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(0, 0);
    private Vector2 tGrass = new Vector2(0, 1);


    //update
    public bool update = false;



    

    void UpdateMesh()
    {
        
        //Mesh rendering stuff
        mesh.Clear();
        mesh.vertices = newVerts.ToArray();
        mesh.triangles = newTris.ToArray();
        mesh.uv = newUVs.ToArray();
        mesh.RecalculateNormals();


        squareCount = 0;
        newVerts.Clear();
        newTris.Clear();
        newUVs.Clear();

        //Mesh collider stuff
        Mesh newMesh = new Mesh();
        newMesh.vertices = colVerts.ToArray();
        newMesh.triangles = colTris.ToArray();
        col.sharedMesh = newMesh;

        colCount = 0;
        colVerts.Clear();
        colTris.Clear();
        
    }

    void GenSquare(int x, int y, Vector2 texture)
    {
        newVerts.Add(new Vector3(x, y, 0)); //top left
        newVerts.Add(new Vector3(x + 1, y, 0)); //top right
        newVerts.Add(new Vector3(x + 1, y - 1, 0)); //bottom right
        newVerts.Add(new Vector3(x, y - 1, 0)); //bottom left

        newTris.Add((squareCount * 4));
        newTris.Add((squareCount * 4) + 1);
        newTris.Add((squareCount * 4) + 3);
        newTris.Add((squareCount * 4) + 1);
        newTris.Add((squareCount * 4) + 2);
        newTris.Add((squareCount * 4) + 3);

        newUVs.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));    //top left
        newUVs.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit)); //top right
        newUVs.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y)); //bottom right
        newUVs.Add(new Vector2(tUnit * texture.x, tUnit * texture.y)); //bottom left

        squareCount++;
    }

    void GenTerrain()
    {
        blocks = new byte[96, 128];


        for (int px = 0; px < blocks.GetLength(0); px++)
        {

            int stone = Noise(px, 0, 80, 15, 1);
            stone += Noise(px, 0, 50, 30, 1);
            stone += Noise(px, 0, 10, 10, 1);
            stone += 75;

            print(stone);

            int dirt = Noise(px, 0, 100, 35, 1);
            dirt += Noise(px, 0, 50, 30, 1);
            dirt += 75;
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;

                    //swathes of dirt
                    if (Noise(px, py, 12, 16, 1) > 10)
                    {
                        blocks[px, py] = 2;
                    }

                    //caves
                    if (Noise(px, py * 2, 16, 14, 1) > 10)
                    {
                        blocks[px, py] = 0;
                    }
                }
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
                }
                
            }
        }
    }

    void GenCollider(int x, int y)
    {
        //top
        if (Block(x, y + 1) == 0)
        {
            colVerts.Add(new Vector3(x, y, 1)); //top left
            colVerts.Add(new Vector3(x + 1, y, 1)); //top right
            colVerts.Add(new Vector3(x + 1, y, 0)); //bottom right
            colVerts.Add(new Vector3(x, y, 0)); //bottom left


            ColliderTriangles();

            colCount++;
        }

        //bottom
        if (Block(x, y - 1) == 0)
        {
            colVerts.Add(new Vector3(x, y - 1, 0)); //top left
            colVerts.Add(new Vector3(x + 1, y - 1, 0)); //top right
            colVerts.Add(new Vector3(x + 1, y - 1, 1)); //bottom right
            colVerts.Add(new Vector3(x, y - 1, 1)); //bottom left

            ColliderTriangles();

            colCount++;
        }

        //left
        if (Block(x - 1, y) == 0)
        {
            colVerts.Add(new Vector3(x, y - 1, 1)); //top left
            colVerts.Add(new Vector3(x, y, 1)); //top right
            colVerts.Add(new Vector3(x, y, 0)); //bottom right
            colVerts.Add(new Vector3(x, y - 1, 0)); //bottom left

            ColliderTriangles();

            colCount++;
        }

        //right
        if (Block(x + 1, y) == 0)
        {
            colVerts.Add(new Vector3(x + 1, y, 1)); //top left
            colVerts.Add(new Vector3(x + 1, y - 1, 1)); //top right
            colVerts.Add(new Vector3(x + 1, y - 1, 0)); //bottom right
            colVerts.Add(new Vector3(x + 1, y, 0)); //bottom left

            ColliderTriangles();

            colCount++;
        }
    }


    void ColliderTriangles()
    {
        colTris.Add(colCount * 4);
        colTris.Add((colCount * 4) + 1);
        colTris.Add((colCount * 4) + 3);
        colTris.Add((colCount * 4) + 1);
        colTris.Add((colCount * 4) + 2);
        colTris.Add((colCount * 4) + 3);
    }

    void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); px++)
        {
            for (int py = 0; py < blocks.GetLength(1); py++)
            {
                //if block is not air
                if (blocks[px, py] != 0)
                {
                    GenCollider(px, py);

                    if (blocks[px, py] == 1)
                    {
                        GenSquare(px, py, tStone);
                    }
                    else if (blocks[px, py] == 2)
                    {
                        GenSquare(px, py, tGrass);
                    }
                }
            }
        }
    }

    byte Block(int x, int y)
    {
        if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte)1;
        }

        return blocks[x, y];
    }

    int Noise(int x, int y, float scale, float magnitude, float exponent)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * magnitude), (exponent)));
    }


    // Use this for initialization
    void Start () {

  

        //mesh stuff

        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenTerrain();
        BuildMesh();
        UpdateMesh();

        

     






      


    }
	
	// Update is called once per frame
	void Update () {
        if (update)
        {
            BuildMesh();
            UpdateMesh();
            update = false;
        }
   

    }


}
