using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;
    private HostGameManager gameManager;
    public static HostSingleton Instance{
        get{
            if( instance != null){ return instance;} // if it's already been instantiated, provide the object
            if(!instance){ // if not 
                instance = FindAnyObjectByType<HostSingleton>(); // find it
            }
            if(instance == null){ // if it's still null, log error and return null
                Debug.LogError("No HostSingleton in the scene.");
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

    public void CreateHost(){
        gameManager = new HostGameManager();
    }
}
