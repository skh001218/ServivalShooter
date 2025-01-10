using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;

    public float damage = 25f;

    public float fireRate = 0.12f;
    public float reloadTime = 1f;
}
