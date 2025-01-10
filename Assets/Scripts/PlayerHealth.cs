using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip itemPickupSound;

    private AudioSource audioSource;
    private Animator animator;
    private PlayerMovement movement;
    private PlayerShooter shooter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        movement.enabled = true;
        shooter.enabled = true;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        if (!IsDead)
        {
            audioSource.PlayOneShot(hitSound);
        }

    }

    public override void Die()
    {
        base.Die();

        animator.SetTrigger("Die");

        audioSource.PlayOneShot(deathSound);

        movement.enabled = false;
        shooter.enabled = false;
    }

    public override void AddHp(float add)
    {
        base.AddHp(add);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnDamage(20, Vector3.zero, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddHp(20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDead)
            return;
    }
}
