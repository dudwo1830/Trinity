using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.GraphicsBuffer;

public class Player : LivingEntity
{
    private Coroutine hitEffect;

    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;

    public TextMeshProUGUI coastText;
    private Animator playerAnimator;
    private AudioSource playerAudioSource;

    public int maxCoast { get; private set; } = 3;
    public int currentCoast { get; set; } = 0;


    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
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
        //if (!Dead)
        //{
        //    playerAudioSource.PlayOneShot(hitClip);
        //}
        base.OnDamage(damage, hitPoint, hitNormal);
        if (hitEffect != null)
        {
            StopCoroutine(hitEffect);
        }
        //hitEffect = StartCoroutine(HitEffect(1f));
        UpdateSlider();
    }

    public override void Die()
    {
        base.Die();
        //playerAudioSource.PlayOneShot(deathClip);
        //playerAnimator.SetTrigger("Die");
    }

    public override void Revive(float heal = 50f)
    {
        base.Revive(heal);
        UpdateSlider();
    }

    //IEnumerator HitEffect(float duration)
    //{
    //    image.enabled = true;
    //    float time = 0f;
    //    image.color = startColor;
    //    while (time < duration)
    //    {
    //        time += Time.deltaTime * 5f;
    //        image.color = Color.Lerp(endColor, startColor, time / duration);
    //        yield return null;
    //    }
    //    image.enabled = false;
    //    image.color = startColor;

    //    hitEffect = null;
    //}

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
        var enemy = entity.GetComponentInParent<Enemy>();
        var cardData = HandCard.Instance.selectedCard.cardData;
        switch (HandCard.Instance.selectedCard.cardData.Type)
        {
            case CardData.CardType.None:
                break;
            case CardData.CardType.Attack:
                if (enemy == null)
                {
                    return false;
                }
                var damage = cardData.Amount;
                if (HasConditionById(2))
                {
                    damage = GetConditionById(2).ApplyValue(damage);
                }
                enemy.OnDamage(damage, Vector3.zero, Vector3.zero);
                if (cardData.conditionInfo != null)
                {
                    foreach (var info in cardData.conditionInfo)
                    {
                        enemy.AddCondition(info.Key, info.Value);
                    }
                }
                break;
            case CardData.CardType.Skill:
                if (!(entity is LivingEntity))
                {
                    return false;
                }
                AddShield(cardData.Amount);
                break;
            case CardData.CardType.Heal:
                OnHealByValue(cardData.Amount);
                break;
            default:
                break;
        }
        currentCoast -= HandCard.Instance.selectedCard.cardData.Coast;
        UpdateCoastUI();
        return true;
    }
}
