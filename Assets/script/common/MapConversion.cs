using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConversion : MonoBehaviour {
    public GameObject Cbtn;
    public GameObject BtnParent;
    List<GameObject> Cbtnl;
    public Texture2D inTex;
    Dictionary<Color,List<int> > Cdic = new Dictionary<Color, List<int>>(10000);
    // Use this for initialization
    void Start()
    {

    }
    void Conversion()
    {
        Color[] c = inTex.GetPixels(); ;
        int colortype = 0;
        for (int i = 0; i < c.Length; i++)
        {
            if (!Cdic.ContainsKey(c[i]))
            {
                GameObject g = Instantiate(Cbtn, BtnParent.transform,false);
                List<int> tl = new List<int>();
                tl.Add(i);
                Cdic.Add(c[i],tl);
                Color tmpc = new Color(colortype * 0.1f, colortype * 0.05f, colortype);
                colortype += 1;
                Debug.Log(Cdic.Count + " color " + c[i].ToString() + "  G=" + c[i].g);
                c[i] = tmpc;
            }
            else
            {
               Cdic[c[i]].Add(i);
            }
            //if (c[i] == Transparency)
            //{
            //    c[i] = new Color(0, 0, 0, 0);
            //}
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
