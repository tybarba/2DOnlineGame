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
        SpawnDummyProjectile();
    }
    private void HandlePrimaryFire(bool fired){
        shouldFire = fired;
    }

    private void SpawnDummyProjectile(){
        GameObject projectile = Instantiate(clientProjectilePrefab, projectSpawnPoint.position, Quaternion.identity);
        projectile.transform.up = projectSpawnPoint.up;
    }
}
