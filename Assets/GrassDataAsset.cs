using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GrassData", menuName = "Grass/Grass Data Asset"), System.Serializable]
public class GrassDataAsset : ScriptableObject
{
    public List<GrassObjectData> grassData = new List<GrassObjectData>();
    public void SaveData(Transform parent)
    {
        grassData.Clear();

        if (parent == null) return;

        foreach (Transform child in parent)
        {
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
            if (prefab == null) continue;



            string prefabPath = AssetDatabase.GetAssetPath(prefab);

            grassData.Add(new GrassObjectData
            {
                prefabPath = prefabPath.Substring(17, prefabPath.Length - (7 + 17)),
                position = child.position,
                rotation = child.rotation,
                scale = child.localScale
            });
        }
    }

    public void LoadData(Transform parent, bool useNonSerialized)
    {
        if (parent == null) return;

        foreach (var data in grassData)
        {
            var prefab = Resources.Load<GameObject>(data.prefabPath);

            if (prefab == null) continue;

            GameObject instance = null;

            instance = Instantiate(prefab, parent);
            
            if (instance == null) continue;

            instance.transform.position = data.position;
            instance.transform.rotation = data.rotation;
            instance.transform.localScale = data.scale;

            if (useNonSerialized)
            {
                instance.hideFlags = HideFlags.DontSave;
            }
        }
    }
}


[System.Serializable]
public class GrassObjectData
{
    public string prefabPath;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}