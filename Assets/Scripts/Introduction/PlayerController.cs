using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] Rigidbody2D rb;

    Vector2 lookDir;

    Animator anim;

    private void Start()
    {
        lookDir = new Vector2(0, -1);
        anim = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        HandleLookDir();

    }


    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontalInput * playerSpeed * Time.deltaTime, verticalInput * playerSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            lookDir = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lookDir = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lookDir = new Vector2(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lookDir = new Vector2(1, 0);
        }

        HandleLookDir();


    }

    void HandleLookDir()
    {
        if (lookDir == new Vector2(0, -1))
        {
            anim.SetBool("lookingFront", true);
            anim.SetBool("lookingBack", false);
            anim.SetBool("lookingRight", false);
            anim.SetBool("lookingLeft", false);
        }
        else if (lookDir == new Vector2(0, 1))
        {
            anim.SetBool("lookingFront", false);
            anim.SetBool("lookingBack", true);
            anim.SetBool("lookingRight", false);
            anim.SetBool("lookingLeft", false);
        }
        else if (lookDir == new Vector2(1, 0))
        {
            anim.SetBool("lookingFront", false);
            anim.SetBool("lookingBack", false);
            anim.SetBool("lookingRight", true);
            anim.SetBool("lookingLeft", false);
        }
        else if (lookDir == new Vector2(-1, 0))
        {
            anim.SetBool("lookingFront", false);
            anim.SetBool("lookingBack", false);
            anim.SetBool("lookingRight", false);
            anim.SetBool("lookingLeft", true);
        }
    }
}