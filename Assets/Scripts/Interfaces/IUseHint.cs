using Player;
using UnityEngine;

public interface IUseHint {
    public InteractHinter.InteractType GetInteractType(GameObject obj = null);
}