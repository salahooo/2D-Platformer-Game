using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;     // How far it moves up/down
    public float speed = 2f;            // Movement speed

    private Vector3 startPos;
    private bool movingUp = true;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newY;

        if (movingUp)
        {
            newY = Mathf.MoveTowards(transform.position.y, startPos.y + moveDistance, speed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (Mathf.Approximately(newY, startPos.y + moveDistance))
                movingUp = false;
        }
        else
        {
            newY = Mathf.MoveTowards(transform.position.y, startPos.y - moveDistance, speed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (Mathf.Approximately(newY, startPos.y - moveDistance))
                movingUp = true;
        }
    }
}
