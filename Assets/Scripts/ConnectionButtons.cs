using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    public void StartClient(){
        NetworkManager.Singleton.StartClient();
        Debug.Log("Joining server...");
    }

    public void StartHost(){
        NetworkManager.Singleton.StartHost();
        Debug.Log("Joining as Host.");
    }
}
