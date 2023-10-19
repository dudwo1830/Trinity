using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private CardData action;
    public GameObject actionGuide;
    public Sprite attackImage;
    public Sprite skillImage;

    public int enemyId;

    List<float> hpList = new List<float>()
    {
        20, 30, 40, 50
    };

    private EnemyData enemyData;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();

        //Setup(hpList[Random.Range(0, hpList.Count)]);
        Setup(DataTableManager.GetTable<EnemyTable>().GetDataById(enemyId));
    }

    public void Setup(float health, float damage = 0, float speed = 0, float attackRate = 0)
    {
        startingHealth = health;
        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
    }
    public void Setup(EnemyData data)
    {
        enemyData = data;
        startingHealth = enemyData.StartingHealth;
        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
    }

    private void Start()
    {
        UpdateSlider();
        onDeathEvent += () => {
            BattleSystem.Instance.battleEnemyList.Remove(this);
            Destroy(gameObject);
        };
    }

    private void Update()
    {
        //enemyAnimator.SetInteger("AnimState", 0);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        enemyAnimator.SetTrigger("Hurt");
        StartCoroutine(WaitForAnimation("Hurt"));
        base.OnDamage(damage, hitPoint, hitNormal);
        UpdateSlider();
    }
    

    public override void Die()
    {
        enemyAnimator.SetTrigger("Death");
        StartCoroutine(WaitForAnimation("Death"));
        base.Die();
    }

    public void SetAction(CardData data)
    {
        action = data;
        SetActionGuide();
    }

    public void SetAction()
    {
        var randomId = Random.Range(enemyData.useCardIdList.Min(), enemyData.useCardIdList.Max() + 1);
        action = DataTableManager.GetTable<CardTable>().GetDataById(randomId);
        SetActionGuide();
    }

    public void EnemyAction(LivingEntity target)
    {
        switch (action.Type)
        {
            case CardData.CardType.None:
                break;
            case CardData.CardType.Attack:
                var damage = action.Amount;
                if (HasConditionById(2))
                {
                    damage = target.GetConditionById(2).ApplyValue(damage);
                }
                enemyAnimator.SetTrigger("Attack");
                StartCoroutine(WaitForAnimation("Attack"));
                target.OnDamage(damage, Vector3.zero, Vector3.zero);
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

    public void SetActionGuide()
    {
        var actionText = action.Type switch
        {
            CardData.CardType.Attack => $"{action.Amount}",
            _ => string.Empty
        };
        var actionSprite = action.Type switch
        {
            CardData.CardType.Attack => attackImage,
            CardData.CardType.Skill => skillImage,
            _ => null
        };

        
        if (actionSprite == null)
        {
            actionGuide.SetActive(false);
        }
        else
        {
            actionGuide.GetComponent<Image>().sprite = actionSprite;
            actionGuide.GetComponentInChildren<TextMeshProUGUI>().text = actionText;
            actionGuide.SetActive(true);
        }
    }

    public void UpdateSlider()
    {
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }

    IEnumerator WaitForAnimation(string name)
    {
        while (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            yield return null;
        }
    }
}
