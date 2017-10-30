using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseShell : Weapon
{
    public override void Awake()
    {
        base.Awake();
        print("BaseShell Awake");
        bullet_source = GetComponent<AudioSource>();
    }
    public int localId=0;
    public virtual void blast()
    {
       pool.getGMOBJ(explodEffect, transform).GetComponent<Weapon>().identifer=identifer;
      // pool.put(gameObject); 
    }
    public  virtual void CollisionHandel(GameObject g)
    {

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetInstanceID() == localId)
            return;
        CollisionHandel(collision.gameObject);
    }
    // Use this for initialization
    void Start()
    {
       
    }

    public void OnDisable()
    {
        localId = 0;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
