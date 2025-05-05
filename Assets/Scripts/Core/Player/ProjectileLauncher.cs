using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private Transform projectSpawnPoint; // get rotation to know what direction to fire in
    [SerializeField] private InputReader inputReader;
    [Header("Settings")]
    [SerializeField] private float projectileSpeed;

    private bool shouldFire = false;

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
        if(!IsOwner){return;}
        if(!shouldFire) return;
        
        PrimaryFireServerRpc(projectSpawnPoint.position, projectSpawnPoint.up);
        SpawnDummyProjectile(projectSpawnPoint.position, projectSpawnPoint.up);

    }
    private void HandlePrimaryFire(bool fired){
        shouldFire = fired;
    }

    private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 spawnDirection){
        GameObject projectile = Instantiate(clientProjectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;
    }

    [ServerRpc] // the function must end in ServerRpc
    private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 spawnDirection){
        GameObject projectile = Instantiate(serverProjectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.up = spawnDirection;
        SpawnDummyProjectileClientRpc(spawnPosition, spawnDirection);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 spawnDirection){
        if(IsOwner) return;

        SpawnDummyProjectile(spawnPosition, spawnDirection);
    }
}
