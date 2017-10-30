using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FieldOfView))]
public class BaseDefense : MonoBehaviour {
    public float rotateRate=0;
    public GameObject weapon;
    public float force;
    FieldOfView LocalFOV;
    // Use this for initialization
    void Start () {

        LocalFOV = GetComponent<FieldOfView>();
        //LocalFOV.onDetect += detecte;
        this.visibleTargets = LocalFOV.visibleTargets;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        detecte();
    }
    public List<Transform> visibleTargets;
        void detecte()
    {
        if (visibleTargets.Count != 0)
        {
            for (int i = 0; i < visibleTargets.Count; i++)
            {
                Vector2 v2 = visibleTargets[i].position-transform.position;
                attack(v2);
                return;
            }
        }
        transform.Rotate(new Vector3(0, 0, rotateRate));
    }
    public float reloadTime = 1;
    float Colding = 0;
	void attack(Vector2 Local2Obj)
    {
        Colding += Time.deltaTime;
        if (Colding < reloadTime)
        {
            return;
        }
        Vector2 x = transform.TransformDirection(Vector3.right);
        GameObject prefab = pool.getGMOBJ(weapon, transform);
        //prefab.transform.rotation.Eur = Vector2.right;
        prefab.GetComponent<BaseShell>().localId= GetInstanceID();
        //  OnDrawGizmosSelected();`
        // prefab.transform.eulerAngles = new Vector3(0, 0, 90);
        prefab.GetComponent<Rigidbody2D>().AddForce(Local2Obj * 200);
        Colding=0;
        pool.put(prefab, 4);
    }
}
