using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private Transform projectSpawnPoint; // get rotation to know what direction to fire in
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private CoinWallet coinWallet;

    [Header("Settings")]
    [SerializeField, Range(0f, 100f)] private float projectileSpeed;
    [SerializeField, Range(0f, 10f)] private float fireRate;
    [SerializeField, Range(0f, 10f)] private float muzzleFlashDuration;
    [SerializeField, Range(0f, 10f)] private int costToFire;

    private bool shouldFire = false;
    private float muzzleFlashTimer;
    private float timer;

    // Connect and disconnect from events
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){return;}
        inputReader.OnPrimaryFireEvent += HandlePrimaryFire;
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){return;}
        inputReader.OnPrimaryFireEvent -= HandlePrimaryFire;
    }
    void Update()
    {
        // check for muzzle flash visibility. Everyone should see this, so it goes before the owner and shouldfire checks
        if(muzzleFlashTimer > 0f){
            muzzleFlashTimer -= Time.deltaTime;

            if(muzzleFlashTimer <= 0f){
                muzzleFlash.SetActive(false);
            }
        }

        if(!IsOwner){return;}

        if(timer > 0) timer -= Time.deltaTime;

        if(!shouldFire) return;

        if(timer > 0) return; // To stop cheating, we could validate this in the server. But since we're already trusting client position, we're going to trust this too.

        if(coinWallet.TotalCoins.Value < costToFire) return;

        PrimaryFireServerRpc(projectSpawnPoint.position, projectSpawnPoint.up);
        SpawnDummyProjectile(projectSpawnPoint.position, projectSpawnPoint.up);

        timer = 1/fireRate; // cooldown of fire rate
    }
    private void HandlePrimaryFire(bool fired){
        shouldFire = fired;
    }

/// <summary>
/// Spawns a visual representation of a projectile without any physics logic.
/// </summary>
/// <param name="spawnPosition">Where to spawn the projectile.</param>
/// <param name="spawnDirection">The direction of the projectile to shoot in.</param>
    private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 spawnDirection){
        // Start muzzle flash
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        // Spawn projectile visual
        FireProjectile(clientProjectilePrefab, spawnPosition, spawnDirection);
    }

    [ServerRpc] // the function must end in ServerRpc
    private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 spawnDirection){
        // get coin wallet from projectile 
        coinWallet.TrySpendCoins(costToFire);
        FireProjectile(serverProjectilePrefab, spawnPosition, spawnDirection);

        SpawnDummyProjectileClientRpc(spawnPosition, spawnDirection);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 spawnDirection){
        if(IsOwner) return;

        SpawnDummyProjectile(spawnPosition, spawnDirection);
    }

    private void FireProjectile(GameObject projectilePrefab, Vector3 spawnPosition, Vector3 spawnDirection){
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;

        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        if(projectile.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamageOnContact)){
            dealDamageOnContact.SetOwner(OwnerClientId);
        }
        
        if(projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)){
            rb.linearVelocity = rb.transform.up * projectileSpeed;
        }
    }
}
