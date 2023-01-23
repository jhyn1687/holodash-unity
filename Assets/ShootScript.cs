using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public Transform Gun;

    Vector2 direction;

    public GameObject Bullet;

    public float BulletSpeed;

    public Transform ShootPoint;

    public float fireRate;
    float ReadyForNextShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Gun.position;
        FaceMouse();

        if(Input.GetMouseButton(0)) {
            if(Time.time > ReadyForNextShot) {
                ReadyForNextShot = Time.time + 1/fireRate;
                Shoot();
            }
            Shoot();
        }
    }

    void FaceMouse()
    {
        Gun.transform.right = direction;
    }

    void Shoot() {
        GameObject BulletInstance = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        BulletInstance.GetComponent<Rigidbody2D>().AddForce(BulletInstance.transform.right * BulletSpeed);
        if (BulletInstance.tag == "Cell") 
         {
             Destroy(BulletInstance);
         }
    }
}
