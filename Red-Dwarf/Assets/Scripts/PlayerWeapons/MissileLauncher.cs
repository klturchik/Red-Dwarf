using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : BasePlayerWeapon {

    [Header("Weapon Settings")]
    public float reloadTime = 2f;
    public float missileSpeed = 10f;
    public float projectileLifetime;
    public Transform missileOrigin;
    public GameObject missilePrefab;


    [Header("Particle Effects")]
    public GameObject hitParticleEffect;

    private MissileLockOn lockonScript;

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        lockonScript = GetComponent<MissileLockOn>();
    }

    public override bool Fire()
    {
        base.Fire();
        if (IsReady())
        {
            GameObject missile = Instantiate(missilePrefab, missileOrigin.position, missileOrigin.rotation);
            HomingMissile missileScript = missile.GetComponent<HomingMissile>();
            missileScript.target = lockonScript.currentTarget;

            Rigidbody bulletRb = missile.GetComponent<Rigidbody>();

            bulletRb.velocity = transform.forward * missileSpeed;
            Destroy(missile, projectileLifetime);

            StartCoroutine(Cooldown());

            return true;
        }
        return false;
    }


}
