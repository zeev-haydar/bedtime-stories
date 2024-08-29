using GameObjects;
using Player;
using UnityEngine;

public class AmmoBox : MonoBehaviour, IAppliableObject, IUseHint
{
    // Start is called before the first frame update
    public GameObject ammoObject;
    // Update is called once per frame
    void Update()
    {
        
    }

    public InteractHinter.InteractType GetInteractType(GameObject obj = null)
    {
        return InteractHinter.InteractType.PickUp;
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
