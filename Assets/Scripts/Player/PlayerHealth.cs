using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Coroutine hitEffect;

    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;

    private Animator playerAnimator;
    private AudioSource playerAudioSource;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
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

    public override void OnHeal(float heal)
    {
        base.OnHeal(heal);
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

    public void UpdateSlider()
    {
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }
}
