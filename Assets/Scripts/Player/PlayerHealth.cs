using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] PlayerDeathScreenUI playerDeathScreenUIPrefab;

    // Update is called once per frame
    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerDeathScreenUI playerDeathScreenUI = GameObject.Instantiate(playerDeathScreenUIPrefab);
        playerDeathScreenUI.transform.SetParent(GameObject.Find("Canvas").transform,false);
    }
}