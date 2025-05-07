using UnityEngine;
using System;

public class RespawningCoin : Coin
{
    private Vector3 previousPosition;
    public event Action<RespawningCoin> OnCollected;

    void Start()
    {
        previousPosition = this.gameObject.transform.position;
    }

    void Update()
    {
        if(IsServer) return;

        if(gameObject.transform.position != previousPosition){
            Show(true);
            previousPosition = gameObject.transform.position;
        }
    }
    public override int Collect()
    {
        if(!IsServer){
            Show(false);
            return 0;
        }

        if(alreadyCollected) return 0;

        alreadyCollected = true;

        OnCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }

    
}
