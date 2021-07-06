using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SaveTowers(TowerData[] towers)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/towers.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, towers);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path + ", creating file");
            return null;
        }
    }

    public static TowerData[] LoadTowers()
    {
        string path = Application.persistentDataPath + "/towers.json";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TowerData[] data = (TowerData[])formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path + ", creating file");
            return null;
        }
    }

    public static void ClearData()
    {
        string path = Application.persistentDataPath + "/towers.json";
        File.Delete(path);
        path = Application.persistentDataPath + "/player.json";
        File.Delete(path);
    }
}
