using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        bool isDedicatedServer = SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;
        await LaunchInMode(isDedicatedServer);
    }

    private async Task LaunchInMode(bool isDedicatedServer){
        if(isDedicatedServer){

        }else{
            //spawn in client singleton
            ClientSingleton clientSingleton = Instantiate(clientPrefab);
            await clientSingleton.CreateClient();
            // spawn inhost singleton
            HostSingleton hostSingleton = Instantiate(hostPrefab);
            hostSingleton.CreateHost();
        }
    }
}
