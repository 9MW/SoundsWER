using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HEeffect : ShellEffect
{
    public int maxeffict = 3;//this variable defined effective particle quantity
    // Use this for initialization
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    //int i = 0;
    //private void OnParticleCollision(GameObject other)
    //{
    //    int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);
        
    //        if (i >= maxeffict)
    //        {
    //            i = 0;
    //            return;
    //    }
    //    print("destoryed tile=" + i);
    //    print("particleCollision=" + collisionEvents[i].colliderComponent.gameObject.name+ " numCollisionEvents="+ numCollisionEvents);
    //            i++;
    //        try {
    //        pool.put(other);
    //        }
    //    catch
    //        {
    //            other.SetActive(false);
    //        }
    //}
    public void accomplishaction()
    {
        pool.put(gameObject);
    }
   
    // Update is called once per frame
    void Update()
    {

    }
}
