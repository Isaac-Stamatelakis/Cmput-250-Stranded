using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blood Collection", menuName = "Enemy/Blood Collection")]
public class EnemyDeathBloodCollection : ScriptableObject
{
    public Sprite[] Sprites;

    public GameObject GetBlood()
    {
        Sprite randomSprite = Sprites[Random.Range(0, Sprites.Length)];
        GameObject blood = new GameObject();
        blood.name = randomSprite.name;
        blood.AddComponent<SpriteRenderer>().sprite = randomSprite;
        float randomZRotation = Random.Range(0f, 360f);
        blood.transform.Rotate(0,0,randomZRotation);
        return blood;
    }
}
