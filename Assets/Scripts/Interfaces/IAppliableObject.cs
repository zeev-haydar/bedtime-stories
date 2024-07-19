using System;
using Items;
using Player;
using UnityEngine;

public interface IAppliableObject
{
    void Apply(ItemObject item, PlayerObject player);
    bool CanApply(ItemObject item);
}