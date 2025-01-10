using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask targetLayers;

    private LivingEntity target;
    public float findTargetDistance = 10f;

    private Animator animator;
    private NavMeshAgent agent;
    private AudioSource audioSource;

    private Coroutine coUpdatePath;

    public ParticleSystem hitEffect;
    public AudioClip hitSound;
    public AudioClip deathSound;

    public float attakRate = 0.5f;
    public float lastAttackTime = 0;
    public float damage = 20f;

    private Renderer ren;

    public bool HasTarget
    {
        get
        {
            return target != null && !target.IsDead;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        ren = GetComponentInChildren<Renderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        var cols = GetComponents<Collider>();
        foreach (var col in cols)
        {
            col.enabled = true;
        }

        coUpdatePath = StartCoroutine(CoUpdatePath());
    }

    protected void OnDisable()
    {
        StopCoroutine(coUpdatePath);
        coUpdatePath = null;

        target = null;
    }

    public void Setup(EnemyData data)
    {
        maxHp = data.hp;
        damage = data.damage;
        agent.speed = data.speed;
    }

    private void Update()
    {
        if (isAttacking && HasTarget && lastAttackTime + attakRate < Time.time)
        {
            lastAttackTime = Time.time;
            if (!target.IsDead)
            {
                var hitPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
                var hitNormal = (target.transform.position - transform.position).normalized;
                target.OnDamage(damage, hitPoint, hitNormal);
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }

    private IEnumerator CoUpdatePath()
    {
        while (true)
        {
            if (!HasTarget)
            {
                agent.isStopped = true;
                target = FindTarget();
            }

            if (HasTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    public LivingEntity FindTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, findTargetDistance, targetLayers.value);
        foreach (var col in cols)
        {
            var livingEntity = col.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.IsDead)
                return livingEntity;
        }
        return null;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        audioSource.PlayOneShot(hitSound);
    }

    public override void Die()
    {
        base.Die();

        //audioSource.PlayOneShot(deathSound);
        animator.SetTrigger("Die");

        StopCoroutine(coUpdatePath);

        agent.isStopped = true;
        agent.enabled = false;
        isAttacking = false;

        var cols = GetComponents<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }
    }

    private bool isAttacking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDead && HasTarget && other.gameObject == target.gameObject)

        {
            isAttacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HasTarget && other.gameObject == target.gameObject)
        {
            isAttacking = false;
        }
    }
}
