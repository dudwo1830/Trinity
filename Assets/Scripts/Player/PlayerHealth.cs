using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Coroutine hitEffect;

    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;

    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private AudioSource playerAudioSource;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = Health;

        playerMovement.enabled = true;
    }

    private void Update()
    {
        healthSlider.value = Health;
    }

    public override void OnHeal(float heal)
    {
        base.OnHeal(heal);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!Dead)
        {
            playerAudioSource.PlayOneShot(hitClip);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = Health;
        if (hitEffect != null)
        {
            StopCoroutine(hitEffect);
        }
        //hitEffect = StartCoroutine(HitEffect(1f));
    }

    public override void Die()
    {
        base.Die();

        playerAudioSource.PlayOneShot(deathClip);

        //playerAnimator.SetTrigger("Die");
        playerMovement.enabled = false;
    }

    public override void Revive(float heal = 50f)
    {
        base.Revive(heal);
        healthSlider.value = Health;
        playerMovement.enabled = true; 
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
}
