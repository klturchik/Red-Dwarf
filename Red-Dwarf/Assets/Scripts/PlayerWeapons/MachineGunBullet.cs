using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : MonoBehaviour {

    public float damage = 5f;

	// Use this for initialization
	void Start () {
		
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
        Destroy(gameObject);
    }

}
