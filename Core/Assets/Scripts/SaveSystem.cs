using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveHookType(HookType hook)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/unlockedHook.cap";
        
        FileStream fout = new FileStream(filePath, FileMode.Create);
        
        formatter.Serialize(fout, (int)hook);
        fout.Close();
    }
    
    public static HookType LoadHookType()
    {
        string filePath = Application.persistentDataPath + "/unlockedHook.cap";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream fin = new FileStream(filePath, FileMode.Open);
            
            int hook = (int) formatter.Deserialize(fin);
            fin.Close();
            return (HookType)hook;
        }
        
        return 0;
    }

    public static void SaveUnlockedLures(Dictionary<Lure, bool> lures)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/unlockedLures.cap";
        
        FileStream fout = new FileStream(filePath, FileMode.Create);

        List<bool> unlockedLures = new();

        foreach (var lure in lures)
        {
            unlockedLures.Add(lure.Value);
        }
        
        formatter.Serialize(fout, unlockedLures);
        fout.Close();
    }

    public static List<bool> LoadUnlockedLures()
    {
        string filePath = Application.persistentDataPath + "/unlockedLures.cap";
        
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream fin = new FileStream(filePath, FileMode.Open);
            
            List<bool> luresUnlockedState = (List<bool>) formatter.Deserialize(fin);
            fin.Close();
            return luresUnlockedState;
        }
        
        return new();  
    }
}
