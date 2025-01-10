using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    public State GunState { get; private set; }

    public GunData data;

    public Transform firePosition;

    public float fireDistance = 50f;

    private AudioSource audioSource;
    private LineRenderer lineRenderer;
    public ParticleSystem muzzleEffect;

    private float lastFireTime;

    private int ammoRemain;
    private int magAmmo;

    //private UIManager uiManager;

    private void Start()
    {

    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        GunState = State.Ready;
        lastFireTime = 0f;

       /* if (uiManager == null)
        {
            var findGo = GameObject.FindWithTag("GameController");
            var gm = findGo.GetComponent<GameManager>();
            uiManager = gm.uiManager;
        }*/
    }

    public void Fire()
    {
        if (GunState == State.Ready && Time.time > lastFireTime + data.fireRate)
        {
            lastFireTime = Time.time;
            var endPos = firePosition.position + firePosition.forward * fireDistance;
            var ray = new Ray(firePosition.position, firePosition.forward);
            var hits = Physics.RaycastAll(ray, fireDistance);
            if (hits.Length > 0)
            {
                var hit = hits.OrderBy(x => x.distance).First();
                var damagalbe = hit.collider.GetComponent<IDamageable>();
                if (damagalbe != null)
                {
                    Debug.Log($"{hit.point}, {hit.normal}");
                    damagalbe.OnDamage(data.damage, hit.point, hit.normal);
                }
            }

            StartCoroutine(ShotEffect(endPos));
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPoint)
    {
        //audioSource.PlayOneShot(data.shotClip);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, hitPoint);

        muzzleEffect.Play();

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }
    private void OnDrawGizmos()
    {
        var hits = Physics.RaycastAll(firePosition.position, firePosition.forward, fireDistance);
        System.Array.Sort(hits, (x, y) => Vector3.Distance(transform.position, x.point).CompareTo(Vector3.Distance(transform.position, y.point)));
        if (hits.Length > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePosition.position, hits[0].point);
        }
    }
}
