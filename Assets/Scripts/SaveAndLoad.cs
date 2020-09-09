using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveAndLoad
{
    public static void SavePlayerInfo(string id, double balance)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + id + ".sy";

        FileStream stream = new FileStream(path, FileMode.Create);

        UserInfo userInfo = new UserInfo(id, balance);
        formatter.Serialize(stream, userInfo);
        stream.Close();
    }

    public static UserInfo LoadPlayerInfo(string id)
    {
        string path = Application.persistentDataPath + "/" + id + ".sy"; ;

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            UserInfo userInfo = formatter.Deserialize(stream) as UserInfo;
            stream.Close();

            return userInfo;
        }
        Debug.LogError("Save file not found in" + path);
        return null;
    }
}