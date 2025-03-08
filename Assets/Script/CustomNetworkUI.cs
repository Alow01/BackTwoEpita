using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Assets.HeroEditor4D.Common.Scripts.Common;

public class CustomNetworkUI : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button joinButton;
    public Button stopButton;
    public TMP_Text statusText;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        stopButton.onClick.AddListener(StopNetwork);
    }

    void StartHost()
    {
        networkManager.StartHost();
        statusText.text = "Server Active...";
        stopButton.SetActive(true);
    }

    void StartClient()
    {
        networkManager.networkAddress = ipInput.text;
        networkManager.StartClient();
        statusText.text = "Connect to " + ipInput.text;
        stopButton.SetActive(true);
    }

    void StopNetwork()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            networkManager.StopHost();
            statusText.text = "Server is Down";
            stopButton.SetActive(false);
        }
        else if (NetworkClient.isConnected)
        {
            networkManager.StopClient();
            statusText.text = "Disconnected from the Server";
            stopButton.SetActive(false);
        }
    }
}
