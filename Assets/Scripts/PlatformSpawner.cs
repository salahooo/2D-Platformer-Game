using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;

    private GameObject currentPlatform;

    private void OnEnable()
    {
        PlayerLife.OnPlayerRespawn += SpawnPlatform;
    }

    private void OnDisable()
    {
        PlayerLife.OnPlayerRespawn -= SpawnPlatform;
    }

    private void Start()
    {
        SpawnPlatform();
    }

    public void SpawnPlatform()
    {
        // Destroy only if it exists (and hasn’t been destroyed already)
        if (currentPlatform != null)
        {
            Destroy(currentPlatform);
        }

        currentPlatform = Instantiate(platformPrefab, transform.position, Quaternion.identity);
    }
}
