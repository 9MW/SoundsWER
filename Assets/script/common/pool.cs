using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Collections;

public class pool : MonoBehaviour
    {
       public delegate void  OnUsed(GameObject g,bool isPut);
       public static event OnUsed EOnUsed;
        static MYDaleyEvent mevt=new MYDaleyEvent();
        static Dictionary<string, List<GameObject>> pol = new Dictionary<string, List<GameObject>>();
        public static bool AIPD = false;
        static float delaytim;
        static GameObject tmpGameobject;
   
    private void Awake()
    {
        mevt.AddListener(xc);
    }
    public static GameObject getGMOBJ(GameObject g,Transform t)
    {
     return getGMOBJ(g, t.transform.position, t.transform.rotation);
    }
      public static GameObject getGMOBJ(GameObject got, Vector3 wz, Quaternion xz)
    {
        GameObject spawn;
            string objnm = got.name;
            // Debug.Log("取的是"+objnm);
            if (AIPD)
            {
                objnm = "AIPD";
                AIPD = false;
            }
            if (pol.ContainsKey(objnm))
            {
                if (pol[objnm].Count != 0)
                {
                     spawn = pol[objnm][0];
                    spawn.transform.position = wz;
                    spawn.transform.rotation = xz;
                    spawn.SetActive(true);
                    pol[objnm].RemoveAt(0);
                EOnUsed(spawn, false);
                    return spawn;
                 }
                else
                {
                spawn = Instantiate(got, wz, xz) as GameObject;
                spawn.name = objnm;
                EOnUsed(spawn, false);
                return spawn;
                 }
            }
            else
            {
            spawn = Instantiate(got, wz, xz) as GameObject;
            spawn.name = objnm;
                pol.Add(objnm, new List<GameObject>());
            EOnUsed(spawn, false);
            return spawn;
             }
            
        }
    public static void put(GameObject pt, float tim = 0)
        {
        //   yield return new WaitForSeconds(tim);
        if (tim != 0)
        {
           //Debug.Log("delaytime = "+tim);
            mevt.Invoke(pt, tim);
            return;
        }
            EOnUsed(pt,true);
            pt.transform.parent = ScreenManager.boundary.transform;
            pt.SetActive(false);
        try
        {

            pol[pt.name].Add(pt);
        }
        catch (Exception)
        {
            Destroy(pt);
            Debug.LogWarning("the gamobject " + pt.name + " not present in pool");
        }
            //   Debug.Log(nm+"已放入");
        }
    void xc(GameObject trg, float tm)
    {
        //        Debug.Log("Starting " + Time.time);
        StartCoroutine(e(trg, tm));
  //      Debug.Log("Done " + Time.time);
    }
    IEnumerator e(GameObject pt, float tim)
    {
        yield return new WaitForSeconds(tim);
       // print(pt.name + "，" + pt.GetInstanceID());
        pt.SetActive(false);
        put(pt);
    }
    private void Update()
    {
        if (delaytim != 0) {
    //        Invoke("dely",delaytim );
            delaytim = 0;
        }
    }
}
 class MYDaleyEvent: UnityEvent<GameObject,float>
{

}
