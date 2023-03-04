    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootScript : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool inGround;

    [Header("Bullet Properties")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private BulletProperty bulletProps;
    private Transform bulletContainer;

    [Header("Gun Properties")]
    [SerializeField] private GameObject gunSprite;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate;
    [SerializeField] private int numShots;

    private Animator ani;
    private EnemyBehavior eb;
    private BoxCollider2D coll;
    private EnemyBulletScript bullet_bs;
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
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(coll.bounds.center.x, coll.bounds.center.y), new Vector2(coll.bounds.size.x, coll.bounds.size.y), 0f, direction, 0f, LayerMask.GetMask("Ground"));
        inGround = (raycastHit2D.collider != null);
        if (eb.currentState == EnemyBehavior.EnemyState.Attack) {
            gunSprite.GetComponent<SpriteRenderer>().enabled = true;
            direction = eb.playerLastLocation - (Vector2)this.transform.position;
            FacePlayer();
        } else if (eb.currentState == EnemyBehavior.EnemyState.Alert) { 
            gunSprite.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            gunSprite.GetComponent<SpriteRenderer>().enabled = false;
            ReadyForNextShot = Time.time + 1 / fireRate;
        }
        if(ani.GetBool("Taking Damage"))
        {
            gunSprite.GetComponent<SpriteRenderer>().enabled = false;
        } else if (!inGround && eb.currentState == EnemyBehavior.EnemyState.Attack) {
            if (Time.time > ReadyForNextShot) {
                ReadyForNextShot = Time.time + 1/fireRate;
                Shoot();
            }
        }
    }

    void Reset() {
        sr = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponentInChildren<BoxCollider2D>();
        eb = GetComponentInParent<EnemyBehavior>();
        ani = GetComponentInParent<Animator>();
        bullet_sr = bullet.GetComponent<SpriteRenderer>();
        bullet_bs = bullet.GetComponent<EnemyBulletScript>();
        bullet_sr.sprite = bulletProps.bulletSprite;
        bullet_bs.bulletProps = Instantiate(this.bulletProps);
        if (bulletContainer != null) {
            foreach (Transform child in bulletContainer) {
                Destroy(child.gameObject);
            }
        } else if (GameObject.Find("Enemy Bullet Container") != null) {
            bulletContainer = GameObject.Find("Enemy Bullet Container").transform;
        } else {
            Debug.Log("Can't find enemy bullet container");
        }
        inGround = false;
    }

    void FacePlayer()
    {
        if (direction.x < 0) {
            this.transform.right = direction;
            sr.flipY = true;
            shootPoint.localPosition = new Vector2(shootPoint.localPosition.x, Mathf.Abs(shootPoint.localPosition.y) * -1);
        } else {
            this.transform.right = direction;
            sr.flipY = false;
            shootPoint.localPosition = new Vector2(shootPoint.localPosition.x, Mathf.Abs(shootPoint.localPosition.y));
        }
    }

    void Shoot(float angle) {
        GameObject BulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.Euler(0, 0, angle));
        BulletInstance.transform.SetParent(bulletContainer);
        
        Debug.DrawRay(shootPoint.position, Quaternion.Euler(0, 0, angle).eulerAngles.normalized * 10f, Color.blue, 2f);
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

    void OnEnable() {
        GameManager.OnReset += Reset;
    }

    void OnDisable() {
        GameManager.OnReset -= Reset;
    }
}
