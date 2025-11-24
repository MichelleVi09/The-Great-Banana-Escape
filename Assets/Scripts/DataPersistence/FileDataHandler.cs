using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";
    //private bool useEncryption = false;
    private const string encryptionCodeWord = "Ricardo2021";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (!File.Exists(fullPath)) return null;

        // file is always encrypted
        string encrypted = File.ReadAllText(fullPath);
        string json = EncryptDecrypt(encrypted);

        try
        {
            return JsonUtility.FromJson<GameData>(json);
        }
        catch
        {
            Debug.LogError("Save file is not valid JSON after decryption. " +
                           "Delete it and start fresh: " + fullPath);
            return null;
        }
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        Directory.CreateDirectory(dataDirPath);

        string json = JsonUtility.ToJson(data, true);
        // always encrypt before writing
        string encrypted = EncryptDecrypt(json);

        File.WriteAllText(fullPath, encrypted);



#if UNITY_EDITOR
        Debug.Log("Saved to: " + fullPath);
#endif
    }

    //XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
