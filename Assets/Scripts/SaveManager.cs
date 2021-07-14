using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static BinaryFormatter formatter = new BinaryFormatter();

    public static void SavePlayer(Player player)
    {
        string path = Application.persistentDataPath + "/player.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SaveTowers(TowerData[] towers)
    {
        string path = Application.persistentDataPath + "/towers.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, towers);
        stream.Close();
    }

    public static void SaveLevelData(LevelData data)
    {
        string path = Application.persistentDataPath + "/levelData.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveStats(StatsData data)
    {
        string path = Application.persistentDataPath + "/stats.json";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
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
            FileStream stream = new FileStream(path, FileMode.Open);

            TowerData[] data = (TowerData[])formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Tower data not found in " + path + ", creating file");
            return null;
        }
    }

    public static LevelData LoadLevelData()
    {
        string path = Application.persistentDataPath + "/levelData.json";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = (LevelData)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Level file not found in " + path + ", creating file");
            return null;
        }
    }
    public static StatsData LoadStats()
    {
        string path = Application.persistentDataPath + "/stats.json";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            StatsData data = (StatsData)formatter.Deserialize(stream);
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
    public static void ClearSaveData()
    {
        string path = Application.persistentDataPath + "/stats.json";
        File.Delete(path);
    }
}
