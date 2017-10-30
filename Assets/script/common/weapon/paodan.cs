using UnityEngine;
using System.Collections;
//[RequireComponent(typeof(Rigidbody2D))]
    public class paodan : Weapon
    {
        float lifetime = 0;
        public GameObject explod1;
        public GameObject enemyexplod;
        AudioSource bullet_source;
        public AudioClip zhuan_clip;
        public AudioClip plate_clip;
        public AudioClip enemy_killclip;
        public AudioClip enemy_hitClip;
        BaseTank smz;
    //the variable subsistencetim is the longest time that this gameobject can exist
        public float subsistencetim = 4;
        // Use this for initialization
        GuiAndManage GM;
        private void Start()
        {
        GM = GameObject.FindObjectOfType<GuiAndManage>();
        bullet_source = gameObject.GetComponent<AudioSource>();
        }
    //this method called when gameobject active
    private void OnEnable()
    {
        lifetime = 0;
    }

    void OnTriggerEnter2D(Collider2D e)
        {
        // Collider[] cols = Physics.OverlapSphere(transform.position, 0.2f);
        // for(int i=0;i<cols.Length;i++){
        //istEnemy.Add（e.gameObject）;

        //if (e.gameObject.name == "砖块")
        //{
        //    i++;
        //    Destroy(e.gameObject);
        //    if (i >= 5)
        //    {
        //        prefab = pool.getGMOBJ(explod1, transform.position, transform.rotation);
        //    pool.put(prefab, 0.2f);
        //        pool.put(gameObject);


        //        //Debug.Log("砖块" + i);
        //    }
        //    // bullet_source.clip = zhuan_clip;
        //    //bullet_source.Play();
        //}
        //else if (e.gameObject.name == "界")
        //{
        //    pool.put(gameObject);
        //}
        //if (e.gameObject.CompareTag("enemy") && name != "AIPD")//no AIPD judgment will destory AI self
        //{
        //    int z = Random.Range(1, 2);
        //    smz = e.gameObject.GetComponent<AI>();
        //    //Debug.Log(z);
        //    if (z == 1)
        //    {
        //        smz.hp--;
        //        if (smz.hp == 0)
        //        {
        //            GameObject pre = pool.getGMOBJ(explod1, transform.position, transform.rotation);
        //            pool.put(e.gameObject);
        //            pool.put(pre, 0.2f);
        //           GM.onenemykill( e.gameObject);//将炮弹作为摄像机子物体
        //            bullet_source.clip = enemy_killclip;
        //            bullet_source.Play();
        //        }
        //    }
        //    else
        //    {
        //        pool.put(this.gameObject);
        //        bullet_source.clip = enemy_hitClip;
        //        bullet_source.Play();
        //    }
        //    pool.put(gameObject);
        //}
       if (e.gameObject == null  )
            print("碰撞的物体=nill") ;
        CollisionHandel(e.gameObject);
        }

    private void FixedUpdate()
    {
        if (subsistencetim > lifetime)
        {
            lifetime += Time.deltaTime;
        }
        else
        {
            pool.put(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("砖块" + collision.gameObject.name);
    }
    void CollisionHandel(GameObject g)
    {

       switch(g.tag)
        {
            case "tile":
                Destroy(g);
                enemyexplod = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                pool.put(enemyexplod, 0.2f);
                pool.put(gameObject);
                break;
            
            case "enemy":
                if (gameObject.name== "AIPD")//no AIPD judgment will destory AI self
                    break;
                {
                    int z = Random.Range(1, 2);
                    smz = g.gameObject.GetComponent<BaseTank>();
                    //Debug.Log(z);
                    if (z == 1)
                    {
                        smz.hp--;
                        if (smz.hp == 0)
                        {
                            enemyexplod = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                            pool.put(enemyexplod, 0.2f);
                            GM.onenemykill(g);//击杀奖励
                            bullet_source.clip = enemy_killclip;
                            bullet_source.Play();
                            pool.put(gameObject);
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
                enemyexplod = pool.getGMOBJ(explod1, transform.position, transform.rotation);
                pool.put(gameObject);
                pool.put(enemyexplod, 0.2f);
                Debug.Log("进入default name=" +g.name+" Its tag="+g.tag);
                break;
        }
        
    }
}
    // }

    //pool.put(gameObject);



