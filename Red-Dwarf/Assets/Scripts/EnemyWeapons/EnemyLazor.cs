using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazor : MonoBehaviour {

    [Header("Weapon Settings")]
    public float hitscanWeaponRange = 1000f;
    public float lineVisibleTime = 0.1f;

    [Header("Particle Effects")]
    public GameObject hitscanHitEffect;

    private LineRenderer line;
    private bool isReady = false;
    public float damage = 1f;
    float time;
	float interval = 10f;
	public float spawnTime;

    // Use this for initialization
    void Awake()
    {
		spawnTime= Random.value*10 +interval;
        line = GetComponent<LineRenderer>();
        time = spawnTime;
    }


    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        
        if (time < 0)
        {
            print("shots fired");
            Fire();
            time = interval;
        }

    }

    public void Fire()
    {
        
     
            Vector3 hitPos;
            RaycastHit hit;
            Vector3 originPos = transform.position;
            Vector3 direction = transform.forward;
            Physics.Raycast(originPos, direction, out hit, hitscanWeaponRange);

            if (hit.collider != null)
            {
                Debug.Log("hit " + hit.collider.name);
                DoDamage(hit);
                //GameObject hitEffect = Instantiate(hitscanHitEffect, hit.point, hit.collider.transform.rotation);
               // Destroy(hitEffect, 1f);
                hitPos = hit.point;
            }

            else
            {
                hitPos = transform.forward * hitscanWeaponRange;
            }

            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitPos);
            StartCoroutine(lineVisible());
            
        
    }

    private void DoDamage(RaycastHit hit)
    {
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj.tag == "Player")
        {
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

    private IEnumerator lineVisible()
    {
        line.enabled = true;
        float readyTime = Time.time + lineVisibleTime;

        while (Time.time < readyTime)
        {
            yield return new WaitForEndOfFrame();
        }

        line.enabled = false;
    }


}

