using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[RequireComponent(typeof(Renderer))]
public class ApplyTiling : MonoBehaviour
{
    [Tooltip("The sliced sprite you want to render")]
    public Sprite sprite;
    
    [Header("Material Settings")]
    [Tooltip("Base material to use for auto-generated materials")]
    public Material baseMaterial;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;
    
    private Material instanceMaterial;
    private Renderer cachedRenderer;
    
    private void Start()
    {
        if (Application.isPlaying)
        {
            ApplyTilingSettings();
        }
    }
    
    private void OnValidate()
    {
        // Apply in editor when values change
        if (!Application.isPlaying)
        {
            // Delay the call to ensure components are initialized
#if UNITY_EDITOR
            EditorApplication.delayCall += () => {
                if (this != null) // Check if object still exists
                {
                    ApplyTilingSettings();
                }
            };
#endif
        }
    }
    
    private void ApplyTilingSettings()
    {
        if (sprite == null)
        {
            if (showDebugInfo)
                Debug.LogWarning("No sprite assigned!", this);
            return;
        }
        
        if (cachedRenderer == null)
            cachedRenderer = GetComponent<Renderer>();
        
        // Safety check - renderer might not be ready in editor
        if (cachedRenderer == null)
        {
            if (showDebugInfo)
                Debug.LogWarning("Renderer component not found or not ready!", this);
            return;
        }
        
        // Find or create appropriate material
        if (instanceMaterial == null)
        {
            instanceMaterial = FindOrCreateMaterial();
        }
        
        // Additional safety check
        if (instanceMaterial == null)
        {
            if (showDebugInfo)
                Debug.LogWarning("Failed to create or find material!", this);
            return;
        }
        
        // Set the base texture from the sprite
        instanceMaterial.mainTexture = sprite.texture;
        
        // Get texture dimensions
        Rect rect = sprite.textureRect;
        Texture tex = sprite.texture;
        
        if (tex == null)
        {
            Debug.LogError("Sprite texture is null!", this);
            return;
        }
        
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
        
        // Apply to material (your Shader Graph must use *Tiling and *Offset)
        instanceMaterial.SetVector("_Tiling", tiling);
        instanceMaterial.SetVector("_Offset", offset);
        
        // Apply the material to renderer
        cachedRenderer.sharedMaterial = instanceMaterial;
        
        if (showDebugInfo)
        {
            Debug.Log($"Applied tiling: {tiling}, offset: {offset} to {gameObject.name} with material: {instanceMaterial.name}");
        }
    }
    
    private Material FindOrCreateMaterial()
    {
        string materialName = GenerateMaterialName();
        
#if UNITY_EDITOR
        // First, try to find existing material
        Material existingMaterial = FindExistingMaterial(materialName);
        if (existingMaterial != null)
        {
            if (showDebugInfo)
                Debug.Log($"Found existing material: {existingMaterial.name}");
            return existingMaterial;
        }
        
        // If not found, create new material
        return CreateNewMaterial(materialName);
#else
        // In runtime, try to load from Resources
        Material runtimeMaterial = Resources.Load<Material>($"Materials/{materialName}");
        if (runtimeMaterial != null)
        {
            return runtimeMaterial;
        }
        
        // Fallback to creating instance material
        return CreateFallbackMaterial();
#endif
    }
    
    private string GenerateMaterialName()
    {
        string spriteName = sprite != null ? sprite.name : "NoSprite";
        string textureName = sprite != null && sprite.texture != null ? sprite.texture.name : "NoTexture";
        
        // Create unique name based on sprite and texture
        return $"AutoGen_{spriteName}_{textureName}";
    }
    
#if UNITY_EDITOR
    private Material FindExistingMaterial(string materialName)
    {
        string materialPath = $"Assets/Resources/Materials/{materialName}.mat";
        return AssetDatabase.LoadAssetAtPath<Material>(materialPath);
    }
    
