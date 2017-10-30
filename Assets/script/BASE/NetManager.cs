﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class NetManager : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject[] enemyPrefeb;
    [HideInInspector]
    public NetWorkAdapter adapter;
    control c;
    int[] controlData;
    bool isPlaying;
    public bool isSever=false;
    ConnectionConfig config;
   public GameObject PlayePrefab;
    int maxAccpet;
    GameObject[] weapon;
    // Vector3[] SyncPosition;
    public Dictionary<string, SerializeUDP> SynchronizationMessage;
    //Dictionary<string, List<Gamobjectsinfo>> NPCPosition =new Dictionary<string, List<Gamobjectsinfo>>(5);
    public  Dictionary<string, GameObject> synchronizationObj;
    void OnEnable()
    {
       
        synchronizationObj = new Dictionary<string, GameObject>(20);
        SynchronizationMessage = new Dictionary<string, SerializeUDP>(50);
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
   public IUIHandler inData;
    private void Awake()
    {
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        PlayePrefab = (GameObject) Resources.Load("prefab/玩家1", typeof(GameObject));
        enemyPrefeb = new GameObject[3];
        enemyPrefeb[0] = (GameObject) Resources.Load("prefab/multip/装甲车", typeof(GameObject));
        enemyPrefeb[1] = (GameObject) Resources.Load("prefab/multip/中型坦克", typeof(GameObject));
        enemyPrefeb[2] = (GameObject) Resources.Load("prefab/multip/重型坦克", typeof(GameObject));
        adapter =gameObject.GetComponent<NetWorkAdapter>();
      //  isSever = adapter.isServer;
        maxAccpet = 10;
        controlData = new int[maxAccpet*2];
        adapter.Receiver += receiveDataHandel;
        if (gameObject.GetComponent<IUIHandler>() != null)//avoid addcomponent repeatedly
            return;
        inData = gameObject.AddComponent<IUIHandler>();
        inData.receiveData += processMessage;//this delegate for deal with incoming UDP data
        gameObject.AddComponent<NetworkHandler>();
        gameObject.AddComponent<MainThreadProcessor>();
        
    }
    Dictionary<GameObject, Vector3> destinationList = new Dictionary<GameObject, Vector3>(27);
  public virtual void processMessage(byte[] Udpdata,System.Net.IPEndPoint ipinfo) { }
  public void SynchronizationData(GameObject g, SerializeUDP infomation) {
        BaseTank   operatingObj = g.GetComponent<BaseTank>();
      //  g.transform.position = new Vector3(infomation.x, infomation.y, infomation.z);
        operatingObj.setDirection(infomation.angle);
        if (!destinationList.ContainsKey(g))
        {
            destinationList.Add(g, new Vector3(infomation.x, infomation.y, infomation.z));
        }
        else
        {
            destinationList[g] = new Vector3(infomation.x, infomation.y, infomation.z);
        }
       // SyncPosition(g, new Vector3(infomation.x, infomation.y, infomation.z));
        //operatingObj.frie(infomation.state[0]); abandoned operating
    }
    //assumption is clint
 /* public  byte[] collectData()
    {
            byte[] syncbytes;
        //if (isSever && isPlaying)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("enemy");
           // weapon= GameObject.FindGameObjectsWithTag("weapon");
            tmpContainer = new GameObject[][] { players, enemys };
            int activeGobjnum = players.Length + enemys.Length;
            activeObj =new GameObject[activeGobjnum];
           // SyncPosition = new Vector3[activeGobjnum];
            players.CopyTo(activeObj, 0);
            enemys.CopyTo(activeObj, players.Length);
            int id;
            
            BaseContol objectstate;
            SynchronizationMessage.Clear();
            for(int i=0;i<tmpContainer.Length;i++)
            {
               
                GameObject[] g = tmpContainer[i];
                foreach (GameObject g1 in g)
                {
                    objectstate = g1.GetComponent<BaseContol>();
                    id = objectstate.NetworkId;
                    byte FireState = objectstate.bullect;
                    if (id == -1)
                    {
                        enemycounter += 1;
                        objectstate.NetworkId = enemycounter;
                        SynchronizationMessage.Add(g1.name+ enemycounter, new Gamobjectsinfo(objectstate.NetworkId, new byte[] { FireState }, g1.transform.position, g1.transform.rotation));
                        Debug.Log(g1.name+"-counter=" + enemycounter);
                    }
                    else
                    {
                        SynchronizationMessage.Add(g1.name+id, new Gamobjectsinfo(id, new byte[] { (byte)FireState }, g1.transform.position, g1.transform.rotation));
                    }

                    objectstate.bullect = 0;

                }
            }
  
       }
        syncbytes= ObjectToByteArray(SynchronizationMessage);
        NPCPosition.Clear();
        return syncbytes;
       // controlData[1] = c.whichBTN;
    }*/
   
    public virtual void syncData() {
       
    }
    /* “QosType.Reliable” will deliver message and assure 
     * that the message is delivered, while “QosType.Unreliable”
     *  will send message without any assurance, 
     *  but will do this faster.*/
    public bool GaneStart=false;
   Queue<Color> availableColor = new Queue<Color>(27);
   public virtual void init() {
       /* generateDifferColor();//use color to identify players
        uint PN = 0;
        Debug.Log("Entry to init method adapter.connectionClient="+adapter.connectionClient.Count);
        //instance player
        foreach (int id in adapter.connectionClient.Keys)
        {
            Color tmpc ;
            adapter.sendmessage(id, new byte[] { (byte)id },msgType.SeverSideId);
            Debug.Log("clientId=" + id + " byteId=" + (byte)id);
            tmpc = availableColor.Dequeue();
            byte[] colinfo = NetWorkAdapter.GetBytes(tmpc.r + "," + tmpc.g + "," + tmpc.b);
            adapter.sendmessage(id, colinfo,msgType.color);
            Vector3 spawnPOS = new Vector3(-synchronizationObj.Count + PN, 0);
            GameObject  player = Instantiate(PlayePrefab, spawnPOS,gameObject.transform.rotation);
            player.name = "player" + id;
            player.GetComponent<SpriteRenderer>().color = tmpc;
            synchronizationObj.Add("player"+id,player);
        }
        */
     }

    public virtual void receiveDataHandel(int outConnectionId, byte[] buffer, int receivedSize,int MT)
    {
    }
 
    void startClinet() {
      
    }

  public  byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
   public object BytesToOBJ(byte[] ba)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(ba, 0, ba.Length);
            ms.Seek(0, SeekOrigin.Begin);
          var obj =  bf.Deserialize(ms);
            return obj;
        }
    }
    void startHost() {
        
    }
  public  Dictionary<string, int> IpToObj = new Dictionary<string, int>(27);
    float PosendRate= 0.05f;
    void Update()
    {
        foreach (GameObject g in destinationList.Keys)
        {
            SyncPosition(g,destinationList[g]);
        }
//        Debug.Log("tempTime=" + tempTime);
        if (GaneStart)
        {
            InvokeRepeating("syncData", 0, PosendRate);
            
        }
      
    }
   byte[]  buffer = new byte[1024]; 
