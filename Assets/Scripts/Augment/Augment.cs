using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Augment")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class Augment : ScriptableObject
{

	[Header("Augment")]
	public int ID;
	public string description;
	public Sprite sprite;
}