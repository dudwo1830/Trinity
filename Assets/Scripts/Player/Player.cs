using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;

    public TextMeshProUGUI coastText;
    private Animator playerAnimator;

    private List<LivingEntity> targetList = new List<LivingEntity>();
    private CardData currentCardData;

    public int maxCoast { get; private set; } = 3;
    public int currentCoast { get; set; } = 0;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        startingHealth = 80;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
        UpdateSlider();

    }

    private void Update()
    {
        //playerAnimator.SetInteger("AnimState", 0);
    }

    public override void OnHealByValue(float heal)
    {
        base.OnHealByValue(heal);
        UpdateSlider();
    }
    public override void OnHealByRate(float healRate)
    {
        base.OnHealByRate(healRate);
        UpdateSlider();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        playerAnimator.SetTrigger("Hurt");
        base.OnDamage(damage, hitPoint, hitNormal);
        UpdateSlider();
    }

    public override void Die()
    {
        playerAnimator.SetTrigger("Death");
        base.Die();
    }

    public override void Revive(float heal = 50f)
    {
        base.Revive(heal);
        UpdateSlider();
    }

    public void RestartLevel()
    {
        playerAnimator.SetFloat("Move", 0f);
    }

    public void ResetCoast()
    {
        currentCoast = maxCoast;
        UpdateCoastUI();
    }

    public void UpdateSlider()
    {
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }

    public void UpdateCoastUI()
    {
        coastText.text = $"{currentCoast}/{maxCoast}";
    }

    public bool CanUseCard()
    {
        return HandCard.Instance.selectedCard != null && currentCoast >= HandCard.Instance.selectedCard.cardData.Coast;
    }

    public bool ActiveCard(LivingEntity entity)
    {
        targetList.Clear();
        var enemy = entity.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            targetList.Add(enemy);
        }
        currentCardData = HandCard.Instance.selectedCard.cardData;
        switch (HandCard.Instance.selectedCard.cardData.Type)
        {
            case CardData.CardType.None:
                break;
            case CardData.CardType.Attack:
                if (targetList.Count <= 0)
                {
                    return false;
                }
                playerAnimator.SetTrigger("Attack");
                StartCoroutine(WaitForAnimation("Attack"));
                break;
            case CardData.CardType.Skill:
                if (!(entity is LivingEntity))
                {
                    return false;
                }
                AddShield(currentCardData.Amount);
                break;
            default:
                break;
        }
        currentCoast -= HandCard.Instance.selectedCard.cardData.Coast;
        UpdateCoastUI();
        return true;
    }

    IEnumerator WaitForAnimation(string name)
    {
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            yield return null;
        }
        Attack();
    }

    public void Attack()
    {
        var damage = currentCardData.Amount;
        if (HasConditionById(2))
        {
            damage = GetConditionById(2).ApplyValue(damage);
        }
        foreach (var enemy in targetList)
        {
            enemy.OnDamage(damage, Vector3.zero, Vector3.zero);
            if (currentCardData.conditionInfo != null)
            {
                foreach (var info in currentCardData.conditionInfo)
                {
                    enemy.AddCondition(info.Key, info.Value);
                }
            }
        }
    }
}
