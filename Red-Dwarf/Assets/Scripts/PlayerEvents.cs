using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public delegate void DefaultEvent();
    public delegate void WeaponReadyEvent(GameObject weapon, bool value);
    public delegate void FloatArgEvent(float amount);
    public event DefaultEvent OnSpawn;
    public event DefaultEvent OnDie;
    public event FloatArgEvent OnTakeDamage;
    public event FloatArgEvent OnAddHealth;
    public event FloatArgEvent OnAddScore;
    public event WeaponReadyEvent OnWeaponReady;

    public void Spawn()
    {
        if (OnSpawn != null)
        {
            OnSpawn();
        }
    }

    public void Die()
    {
        if (OnDie != null)
        {
            OnDie();
        }
    }

    public void TakeDamage(float amount)
    {
        if (OnTakeDamage != null)
        {
            OnTakeDamage(amount);
        }
    }

    public void AddHealth(float amount)
    {
        if (OnAddHealth != null)
        {
            OnAddHealth(amount);
        }
    }

    public void AddScore(float amount)
    {
        if (OnAddScore != null)
        {
            OnAddScore(amount);
        }
    }

    public void WeaponReady(GameObject weapon, bool value)
    {
        if (OnWeaponReady != null)
        {
            OnWeaponReady(weapon, value);
        }
    }

}
