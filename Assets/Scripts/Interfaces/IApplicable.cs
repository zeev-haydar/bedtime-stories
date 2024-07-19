using System;
using Items;
using UnityEngine;

public interface IApplicable
{
    void Apply(Item item);
    bool CanApply(Item item);
}