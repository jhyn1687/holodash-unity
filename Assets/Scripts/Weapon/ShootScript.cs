    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private SpriteRenderer sr;


    [Header("Bullet Properties")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private BulletProperty bulletProps;
    [SerializeField] private Transform bulletContainer;

    [Header("Gun Properties")]
    [SerializeField] private GameObject gunSprite;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate;
    [SerializeField] private int numShots;


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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)this.transform.position;
        FaceMouse();
        if(direction.x < 0) 
        {
            sr.flipY = true; 
        } 
        else 
        {
            sr.flipY = false;
        }

        bool fire = Input.GetButton("Fire1");

        if(fire) {
            if(Time.time > ReadyForNextShot) {
                ReadyForNextShot = Time.time + 1/fireRate;
                Shoot();
            }
        }
    }

    void Reset() {
        bullet_sr = bullet.GetComponent<SpriteRenderer>();
        bullet_bs = bullet.GetComponent<BulletScript>();
        bullet_sr.sprite = bulletProps.bulletSprite;
        bullet_bs.bulletProps = this.bulletProps;
        sr = gunSprite.GetComponent<SpriteRenderer>();
        numShots = 1;
        foreach (Transform child in bulletContainer) {
            Destroy(child.gameObject);
        }
    }

    void FaceMouse()
    {
        this.transform.right = direction;
    }

    void Shoot(float angle) {
        GameObject BulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.Euler(0, 0, angle));
        BulletInstance.transform.SetParent(bulletContainer);
    }
    void Shoot()
    {
        float currentAim = shootPoint.rotation.eulerAngles.z;
        float minAim = currentAim + (1.5f * numShots);

        for (int i = 0; i < numShots; i++)
        {
            Shoot(((minAim - (i * 3f)) + 360) % 360);
        }
    }
    void OnAugmentPickup(int id) {
        switch (AugmentManager.GetName(id)) {
            case "Double Shot":
                numShots += 1;
                bullet_bs.bulletProps.DOTDamageMultiplier /= 2;
                break;
            case "Ricochet":
                bullet_bs.bulletProps.ricochets += 1;
                break;
            case "Fire Bullets":
                bullet_bs.bulletProps.DOTDamage += 5;
                break;
            default:
                break;
        }
    }

    void OnEnable() {
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
        GameManager.OnReset += Reset;
    }

    void OnDisable() {
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
        GameManager.OnReset -= Reset;
    }
}
