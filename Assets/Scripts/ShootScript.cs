using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    [SerializeField] private Transform Gun;

    Vector2 direction;

    [SerializeField] private GameObject Bullet;

    [SerializeField] private BulletProperty bulletProps;

    [SerializeField] private Transform ShootPoint;

    [SerializeField] private float fireRate;
    float ReadyForNextShot;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = Bullet.GetComponent<SpriteRenderer>();
        sr.sprite = bulletProps.bulletSprite;
        BulletScript bs = Bullet.GetComponent<BulletScript>();
        bs.bulletProps = this.bulletProps;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Gun.position;
        FaceMouse();

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
        Gun.transform.right = direction;
    }

    void Shoot() {
        GameObject BulletInstance = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
    }
}
