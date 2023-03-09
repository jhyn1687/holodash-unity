    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool inGround;

    [Header("Bullet Properties")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private BulletProperty bulletProps;
    [SerializeField] private Transform bulletContainer;

    [Header("Gun Properties")]
    [SerializeField] private GameObject gunSprite;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate;
    [SerializeField] private int numShots;
    [SerializeField] private int burstFire = 0;

    private Animator ani;
    private BoxCollider2D coll;
    private BulletScript bullet_bs;
    private SpriteRenderer bullet_sr;
    Vector2 direction;
    float ReadyForNextShot;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) {
            return;
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)this.transform.position;
        FaceMouse();
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(coll.bounds.center.x, coll.bounds.center.y), new Vector2(coll.bounds.size.x, coll.bounds.size.y), 0f, direction, 0f, LayerMask.GetMask("Ground"));
        inGround = (raycastHit2D.collider != null);
        
        bool fire = Input.GetButton("Fire1");
        
        if(ani.GetBool("Taking Damage"))
        {
            gunSprite.GetComponent<SpriteRenderer>().enabled = false;
        } 
        else {
            gunSprite.GetComponent<SpriteRenderer>().enabled = true;
            if (!inGround && fire)
            {
                if (Time.time > ReadyForNextShot)
                {
                    ReadyForNextShot = Time.time + 1 / fireRate;
                    Shoot();
                }
            }
        }
    }

    void Reset()
    {
        ani = GetComponentInParent<Animator>();
        coll = GetComponentInChildren<BoxCollider2D>();
        bullet_sr = bullet.GetComponent<SpriteRenderer>();
        bullet_bs = bullet.GetComponent<BulletScript>();
        bullet_sr.sprite = bulletProps.bulletSprite;
        bullet_bs.bulletProps = Instantiate(this.bulletProps);
        inGround = false;
        sr = gunSprite.GetComponent<SpriteRenderer>();
        if (GameObject.Find("Bullet Container") != null) {
            GameObject bulletContainer = GameObject.Find("Bullet Container");
        }
        numShots = 1;
        fireRate = 2;
        burstFire = 0;
        foreach (Transform child in bulletContainer) {
            Destroy(child.gameObject);
        }
    }

    void FaceMouse()
    {
        if (direction.x <= 0) {
            this.transform.rotation = Quaternion.Euler(0, 180, Vector2.Angle(Vector2.down, direction) - 90);
        } else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.down, direction) - 90);
        }
    }

    IEnumerator BurstFire(float angle) {
        for (int i = 0; i < burstFire; i++) {
            yield return new WaitForSeconds(0.1f/burstFire);
            Shoot(angle);
        }
    }
    void Shoot(float angle) {
        GameObject BulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, angle));
        BulletInstance.transform.SetParent(bulletContainer);
    }
    void Shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)shootPoint.position;
        float currentAim = shootPoint.rotation.eulerAngles.z;
        float minAim = currentAim + Mathf.Min((1.5f * (numShots - 1)), 15f);
        float maxAim = currentAim - Mathf.Min((1.5f * (numShots - 1)), 15f);
        for (int i = 0; i < numShots; i++)
        {
            Shoot(((minAim - (i * (minAim - maxAim) / numShots)) + 360) % 360);
        }
        StartCoroutine(BurstFire(currentAim));
    }
    void OnAugmentPickup(int id) {
        switch (AugmentManager.GetName(id)) {
            case "Splinter Shot":
                numShots += 1;
                bullet_bs.bulletProps.DamageMultiplier *= .8f;
                bullet_bs.bulletProps.DOTDamageMultiplier *= .5f;
                break;
            case "Ricochet":
                bullet_bs.bulletProps.ricochets += 1;
                bullet_bs.bulletProps.lifetime += 1;
                break;
            case "Fire Bullets":
                bullet_bs.bulletProps.DOTDamage += 5;
                break;
            case "Electric Bullets":
                bullet_bs.bulletProps.DOTDamage += 1;
                bullet_bs.bulletProps.DOTDamageMultiplier += .2f;
                break;
            case "Infused Bullets":
                bullet_bs.bulletProps.damage += 4;
                break;
            case "Quick Trigger":
                fireRate += 1;
                break;
            case "Velocity Boost":
                bullet_bs.bulletProps.bulletSpeed += 5;
                break;
            case "Burst Fire":
                burstFire += 1;
                break;
            case "Sawed Off":
                numShots = 15;
                fireRate = .5f;
                bullet_bs.bulletProps.LifetimeMultiplier = .5f;
                break;
            case "Symbiosis":
                bullet_bs.bulletProps.lifesteal += 1;
                bullet_bs.bulletProps.lifestealChance += .05f;
                break;
            case "One Shot One Kill":
                bullet_bs.bulletProps.DamageMultiplier = 3f;
                fireRate = .5f;
                numShots = 1;
                break;
            case "Rapid Toxin":
                bullet_bs.bulletProps.DOTTimeMultiplier *= .9f;
                break;
            case "Toxic Shock":
                bullet_bs.bulletProps.DOTTime -= 1f;
                break;
            case "Penetrating Rounds":
                bullet_bs.bulletProps.damage += 2;
                break;
            case "Everlasting Rounds":
                bullet_bs.bulletProps.lifetime += .5f;
                break;
            case "Glass Cannon":
                bullet_bs.bulletProps.DamageMultiplier += .5f;
                bullet_bs.bulletProps.DOTDamageMultiplier += .25f;
                break;
            default:
                break;
        }
    }

    void OnAugmentReset() {
        bullet_bs.bulletProps = Instantiate(this.bulletProps);
        bullet_bs.bulletProps.bulletSpeed += 5 * GameManager.Instance.speedUpgrade;
        bullet_bs.bulletProps.damage += 2 * GameManager.Instance.speedUpgrade;
        numShots = 1;
        fireRate = 2;
        burstFire = 0;
    }

    void OnDataLoaded() {
        bullet_bs.bulletProps = Instantiate(this.bulletProps);
        bullet_bs.bulletProps.bulletSpeed += 5 * GameManager.Instance.speedUpgrade;
        bullet_bs.bulletProps.damage += 2 * GameManager.Instance.speedUpgrade;
    }


    void OnEnable() {
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
        AugmentManager.OnAugmentReset += OnAugmentReset;
        GameManager.OnDataLoaded += OnDataLoaded;
        GameManager.OnReset += Reset;
    }

    void OnDisable() {
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
        AugmentManager.OnAugmentReset -= OnAugmentReset;
        GameManager.OnDataLoaded -= OnDataLoaded;
        GameManager.OnReset -= Reset;
    }
}
