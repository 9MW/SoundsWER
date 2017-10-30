using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneraData : MonoBehaviour {
  public enum Cannonball { AitoTrace,Normal,ControlledBlast}
    public static string BaseName = "基地";
    [SerializeField]
    int coin;
    public static float replenishmentInterval { get; internal set; }
    #region vehicle property
    static readonly float generalspeed = 0.3f;
    public static int PrimaryHP { get { return 100; } }
    public static int APdamage { get { return 15; } }
    public static int HEdamage { get { return 10; } }
    public static int unLoad { get { return 3; }internal set { } }

    public static GameObject HESHELL,APSHELL, Goods;
    #endregion  
    // Use this for initialization
    void Awake () {
    HESHELL = Resources.Load("prefab/shell/HEshell", typeof(GameObject)) as GameObject;
    APSHELL = Resources.Load("prefab/shell/shell", typeof(GameObject)) as GameObject;
        Goods = Resources.Load("prefab/shell/Goods", typeof(GameObject)) as GameObject;
}
	public static float getSpeed(Type4Character characterSpecies)
    {
        return generalspeed;
    }

	//// Update is called once per frame
	//void Update () {
		
	//}
}
public enum Type4Character{
    lightTK,
    middleTK,
    heavyTK,
}
public class TAG
{
   public  const string
    tile = "tile",
    enemy = "enemy",
    Player = "Player";
} 