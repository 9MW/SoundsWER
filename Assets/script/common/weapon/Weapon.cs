using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject explodEffect;
    public GuiAndManage manager;
    public  int  identifer;
    //float lifetime = 0.5f;
    public AudioSource bullet_source;
    public AudioClip onCollision_clip;
    int damage=1;
    public float radius4explosion=1;
    public virtual void Awake()
    {
       
    }
    // Use this for initialization
    void Start()
    {

    }
   public void explosion(GameObject g)
    {
        BaseTank b= g.GetComponent<BaseTank>();
        Rigidbody2D physis = g.GetComponent<Rigidbody2D>();
        if (physis != null)
        {
            if (b != null)
            {
                b.onHit(ref damage);
                pool.getGMOBJ(explodEffect, transform);
            }
            else
            {
                pool.getGMOBJ(explodEffect, transform);
                float dis = Vector2.Distance(g.transform.position, transform.position);
                Vector2 direction = Vector3.Normalize((g.transform.position - transform.position));
                physis.AddForce(direction * dis );
            }
        }
        else
        {
            pool.getGMOBJ(explodEffect, transform);
        }       
    }
    public void getkill(string id)
    {
        StartCoroutine(isExistObj());
    }
    IEnumerator isExistObj()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
