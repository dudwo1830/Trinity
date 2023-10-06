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
    private Renderer enemyRenderer;

    private float damage = 20f;
    private float lastAttackTime;

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
        enemyRenderer = GetComponent<Renderer>();
    }
    public void Setup(float health, float damage, float speed, float attackRate)
    {
        startingHealth = health;
        this.damage = damage;

        healthSlider.minValue = 0f;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = Health;
        //pathFinder.speed = speed;
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
    }

    public override void Die()
    {
        Debug.Log("Enemy Die");
        base.Die();
        
        enemyAnimator?.SetTrigger("Die");
        enemyAudioSource?.PlayOneShot(deathClip);
    }
}
