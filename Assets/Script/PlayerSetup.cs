using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerSetup : NetworkBehaviour
{
    [SyncVar]
    public string playerRole;

    [SerializeField]
    Behaviour[] notLocalPlayer;

    Camera playerCamera;
    GameObject defaultCam;



    //  MENU
    public GameObject menuPanel;
    public Button menuButton;

    private bool isMenuOpen = false;

    void Start()
    {
        
        if (!isLocalPlayer)
        {
            foreach(var component in notLocalPlayer)
            {
                component.enabled = false;
            }
        }
        else
        {
            playerCamera = Camera.main;
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
            }
            defaultCam = GameObject.FindWithTag("DefaultCamera");

            if (defaultCam != null)
            {
                defaultCam.gameObject.SetActive(false);
            }

        }
        //menuPanel = GameObject.FindGameObjectWithTag("Menu");
        //menuButton = GameObject.FindGameObjectWithTag("MenuButton").GetComponent<Button>();
        //menuPanel.SetActive(false);
        //menuButton.onClick.AddListener(ToggleMenu);
    }

    private void OnDisable()
    {
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
        }
        if (defaultCam != null)
        {
            defaultCam.gameObject.SetActive(true);
        }
        //Menu.SetActive(true);

    }

    public override void OnStartServer()
    {
        if (isServer) playerRole = "Player 1";
    }

    public override void OnStartClient()
    {
        if (!isServer) playerRole = "Player 2";
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
    }
}
