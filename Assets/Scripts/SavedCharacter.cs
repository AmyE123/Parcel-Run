using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SavedCharacter : ScriptableObject
{
    public string eyeName;
    public string browName;
    public string mouthName;
    public string shirtName;
    public string legsName;

    [Space(8)]
    public Color skinColor;
    public Color shirtColor;
    public Color legsColor;

    [Space(8)]
    [SerializeField]
    private string _prefsKey;

    private string PrefsKey => $"customCharacter_{_prefsKey}";

    public void SaveToPlayerPrefs()
    {
        string jsonData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PrefsKey, jsonData);
        PlayerPrefs.Save();

        Debug.Log($"Saved data to PlayerPrefs({PrefsKey}): {jsonData}");
    }

    public void LoadFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(PrefsKey) == false)
            return;

        string jsonData = PlayerPrefs.GetString(PrefsKey);
        JsonUtility.FromJsonOverwrite(jsonData, this);
        Debug.Log($"Loaded data from PlayerPrefs({PrefsKey}): {jsonData}");
    }
}
