using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider != null)
            {
                if(hit.collider.CompareTag("Interactable"))
                {
                    hit.collider.GetComponent<Interactable>().OnInteract();
                }
                else
                {
                    targetPosition = hit.point;
                    isMoving = true;
                }
            }
        }

        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }
    }
}
