using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {

    public float health = 10f;
    public float scoreAmount = 10f;
    [Range(0f, 1f)]
    public float dropRate = 0.1f;
    public GameObject deathEffect;
    public GameObject dropItem;

    private PlayerEvents playerEvents;

    private void Start()
    {
        playerEvents = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEvents>();

    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        GameObject deathParticle = Instantiate(deathEffect, transform.position, transform.rotation);
        playerEvents.AddScore(scoreAmount);
        DropItem();
        Destroy(deathParticle, 2f);
        Destroy(gameObject);

    }

    private void DropItem()
    {
        if (Random.Range(0f, 1f) < dropRate && dropItem != null)
        {
            Debug.Log("Item drop!");
            GameObject drop = Instantiate(dropItem, transform.position, transform.rotation);
        }
    }
}
