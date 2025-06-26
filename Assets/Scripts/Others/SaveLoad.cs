using UnityEngine;
using System.IO;

public static class SaveLoad
{
    static string _savePath;
    static int gameVersion = 2;
    public static bool settingsLoaded;

    public static void SaveSettings()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "DandyBasics.settings." + gameVersion);

        FileStream stream = new FileStream(_savePath, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(Collector.settings.fullscreen);
        writer.Write(Collector.settings.captions);
        writer.Write(Collector.settings.mouseSensitivity);
        writer.Write(AudioListener.volume);
        
        stream.Close();
    }
    public static void LoadSettings()
    {
        settingsLoaded = false;
        _savePath = Path.Combine(Application.persistentDataPath, "DandyBasics.settings." + gameVersion);

        if (!File.Exists(_savePath))
        {
            Debug.Log("Save file not found in " + _savePath);
            return;
        }

        FileStream stream = new FileStream(_savePath, FileMode.Open);
        BinaryReader reader = new BinaryReader(stream);

        Collector.settings.fullscreen = reader.ReadBoolean();
        Collector.settings.captions = reader.ReadBoolean();
        Collector.settings.mouseSensitivity = reader.ReadSingle();
        AudioListener.volume = reader.ReadSingle();
        settingsLoaded = true;

        stream.Close();
    }
}