    private Material CreateNewMaterial(string materialName)
    {
        // Ensure Resources/Materials folder exists
        string resourcesPath = "Assets/Resources";
        string materialsPath = "Assets/Resources/Materials";
        
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        
        if (!AssetDatabase.IsValidFolder(materialsPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Materials");
        }
        
        // Determine base material to use
        Material sourceMessage = DetermineBaseMaterial();
        if (sourceMessage == null)
        {
            Debug.LogError("No base material found! Please assign a base material or ensure renderer has a material.", this);
            return null;
        }
        
        // Create new material
        Material newMaterial = new Material(sourceMessage);
        newMaterial.name = materialName;
        
        // Save to Resources folder
        string materialPath = $"Assets/Resources/Materials/{materialName}.mat";
        AssetDatabase.CreateAsset(newMaterial, materialPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        if (showDebugInfo)
        {
            Debug.Log($"Created new material: {materialName} at {materialPath}");
        }
        
        return newMaterial;
    }
    
    private Material DetermineBaseMaterial()
    {
        // Priority order:
        // 1. Explicitly assigned base material
        // 2. Current renderer material
        // 3. Default material search
        
        if (baseMaterial != null)
        {
            return baseMaterial;
        }
        
        if (cachedRenderer != null && cachedRenderer.sharedMaterial != null)
        {
            return cachedRenderer.sharedMaterial;
        }
        
        // Try to find a default material
        string[] defaultMaterialNames = { "Default-Material", "Sprites-Default", "UI-Default" };
        
        foreach (string matName in defaultMaterialNames)
        {
            string[] guids = AssetDatabase.FindAssets($"t:Material {matName}");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                Material foundMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (foundMaterial != null)
                {
                    return foundMaterial;
                }
            }
        }
        
        return null;
    }
#endif
    
    private Material CreateFallbackMaterial()
    {
        // Runtime fallback - create instance material
        if (cachedRenderer == null)
            cachedRenderer = GetComponent<Renderer>();
        
        if (cachedRenderer != null && cachedRenderer.sharedMaterial != null)
        {
            Material fallback = new Material(cachedRenderer.sharedMaterial);
            fallback.name = cachedRenderer.sharedMaterial.name + " (Runtime Instance)";
            return fallback;
        }
        
        // Ultimate fallback - create with standard shader
        Material ultimateFallback = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        ultimateFallback.name = "Emergency Fallback Material";
        return ultimateFallback;
    }
    
    private void OnDestroy()
    {
        // Only clean up runtime instance materials, not Resources materials
        if (instanceMaterial != null && instanceMaterial.name.Contains("Runtime Instance"))
        {
            if (Application.isPlaying)
            {
                Destroy(instanceMaterial);
            }
#if UNITY_EDITOR
            else
            {
                DestroyImmediate(instanceMaterial);
            }
#endif
        }
    }
    
    // Public method to manually apply settings
    public void ApplySettings()
    {
        ApplyTilingSettings();
    }
    
    // Public method to force recreate material
    public void ForceRecreateMaterial()
    {
        instanceMaterial = null;
        ApplyTilingSettings();
    }
    
    // Public method to clean up and reset
    public void CleanupMaterial()
    {
        if (instanceMaterial != null && instanceMaterial.name.Contains("Runtime Instance"))
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Destroy(instanceMaterial);
            }
            else
            {
                DestroyImmediate(instanceMaterial);
            }
#endif
        }
        instanceMaterial = null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ApplyTiling))]
public class ApplyTilingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        ApplyTiling script = (ApplyTiling)target;
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Apply Tiling Settings"))
        {
            script.ApplySettings();
        }
        
        if (GUILayout.Button("Force Recreate Material"))
        {
            script.ForceRecreateMaterial();
        }
        
        if (GUILayout.Button("Cleanup Material"))
        {
            script.CleanupMaterial();
        }
        
        GUILayout.Space(5);
        
        // Show material info
        ApplyTiling applyTiling = (ApplyTiling)target;
        Renderer renderer = applyTiling.GetComponent<Renderer>();
        if (renderer != null && renderer.sharedMaterial != null)
        {
            EditorGUILayout.LabelField("Current Material:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Name: {renderer.sharedMaterial.name}");
            EditorGUILayout.LabelField($"Shader: {renderer.sharedMaterial.shader.name}");
        }
        
        if (script.sprite != null)
        {
            EditorGUILayout.LabelField("Sprite Info:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Texture: {script.sprite.texture?.name ?? "None"}");
            EditorGUILayout.LabelField($"Rect: {script.sprite.textureRect}");
            
            if (script.sprite.texture != null)
            {
                Rect rect = script.sprite.textureRect;
                Texture tex = script.sprite.texture;
                
                Vector2 tiling = new Vector2(
                    rect.width / tex.width,
                    rect.height / tex.height
                );
                
                Vector2 offset = new Vector2(
                    rect.x / tex.width,
                    rect.y / tex.height
                );
                
                EditorGUILayout.LabelField($"Calculated Tiling: {tiling}");
                EditorGUILayout.LabelField($"Calculated Offset: {offset}");
                
                string materialName = $"AutoGen_{script.sprite.name}_{script.sprite.texture.name}";
                EditorGUILayout.LabelField($"Material Name: {materialName}");
            }
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Open Resources/Materials Folder"))
        {
            string path = "Assets/Resources/Materials";
            if (AssetDatabase.IsValidFolder(path))
            {
                EditorUtility.RevealInFinder(path);
            }
            else
            {
                EditorUtility.DisplayDialog("Folder Not Found", "Resources/Materials folder doesn't exist yet. It will be created when you first apply tiling settings.", "OK");
            }
        }
    }
}
#endif