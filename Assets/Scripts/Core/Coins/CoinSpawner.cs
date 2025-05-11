using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin coinPrefab;
    // wher
    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;
    //range on x axis and y axis
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask layerMask; // does a physics check to make sure that coins can be placed
    private float coinRadius;
    private Collider2D[] coinBuffer = new Collider2D[1];
    private Vector2 noSpaceResult = new Vector2(0,0);

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;

        // spawn coins to be visible to players
        for(int i = 0; i < maxCoins; i++){
            SpawnCoin();
        }
    }

    private void SpawnCoin(){
        //instantiate the prefab with the position where you want to put it
        Vector2 spawnPoint = GetCoinSpawnPoint();
        if(spawnPoint == noSpaceResult) return;

        RespawningCoin coinInstance = Instantiate(coinPrefab, GetCoinSpawnPoint(), Quaternion.identity);
        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();
        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetCoinSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetCoinSpawnPoint(){
        float x = 0;
        float y = 0;
        int numTries = 100;
        Vector2 spawnPoint = new Vector2(0,0);
        // randomly draw a circle on the map
        while(numTries > 0){
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            spawnPoint = new Vector2(x, y);
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(layerMask);
            int numColliders = Physics2D.OverlapCircle(spawnPoint, coinRadius, filter, coinBuffer);
            if(numColliders == 0){
                break;
            }
            numTries--;
        }
        // check if it's a safe space to spawn the coin (not spawning inside another object)
        return spawnPoint;
    }
}
