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
    private float damage = 20f;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
    }
    public void Setup(float health, float damage, float speed, float attackRate)
    {
        startingHealth = health;
        this.damage = damage;

        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        healthSlider.value = Health;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"{Health}/{startingHealth}";
    }

    public override void Die()
    {
        Debug.Log("Enemy Die");
        base.Die();
        
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
}
