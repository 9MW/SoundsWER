using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CircleCollider2D))]
[System.Serializable]
public class Goods : MonoBehaviour
{
    //[SerializeField]
    public Cargo cargo;//{ get { return this.cargo; }set { this.cargo = value; } }// Use this for initialization
    void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("touched goods");
        BaseTank TKinstance = collision.GetComponent<BaseTank>();
        if (TKinstance == null)
        {
            return;
        }
        else
        {
            TKinstance.replenishment(cargo);
            if (cargo.fuel == 0&&cargo.shell==0)
            {
                pool.put(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class Cargo
{
   public int fuel=0, shell=0;
    Cargo(int fuel, int shell)
    {
        this.fuel = fuel;
        this.shell = shell;
    }
}