using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Property")]
public class BulletProperty : ScriptableObject {
	[Header("Bullet Properties")]
	public Sprite bulletSprite;
	public float damage;
	public float DOTDamage;
    [Range(0f, 2f)]
	public float DOTDamageMultiplier;
	public float DOTTime;
    [Range(0f, 2f)]
	public float DOTTimeMultiplier;
	public float bulletSpeed;
    public int ricochets;
}