/* public virtual void fresh()
    {
        int outHostId;
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
               NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
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
        
    }*/
    bool isHead = true;
    int receiveMsgType;
    public void OnData(int outHostId, int outConnectionId, int outChannelId, byte[] buffer, int receivedSize, NetworkError error)
    {
        Debug.Log("Entry OnData");
        if (isHead)
        {
            isHead = false;
            string[] s = NetWorkAdapter.splitString(buffer);
            Debug.Log("Entry isHead info="+NetWorkAdapter.GetString(buffer)+" bufferLength="+this.buffer);
           this. buffer = new byte[int.Parse(s[1])];
           receiveMsgType = int.Parse(s[0]);
           adapter. connectionClient[outConnectionId].setMessageType(receiveMsgType);
            Debug.Log("connectionClient[outConnectionId].receiveMsgType=" + receiveMsgType + "isHead=" + isHead);
            return;
        }
        Debug.Log("Entry Receiver");
        isHead = true;
        if (receiveMsgType == msgType.testMessage)
        {
            Debug.Log("Receive test message="+NetWorkAdapter.GetString(buffer));
            this.buffer = new byte[100];
            return;
        }
        receiveDataHandel(outConnectionId, buffer, receivedSize,receiveMsgType);
    }
    public virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "scene2")
        {

            GaneStart = true;
        }
        else
        {
            GaneStart = false;
        }
    }
    public void SyncPosition(GameObject gm,Vector3 destination) {
        gm.transform.position = Vector3.MoveTowards(gm.transform.position,destination,gm.GetComponent<BaseTank>().maxvelocity*Time.deltaTime*1.2f);
    }
    public void broadcast(byte[] data,int msgType)
    {
        foreach (int id in adapter.connectionClient.Keys)
        {
            adapter.sendmessage(id, data, msgType);
        }
    }
    //  Codeing shot action,id is the key of syncObj dictionary
    public void SendAction(byte bulletCategory, string id)
    {
        string info = id + "," + bulletCategory;
        string patten = @"play\w";
        
        if (Regex.IsMatch(id, patten))
        {
            print("shotAction "+info);
        }
        byte[] transfer = NetWorkAdapter.GetBytes(info);
        broadcast(transfer, msgType.shoot);
    }
    public string GetSyncObjKey(int connecetionId)
    {
        return "player" + connecetionId;
    }
    public BOTFireAction handle;
}
public  delegate void  BOTFireAction(byte Category, int AIid);
/*
[Serializable]
public class Gamobjectsinfo
{
    public int id;
    public byte[] state;
    public byte angle;
       public   float x, y, z=0;
    public Gamobjectsinfo(int identification,byte[] s, Vector3 v, Quaternion rotation)
    {
        id = identification;
        state = s;
        angle = 0;
        this.x = v.x;
        this.y = v.y;
       switch ((int)rotation.eulerAngles.z)
        {
            case 0:
                angle = 1;
                break;
            case 180:
                angle = 2;
                break;
            case 90:
                angle = 3;
                break;
            case 270:
                angle = 4;
                break;

        }
    }
 
}*/
public  static class  msgType
{
   public  const int 
        color = 1,Data=0,SeverSideId=2,
        testMessage=3,shoot=4;        
}
[System.Serializable]
public class SerializeUDP
{
    public int id;
    public byte angle;
    public float x, y, z = 0;
    public SerializeUDP(int identification, Transform transform)
    {
        id = identification;
        angle = 0;
        this.x = transform.position.x;
        this.y = transform.position.y;
        angle = (byte)transform.GetComponent<BaseTank>().WhichDirection;
    }
    public SerializeUDP(int identification, Vector3 v, Transform transform)
    {
        id = identification;
        angle = 0;
        this.x = v.x;
        this.y = v.y;
        angle = (byte)transform.GetComponent<BaseTank>().WhichDirection;
        /*  switch (transform.GetComponent<BaseContol>().WhichDirection)
          {
              case 0:
                  angle = 1;
                  break;
              case 180:
                  angle = 2;
                  break;
              case 90:
                  angle = 3;
                  break;
              case 270:
                  angle = 4;
                  break;
             // default:

          }*/
    }
}
