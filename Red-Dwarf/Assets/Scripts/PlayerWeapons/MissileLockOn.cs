using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLockOn : MonoBehaviour {

    public WalkerControl enemyController;
    public Camera lockonCam;
    [HideInInspector]
    public Transform currentTarget;        //which enemy is currently being targeted
    public GameObject lockonUI;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        currentTarget = GetClosestTarget();
        PlaceLockonUI(currentTarget);
	}

    private Transform GetClosestTarget()
    {
        Transform closestTransform = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemyController.currentEnemies)
        {
            Vector3 viewPortPoint;
            if (enemy != null)
            {
                viewPortPoint = lockonCam.WorldToViewportPoint(enemy.transform.position);
                if (viewPortPoint.x >= .3 && viewPortPoint.x <= .7 && viewPortPoint.y >= .3 && viewPortPoint.y <= .7)
                {
                    RaycastHit hit;
                    viewPortPoint -= new Vector3(.5f, .5f, 0);
                    viewPortPoint.z = 0;
                    Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit, float.MaxValue);
                    if (viewPortPoint.magnitude < closestDist && hit.collider.gameObject == enemy)
                    {
                        //if this enemy is the closeset to the center and has line of sight
                        closestDist = viewPortPoint.magnitude;
                        closestTransform = enemy.transform;
                    }
                }
            }
        }

        return closestTransform;
    }

    private void PlaceLockonUI(Transform target)
    {
        if (currentTarget != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            lockonUI.SetActive(true);
            lockonUI.transform.position = target.position - dir * 5;
            lockonUI.transform.LookAt(transform.position);
        }
        else
        {
            lockonUI.SetActive(false);
        }
    }

}
