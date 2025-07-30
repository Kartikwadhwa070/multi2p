using UnityEngine;

public class ConnectUIscript : MonoBehaviour
{
    void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    void Update()
    {
        
    }

    private void HostButtonOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void ClientButtonOnClick()
    {
        NetowrkManager.Singleton.StartClient(); 
    }

}
