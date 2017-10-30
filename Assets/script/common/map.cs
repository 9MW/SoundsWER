using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class map : MonoBehaviour {
    public GameObject prefab;
    List<GameObject> exist = new List<GameObject>();
    string MapSeed;
    [Tooltip("the color's tile won't be instantiate")]
    public Color col;
    public Texture2D mapinfo;
    public Vector2 tileRate =new Vector2(50,50);
    public Vector2 offset = Vector2.one * 0.3f;
    private void OnEnable()
    {
   
        Vector2[,] grabMapinfomation = grabMapinfo(col);
//        int i = 0;
//        foreach (Vector2 m in grabMapinfomation)
//        {
//GameObject  p=
//         Instantiate(prefab,(m - tileRate * 0.5f), Quaternion.identity);
//            p.transform.SetParent(transform);
//            exist.Add(p);
//            i++;
//            //Instantiate(prefab, (m - tileRate * 0.5f) * 0.28f-offset, Quaternion.identity).transform.SetParent(transform);
//        }
    }
    void Start()
    {
        GameObject map = new GameObject("map");
       /* for (int i = -50; i < 50; i++)
        {
            float f =30* Mathf.Sin(i);
               Vector2 positionx = new Vector2(i, f);
               Vector2 positiony = new Vector2(f, i);
               GameObject x=(GameObject) Instantiate(prefab, positionx, Quaternion.identity);
               GameObject y = (GameObject)Instantiate(prefab, positiony, Quaternion.identity);
               y.transform.parent = x.transform.parent = map.transform;
            
        }*/
    }
   void updateMap()
    {
       // offset = offset;
        Vector2[,] grabMapinfomation = grabMapinfo(col);
        int i = 0;
        foreach (Vector2 m in grabMapinfomation)
        {
            exist[i].transform.position = (m - tileRate * 0.5f);
            i++;
            //Instantiate(prefab, (m - tileRate * 0.5f) * 0.28f-offset, Quaternion.identity).transform.SetParent(transform);
        }
    }
    // Use this for initialization
    Vector2[,] grabMapinfo(Color match) {
        Vector2[,] outmapCoord=new Vector2[mapinfo.width,mapinfo.height];
        int xoffset = (int)(mapinfo.width / tileRate.x) ;
        int yoffset = (int)(mapinfo.height / tileRate.y) ;
        Vector2 Wholeoffset = new Vector2(tileRate.x * offset.x, tileRate.y * offset.y) / 2;
        for (int x = 0; x < tileRate.x; x++)
        {
            float offsetx = x * offset.x;
            for (int y = 0; y < tileRate.y; y++)
            {
                if(mapinfo.GetPixel(xoffset*x+(int)offset.x,yoffset*y+(int)offset.y).a!=0)
                {
                    float offsety =y * offset.y;
                    outmapCoord[x,y]=new Vector2(offsetx, offsety)- Wholeoffset;
                    GameObject p=Instantiate(prefab, (outmapCoord[x, y]), Quaternion.identity);
                    p.transform.SetParent(transform);
                    p.GetComponent<SpriteRenderer>().color = mapinfo.GetPixel(xoffset * x + (int)offset.x, yoffset * y + (int)offset.y);
                    exist.Add(p);
                }

            }

        }
        return outmapCoord;
    }
	
	// Update is called once per frame
	void Update () {
        //if (offset != offset)
        //{
        //    updateMap();
        //}
	}
}
