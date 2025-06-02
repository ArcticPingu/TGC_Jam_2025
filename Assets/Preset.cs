using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class Preset : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        int qualityCount = QualitySettings.names.Length;

        List<OptionData> data = new();

        for (int i = 0; i < qualityCount; i++)
        {
            string presetName = QualitySettings.names[i];
            data.Add(new(presetName));
        }

        dropdown.AddOptions(data);

        dropdown.onValueChanged.AddListener((newIndex) => { HandleChange(dropdown, newIndex); });
    }

    private void HandleChange(object currentDropdown, int newIndex)
    {
        QualitySettings.SetQualityLevel(newIndex);
    }
}
