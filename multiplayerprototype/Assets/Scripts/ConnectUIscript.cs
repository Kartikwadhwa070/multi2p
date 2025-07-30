using UnityEngine;
using UnityEngine.UI;                      // For Button
using Unity.Netcode;                      // For NetworkManager

public class ConnectUIscript : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    private void HostButtonOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void ClientButtonOnClick()
    {
        NetworkManager.Singleton.StartClient(); // Typo fixed from "NetowrkManager"
    }
}
