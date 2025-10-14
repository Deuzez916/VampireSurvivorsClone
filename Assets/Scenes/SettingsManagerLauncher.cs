using UnityEngine;

public class SettingsManagerLauncher : MonoBehaviour
{
    public static PreviousScene previousScene;

    void Awake()
    {
        SettingsManager settings = GetComponent<SettingsManager>();
        if (settings != null)
        {
            settings.previousScene = previousScene;
        }
    }
}
