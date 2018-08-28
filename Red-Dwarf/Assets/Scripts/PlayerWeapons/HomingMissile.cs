using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {

    public Transform target;
    public float rotationSpeed = 3f;
    public float missileSpeed = 50f;
    public float damage = 100f;
    public GameObject hitParticleEffect;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            RotateToTarget();
        }
        AddThrust();
        
	}

    private void RotateToTarget()
    {
        Vector3 localDist = target.position - transform.position;
        if (localDist.magnitude < 10f)
        {
            rotationSpeed = 100f;
        }
        transform.up = Vector3.Slerp(transform.up, localDist, Time.deltaTime * rotationSpeed);
    }

    private void AddThrust()
    {
        rb.velocity = transform.up * missileSpeed;
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject hit = col.collider.gameObject;
        if (hit != null && (hit.tag == "Enemy" || hit.tag == "Debris"))
        {
            EnemyStatus enemy = hit.GetComponent<EnemyStatus>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        if (hitParticleEffect != null)
        {
            GameObject hitEffect = Instantiate(hitParticleEffect, transform.position, transform.rotation);
            Destroy(hitEffect, 1f);
        }
        Destroy(gameObject);
    }
}
