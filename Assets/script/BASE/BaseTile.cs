using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour {
    bool someonehere=true;
	// Use this for initialization
	void Start () {
        StartCoroutine(isExistObj());
    }
    IEnumerator isExistObj()
        {
        yield return new WaitForSeconds(0.2f);
        someonehere = false;
        }
// Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (someonehere)
        {
            pool.put(gameObject);
        }
    }
}
