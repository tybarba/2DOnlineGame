using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private const int MaxConnections = 20;
    private const string ConnectionType = "udp";
    private const string SceneName = "Game";
    public async Task InitAsync()
    {
        // Authenticate player
    }

    public async Task StartHostAsync()
    {
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(MaxConnections);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, ConnectionType);
            unityTransport.SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            Debug.Log("Join code: " + joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }
}
