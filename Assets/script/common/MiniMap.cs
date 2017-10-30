
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MiniMap : MonoBehaviour,IPointerClickHandler
{
    public Transform t;
    public float maxDistanceDelta = 0;
    public Vector2 mapSize = new Vector2(100, 100);
    public Vector2 miniMapSize;
    public float reFreshinterval = 1;
    Material mapProperty;
    Queue<Vector4[]> thread_pos = new Queue<Vector4[]>();
    Thread POSthread;
    public GameObject miniMap;
    public static Dictionary<GameObject, GameObject> inMapObj = new List<GameObject, GameObject>();
    public Color[] mappresentColor = { Color.green, Color.red, Color.white };
    RectTransform miniMaprtf;
    RectTransform quondam;
    RawImage backgroundimage;
    Vector2 originalSize;
    // Use this for initialization
    void Start()
    {
        if (miniMap == null)
        {
            miniMap = GameObject.Find("Minimap");
            miniMaprtf = miniMap.GetComponent<RectTransform>();
            miniMapSize = new Vector2(miniMaprtf.rect.width, miniMaprtf.rect.height);
            quondam = Instantiate(miniMaprtf);
        }
        MapCursor.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,MapCursorRadius);
        MapCursor.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MapCursorRadius);
        backgroundimage = miniMap.GetComponent<UnityEngine.UI.RawImage>();
        mapProperty = backgroundimage.material;
        pool.EOnUsed += OnObjChane;
        init();
        originalSize = miniMapSize;
        //StartCoroutine(() =>
        //{
        //    yield return 0.1;
        //};)
    }
    float tm = 0;
    //Update is called once per frame
    void Update()
    {
        if (tm > 1)
        {
           // init();
            // mapUpdate();
            map4Update();
            tm = 0;
            // mapProperty.SetVectorArray("activityOBJ", mapUpdate());
        }
        tm += Time.deltaTime;
    }
    #region  for inspire
    //POSthread = new Thread(new ThreadStart(threadMAPupdate));
    //POSthread.IsBackground = true;
    //POSthread.Start();
    #endregion
    void threadMAPupdate()
    {
        while (true)
        {
            if (tm == 0)
                continue;
            lock (thread_pos)
            {
                thread_pos.Enqueue(mapUpdate());
                tm -= 1;
            }
        }
    }
    //shader version 
    Vector4[] mapUpdate()
    {
        GameObject[][] tkobj = new GameObject[][]{
            GameObject.FindGameObjectsWithTag("Player"),
            GameObject.FindGameObjectsWithTag("enemy"),
        };
        Vector4[] pos = new Vector4[holeMapObjCounter(tkobj)];
        mapProperty.SetInt("_OBJsize", pos.Length);
        int counter = 0;
        for (int j = 0; j < tkobj.Length; j++)
        {
            GameObject[] g1 = tkobj[j];
            for (int i = 0; i < g1.Length; i++)
            {
                //if (!inMapObj.ContainsKey(g1[i].GetInstanceID()))
                //{
                //    inMapObj.Add(g1[i].GetInstanceID(), g1[i]);
                //}
                //pos[counter] = map2mini(g1[i].transform.position);
                //switch (g1[i].tag)
                //{
                //    case TAG.Player:
                //        pos[counter].w = 0;//0 means player self or alies
                //        break;
                //    case TAG.enemy:
                //        pos[counter].w = 1;//1 means player self or enemy
                //        break;
                //    default:
                //        pos[counter].w = 2;
                //        break;
                //}
                //counter++;
            }
        }
        return pos;
    }
    public float MapCursorRadius = 0.1f;
    public GameObject MapCursor;
    Dictionary<int, GameObject> slowingCursor = new Dictionary<int, GameObject>();
    void map4Update()
    {

        foreach (GameObject gid in inMapObj.Keys)
        {
            Map2mini(gid.transform, inMapObj[gid].transform);
            GameObject g = inMapObj[gid];

            switch (gid.tag)
            {
               
                case TAG.Player:
                    g.GetComponent<Image>().color = mappresentColor[0];//0 means player self or alies
                    break;
                case TAG.enemy:
                    g.GetComponent<Image>().color = mappresentColor[1];//1 means player self or enemy
                    break;
                default:
                    g.GetComponent<Image>().color = mappresentColor[2];
                    break;
            }
        }
    }
    Vector4 map2mini(Vector3 worldpos)
    {
        Vector2 mini = worldpos + new Vector3(mapSize.x, mapSize.y, 0) * 1 / 2;//mini range is 0-1.
        mini.x /= mapSize.x;
        mini.y /= mapSize.y;
        return mini;
    }
    //world position map to local 2D coordinate system
    void Map2mini(Transform inTransform, Transform localPOS)
    {
        Vector2 vector2;
        vector2.x= inTransform.transform.position.x *miniMapSize.x/ mapSize.x;
        vector2.y= inTransform.transform.position.y * miniMapSize.y / mapSize.y;
        localPOS.localPosition = vector2;

       // return vector2;
    }
    //invoke this method while gameobject put or get
    void OnObjChane(GameObject g, bool isput)
    {
        if (SlowInTheMap(g))
        {
            if (isput&&inMapObj.ContainsKey(g))//true while put otherwise get
            {
                inMapObj.Remove(g);
            }
            else
            {
                MapCursor = pool.getGMOBJ(MapCursor,transform);
                if (MapCursor.transform.parent != miniMap)
                {
                    MapCursor.transform.parent = miniMap.transform;
                }
                Map2mini(miniMap.transform, MapCursor.transform);
                inMapObj.Add(g, MapCursor);
                switch (g.tag)
                {
                    case TAG.Player:
                        MapCursor.GetComponent<Image>().color = mappresentColor[0];//0 means player self or alies
                        break;
                    case TAG.enemy:
                        MapCursor.GetComponent<Image>().color = mappresentColor[1];//1 means player self or enemy
                        break;
                    default:
                        MapCursor.GetComponent<Image>().color = mappresentColor[2];
                        break;
                }
            }
        }

    }
    
   public List<string> ShouldInMapTag;
    bool SlowInTheMap(GameObject gameObject){
        return ShouldInMapTag.Contains(gameObject.tag);
        }
    void init()
    {

        GameObject[][] tkobj = new GameObject[][]{
            GameObject.FindGameObjectsWithTag("Player"),
            GameObject.FindGameObjectsWithTag("enemy"),
        };
        for (int j = 0; j < tkobj.Length; j++)
        {
            GameObject[] g1 = tkobj[j];
            for (int i = 0; i < g1.Length; i++)
            {
                if (!inMapObj.ContainsKey(g1[i]))
                {
                    MapCursor = pool.getGMOBJ(MapCursor, map2mini(g1[i].transform.position), Quaternion.identity);
                    MapCursor.transform.SetParent(miniMap.transform);
                    Map2mini(g1[i].transform, MapCursor.transform);
                    inMapObj.Add(g1[i], MapCursor);
                    print(g1[i].name + "already add in");
                }
              //  pos[counter] = map2mini(g1[i].transform.position);
            }
        }
    }
    int holeMapObjCounter(GameObject[][] tkobj)
    {
            int l = 0;
            for (int i = 0; i < tkobj.Length; i++)
            {
                l += tkobj[i].Length;
            }
            return l;
    }
    private void OnDisable()
    {
      //  POSthread.Abort();
    }
    bool isDuringShowing = false;
    Vector4 preinfo4RTF;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!isDuringShowing)
        {
            //miniMaprtf.anchoredPosition = Vector2.zero;
            isDuringShowing = !isDuringShowing;
            preinfo4RTF = new Vector4(miniMaprtf.rect.size.x, miniMaprtf.rect.size.y,miniMaprtf.localPosition.x, miniMaprtf.localPosition.y);
            miniMaprtf.localPosition =Vector2.zero;// new Vector2(Screen.width, Screen.height)*-1/2;
            //miniMaprtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -Screen.width/2, Screen.width);
            //miniMaprtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height);
            miniMapSize = new Vector2(Screen.width, Screen.height);
            miniMaprtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, miniMapSize.x);
            miniMaprtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, miniMapSize.y);
        }
        else
        {
            isDuringShowing = !isDuringShowing;
            miniMaprtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x);
            miniMaprtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y);
            miniMaprtf.anchoredPosition = -originalSize;//minimap shoe in top right cornor
            //miniMapSize = new Vector2(preinfo4RTF.x, preinfo4RTF.y);
            //rtf.localPosition = new Vector2(preinfo4RTF.z, preinfo4RTF.w);
        }
    }
}
public struct R2M
{
    R2M(GameObject R,GameObject M)
    {

    }

}