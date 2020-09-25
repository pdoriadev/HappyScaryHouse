using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Collector))]
public class Player : MonoBehaviour
{
    #region VARS
    public float moveSpeed;
    [SerializeField]
    private SpriteRenderer SR = default;
    [SerializeField]
    private GameObject[] HealthHearts = default;
    [SerializeField]
    private DamagerMono Weapon = default;
    [SerializeField]
    private GameObject DominantHand = default;
    [SerializeField]
    private InteractablesTracker Tracker = default;
    private Collector Collector;
    private DamageableMono DamageableMono;
    private Animator KidAnimator;
    private Pickup PickupInRange = null;
    private Rigidbody2D RB2D;
    private bool ShouldAttack = false;
    private bool ShouldInteract = false;
    private Vector2 MoveDir;
    private Vector3 DropPos;
    private float DirLastFacing;
    private Color NormalColor;

#endregion

    void Awake()
    {
        Collector = GetComponent<Collector>();
        RB2D = GetComponent<Rigidbody2D>();
        KidAnimator = GetComponentInChildren<Animator>();

    }
    void Start()
    {       
        DamageableMono = GetComponent<DamageableMono>();
        DamageableMono.onDamageMonoEvent += OnDamaged;
        DamageableMono.onDeathMonoEvent += OnDeath;
    }
    void OnDestroy()
    {
        DamageableMono.onDamageMonoEvent -= OnDamaged;
        DamageableMono.onDeathMonoEvent -= OnDeath;
    }
    void Update()
    {
        InputChecks();
        InteractChecks();

        if(MoveDir.x > 0)
            transform.localScale = -Vector2.right + Vector2.up;
        else if (MoveDir.x < 0 )
            transform.localScale = Vector2.right + Vector2.up;
        
        if (MoveDir.x != 0)
            DirLastFacing = MoveDir.x;
        if (MoveDir.y!= 0)
            DirLastFacing = MoveDir.y;

        KidAnimator.SetFloat("Movement", Mathf.Abs(MoveDir.x) + Mathf.Abs(MoveDir.y));

        DropPos = transform.position;
        DropPos = new Vector3(MoveDir.x + transform.position.y, MoveDir.y + transform.position.y, transform.position.z);

    }
    void FixedUpdate()
    {
        RB2D.velocity = MoveDir * moveSpeed * Time.fixedDeltaTime;
        if (ShouldAttack && Weapon != null)
        {
            Weapon.Attack();
        }
    }
    private void InputChecks()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.y = Input.GetAxisRaw("Vertical");
        ShouldAttack = Input.GetButtonDown("Fire1");
        if (Input.GetButtonDown("Interact"))
            ShouldInteract = true;
        else if (Input.GetButtonUp("Interact"))
            ShouldInteract = false;
    }
    private void InteractChecks()
    {
        if (ShouldInteract)
        {
            IInteractable inter = Tracker.GetNearestInteractable();
            if (inter != null)
            {
                inter.Interact();
            }
            else if (PickupInRange)
            {
                DamagerMono damager = PickupInRange.GetPickup().GetComponent<DamagerMono>();
                if (damager != null)
                {
                    PickupWeapon(damager);
                }
            }
            else if (PickupInRange == false && Weapon)
            {
                DropWeapon();
            }
        }
    }
    private void PickupWeapon(DamagerMono damager)
    {
        DropWeapon();
        Weapon = GameObject.Instantiate(damager, DominantHand.transform);
    }
    private void DropWeapon()
    {
        if (Weapon != null)
        {
            Pickup pToMatch = Weapon.damagerSO.pickup.GetComponent<Pickup>();
            Pickup p = pToMatch.pool.TakePooledPickup(pToMatch); 
            if (p)
            {
                p.transform.position = DropPos;
            }
            else
                GameObject.Instantiate(Weapon.damagerSO.pickup, DropPos, Quaternion.identity);

            Weapon.gameObject.SetActive(false);   
        }
        Weapon = null;
    }
    private void OnDamaged()
    {
        NormalColor = SR.color;
        SR.color = Color.blue;

        for (int i = HealthHearts.Length - 1; i >= 0; i--)
        {
            HealthHearts[i].GetComponent<Image>().color = Color.blue;
        }

        OnDamagedCo = StartCoroutine(CoOnDamaged());
    }
    private Coroutine OnDamagedCo;
    private IEnumerator CoOnDamaged()
    {
        float DamagedTime = 0;
        bool HeartFaded = false;
        while (true)
        {
            DamagedTime += Time.deltaTime;
            if (DamagedTime > 0.1f)
            {
                for (int i = HealthHearts.Length - 1; i >= 0 ; i--)
                {
                    Image heart = HealthHearts[i].GetComponent<Image>();
                    heart.color = NormalColor;
                    if (HeartFaded == false && heart.IsActive())
                    {
                        HeartFaded = true;
                        heart.enabled = false;
                    }   
                }
                SR.color = NormalColor;
                StopCoroutine(OnDamagedCo);
            }

            yield return null;
        }
    }
    private void OnDeath()
    {
        SceneManager.LoadScene(0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Pickup p = collider.GetComponent<Pickup>();
        if (p != null)
        {
            PickupInRange = p;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        Pickup p = collider.GetComponent<Pickup>();
        if (p != null)
        {
            if (p == PickupInRange)
            {
                PickupInRange = null;
            }
        }
    }


}
