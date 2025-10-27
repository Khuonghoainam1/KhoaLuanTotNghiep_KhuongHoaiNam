using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataPersistent
{
    public static T ReadDataExist<T>(string path, bool saveInPersistentFolder = true) where T : class
    {
        if (saveInPersistentFolder)
            path = $"{Application.persistentDataPath}/{path}";

        FileStream stream = null;

        try
        {
            stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();            
            var data = formatter.Deserialize(stream) as T;
            stream.Flush();
            stream.Close();
            return data;
        }
        catch (System.Exception ex)
        {
            if (stream != null)
            {
                stream.Flush();
                stream.Close();
            }
            Debug.Log(ex.Message);
        }
        return null;
    }

    public static void SaveData<T>(string path, T data, bool saveInPersistentFolder = true)
    {
        if (data == null)
        {
            return;
        }
        if (saveInPersistentFolder)
            path = $"{Application.persistentDataPath}/{path}";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Flush();
        stream.Close();

       // Debug.Log($"Saving {path} success!");
    }

    public static void ClearData(string path, bool saveInPersistentFolder = true)
    {
        if (saveInPersistentFolder)
            path = $"{Application.persistentDataPath}/{path}";
        File.Delete(path);
    }
    public static void ClearAll()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo($"{Application.persistentDataPath}");

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }

        Debug.Log($"Clear Data Success {Application.persistentDataPath}");
    }
    public static void SaveTexture2D(string path, Texture2D texture, bool saveInPersistentFolder = true)
    {
        if (saveInPersistentFolder)
            path = $"{Application.persistentDataPath}/{path}";

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log($"Saving {path} success!");
    }

    public static Texture2D ReadTexture2D(string path, int width, int height, bool saveInPersistentFolder = true)
    {
        if (saveInPersistentFolder)
            path = $"{Application.persistentDataPath}/{path}";

        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);

            return texture;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }
}