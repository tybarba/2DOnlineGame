using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damageValue;
    private ulong ownerClientId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null) return;

        if(collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj)){
            if(ownerClientId == netObj.OwnerClientId) return;
        }

        if(collision.attachedRigidbody.TryGetComponent<Health>(out Health health)){
            health.TakeDamage(damageValue);
        }
    }

    public void SetOwner(ulong ownerClientId){
        this.ownerClientId = ownerClientId;
    }
}
