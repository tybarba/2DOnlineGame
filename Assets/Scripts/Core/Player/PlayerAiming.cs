using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    // InputReader
    [SerializeField] private InputReader inputReader;
    // Turret that we're gonna rotate
    [SerializeField] private Transform turretTransform;

    // check where mouse is 
    // find where turret is
    // rotate turn to aim at cursor

    // this happens after normal updates
    // we're rotating the turret here to avoid jitter by allowing the tank movement to be processed first
    private void LateUpdate()
    {
        if(!IsOwner) return;

        Vector2 cursorPosition = inputReader.AimPosition;
        Vector2 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorPosition);

        turretTransform.up = new Vector2(cursorWorldPosition.x - turretTransform.position.x, cursorWorldPosition.y - turretTransform.position.y);
    }
}
