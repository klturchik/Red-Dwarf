using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazePawns : MonoBehaviour {

    public float damage = 5;
    public GameObject deathEffect;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Debris")
        {
            Destroy(col.gameObject);
            GameObject deathParticle = Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(deathParticle, 2f);
            Destroy(transform.gameObject);
        }

        if(col.gameObject.tag == "Player")
        {
            print("Im dying!!!");
            GameObject deathParticle = Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(deathParticle, 2f);
            Destroy(transform.gameObject);
            GameObject hitObj = col.collider.gameObject;

            PlayerEvents status = hitObj.GetComponent<PlayerEvents>();

            if (status != null)
            {
                status.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Error: " + hitObj + " is missing EnemyStatus script");
            }
        }

    }
    }
