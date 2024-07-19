using System.Collections;
using System.Collections.Generic;
using GameObjects;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoBox : MonoBehaviour, IAppliableObject
{
    // Start is called before the first frame update
    public GameObject ammoObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetItem()
    {
        // AmmoObject newAmmo = Instantiate
        return ammoObject;
    }

    public void Apply(ItemObject item, PlayerObject player)
    {
        if (player == null || player.HasItem) return;
        GameObject newAmmo = Instantiate(ammoObject);
        newAmmo.GetComponent<AmmoObject>().pick();
        Debug.Log("Pickup ammo from the ammo box");
        player.Pickup(newAmmo.GetComponent<AmmoObject>());
    }

    public bool CanApply(ItemObject item)
    {
        return item == null;
    }
}
