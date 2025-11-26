using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;

void Pickup() { 

Inventory.Instance.Add(Item);
Destroy(gameObject);

}

    private void OnTriggerEnter(Collider other)
    {
        // Eðer çarpýþan obje Player tag'ine sahipse
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
