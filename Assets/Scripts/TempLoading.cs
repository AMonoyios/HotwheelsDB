using UnityEngine;
using PD.Logger;
using PD.Networking;
using UnityEngine.SceneManagement;

public class TempLoading : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = -50.0f;
    [SerializeField]
    private float checkConnectionIntervals = 10.0f;
    [SerializeField, Range(0, 100)]
    private int maxConnectionChecks = 20;

    private void OnValidate()
    {
        if (checkConnectionIntervals < 0.0f)
            checkConnectionIntervals = 0.0f;
    }

    private void Start()
    {
        CoreRequest.Init();

        CoreRequest.CheckConnection("https://hotwheels.fandom.com/wiki/Hot_Wheels",
                                    checkConnectionIntervals,
                                    maxConnectionChecks,
                                    (string error) => CoreLogger.LogMessage($"Failed to establish connection: {error}"),
                                    () => LoadApp());
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
