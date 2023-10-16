using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public Slider healthSlider;

    public ParticleSystem hitEffect;
    public AudioClip deathClip;
    public AudioClip hitClip;

    private Animator enemyAnimator;
    private AudioSource enemyAudioSource;

    public TextMeshProUGUI actionTextUI;
    private CardData action;

    List<float> hpList = new List<float>()
    {
        20, 30, 40, 50
    };

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
        Setup(hpList[Random.Range(0, hpList.Count)]);
    }

    public void Setup(float health, float damage = 0, float speed = 0, float attackRate = 0)
    {
        startingHealth = health;
        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
    }

    private void Start()
    {
        UpdateSlider();
        onDeathEvent += () => {
            BattleSystem.Instance.battleEnemyList.Remove(this);
            Debug.Log(BattleSystem.Instance.battleEnemyList.Count);
            Destroy(gameObject);
            if (BattleSystem.Instance.battleEnemyList.Count <= 0)
            {
                BattleSystem.Instance.Win();
            }
        };
    }

    private void Update()
    {

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        UpdateSlider();
    }
    

    public override void Die()
    {
        Debug.Log("Enemy Die");
        base.Die();
        
    }

    public void SetAction(CardData data)
    {
        action = data;
        SetActionText($"{action.Name} / {action.Amount}");
    }

    public void EnemyAction(LivingEntity target)
    {
        switch (action.Type)
        {
            case CardData.CardType.None:
                break;
            case CardData.CardType.Attack:
                //var damage = action.Amount;
                //if (target.HasConditionById(1))
                //{
                //    damage = target.GetConditionById(1).ApplyValue(damage);
                //}
                target.OnDamage(action.Amount, Vector3.zero, Vector3.zero);
                if (action.conditionInfo != null)
                {
                    foreach (var info in action.conditionInfo)
                    {
                        target.AddCondition(info.Key, info.Value);
                    }
                }
                break;
            case CardData.CardType.Skill:
                AddShield(action.Amount);
                break;
            case CardData.CardType.Heal:
                OnHealByValue(action.Amount);
                break;
            default:
                break;
        }
    }

    public void SetActionText(string text)
    {
        if (text == string.Empty || text == null)
        {
            actionTextUI.text = string.Empty;
        }
        else
        {
            actionTextUI.text = text;
        }
    }

    public void UpdateSlider()
    {
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }
}
