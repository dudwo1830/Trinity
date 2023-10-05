using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public LayerMask targetMask;
    private LivingEntity targetEntity;

    private NavMeshAgent pathFinder;

    //public ParticleSystem hitEffect;
    public AudioClip deathClip;
    public AudioClip hitClip;

    private Animator enemyAnimator;
    private AudioSource enemyAudioSource;
    private Renderer enemyRenderer;

    private float damage = 20f;
    private float lastAttackTime;

    private bool HasTarget
    {
        get
        {
            return targetEntity != null && !targetEntity.Dead;
        }
    }

    private void Awake()
    {
        pathFinder = null; //GetComponent<NavMeshAgent>();
        enemyAnimator = null; //GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
        enemyRenderer = GetComponent<Renderer>();
    }
    public void Setup(float health, float damage, float speed, float attackRate)
    {
        startingHealth = health;
        this.damage = damage;
        //pathFinder.speed = speed;
    }

    private void Start()
    {
        //StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("HasTarget", HasTarget);
        }
    }

    private IEnumerator UpdatePath()
    {
        float updatePathInterval = 0.25f;
        float findRadius = 50f;

        while (!Dead)
        {
            if (HasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, findRadius, targetMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    var livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.Dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(updatePathInterval);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        var colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioSource.PlayOneShot(deathClip);
    }
}
