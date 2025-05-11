using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.CurrentHealth.Value); // Set it manually the first time, in case the health has chagned before subscribing
    }

    public override void OnNetworkDespawn()
    {
        if(!IsClient) return;
        health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    // Network variables always sync the new and old values,. so any handle functions need to take both parameters
    private void HandleHealthChanged(int oldHealth, int newHealth){
        healthBarImage.fillAmount = (float) newHealth/health.MaxHealth;
    }
}
