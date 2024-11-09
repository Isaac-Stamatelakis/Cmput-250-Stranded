using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CinematicStep
{
    public Vector3 location;
    public float timeAtLocation;
}

public class CinematicController : MonoBehaviour
{
    [Header("Cinematic Settings")]
    public bool cinematicControlled = true;
    public List<CinematicStep> cinematicSteps;
    public float Speed = 2f;

    private int _cinematicIndex = 0;
    private float _cutsceneTimer = 0;
    private Animator animator;
    private Vector3 _priorPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _priorPosition = transform.position;
    }

    private void Update()
    {
        if (cinematicControlled)
        {
            HandleCinematicMovement();
            UpdateAnimation();
        }
    }

    private void HandleCinematicMovement()
    {
        if (_cinematicIndex < cinematicSteps.Count)
        {
            // Move to the next cinematic step location if not yet reached
            if ((transform.position - cinematicSteps[_cinematicIndex].location).magnitude > 0.005f)
            {
                Vector3 direction = (cinematicSteps[_cinematicIndex].location - transform.position).normalized;
                transform.position += direction * Time.deltaTime * Speed;
            }
            else
            {
                // Snap to the target location to avoid float inaccuracies
                transform.position = cinematicSteps[_cinematicIndex].location;

                // Check if the time spent at this location is complete
                if (_cutsceneTimer >= cinematicSteps[_cinematicIndex].timeAtLocation)
                {
                    _cinematicIndex++;
                    _cutsceneTimer = 0; // Reset the timer for the next step
                }
                else
                {
                    _cutsceneTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            // End of cinematic: turn off control and reset animation to idle
            cinematicControlled = false;
            animator.SetBool("IsMoving", false);
        }
    }

    private void UpdateAnimation()
    {
        // Determine if the character is moving
        bool isMoving = (transform.position - _priorPosition).magnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            Vector3 movementDirection = (transform.position - _priorPosition).normalized;

            // Set animator parameters based on movement direction
            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                // Horizontal movement
                animator.SetFloat("MoveX", movementDirection.x);
                animator.SetFloat("MoveY", 0);
            }
            else
            {
                // Vertical movement
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", movementDirection.y);
            }
        }
        else
        {
            // Set to idle when not moving
            animator.SetBool("IsMoving", false);
        }

        // Update prior position for the next frame
        _priorPosition = transform.position;
    }

    // Optional method to restart the cinematic sequence
    public void RestartCinematic()
    {
        _cinematicIndex = 0;
        _cutsceneTimer = 0;
        cinematicControlled = true;
        _priorPosition = transform.position;
    }
}
