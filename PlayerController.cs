using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.EventSystems;
using System;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] float moveSpeed = 3f;

    public bool stopMovement = true;
    public bool suppressNextClick = false;
    public Action OnArrival;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private int interactableLayerMask;
    private int groundLayerMask;
    private Interactable pendingInteraction;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        interactableLayerMask = LayerMask.GetMask("Interactable");
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

            if(direction.x > 0.5f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(direction.x < -0.5f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            anim.Play("Walk");

            if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
            {
                isMoving = false;
                rb.linearVelocity = Vector2.zero;

                anim.Play("Idle");

                OnArrival?.Invoke();
                OnArrival = null;

                if (pendingInteraction != null)
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
        else
        {
            anim.Play("Idle");
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        stopMovement = DialogueUI.Instance != null && DialogueUI.Instance.IsDialogueActive() ||
        InteractionUI.Instance != null && InteractionUI.Instance.IsPopupActive() ||
        InventoryUI.Instance != null && InventoryUI.Instance.IsInventoryActive();

        if (stopMovement)
        {
            return;
        }

        if(suppressNextClick)
        {
            suppressNextClick = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
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
                        targetPosition = interactable.walkToPoint != null
                            ? interactable.walkToPoint.position
                            : interactable.transform.position;
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

        if(Input.GetKey(KeyCode.Space))
        {
            foreach(var i in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
            {
                i.ShowIcon(true);
            }
        }
        else
        {
            foreach(var i in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
            {
                i.ShowIcon(false);
            }
        }
    }

    public void MoveToInteract(Interactable target)
    {
        pendingInteraction = target;
        targetPosition = target.walkToPoint != null ? target.walkToPoint.position : target.transform.position;
        isMoving = true;
    }
}
