using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HEshell : BaseShell
{
    public AudioClip enemy_killclip, enemy_hitClip;
    public Vector2 speed;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.GetCurrentAnimatorStateInfo(0)
        speed = GetComponent<Rigidbody2D>().velocity;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    

   public override void CollisionHandel(GameObject g)
    {

        switch (g.tag)
        {
            case "tile":
                Destroy(g);
                blast();
                break;

            case "enemy":
                if (gameObject.name == "AIPD")//no AIPD judgment will destory AI self
                    break;
                {
                    int z = Random.Range(1, 2);
                    BaseTank smz = g.gameObject.GetComponent<BaseTank>();
                    //Debug.Log(z);
                    if (z == 1)
                    {
                        smz.hp--;
                        if (smz.hp == 0)
                        {
                            //GM.onenemykill(g);//击杀奖励
                            bullet_source.clip = enemy_killclip;
                            bullet_source.Play();
                            pool.put(g);
                        }
                        else
                        {
                            bullet_source.clip = enemy_hitClip;
                            bullet_source.Play();
                            pool.put(gameObject);
                        }
                    }
                    else
                    {
                        bullet_source.clip = enemy_hitClip;
                        bullet_source.Play();
                        pool.put(gameObject);
                    }
                }
                break;
            default:
                // enemyexplod = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                blast();
               // pool.put(enemyexplod, 0.2f);
                Debug.Log("进入default name=" + g.name);
                break;
        }

    }
    public override void blast()
    {
        base.blast();
        pool.put(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
