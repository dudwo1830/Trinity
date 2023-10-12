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
    public float Shield { get; private set; }

    public event Action onDeathEvent;

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (Shield > 0)
        {
            Debug.Log($"Shield: {Shield}");
            damage = OnDamageShield(damage);
            Debug.Log($"Real Damage: {damage}");
        }

        Health -= damage;
        if (Health <= 0 && !Dead)
        {
            Die();
        }
    }

    public virtual float OnDamageShield(float damage)
    {
        return Mathf.Clamp(Shield -= damage, 0, damage);
    }

    public virtual void OnHeal(float heal)
    {
        Health += heal;
    }

    public virtual void AddShield(float shield)
    {
        Shield += shield;
    }

    public virtual void ResetShield()
    {
        Shield = 0;
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
