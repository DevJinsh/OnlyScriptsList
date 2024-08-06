using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private static string playerDataPath = Application.persistentDataPath + "/data.txt";
    private static string optionDataPath = Application.persistentDataPath + "/option.txt";
    public static void SavePlayer(Vector3 savePoint)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(playerDataPath, FileMode.Create);

        PlayerData data = new PlayerData(savePoint);
        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(playerDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(playerDataPath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + playerDataPath);
            return null;
        }
    }

    public static void DeletePlayerData()
    {
        File.Delete(playerDataPath);
    }

    public static void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SoundManager.instance.SoundDeadAfter();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public static void SaveOptionData(SettingManager settingManager)
    {
        FileStream stream = new FileStream(optionDataPath, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();

        OptionData data = new OptionData(
                settingManager.masterVolumeSlider.mainSlider.value,
                settingManager.musicVolumeSlider.mainSlider.value,
                settingManager.sfxVolumeSlider.mainSlider.value,
                settingManager.selectResolution.selectedItemIndex,
                settingManager.brightSlider.mainSlider.value,
                settingManager.selectLanguage.selectedItemIndex,
                settingManager.useHaptic.isOn
        );

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static OptionData LoadOptionData()
    {
        if (File.Exists(optionDataPath))
        {
            FileStream stream = new FileStream(optionDataPath, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            OptionData data = formatter.Deserialize(stream) as OptionData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + optionDataPath);
            return null;
        }
    }
}
