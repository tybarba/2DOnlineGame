using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField, Range(1, 200)] public int MaxHealth {get; private set;} = 100;
    // network variables are only modifiable by the server
    // clients are told when it's been changed
    [SerializeField]
    public NetworkVariable<int> CurrentHealth = new ();
    private bool isDead = false;
    public Action<Health> OnDeath;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue){
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue){
        ModifyHealth(healValue);
    }

    public void ModifyHealth(int value){
        if(isDead) return;
        var resultHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(resultHealth, 0, MaxHealth);
        if(CurrentHealth.Value < 1){
            isDead = true;
            OnDeath?.Invoke(this);
        }
    }
}
