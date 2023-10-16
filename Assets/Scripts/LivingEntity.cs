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

    public List<ConditionData> conditions = new List<ConditionData>();
    private void Awake()
    {
        shieldUI.gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = startingHealth;
        conditions = DataTableManager.GetTable<ConditionTable>().ToList();
        foreach (var item in conditions)
        {
            Debug.Log($"{item.Name} / {item.duration}");
        }
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (HasConditionById(1))
        {
            damage = GetConditionById(1).ApplyValue(damage);
        }
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

    public virtual void AddCondition(int id, int duration)
    {
        Debug.Log("상태이상");
        if (GetConditionById(id) != null)
        {
            GetConditionById(id).duration += duration;
            return;
        }

        var table = DataTableManager.GetTable<ConditionTable>();
        var condition = new ConditionData(table.GetDataById(id));
        condition.duration = duration;
        conditions.Add(condition);
    }

    public virtual void OnHealByValue(float heal)
    {
        Health += heal;
        Health = Mathf.Clamp(Health, 0, startingHealth);
    }
    public virtual void OnHealByRate(float healRate)
    {
        Health += (startingHealth * healRate);
        Health = Mathf.Clamp(Health, 0, startingHealth);
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

    public virtual void UpdateConditions()
    {
        conditions.ForEach(condition => condition.duration = Mathf.Clamp(--condition.duration, 0, 99));
    }

    public virtual bool HasConditionById(int id)
    {
        foreach (var item in conditions)
        {
            if (item.Id == id && item.duration > 0)
            {
                return true;
            }
        }
        return false;
    }
    public virtual ConditionData GetConditionById(int id)
    {
        foreach (var item in conditions)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }
    public virtual bool HasConditionByName(string name)
    {
        foreach (var item in conditions)
        {
            if (item.Name == name && item.duration > 0)
            {
                return true;
            }
        }
        return false;
    }
}
