using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<bool> OnPrimaryFireEvent;
    public Vector2 AimPosition {get; private set;}
    private Controls controls;
    private void OnEnable(){
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        OnMoveEvent?.Invoke(movement);
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        OnPrimaryFireEvent?.Invoke(context.performed);
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void OnDestroy()
    {
        controls.Player.Disable();
        controls.Player.SetCallbacks(null);
        controls = null;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        // we are setting it to a variable that we can read instead of an event, because the number of times this will be triggered makes it more performant this way
        AimPosition = context.ReadValue<Vector2>();
    }
}
