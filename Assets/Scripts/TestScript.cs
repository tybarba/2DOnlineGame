using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private InputReader inputReader;
    private void Awake()
    {
        inputReader.OnMoveEvent += HandleMove;
        inputReader.OnPrimaryFireEvent += HandlePrimaryFire;
    }
    void Start()
    {
        
    }

    // Always unsubscribe from events to avoid memory leaks
    private void OnDestroy() {
        inputReader.OnMoveEvent -= HandleMove;
        inputReader.OnPrimaryFireEvent -= HandlePrimaryFire;
    }

    private void HandleMove(Vector2 movement)
    {
        Debug.Log($"Movement: {movement}");
    }
    private void HandlePrimaryFire(bool performed)
    {
        Debug.Log($"Primary Fire: {performed}");
    }
}
