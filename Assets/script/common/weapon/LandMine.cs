 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class LandMine : Weapon {
    Material materia;
    bool isActived=false;	// Use this for initialization
	void Start () {
        materia = GetComponent<SpriteRenderer>().material;
        materia.color = Color.green;
	}

    public void OnEnable()
    {

        isActived = false;
        materia.color = Color.green;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseTank bt = collision.GetComponent<BaseTank>();
        if (isActived)
        {
            explosion(explodEffect);
            pool.put(gameObject);
            print("mine explosion!");
            return;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        BaseTank bt = collision.GetComponent<BaseTank>();
        if (bt != null && bt.NetworkId == identifer)
        {
            isActived = true;
            materia.color = Color.red;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
