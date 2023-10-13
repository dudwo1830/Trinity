using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; private set; }
    public float Defense { get; private set; }
    public float Damage { get; private set; }
    public float Strength { get; private set; }
    public bool Dead { get; private set; }
    public float Shield { get; private set; }

    public Image shieldUI;

    public event Action onDeathEvent;

    public List<Condition> conditions = new List<Condition>();

    private void Awake()
    {
        shieldUI.gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        var realDamage = Shield -= damage;
        UpdateShieldText();

        if (realDamage > 0)
        {
            return;
        }

        Health -= -realDamage;
        if (Health <= 0 && !Dead)
        {
            Die();
        }
    }

    public virtual void OnHeal(float heal)
    {
        Health += heal;
    }

    public virtual void AddShield(float shield)
    {
        Shield += shield;
        UpdateShieldText();
    }

    public virtual void ResetShield()
    {
        Shield = 0;
        UpdateShieldText();
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

    public virtual void UpdateShieldText()
    {
        if (Shield > 0)
        {
            shieldUI.gameObject.SetActive(true);
            shieldUI.GetComponentInChildren<TextMeshProUGUI>().text = Shield.ToString();
        }
        else
        {
            Shield = 0;
            shieldUI.gameObject.SetActive(false);
        }
    }
}
