using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************
* file: PlayerRadar.cs
* author: Kyle Turchik
* class: CS 470 – Game Development
*
* assignment: class project
* date last modified: 5/27/2017
*
* purpose: This class tracks objects tagged as "Enemy" relative 
*          to the player and displays them on the radar
*
****************************************************************/


public class PlayerRadar : MonoBehaviour
{

    //Variables
    public float maxDetectDist = 250; //Maximum distance radar will detect enemies
    public GameObject enemyIndicatorIcon;
    public GameObject debrisIndicatorIcon;
    public GameObject player;

    private SphereCollider detectZone;
    private float worldDistance;
    private float localDistance;
    private GameObject enemyObject;
    private GameObject enemyIndicator;
    private Dictionary<GameObject, GameObject> enemyList = new Dictionary<GameObject, GameObject>();


    // Use this for initialization
    void Start()
    {
        detectZone = transform.GetComponent<SphereCollider>();
        detectZone.radius = maxDetectDist; //scale trigger collider to maxDectDist
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //prevent the radar from rotating with the ship

        foreach (KeyValuePair<GameObject, GameObject> enemy in enemyList)
        {
            if (enemy.Key != null)
            {
                //distance from center of radar to edge is ~0.16
                worldDistance = Vector3.Distance(transform.position, enemy.Key.transform.position);  //get distance from player to enemy
                localDistance = (worldDistance / maxDetectDist) * 0.16f;  //rescale distance to local scale

                //gets a vector that points from the player's position to the target's.
                Vector3 heading = enemy.Key.transform.position - transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance; //this is the normalized direction.

                enemy.Value.transform.localPosition = Vector3.zero + (direction * localDistance);  //update enemy indicator position
            }
            //if enemy is destroyed also destroy indicator
            else
            {
                Destroy(enemyList[enemy.Key.gameObject]); //destroy enemy radar indicator
                enemyList.Remove(enemy.Key.gameObject); //remove enemy from dictionary, no longer being tracked
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyIndicator = Instantiate(enemyIndicatorIcon, transform.position, transform.rotation); //create enemy indicator
            enemyIndicator.transform.parent = transform;
            //for testing
            enemyIndicator.GetComponent<ShowEnemy>().trackedEnemy = other.gameObject;
            //for testing
            if (!enemyList.ContainsKey(other.gameObject))
            {
                enemyList.Add(other.gameObject, enemyIndicator);  //add to list reference to enemy object, reference to enemy radar indicator
            }
        }
        if (other.tag == "Debris")
        {
            enemyIndicator = Instantiate(debrisIndicatorIcon, transform.position, transform.rotation);  //create enemy indicator
            enemyIndicator.transform.parent = transform;
            //for testing
            enemyIndicator.GetComponent<ShowEnemy>().trackedEnemy = other.gameObject;
            //for testing
            if (!enemyList.ContainsKey(other.gameObject))
            {
                enemyList.Add(other.gameObject, enemyIndicator);  //add to list reference to enemy object, reference to enemy radar indicator
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Debris")
        {
            Destroy(enemyList[other.gameObject]); //destroy enemy radar indicator
            enemyList.Remove(other.gameObject); //remove enemy from dictionary, no longer being tracked
        }
    }
}