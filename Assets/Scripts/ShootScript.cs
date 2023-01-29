using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private GameObject gunSprite;
    [SerializeField] private GameObject bullet;
    [SerializeField] private BulletProperty bulletProps;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate;
    
    Vector2 direction;
    float ReadyForNextShot;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer bullet_sr = bullet.GetComponent<SpriteRenderer>();
        bullet_sr.sprite = bulletProps.bulletSprite;
        BulletScript bullet_bs = bullet.GetComponent<BulletScript>();
        bullet_bs.bulletProps = this.bulletProps;
        sr = gunSprite.GetComponent<SpriteRenderer>();
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
                HandleAugments();
            }
        }
    }

    void FaceMouse()
    {
        this.transform.right = direction;
    }

    void Shoot(float angle) {
        GameObject BulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.Euler(0, 0, angle));
    }

    void HandleAugments()
    {
        float numShots = 1;
        if (AugmentManager.Instance.hasAugment(AugmentManager.GetID(2)))
        {
            numShots += 1;
        }

        float currentAim = shootPoint.rotation.eulerAngles.z;
        float minAim = currentAim + (1.5f * numShots);

        for (int i = 0; i < numShots; i++)
        {
            Shoot(((minAim - (i * 3f)) + 360) % 360);
        }
    }
}
