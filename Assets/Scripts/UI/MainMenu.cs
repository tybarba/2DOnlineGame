using System.Threading.Tasks;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public async void StartHost()
    {
        // find host singleton
        HostSingleton host = HostSingleton.Instance;
        // get its game manager
        HostGameManager gameManager = host.GameManager;
        // call the host option on that manager
        await gameManager.StartHostAsync();
    }
}
