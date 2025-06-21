using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkLauncher : MonoBehaviour
{
    public ushort port = 7777; 

    public void StartHost()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = "0.0.0.0";
        transport.ConnectionData.Port = port;

        NetworkManager.Singleton.StartHost();
    }

    public void StartClient(string ipAddress)
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
        transport.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();
    }
}
