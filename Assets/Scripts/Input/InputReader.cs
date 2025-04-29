using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<bool> OnPrimaryFireEvent;
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
}
