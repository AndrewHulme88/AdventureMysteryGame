using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private int interactableLayerMask;
    private int groundLayerMask;
    private Interactable pendingInteraction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        interactableLayerMask = LayerMask.GetMask("Interactable");
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                rb.linearVelocity = Vector2.zero;

                if(pendingInteraction != null)
                {
                    pendingInteraction.Interact();
                    pendingInteraction = null;
                }
            }
            else
            {
                rb.MovePosition(newPosition);
            }
        }
    }

    private void Update()
    {
        if((DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueActive()) || (InteractionUI.Instance != null && InteractionUI.Instance.IsPopupActive()))
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D interactableHit = Physics2D.OverlapPoint(clickPos, interactableLayerMask);
            RaycastHit2D groundHit = Physics2D.Raycast(ray.origin, ray.direction, groundLayerMask);

            if (interactableHit != null && interactableHit.CompareTag("Interactable"))
            {
                Interactable interactable = interactableHit.GetComponent<Interactable>();

                if(interactable != null)
                {
                    if(interactable.requireWalk)
                    {
                        pendingInteraction = interactable;
                        targetPosition = interactable.transform.position;
                        isMoving = true;
                    }
                    else
                    {
                        interactable.Interact();
                    }
                }

                return;
            }

            if(groundHit.collider != null)
            {
                targetPosition = groundHit.point;
                isMoving = true;
                pendingInteraction = null;
            }
        }
    }
}
