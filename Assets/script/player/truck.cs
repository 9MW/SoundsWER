using UnityEngine;
using System.Collections;

public class truck : BaseTank {
    //ArrayList<Vector2> d;
    int capacity=30;
    float replenishmentInterval = 1;
    public int Curent=25;
    public GameObject expression;//represent intensify object
    int WeaponCofficient = 1;//this variable means once replenishment number
    public  Cargo CurrentCargo;
    public GameObject Goods;
    void Start () {
        replenishmentInterval = GeneraData.replenishmentInterval;
        //CurrentWeapon = capacity;
    }
    bool IsReplenishment=false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == GeneraData.BaseName)
        {
            IsReplenishment = false;
            InvokeRepeating("replenishment", replenishmentInterval, replenishmentInterval);
        }
    }
    void Replenishment()
    {

    }
    int unLoadAmount=GeneraData.unLoad;
    Cargo unLoad;//cargo that need unload
    
    void Unload()
    {
        if (CanUnload(unLoad))
        {
            Goods = pool.getGMOBJ(Goods, transform);
            Goods munitions = Goods.GetComponent<Goods>();
            munitions.cargo = unLoad;
        }
       // pool.getGMOBJ(expression, transform);
    }
    bool CanUnload(Cargo cargo)
    {
        return true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GeneraData.BaseName)
        {
            IsReplenishment = true;
            
            StartCoroutine(replenishment());
        }
    }
    IEnumerator replenishment()
    {
        if (Curent >= capacity|| IsReplenishment)
            yield break;
        yield return new WaitForSeconds(replenishmentInterval);
        Curent += WeaponCofficient;
        StartCoroutine(replenishment());
    }
}