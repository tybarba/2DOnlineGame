using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent<Coin>(out Coin coin)) return;

        int coinValue = coin.Collect();

        if(!IsServer) return;

        TotalCoins.Value += coinValue;
    }

    public bool TrySpendCoins(int coins){
        if(TotalCoins.Value >= coins){
            TotalCoins.Value -= coins;
            return true;
        }
        return false;
    }

}
