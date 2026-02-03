using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    // The target the camera follows (the player)
    [SerializeField] private Transform target;

    // Smoothness of camera movement (lower = more smooth / slower)
    [SerializeField] private float smoothSpeed = 0.15f;

    // Offset so camera is slightly above the player and behind
    [SerializeField] private Vector3 offset = new Vector3(0, 1, -10);

    [Header("Camera Bounds")]
    // A BoxCollider2D that defines how far the camera can move
    public BoxCollider2D levelBounds;

    private Vector3 velocity = Vector3.zero;

    // Min/max camera movement values calculated from bounds
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // If this level has camera bounds assigned, calculate them
        if (levelBounds != null)
            CalculateBounds();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position
        Vector3 targetPos = target.position + offset;
        targetPos.z = -10; // Always keep camera at z = -10

        // Smoothly move camera toward target
        Vector3 smoothed = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothSpeed
        );

        smoothed.z = -10;

        // If bounds exist, limit camera so it can't move outside the map
        if (levelBounds != null)
        {
            float clampedX = Mathf.Clamp(smoothed.x, minX, maxX);
            float clampedY = Mathf.Clamp(smoothed.y, minY, maxY);
            smoothed = new Vector3(clampedX, clampedY, -10);
        }

        transform.position = smoothed;
    }

    private void CalculateBounds()
    {
        // Get full collider box of the level
        Bounds b = levelBounds.bounds;

        // Calculate the visible size of the camera
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        // Compute limits so the camera never shows outside the level
        minX = b.min.x + camWidth;
        maxX = b.max.x - camWidth;
        minY = b.min.y + camHeight;
        maxY = b.max.y - camHeight;
    }
}
