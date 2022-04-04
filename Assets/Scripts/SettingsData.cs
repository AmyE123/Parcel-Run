using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    [System.Serializable]
    public class DifficultySetting
    {
        public string name;
        public int startHearts;
    }

    public int chosenDifficulty;

    public float musicVolume;

    public float soundVolume;

    public DifficultySetting[] difficulties;

    public int GetStartHealth()
    {
        if (chosenDifficulty < 0 || chosenDifficulty >= difficulties.Length)
            return 38;

        return difficulties[chosenDifficulty].startHearts * 4;
    }

    private string PrefsKey => $"settingsData";

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
