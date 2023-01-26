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
                Shoot();
            }
        }
    }

    void FaceMouse()
    {
        this.transform.right = direction;
    }

    void Shoot() {
        GameObject BulletInstance = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
    }
}
