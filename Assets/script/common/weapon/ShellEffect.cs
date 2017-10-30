using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEffect : Weapon {
    public bool existParent = false;
    public ParticleSystem particle;
    private void OnEnable()
    {
        if (particle == null)
        {
            if ((particle = GetComponent<ParticleSystem>()) == null)
            {
                print(gameObject.name + "do not have particlesystem");
                return;
            }
        }
        pool.put(particle.gameObject, particle.main.duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

  
    // Update is called once per frame
    void Update () {
		
	}
}
