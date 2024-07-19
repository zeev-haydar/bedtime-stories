using System.Collections;
using System.Collections.Generic;
using BedMechanics;
using Items;
using Player;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    public Item item;

    public void Use(IAppliableObject appliableObject, PlayerObject player)
    {
        item.pickable = false;
        appliableObject.Apply(this, player);
    }
    public bool isPickable()
    {
        return item.pickable;
    }
    public bool pick()
    {
        bool tmp = item.pickable;
        item.pickable = false;
        return tmp;
    }
    public bool drop()
    {
        item.pickable = true;
        return true;
    }
}
