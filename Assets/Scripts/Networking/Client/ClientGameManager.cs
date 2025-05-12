using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string menuSceneName = "Menu";
    public async Task<bool> InitAsync(){
        await UnityServices.InitializeAsync(); // connect to unity services first

        AuthState authState = await AuthenticationWrapper.DoAuth();

        if(authState == AuthState.Authenticated) return true;

        return false;
    }

    public void GoToMenu(){
        SceneManager.LoadScene(menuSceneName);
    }
}
