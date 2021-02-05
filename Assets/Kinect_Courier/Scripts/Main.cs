using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] TextMesh tm;
    void Start()
    {
        int x = 3;
        int y = 2;
        
        MessageManager.Instance.InitializeMessage();
        string ip = "192.168.68.187";
        ip = GetLocalIP(ip);
        tm.text = ip;
        MessageManager.Instance.SendConnectServerMsg(ip, "443");
    }

    public static string GetLocalIP(string ip)
    {
        string path;
#if UNITY_ANDROID
        path = Application.persistentDataPath + "/IPAddress.txt";
        //path = "/storage/emulated/0/ABRes";
#else
        path = Directory.GetParent(Application.dataPath).FullName + "/IPAddress.txt";
#endif
        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return ip;
    }
}
