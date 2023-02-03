using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Property")]
public class BulletProperty : ScriptableObject {
	[Header("Bullet Properties")]
	public Sprite bulletSprite;
	public float damage;
	public float DOTDamage;
	public float DOTTime;
	public float bulletSpeed;
    public int ricochets;
}