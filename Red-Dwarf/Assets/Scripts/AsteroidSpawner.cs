using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject[] asteroidPrefabs;

    public int numAsteroids = 100;

    public float minScale = 1f;
    public float maxScale = 10f;

    private Vector3 extents;


    // Use this for initialization
    void Start()
    {
        extents = GetComponent<BoxCollider>().bounds.extents;

        for (int i = 0; i < numAsteroids; i++)
        {
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        float x = Random.Range(-extents.x, extents.x);
        float y = Random.Range(-extents.y, extents.y);
        float z = Random.Range(-extents.z, extents.z);

        float scale = Random.Range(minScale, maxScale);

        Vector3 spawnPosition = transform.position + new Vector3(x, y, z);

        float xRot = Random.Range(-5, 5);
        float yRot = Random.Range(-5, 5);

        Quaternion spawnRotation = Quaternion.Euler(xRot, 0, yRot);

        GameObject asteroid = Instantiate(PickRandomAsteroid(), spawnPosition, spawnRotation);
        Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
        asteroid.transform.localScale = new Vector3(scale, scale, scale);
        asteroidRb.angularVelocity = new Vector3(xRot, 0, yRot);
    }

    private GameObject PickRandomAsteroid()
    {
        int index = Random.Range(0, asteroidPrefabs.Length);
        return asteroidPrefabs[index];
    }
}
