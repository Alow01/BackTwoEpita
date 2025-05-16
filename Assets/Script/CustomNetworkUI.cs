using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using Mirror.Discovery;

public class CustomNetworkUI : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button joinButton;
    public Button stopButton;
    public TMP_Text statusText;

    public Button refreshButton;
    public GameObject deviceEntryPrefab; 
    public Transform contentParent; 
    public NetworkDiscovery networkDiscovery;
    private Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        stopButton.onClick.AddListener(StopNetwork);
        refreshButton.onClick.AddListener(RefreshIPList);
    }

    void StartHost()
    {
        networkManager.StartHost();
        statusText.text = "Server Active...";
        stopButton.gameObject.SetActive(true);
        networkDiscovery.AdvertiseServer();
    }

    void StartClient()
    {
        networkManager.networkAddress = ipInput.text;
        networkManager.StartClient();
        statusText.text = "Connect to " + ipInput.text;
        stopButton.gameObject.SetActive(true);
    }

    void StopNetwork()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            networkManager.StopHost();
            statusText.text = "Server is Down";
        }
        else if (NetworkClient.isConnected)
        {
            networkManager.StopClient();
            statusText.text = "Disconnected from the Server";
        }
        stopButton.gameObject.SetActive(false);

    }

    public void RefreshIPList()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        if (!discoveredServers.ContainsKey(info.serverId))
        {
            discoveredServers[info.serverId] = info;
            Debug.Log("Found server: " + info.EndPoint);

            GameObject entry = Instantiate(deviceEntryPrefab, contentParent);
            entry.GetComponentInChildren<TMP_Text>().text = $"IP: {info.EndPoint.Address}";

            // You can automatically connect to the first one
            //networkManager.networkAddress = info.EndPoint.Address.ToString();
            //networkManager.StartClient();
        }
    }


    /*
    IEnumerator ScanNetworkBatched()
    {
        string baseIp = "192.168.1.";
        List<Coroutine> activeCoroutines = new List<Coroutine>();

        for (int i = 1; i <= 254; i++)
        {
            Debug.Log("Starting to check :" + baseIp + i + " on port : " + portToCheck);
            string ip = baseIp + i;
            Coroutine c = StartCoroutine(CheckIP(ip, portToCheck));
            activeCoroutines.Add(c);

            // When batch size is reached, wait for a bit
            if (activeCoroutines.Count >= concurrentChecks)
            {
                yield return new WaitForSeconds(delayBetweenBatches);
                activeCoroutines.Clear(); // Let Unity manage coroutine cleanup
            }
        }
    }

    IEnumerator CheckIP(string ip, int port)
    {
        bool isOpen = false;

        using (TcpClient client = new TcpClient())
        {
            var result = client.BeginConnect(ip, port, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(System.TimeSpan.FromMilliseconds(100));
            if (success)
            {
                Debug.Log($"Connect finished for {ip}, connected: {client.Connected}");
            }
            if (success && client.Connected)
            {
                isOpen = true;
            }
        }

        if (isOpen)
        {
            GameObject entry = Instantiate(deviceEntryPrefab, contentParent);
            entry.GetComponentInChildren<TMP_Text>().text = $"IP: {ip}";
            Debug.Log($"Found Open IP: {ip}");
        }
        else Debug.Log("No Port : "+ port + " open on IP :" + ip);
        yield return null;
    }
    */
}
