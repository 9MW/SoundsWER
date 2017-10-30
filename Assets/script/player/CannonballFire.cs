using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//this class for spawn and add fouce to cannonball
public class CannonballFire : MonoBehaviour {
    public GameObject pre;
    public int danforce=200;
    public float detroypd =5;
	public AudioSource as_Audio;
	public AudioClip ac_shoot;
    [Tooltip("time of replenishment interval")]
    public int fltm=1;//补给时间   
    [Tooltip("time of attack interval")]
    public int sttim=1;//发射时间
    int paodanshu = 200;
    int supplyradius = 1;//补给半径
    GameObject jd;
    [Tooltip("contolINfo")]
   public BaseTank contolINfo;
    float st = 0;
    bool touchkh=false;
    Text sypd;
    bool ismutiple = false;
    NetManager nManager;
    //cannonball acquired from generaData class
     GameObject HESHELL;
     GameObject APSHELL;
    // Use this for initialization
    void Start () {
        HESHELL = GeneraData.HESHELL;
        APSHELL = GeneraData.APSHELL;
        try
        {

            nManager = GameObject.Find("FlowControl").GetComponent<NetManager>();
        }
        catch
        {
            nManager = null;
        }
        if (nManager != null)
            ismutiple = true; 
        sypd = GameObject.Find("剩余炮弹数").GetComponent<Text>();
       jd = GameObject.Find("基地");
       // contolINfo = transform.parent.GetComponent<BaseTank>();
        contolINfo.frie = kh;//receive and execute action
        control.KeyPress += kh;
//        Debug.Log("transform.parent.gameObject is" + transform.parent.gameObject.name);
        /*  for (int i = 0; i < 50; i++)
          {
              for (int r = 0; r < 10; r++) {
                  r = i ^ 2;
                  Vector3 z = new Vector3(i, r,-10);
                  GameObject prefab = (GameObject)Instantiate(pre, z, transform.rotation);

              }
          }*/
//        Physics2D.OverlapCircle(jd.transform.position,1);
	}
    public inputAction inputAction;
    //this function for execute networkplayer operation
    public  void kh(byte variety) {
       
        if (variety == 0)
            return;
      
        touchkh = true ;
    }
  
    //Following method for deal remote input 
    void kh(inputAction inputAction)
    {
        switch (inputAction)
        {
            case inputAction.Key1:
                pre = HESHELL;
                    break;
            case inputAction.Key2:
                pre = APSHELL;
                contolINfo.putLandMine();
                break;
        }
        if(pre!=null)
        pre.GetComponent<Weapon>().identifer = contolINfo.NetworkId;
        touchkh = true;
    }
    //this method manipulated by localplayer 
    public void kh(byte variety,bool ishumanity)
    {
        if (ismutiple&&ishumanity)
        {
            nManager.SendAction(variety, contolINfo.gameObject.name + contolINfo.NetworkId);
        }
        if (variety == 0)
            return;
        touchkh = true;
    }
    public void kh()
    {
        kh(1,true);
    }

	// Update is called once per frame
	void Update () {
        
        shoot(sttim);
        fillling(fltm);
        touchkh =false;
	
	}
    //trade为填装炮弹所需时间
   public void shoot(float fillingtime) 
    {
       if(st<fillingtime)
        st += Time.deltaTime;
        if (touchkh&&st >= fillingtime && paodanshu != 0)
        {

                contolINfo.bullect = 1;
                as_Audio.clip = ac_shoot;
                as_Audio.Play();
                Vector2 x = transform.TransformDirection(0, 1, 0);
                GameObject prefab = pool.getGMOBJ(pre, transform.position, transform.rotation);
                paodanshu--;
            sypd.text = "" + paodanshu;
            //  OnDrawGizmosSelected();`
            // prefab.transform.eulerAngles = new Vector3(0, 0, 90);
            prefab.GetComponent<Rigidbody2D>().AddForce(x * danforce);
                st = 0;
        }
    }
    //filltim为补给炮弹所需时间
    void fillling(int filltim) {
       

        if (jd != null && paodanshu != 20 &&
            Vector2.Distance(transform.position, jd.transform.position) < supplyradius )
        {
            StartCoroutine(wait(filltim));
        }
   
    
    }
    IEnumerator wait(int tm)
    {
        paodanshu += 1;
        yield return new WaitForSeconds(tm);
    }
    //void FixedUpdate() {
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    Debug.DrawLine(transform.position,ray.direction,Color.green);
      
       
    //}
    //void OnDrawGizmosSelected()
    //{//Gizmos只在物体被选择的时候绘制。Gizmos不能被点选，这可以使设置更容易。例如:一个爆炸脚本可以绘制一个球来显示爆炸半径
    //    Gizmos.color = Color.red;
    //    Vector2 direction = transform.TransformDirection(Vector2.up) * 5;
    //    Gizmos.DrawRay(transform.position, direction);
    //}
}
