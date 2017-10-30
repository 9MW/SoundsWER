using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

//class for deal user input
public class control : MonoBehaviour, MYMSG
{
    public int whichBTN;
    public bool isPass;
    public delegate void inKey(inputAction a);
    public static event inKey KeyPress;//pass key code to subscriber
    public bool isOnNet;
    public static int north = 1;//北
    public static int south = 2;//南
    public static int west = 3;//西
    public static int east = 4;//东
    // Use contObj for initialization
    //public Vector2 speed = new Vector2(50, 50);
    public AudioSource as_Audio;
    GameObject primitiveShell;
    public GameObject AHCR;
    public float li = 0.5f;
    public GameObject manipulate;
    private void Start()
    {
        primitiveShell = Resources.Load("prefab/炮弹", typeof(GameObject)) as GameObject;
        isPass = false;
    }
    GameObject ball;
    public void Highexplode()
    {
        KeyPress(inputAction.Key1);
    }
    
    public void Armorpiercing()
    {
        KeyPress(inputAction.Key2);
    }
    public AudioClip ac_move;
    // 2 - Store the movement
    public inputAction Action;
    public struct MovingJoystick
    {
        public Vector2 joystickAxis;
    }
   public void presskey1()
    {
        KeyPress(inputAction.Key1);
        return;
    }
    void FixedUpdate()
    {
        if (isPass == true )
        {
            if (manipulate == null)
            {
                Debug.Log("contObj 等于 null");
                return;
            }
           manipulate.GetComponent<BaseTank>().move(whichBTN, li);
                //move(whichBTN);
        }
    }
    public void BtnDW(int o)
    {
        isPass = true;
        whichBTN = o;
    }

    public void BtnUp(int o)
    {
        isPass = false;
        whichBTN = o;
    }
  
}
public enum inputAction
{
    Key1,
    Key2,
}

