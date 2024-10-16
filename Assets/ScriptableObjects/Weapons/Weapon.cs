using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float range;
    public float attackTime;
    public Sprite artwork;
    public AudioClip swingSound;
    public AnimatorOverrideController weaponAnimatorOverride; 
}
