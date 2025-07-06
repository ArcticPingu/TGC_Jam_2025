using Unity.VisualScripting;
using UnityEngine;

public class DirectionFacer : MonoBehaviour
{
    private Renderer objectRenderer;
    private Transform player;
    public bool rightAsDefault = true;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        player = FindAnyObjectByType<PlayerController>().transform;
        objectRenderer = GetComponent<SpriteRenderer>();

        InvokeRepeating(nameof(UpdateRotation), 1, 1);
    }

    void UpdateRotation()
    {
        if (!IsVisibleToCamera(objectRenderer, mainCamera))
        {
            bool playerIsLeft = player.position.x < transform.position.x;

            float yRotation = (playerIsLeft ? 180f : 0f) + (rightAsDefault ? 180f : 0f);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }

    bool IsVisibleToCamera(Renderer renderer, Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
