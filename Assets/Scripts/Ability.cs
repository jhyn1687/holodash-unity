using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Ability")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class Ability : ScriptableObject
{
	[Header("Ability")]
	public Sprite image;
	public float castTime;
	public float castCooldown;
	public KeyCode keyPress;
}