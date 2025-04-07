using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Collections.Generic;

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

    public Button refreshButton;
    public GameObject deviceEntryPrefab; 
    public Transform contentParent; 
    public int portToCheck = 7777;
    public int concurrentChecks = 10; // Number of IPs checked in parallel
    public float delayBetweenBatches = 0.1f; 

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
        }
        else if (NetworkClient.isConnected)
        {
            networkManager.StopClient();
            statusText.text = "Disconnected from the Server";
        }
        stopButton.SetActive(false);

    }

    public void RefreshIPList()
    {
        StartCoroutine(ScanNetworkBatched());
    }


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
}
