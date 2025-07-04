using UnityEngine;

[RequireComponent(typeof(Renderer)), ExecuteAlways]
public class ApplyTiling : MonoBehaviour
{
    [Tooltip("The sliced sprite you want to render")]
    public Sprite sprite;

    private void Start()
    {
        if (sprite == null)
        {
            Debug.LogError("No sprite assigned!");
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;

        // Set the base texture from the sprite
        material.mainTexture = sprite.texture;

        // Get texture dimensions
        Rect rect = sprite.textureRect;
        Texture tex = sprite.texture;

        // Calculate tiling (size of the sprite relative to the full texture)
        Vector2 tiling = new Vector2(
            rect.width / tex.width,
            rect.height / tex.height
        );

        // Calculate offset (where the sprite starts in the texture)
        Vector2 offset = new Vector2(
            rect.x / tex.width,
            rect.y / tex.height
        );

        // Apply to material (your Shader Graph must use _Tiling and _Offset)
        material.SetVector("_Tiling", tiling);
        material.SetVector("_Offset", offset);
    }
}
