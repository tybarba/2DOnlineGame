using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] float lifeTimeInSeconds = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTimeInSeconds);
    }

}
