
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveGameData(MainData mainData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/cfData.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData data = new ProgressData(mainData);

        try
        {
            formatter.Serialize(stream, data);
        }
        catch (FileLoadException e)
        {
            Debug.LogError("File load error!");
        }
        
        stream.Close();

    }

    public static ProgressData LoadGameData()
    {
        string path = Application.persistentDataPath + "/cfData.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressData gameData = formatter.Deserialize(stream) as ProgressData;
            stream.Close();

            return gameData;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
