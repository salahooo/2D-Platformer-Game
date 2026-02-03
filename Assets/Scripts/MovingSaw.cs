using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, moveDistance) - (moveDistance / 2);
        transform.position = startPos + new Vector3(movement, 0, 0); 
        // Change to (movement, 0, 0) for horizontal
    }
}
