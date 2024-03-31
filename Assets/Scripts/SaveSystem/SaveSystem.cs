using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveLevels(List<LevelSave> levels)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/levels.dat");

        formatter.Serialize(file, levels);

        file.Close();
    }

    public static List<LevelSave> LoadLevels()
    {
        if (File.Exists(Application.persistentDataPath + "/levels.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/levels.dat", FileMode.Open);

            List<LevelSave> levels = formatter.Deserialize(file) as List<LevelSave>;

            file.Close();

            return levels;
        }
        else
        {
            return null;
        }
    }
}
