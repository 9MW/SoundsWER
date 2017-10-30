﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Net;

public class Client : NetManager
{
    Dictionary<int, GameObject> SyncObj = new Dictionary<int, GameObject>(30);
    int id;
    // string selfname;
    public override void receiveDataHandel(int outConnectionId, byte[] buffer, int receivedSize, int TP)
    {
        switch (TP)
        {
            case msgType.color:
                MessageofColor(outConnectionId, buffer);
                break;
            case msgType.SeverSideId:
                MessageofSeverSideId(outConnectionId, buffer);
                break;
            case msgType.Data:
                if (isS2)
                    MessageofData(outConnectionId, buffer);
                break;
            case msgType.shoot:
                MessageofShot(buffer,outConnectionId);
                    break;
        }

    }

    private void MessageofShot(byte[] buffer,int playerId)
    {
        string[] mg = NetWorkAdapter.splitString(buffer);
        string key = mg[0];
        byte bulletcategory = byte.Parse(mg[1]);
        if (synchronizationObj.ContainsKey(key))
        {
            BaseTank b = synchronizationObj[key].GetComponent<BaseTank>();
            b.frie(bulletcategory);
        }
        else
        {
            Debug.LogError("KEY " + key + " not exist at sunchronization");
        }
    }
    private void MessageofSeverSideId(int outConnectionId, byte[] buffer)
    {
        print("parse string= " + NetWorkAdapter.splitString(buffer)[0]);
         id =int.Parse( NetWorkAdapter.splitString(buffer)[0]);
        SceneManager.LoadScene("scene2");
    }

    private void MessageofData(int outConnectionId, byte[] buffer)
    {

        SerializeUDP info;
        SynchronizationMessage = (Dictionary<string, SerializeUDP>)BytesToOBJ(buffer);
        Debug.Log("SynchronizationMessage.length=" + SynchronizationMessage.Count);
         foreach (string s in SynchronizationMessage.Keys)
         {
             info = SynchronizationMessage[s];
             if (synchronizationObj.ContainsKey(s))
             {
                 SynchronizationData(synchronizationObj[s], info);
             }
             else
             {
                 Debug.Log(" synchronizationObj.Add=" + s);

                 GameObject gm;
                 switch (s[0])
                 {
                     case '装':
                         gm = Instantiate(enemyPrefeb[0]);
                         synchronizationObj.Add(s, gm);
                         SynchronizationData(gm, info);
                         break;
                     case '中':
                         gm = Instantiate(enemyPrefeb[1]);
                         synchronizationObj.Add(s, gm);
                         SynchronizationData(gm, info);
                         break;
                     case '重':
                         gm = Instantiate(enemyPrefeb[2]);
                         synchronizationObj.Add(s, gm);
                         SynchronizationData(gm, info);
                         break;
                     case '玩':
                         if (info.id == id)
                             break;
                         gm = Instantiate(PlayePrefab);
                        gm.name = s;
                         synchronizationObj.Add(s, gm);
                         SynchronizationData(gm, info);
                         break;
                 }

             }
         }
    }
    public override void init()
    {
      // string which = adapter.connectionClient[id].Stext;
      // string ip = which.Replace("::ffff:", "");
      //  IpToObj.Add(ip, id);
    }
    Color c;
    Vector2 LocalPlayCoordinate;
    private void MessageofColor(int outConnectionId, byte[] buffer)
    {
        float r, g, b;
        string[] s = NetWorkAdapter.splitString(buffer);
        r = float.Parse(s[0]);
        g = float.Parse(s[1]);
        b = float.Parse(s[2]);
        c = new Color(r, g, b,255);//255 expression transparency
        //To set initial player position of player
        if (playerObject != null)
        {
            playerObject.GetComponent<SpriteRenderer>().color = c;
        }
        LocalPlayCoordinate = new Vector2(float.Parse(s[3]), float.Parse(s[4]));

    }
    //use UDP protocol transfer position info.
    //The UDPclient class initialized in the base class
    public override void syncData()
    {
        // BaseContol.NetworkId = id;
        print("entry synchronization function");
        SerializeUDP info = new SerializeUDP(id, playerObject.transform);
            adapter.UDPC.sendByte(ObjectToByteArray(info));
        //adapter.sendmessage(adapter.connectionId, ObjectToByteArray(info), msgType.Data);
    }
    bool isS2 = false;
    public override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "scene2")
        {
            GuiAndManage gm = GameObject.FindObjectOfType<GuiAndManage>();
            gm.enemy = 0;
            playerObject = GameObject.Find("玩家1");
            playerObject.GetComponent<BaseTank>().NetworkId = id;
            isS2 = true;
            GaneStart = true;
            playerObject.GetComponent<SpriteRenderer>().color = c;
            playerObject.transform.position = LocalPlayCoordinate;
        }
    }
    byte[] buffer = new byte[1024];
    public void f4resh()
    {
        int outHostId = adapter.hostId;
        int outConnectionId;
        int outChannelId;
        string outConnIp;
        int outPort;
        int receivedSize;
        byte error;
        NetworkEventType evt;

        do
        {
            evt =
               NetworkTransport.ReceiveFromHost(adapter.hostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
            if (error != 0)
                Debug.Log("Receive error=" + error);

            switch (evt)
            {
                case NetworkEventType.ConnectEvent:
                    {
                        Debug.Log("receiveChanneId=" + outChannelId);
                        UnityEngine.Networking.Types.NetworkID outID;
                        UnityEngine.Networking.Types.NodeID outNode;
                        NetworkTransport.GetConnectionInfo(outHostId, outConnectionId, out outConnIp, out outPort, out outID, out outNode, out error);
                        adapter.connectionClient.Add(outConnectionId, new StrTxt(Instantiate(adapter.text), outConnIp, true));
                        adapter.OnConnect(outHostId, outConnectionId, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.DisconnectEvent:
                    {
                        adapter.OnDisconnect(outHostId, outConnectionId, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.DataEvent:
                    {

                        OnData(outHostId, outConnectionId, outChannelId, buffer, receivedSize, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.BroadcastEvent:
                    {
                        adapter.OnBroadcast(outHostId, buffer, receivedSize, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.Nothing:
                    break;

                default:
                    Debug.LogError("Unknown network message type received: " + evt);
                    break;
            }
        }
        while (evt != NetworkEventType.Nothing);


    }
    public override void processMessage(byte[] Udpdata, IPEndPoint ipinfo)
    {
        MessageofData(0, Udpdata);
    }
    public  void Accack(byte bullectType)
    {
        adapter.sendmessage(adapter.connectionId,new byte[ bullectType], msgType.shoot);
    }
    private void OnDestroy()
    {
        Destroy(gameObject.GetComponent<UDPClient>());
    }
    
}
