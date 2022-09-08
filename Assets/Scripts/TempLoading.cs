using UnityEngine;
using SW.Logger;
using SW.Networking;
using UnityEngine.SceneManagement;

public class TempLoading : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = -50.0f;

    private void Start()
    {
        CoreNetworking.Init();

        CoreNetworking.CheckConnection("https://hotwheels.fandom.com/wiki/Hot_Wheels",
                                    CoreNetworking.GetConnectionRetryDelta,
                                    CoreNetworking.GetMaxConnectionRetry,
                                    onError: null /*show no connection error popup*/,
                                    onSuccess: () => LoadApp());
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void LoadApp()
    {
        // Loads main app scene.
        SceneManager.LoadScene(1);
    }
}
