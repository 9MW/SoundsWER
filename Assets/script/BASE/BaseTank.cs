using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Shot(byte whichBollet);
public class BaseTank : MonoBehaviour {
    public byte bullect=0;
    public GameObject manipulate;
    public Transform cannonOutput;
    Rigidbody2D rigid;
    [HideInInspector]
    public int WhichDirection;
    public int hp = 10;
    public int NetworkId=-1;
    public  Shot frie;
    public GameObject cannonball;
    public float maxvelocity=0.3f ;
    public bool bookFire = true;//true on mutiplayer mode.
    public int fuel=20, shell,Horsepower=300;
    GuiAndManage manage;
    //[SerializeField]
    //int id = 0;
    Rigidbody2D physicsbody;
    public  void Awake()
    {
        if (NetworkId == -1)
        {
            NetworkId = GetInstanceID();
        }
        physicsbody = GetComponent<Rigidbody2D>();
        physicsbody.angularDrag = 1000;
        //physicsbody.drag = 0.4f;
        StartCoroutine(isExistObj());
        movetime = fuel;
        if(manipulate==null)
        manipulate = gameObject;
        if(bookFire)
        frie = shot;
        if (gameObject.tag == TAG.Player)
        {
            manage = GameObject.FindObjectOfType(typeof(GuiAndManage)) as GuiAndManage;
            manage.fuel_slider.maxValue = fuel;
        }
    }

    bool someonehere = true;
    IEnumerator isExistObj()
    {
        yield return new WaitForSeconds(0.2f);
        someonehere = false;
    }
    //invoke this function when hit by cannonball;
    public void onHit(ref int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            damage =-hp;
            try
            {
                pool.put(gameObject);
            }
            catch
            {
                Debug.LogError("Object " + gameObject.name + " beyond contrl"); 
                Destroy(gameObject);
            }
        }
        else
        {
            damage = 0;
        }
    }
    public virtual void shot(byte bulletType)
    {
        shot(bulletType, 200);
    }
    // Use this for initialization
    void Start () {
       
        rigid= manipulate.GetComponent<Rigidbody2D>();
            }
    void shot(byte bolletType,float force)
    {
        if (cannonball == null)
            return;
        Vector2 x = transform.TransformDirection(0, 1, 0);
        GameObject prefab = pool.getGMOBJ(cannonball, transform.position, transform.rotation);
        prefab.GetComponent<Weapon>().identifer = NetworkId;
        //  OnDrawGizmosSelected();`
        // prefab.transform.eulerAngles = new Vector3(0, 0, 90);
        prefab.GetComponent<Rigidbody2D>().AddForce(x * force);

        pool.put(prefab, 4);
    }
    public void movement(int direction)
    {
        WhichDirection = direction;
        switch (direction)
        {

            case 4:
                // rigidbody2D.AddForce(Vector2.right * li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(maxvelocity, 0);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 270);

                break;
            case 1:
                // rigidbody2D.AddForce(Vector2.up * li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, maxvelocity);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 3:
                // rigidbody2D.AddForce(Vector2.right * -li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(-maxvelocity, 0);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case 2:
                // rigidbody2D.AddForce(Vector2.up * -li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -maxvelocity);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
        }
    }
    public void setDirection(byte d)
    {
        switch (d)
        {
            case 1:
                zhuan = Vector3.zero;
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 2:
                zhuan = new Vector3(0, 0, 180);
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 3:

                zhuan = new Vector3(0, 0, 90);
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 4:
                zhuan = new Vector3(0, 0, 270);
                manipulate.transform.eulerAngles = zhuan;
                break;
        }
    }
    public void move(int direction)
    {
        move(direction, Horsepower);
    }
    Vector3 zhuan;
    float movetime;//Time that vehicle can continue to move.
    //invoked by player
    public void move(int whichpress, float li)
    {
        setDirection((byte)whichpress);
        //can't move while velocity big than vehicle or fuel deplete
        if (manipulate.GetComponent<Rigidbody2D>().velocity.magnitude > maxvelocity || movetime <= 0)
        {
            return;
        }
        movetime -= Time.fixedDeltaTime;
        if(manage!=null)
        manage.fuel_slider.value = movetime;
        WhichDirection = whichpress;
        switch (whichpress)
        {
            case 1:
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.up * li);
                break;
            case 2:
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -li);
                break;
            case 3:
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -li);
                break;
            case 4:
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.right * li);
                break;
        }
    }
 public void replenishment(Cargo cargo)
    {
        int total = (int)(cargo.fuel + movetime);
        cargo.fuel =fuel-total;
        if (cargo.fuel >= 0)
        {
            movetime = fuel;
        }
        else
        {
            movetime = total;
            cargo.fuel = 0;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(someonehere)
        transform.position -=  new Vector3(0,0.2f,0);
    }
    public GameObject landmine;
    public void putLandMine()
    {
        pool.getGMOBJ(landmine, transform).GetComponent<Weapon>().identifer=NetworkId;
    }
}
public class GameObjectType
{
    public static readonly byte LightTank=1, MiddleTank=2, Heavy=3;
    
}
public static class bulletClass
{
    private const byte normal = 0;

    public static byte Normal
    {
        get
        {
            return normal;
        }
    }
}
public enum CannonBallType
{
    Normal,Explode_half_way,
    Wall,
}