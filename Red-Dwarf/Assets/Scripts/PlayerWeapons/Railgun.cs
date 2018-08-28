using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : BasePlayerWeapon {

    [Header("Weapon Settings")]
    public float hitscanWeaponRange = 1000f;
    public float lineVisibleTime = 0.1f;
    public float aimAssistRadius = 10f;

    [Header("Particle Effects")]
    public GameObject hitscanHitEffect;

    private LineRenderer line;

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        line = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Fire()
    {
        base.Fire();
        if (IsReady())
        {
            Vector3 hitPos;
            StartCoroutine(Cooldown());
            RaycastHit hit;
            Vector3 originPos = transform.position;
            Vector3 direction = transform.forward;
            Physics.SphereCast(originPos, aimAssistRadius, direction, out hit, hitscanWeaponRange);

            if (hit.collider != null)
            {
                Debug.Log("hit " + hit.collider.name);
                DoDamage(hit);
                GameObject hitEffect = Instantiate(hitscanHitEffect, hit.point, hit.collider.transform.rotation);
                Destroy(hitEffect, 1f);
                hitPos = hit.point;
            }
            else
            {
                hitPos = transform.position + transform.forward * 1000;
            }

            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitPos);
            StartCoroutine(lineVisible());
            return true;
        }
        return false;
    }

    private void DoDamage(RaycastHit hit)
    {
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj.tag == "Enemy")
        {
            EnemyStatus status = hitObj.GetComponent<EnemyStatus>();
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
