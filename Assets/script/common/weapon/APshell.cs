using UnityEngine;
using System.Collections;

public class APshell : BaseShell
{
    // public GameObject collisionEffect;
    // Use this for initialization
    BaseTank collisionbase;
    int counter= GeneraData.APdamage;
    public override void CollisionHandel(GameObject e)
    {
        collisionbase = e.GetComponent<BaseTank>();
        if (collisionbase != null)
        {
            blast();
            collisionbase.onHit(ref counter);
            if (counter <= 0)
            {
                counter = GeneraData.APdamage;
                pool.put(gameObject);
            }
            return;
        }

        if (e.name == "界")
        {
            pool.put(gameObject);
            return;
        }
        counter--;
        blast();
        Destroy(e.gameObject);
        if (counter <= 0)
        {
            counter = GeneraData.APdamage;
            print("撞到" + e.name);
            pool.put(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
