using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string directory = "/Saves";

    public static string fileName = "File";


    //Save data to save file defined by playerPref (save num)
    public static void Save(SaveFile sv)
    {
        string dir = Application.streamingAssetsPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(sv);

        File.WriteAllText(dir +  "/" + fileName + PlayerPrefs.GetInt("SaveNum") + ".txt", json);
    }

    //Load data from SaveFile.savenum
    public static SaveFile Load()
    {
        fileName = fileName + PlayerPrefs.GetInt("SaveNum") + ".txt";
        string fPath = Application.streamingAssetsPath + directory + "/" + fileName;
        SaveFile sf = new SaveFile();

        if (File.Exists(fPath))
        {
            string json = File.ReadAllText(fPath);
            sf = JsonUtility.FromJson<SaveFile>(json);
        }
        else
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log("Savefile doesn't exist");
            Save(new SaveFile());
            sf = new SaveFile();
        }

        return sf;
    }
}


//Save Data object
public class SaveFile
{

    public int currentGenre = 1;
    public int lastGenre = 1;               //0 - HUB, 1 - Plat, 2 - Shot, 3 - RPG

    public Vector3 lastPosition;            //Position to load player prefab in

    public string lastScene = "Testing/[Test Scene]";            //Last scene player was in

    public float lastHP = 1.0f;                //Last known hp value


    public bool gameStart;

    public bool platStart;              //Whether platformer stage started (whether to display cutscene)

    public bool platDone;

    public bool platCollect;


    public bool shotStart;

    public bool shotDone;

    public bool shotCollect;


    public bool rpgStart;

    public bool rpgDone;

    public bool rpgCollect;


    public float playerMana;

    public bool mamaGot;

    public float mamaMana;

    public bool helperGot;

    public float helperMana;

    public int goldGot;

    public bool gameDone;
}