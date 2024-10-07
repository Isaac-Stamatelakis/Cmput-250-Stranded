using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea = default; // The ruler or attack area

    private bool attacking = false;

    private float timeToAttack = 0.25f; // Duration for how long the ruler/attack area stays active
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false); // Disable attack area after time runs out
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(true); // Show the ruler or attack area
    }
}
