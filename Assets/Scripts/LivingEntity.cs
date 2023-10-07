using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; private set; }
    public float Defense { get; private set; }
    public float Damage { get; private set; }
    public float Strength { get; private set; }
    public bool Dead { get; private set; }

    public event Action onDeathEvent;

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;
        if (Health <= 0 && !Dead)
        {
            Die();
        }
        Debug.Log(Health);
    }

    public virtual void OnHeal(float heal)
    {
        Health += heal;
    }

    public virtual void Die()
    {
        if (onDeathEvent != null)
        {
            onDeathEvent();
        }

        Dead = true;
    }

    public virtual void Revive(float heal = 50f)
    {
        Dead = false;
        Health = heal;
    }
}
