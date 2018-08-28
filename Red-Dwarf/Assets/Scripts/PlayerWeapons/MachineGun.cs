using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BasePlayerWeapon {

    [Header("Weapon Settings")]
    public float reloadTime = 2f;
    public int clipSize = 50;
    public float bulletSpeed = 100f;
    public float projectileLifetime;
    public bool requiresReload = true;
    public Transform bulletOrigin;
    public GameObject bulletPrefab;


    [Header("Particle Effects")]
    public GameObject hitParticleEffect;

    private int curClip;

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        curClip = clipSize;
    }

    public override bool Fire()
    {
        base.Fire();
        if (IsReady())
        {
            curClip--;
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            bulletRb.velocity = transform.forward * bulletSpeed;
            Destroy(bullet, projectileLifetime);

            if (curClip <= 0 && requiresReload)
            {
                Debug.Log("reloading");
                StartCoroutine(Cooldown(reloadTime));
                curClip = clipSize;
            }
            else
            {
                StartCoroutine(Cooldown());
            }

            return true;
        }
        return false;
    }
}
