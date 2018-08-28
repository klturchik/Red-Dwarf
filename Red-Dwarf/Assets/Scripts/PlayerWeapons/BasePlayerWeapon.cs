using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***************************************************************
* file: BasePlayerWeapon.cs
* author: Colin Trotter
* class: CS 470 – Game Development
*
* assignment: class project
* date last modified: 5/12/2017
*
* purpose: This class serves as a base class for all weapon scripts to inherit from.
*
****************************************************************/
public class BasePlayerWeapon : MonoBehaviour {

    public float cooldownTime;
    private bool ready = true;
    public float damage = 50f;
    private PlayerEvents playerEvents;

    protected virtual void Awake()
    {
        playerEvents = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEvents>();
        Debug.Log(playerEvents);
    }

    public virtual bool Fire()
    {
        return false;
    }

    public bool IsReady()
    {
        return ready;
    }

    //Coolsdown for default cooldown time
    protected IEnumerator Cooldown()
    {
        ready = false;
        playerEvents.WeaponReady(gameObject, ready);
        float readyTime = Time.time + cooldownTime;

        while (Time.time < readyTime)
        {
            yield return new WaitForEndOfFrame();
        }

        
        ready = true;
        playerEvents.WeaponReady(gameObject, ready);
    }

    //Cooldown for specified time
    protected IEnumerator Cooldown(float time)
    {
        ready = false;
        playerEvents.WeaponReady(gameObject, ready);
        float readyTime = Time.time + time;

        while (Time.time < readyTime)
        {
            yield return new WaitForEndOfFrame();
        }

        ready = true;
        playerEvents.WeaponReady(gameObject, ready);
    }

}
