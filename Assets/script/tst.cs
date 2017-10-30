using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class tst : MonoBehaviour {
    public Transform t;
    public float maxDistanceDelta=0;
    public Vector2 mapSize=new Vector2(100,100);
    public float reFreshinterval = 1;
    Material mapProperty;
    Queue<Vector4[]> thread_pos = new Queue<Vector4[]>();
    Thread POSthread;
    // Use this for initialization
    void Start () {
        t = transform;
        //mapProperty = GameObject.Find("Minimap").GetComponent<RawImage>().material;
         POSthread = new Thread(new ThreadStart(threadMAPupdate));
        POSthread.IsBackground = true;
        POSthread.Start();
	}
   // float tm = 0;
	// Update is called once per frame
	void Update () {
        //if (tm > 1)
        //{
        //  //  tm = 0;
        //  mapProperty.SetVectorArray("activityOBJ", mapUpdate());
        //}
        //tm += Time.deltaTime;
	}
    void threadMAPupdate() {
        while (true)
        {
            //if (tm == 0)
            //    continue;
            lock (thread_pos)
            {
                thread_pos.Enqueue(mapUpdate());
               // tm -= 1;
            }
        }
    }
    Vector4[] mapUpdate()
    {
        GameObject[][] tkobj = new GameObject[][]{
            GameObject.FindGameObjectsWithTag("enemy"),
            GameObject.FindGameObjectsWithTag("Player")
        };
        Vector4[] pos = new Vector4[tkobj[0].Length + tkobj[1].Length];
      int counter = 0;
        foreach (GameObject[] g1 in tkobj)
        {
            for (int i=0; i < g1.Length; i++)
            {
                
                pos[counter] = map2mini(g1[i].transform.position);
                counter++;
            }
        }
        return pos;
    }
    Vector4 map2mini(Vector3 worldpos)
    {
        Vector2 mini = worldpos + new Vector3(mapSize.x, mapSize.y, 0)*1/2;
        mini.x /= mapSize.x;
        mini.y /= mapSize.y;
        return mini;
    }

    private void OnDisable()
    {
        POSthread.Abort();
    }
  //  IGamobjDictionary<string, GameObject> dictionary = new IGamobjDictionary<string, GameObject>();
  //public  void rm()
  //  {
  //      dictionary.Remove(t.name);
  //  }

}
public class List<k, v> : Dictionary<GameObject, GameObject>{
  public new void  Remove(GameObject g)
    {
        pool.put(this[g]);
        base.Remove(g);
    }
    
}
//class u<k, n> : IDictionary<TKey, TValue>
//{
//    public TValue this[TKey key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

//    public ICollection<TKey> Keys => throw new System.NotImplementedException();

//    public ICollection<TValue> Values => throw new System.NotImplementedException();

//    public int Count => throw new System.NotImplementedException();

//    public bool IsReadOnly => throw new System.NotImplementedException();

//    public void Add(TKey key, TValue value)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Add(KeyValuePair<TKey, TValue> item)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Clear()
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool Contains(KeyValuePair<TKey, TValue> item)
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool ContainsKey(TKey key)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
//    {
//        throw new System.NotImplementedException();
//    }

//    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool Remove(TKey key)
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool Remove(KeyValuePair<TKey, TValue> item)
//    {
//        throw new System.NotImplementedException();
//    }

//    public bool TryGetValue(TKey key, out TValue value)
//    {
//        throw new System.NotImplementedException();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        throw new System.NotImplementedException();
//    }
//}
