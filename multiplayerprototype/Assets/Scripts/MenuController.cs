using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MenuController : MonoBehaviour
{
    public Button HostButton;
    public Button JoinButton;
    public InputField IPInputField;
    private ushort port = 7777;

    public string nextSceneName = "SampleScene"; // Make sure this is spelled exactly like your scene name in Build Settings

    private void Start()
    {
        Debug.Log("MenuController started");

        HostButton.onClick.AddListener(OnHostClicked);
        JoinButton.onClick.AddListener(OnJoinClicked);
    }

    private void OnHostClicked()
    {
        Debug.Log("Host button clicked");

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("0.0.0.0", port);
        Debug.Log("Transport IP set to 0.0.0.0 and port to " + port);

        bool started = NetworkManager.Singleton.StartHost();
        Debug.Log("StartHost called. Success: " + started);

        if (started)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Already host right after StartHost. Loading scene: " + nextSceneName);
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Subscribing to OnServerStarted just in case...");
                NetworkManager.Singleton.OnServerStarted += OnHostStarted;
            }
        }
        else
        {
            Debug.LogError("StartHost failed");
        }
    }


    private void OnHostStarted()
    {
        Debug.Log("OnHostStarted triggered");

        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Confirmed as Host. Loading scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("OnServerStarted called, but not recognized as Host.");
        }
    }

    private void OnJoinClicked()
    {
        Debug.Log("Join button clicked");

        string ipAddress = IPInputField.text;
        if (string.IsNullOrEmpty(ipAddress))
        {
            Debug.LogWarning("IP Address is empty!");
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipAddress, port);
        Debug.Log("Transport IP set to " + ipAddress + " and port to " + port);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        bool started = NetworkManager.Singleton.StartClient();
        Debug.Log("StartClient called. Success: " + started);
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("OnClientConnected triggered. clientId = " + clientId);

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Confirmed as local client. Loading scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
