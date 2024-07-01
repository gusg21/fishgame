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
        
        return HookType.BASICHOOK;
    }
    
    public static void SaveMoney(int money)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/savedMoney.cap";
        
        FileStream fout = new FileStream(filePath, FileMode.Create);
        
        formatter.Serialize(fout, money);
        fout.Close();
    }
    
    public static int LoadMoney()
    {
        string filePath = Application.persistentDataPath + "/savedMoney.cap";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream fin = new FileStream(filePath, FileMode.Open);
            
            int money = (int) formatter.Deserialize(fin);
            fin.Close();
            return money;
        }
        
        return 5;
    }

    public static void SaveUnlockedLures(List<Lure> unlockedLures)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/unlockedLures.cap";
        
        FileStream fout = new FileStream(filePath, FileMode.Create);

        List<LureType> unlockedLuresTypes = new();

        foreach (var lure in unlockedLures)
        {
            unlockedLuresTypes.Add(lure.Type);
        }
        
        formatter.Serialize(fout, unlockedLuresTypes);
        fout.Close();
    }

    public static List<LureType> LoadUnlockedLures()
    {
        string filePath = Application.persistentDataPath + "/unlockedLures.cap";
        
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream fin = new FileStream(filePath, FileMode.Open);
            
            List<LureType> luresUnlockedState = (List<LureType>) formatter.Deserialize(fin);
            fin.Close();
            return luresUnlockedState;
        }
        
        return new();  
    }
}
