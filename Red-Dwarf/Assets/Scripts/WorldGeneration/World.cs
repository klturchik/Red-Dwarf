using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    /*
    public GameObject planet;
    public GameObject chunk;
    public GameObject[,,] chunks;
    public int chunkSize = 16;

    public byte[,,] data;
    public int nLat = 64;
    public int nLong = 64;
    public int radius = 64;
    public int core_radius = 16;
    private Vector3[,,] GeoCoord;

    // Use this for initialization
    void Start() {

        data = new byte[nLat, nLong, radius];

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    data[x, y, z] = 1;
                }
            }
        }



        chunks = new GameObject[nLat / chunkSize, nLong / chunkSize, radius / chunkSize];

        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                for (int z = 0; z < chunks.GetLength(2); z++)
                {
                    chunks[x, y, z] = Instantiate(chunk, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                    Chunks newChunkScript = chunks[x, y, z].GetComponent("Chunks") as Chunks;
                    newChunkScript.WorldGO = gameObject;
                    newChunkScript.chunkSize = chunkSize;
                    newChunkScript.ChunkX = x * chunkSize;
                    newChunkScript.ChunkY = y * chunkSize;
                    newChunkScript.ChunkZ = z * chunkSize;
                }
            }
        }
        
		
	}


    public byte Block(int x, int y, int z)
    {
        if (x >= nLat || x < 0 || y >= nLong || y < 0 || z >= radius || z < 0)
        {
            return (byte)1;
        }

        return data[x, y, z];
    }
	
	// Update is ca lled once per frame
	void Update () {
		
	}
    */
}
