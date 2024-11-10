using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;
using Dialogue;
using System.Linq;
using Rooms;
using UnityEngine.SceneManagement;

public class DatePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip curedSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float followSpeed = 8f;
    public Material HighlightShader;
    private Material defaultShader;
    private bool isCollidingWithPlayer = false;
    public List<DialogObject> RandomDialogues;
    private List<DialogObject> selectableRandomDialogues;
    private Queue<DialogObject> unselectableRandomDialogues;
    public DialogObject cureDialog;

    public float boostSpeed = 20f;
    public float followDistance = 2f;
    public float stopDistance = 1f;
    public float maxDistance = 10f;

    private Vector2 playerOffset = new Vector2(0, -0.5f);

    private Vector2 lastPosition;
    private int stuckFrames = 0;
    private int maxStuckFrames = 60;

    private Animator animator;
    public bool isInCutscene = false;
    public bool isCured = false; // Boolean to track if DatePlayer is cured

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomDialogues(RandomDialogues);
        this.defaultShader = spriteRenderer.material;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isInCutscene || !Player.Instance.CanMove)
        {
            SetIdleAnimation();
            return;
        }

        if (!isCollidingWithPlayer)
        {
            FollowPlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FollowPlayer()
    {
        Transform playerTransform = Player.Instance.transform;
        PLAYER_MOVE_TEST playerMoveScript = Player.Instance.GetComponent<PLAYER_MOVE_TEST>();

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        float currentFollowSpeed = playerMoveScript.walkSpeed;

        if (distanceToPlayer > maxDistance)
        {
            currentFollowSpeed = boostSpeed;
        }
        Vector2 targetPosition = (Vector2)playerTransform.position + playerOffset;
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;

        if (distanceToPlayer > stopDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer);

            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Vector2 avoidanceDirection = Vector3.Cross(directionToPlayer, Vector3.forward).normalized;
                rb.MovePosition(rb.position + (directionToPlayer + avoidanceDirection * 0.5f) * currentFollowSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rb.MovePosition(rb.position + directionToPlayer * currentFollowSpeed * Time.fixedDeltaTime);
                stuckFrames = 0;
            }

            // Check if DatePlayer hasn't moved for too long
            if (Vector2.Distance(transform.position, lastPosition) < 0.05f) // Minimal movement detected
            {
                stuckFrames++;
                if (stuckFrames >= maxStuckFrames)
                {
                    // Force random movement if stuck for too long
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    rb.MovePosition(rb.position + randomDirection * boostSpeed * Time.fixedDeltaTime);
                    stuckFrames = 0; // Reset stuck frames after forcing movement
                }
            }
            else
            {
                stuckFrames = 0;
            }

            lastPosition = transform.position;

            // Setting animations based on isCured state
            ResetMovementAnimationBools();
            if (directionToPlayer.x < 0)
            {
                animator.SetBool(isCured ? "ndate_left" : "date_left", true);
            }
            else if (directionToPlayer.x > 0)
            {
                animator.SetBool(isCured ? "ndate_right" : "date_right", true);
            }
        }
        else
        {
            SetIdleAnimation();
            rb.velocity = Vector2.zero;
        }
    }

    private void SetIdleAnimation()
    {
        ResetMovementAnimationBools();

        Vector2 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;

        if (directionToPlayer.x < 0)
        {
            animator.SetBool(isCured ? "nface_left" : "face_left", true);
        }
        else if (directionToPlayer.x > 0)
        {
            animator.SetBool(isCured ? "nface_right" : "face_right", true);
        }
    }

    private void ResetMovementAnimationBools()
    {
        // Reset all movement-related animation parameters to avoid conflicts
        animator.SetBool("ndate_left", false);
        animator.SetBool("ndate_right", false);
        animator.SetBool("nface_left", false);
        animator.SetBool("nface_right", false);
        animator.SetBool("date_left", false);
        animator.SetBool("date_right", false);
        animator.SetBool("face_left", false);
        animator.SetBool("face_right", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    public IEnumerator Cure()
    {
        audioSource.PlayOneShot(curedSound);
        isCured = true;
        float delay = 0.1f;
        while (delay > 0) {
            animator.SetBool("isCured", true);
            yield return new WaitForSeconds(delay);
            animator.SetBool("isCured", false);
            yield return new WaitForSeconds(delay);
            delay -= 0.0025f;
        }
        animator.SetBool("isCured", true);
        yield return new WaitForSeconds(1f);
        DialogUIController.Instance.DisplayDialogue(cureDialog,goToEnd);
        
    }

    private void goToEnd() {
        SceneManager.LoadScene("EndingCutscene");
    }


    public void Talk()
    {
        if (DialogUIController.Instance.ShowingDialog)
        {
            return;
        }
        DialogObject dialogObject = cycleDialogues();
        DialogUIController.Instance.DisplayDialogue(dialogObject);
    }

    private DialogObject cycleDialogues()
    {
        if (RandomDialogues.Count == 0)
        {
            return null;
        }
        if (RandomDialogues.Count == 1)
        {
            return RandomDialogues[0];
        }
        int ran = UnityEngine.Random.Range(0, selectableRandomDialogues.Count);
        DialogObject randomlySelected = selectableRandomDialogues[ran];
        selectableRandomDialogues.RemoveAt(ran);
        unselectableRandomDialogues.Enqueue(randomlySelected);

        DialogObject newlySelectable = unselectableRandomDialogues.Dequeue();
        selectableRandomDialogues.Add(newlySelectable);
        return randomlySelected;
    }

    public void setHighlight(bool highlight)
    {
        if (spriteRenderer == null)
        {
            return;
        }
        spriteRenderer.material = highlight ? HighlightShader : defaultShader;
    }

    private void ShuffleList(List<DialogObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void SetRandomDialogues(List<DialogObject> dialogObjects)
    {
        this.RandomDialogues = dialogObjects;
        if (dialogObjects == null || dialogObjects.Count == 1)
        {
            return;
        }
        List<DialogObject> shuffledList = new List<DialogObject>(dialogObjects);
        ShuffleList(shuffledList);

        selectableRandomDialogues = new List<DialogObject>();
        unselectableRandomDialogues = new Queue<DialogObject>();
        for (int i = 0; i < shuffledList.Count; i++)
        {
            if (i % 2 == 0)
            {
                selectableRandomDialogues.Add(shuffledList[i]);
            }
            else
            {
                unselectableRandomDialogues.Enqueue(shuffledList[i]);
            }
        }
    }

    public void activeUpgrade(PlayerUpgrade playerUpgrade)
    {
        switch (playerUpgrade)
        {
            case PlayerUpgrade.DateAura:
                activateComponent<DateAuraUpgrade>();
                break;
            case PlayerUpgrade.DateHeal:
                activateComponent<DateHealUpgrade>();
                break;
            case PlayerUpgrade.DateAttack:
                activateComponent<DateAttackUpgrade>();
                break;
        }
    }

    private T activateComponent<T>() where T : MonoBehaviour
    {
        T component = GetComponentInChildren<T>(true);
        if (component == null)
        {
            Debug.LogWarning($"Date Player doesn't have component {typeof(T).Name}");
            return default(T);
        }
        component.gameObject.SetActive(true);
        return component;
    }
}
