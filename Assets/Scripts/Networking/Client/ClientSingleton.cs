using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance;
    private ClientGameManager gameManager;
    public static ClientSingleton Instance{
        get{
            if( instance != null){ return instance;} // if it's already been instantiated, provide the object
            if(!instance){ // if not 
                instance = FindAnyObjectByType<ClientSingleton>(); // find it
            }
            if(instance == null){ // if it's still null, log error and return null
                Debug.LogError("No client singleton in the scene.");
                return null;
            }
            return instance; // finally return the instance
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }

    public async Task CreateClient(){
        gameManager = new ClientGameManager();
        await gameManager.InitAsync();
    }
}
