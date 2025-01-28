using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SyncVar]
    public string playerRole;

    [SerializeField]
    Behaviour[] notLocalPlayer;

    Camera playerCamera;
    GameObject defaultCam;

    GameObject Menu;

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
        //Menu = GameObject.FindGameObjectWithTag("Menu");
        //Menu.SetActive(false);
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

}
