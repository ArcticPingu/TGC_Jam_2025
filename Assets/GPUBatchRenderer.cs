using System.Collections.Generic;
using UnityEngine;

public class GPUBatchRenderer : MonoBehaviour
{
    [System.Serializable]
    public class InstanceGroup
    {
        public Mesh mesh;
        public Material material;
        public List<Matrix4x4> matrices = new List<Matrix4x4>();
    }

    [Header("Settings")]
    public bool convertOnStart = true;
    public bool debugLog = false;
    
    private List<InstanceGroup> instanceGroups = new List<InstanceGroup>();
    private const int MAX_INSTANCES_PER_BATCH = 1023; // Unity's limit for DrawMeshInstanced
    
    void Start()
    {
        if (convertOnStart)
        {
            ConvertChildrenToInstances();
        }
    }
    
    [ContextMenu("Convert Children to GPU Instances")]
    public void ConvertChildrenToInstances()
    {
        // Clear existing groups
        instanceGroups.Clear();
        
        // Dictionary to group objects by mesh+material combination
        Dictionary<string, InstanceGroup> groupMap = new Dictionary<string, InstanceGroup>();
        
        // Get all child renderers
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
        
        if (debugLog)
            Debug.Log($"Found {childRenderers.Length} child renderers to convert");
        
        foreach (MeshRenderer renderer in childRenderers)
        {
            // Skip if this is the parent object
            if (renderer.gameObject == this.gameObject)
                continue;
                
            MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                continue;
            
            // Create unique key for mesh+material combination
            string key = $"{meshFilter.sharedMesh.name}_{renderer.sharedMaterial.name}";
            
            // Get or create group for this mesh+material combination
            if (!groupMap.ContainsKey(key))
            {
                groupMap[key] = new InstanceGroup
                {
                    mesh = meshFilter.sharedMesh,
                    material = renderer.sharedMaterial
                };
            }
            
            // Add transform matrix to the group
            groupMap[key].matrices.Add(renderer.transform.localToWorldMatrix);
            
            // Disable the original renderer
            renderer.enabled = false;
            
            // Optionally disable the entire GameObject for better performance
            renderer.gameObject.SetActive(false);
        }
        
        // Convert dictionary to list
        instanceGroups.AddRange(groupMap.Values);
        
        if (debugLog)
        {
            Debug.Log($"Created {instanceGroups.Count} instance groups");
            foreach (var group in instanceGroups)
            {
                Debug.Log($"Group: {group.mesh.name} with {group.material.name} - {group.matrices.Count} instances");
            }
        }
    }
    
    void Update()
    {
        RenderInstances();
    }
    
    void RenderInstances()
    {
        foreach (var group in instanceGroups)
        {
            if (group.mesh == null || group.material == null)
                continue;
                
            // Split large groups into batches of MAX_INSTANCES_PER_BATCH
            int totalInstances = group.matrices.Count;
            int batchCount = Mathf.CeilToInt((float)totalInstances / MAX_INSTANCES_PER_BATCH);
            
            for (int batch = 0; batch < batchCount; batch++)
            {
                int startIndex = batch * MAX_INSTANCES_PER_BATCH;
                int instanceCount = Mathf.Min(MAX_INSTANCES_PER_BATCH, totalInstances - startIndex);
                
                // Create array for this batch
                Matrix4x4[] batchMatrices = new Matrix4x4[instanceCount];
                for (int i = 0; i < instanceCount; i++)
                {
                    batchMatrices[i] = group.matrices[startIndex + i];
                }
                
                // Render the batch
                Graphics.DrawMeshInstanced(
                    group.mesh, 
                    0, // submesh index
                    group.material, 
                    batchMatrices, 
                    instanceCount
                );
            }
        }
    }
    
    [ContextMenu("Re-enable Children")]
    public void ReEnableChildren()
    {
        // Re-enable all child renderers and GameObjects
        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>(true);
        
        foreach (MeshRenderer renderer in childRenderers)
        {
            if (renderer.gameObject == this.gameObject)
                continue;
                
            renderer.enabled = true;
            renderer.gameObject.SetActive(true);
        }
        
        // Clear instance groups
        instanceGroups.Clear();
        
        if (debugLog)
            Debug.Log("Re-enabled all child objects");
    }
    
    [ContextMenu("Get Statistics")]
    public void GetStatistics()
    {
        int totalInstances = 0;
        int totalBatches = 0;
        
        foreach (var group in instanceGroups)
        {
            int instances = group.matrices.Count;
            int batches = Mathf.CeilToInt((float)instances / MAX_INSTANCES_PER_BATCH);
            
            totalInstances += instances;
            totalBatches += batches;
            
            Debug.Log($"Group {group.mesh.name}: {instances} instances, {batches} batches");
        }
        
        Debug.Log($"Total: {totalInstances} instances across {totalBatches} draw calls");
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw bounds for each instance group
        Gizmos.color = Color.yellow;
        foreach (var group in instanceGroups)
        {
            if (group.mesh == null) continue;
            
            Bounds meshBounds = group.mesh.bounds;
            foreach (var matrix in group.matrices)
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawWireCube(meshBounds.center, meshBounds.size);
            }
        }
        Gizmos.matrix = Matrix4x4.identity;
    }
}