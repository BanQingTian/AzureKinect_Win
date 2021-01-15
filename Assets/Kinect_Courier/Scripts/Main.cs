using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        MessageManager.Instance.InitializeMessage();
        MessageManager.Instance.SendConnectServerMsg("192.168.68.187", "443");
    }

   
}
