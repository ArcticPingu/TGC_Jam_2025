using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrassTilemapSpawner))]
public class GrassTilemapSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GrassTilemapSpawner spawner = (GrassTilemapSpawner)target;
        if (GUILayout.Button("Spawn Grass"))
        {
            spawner.SpawnGrass();
        }
    }
}
