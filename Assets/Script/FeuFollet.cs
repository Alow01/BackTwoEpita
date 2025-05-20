using Mirror;
using UnityEngine;

public class FeuFollet : NetworkBehaviour
{
    public int hasBeenHitCounter;
    public bool isCaptured;
    public bool IsPlaying;
    public int willBeCapturedAfter;

    public Enigme2Medieval enigmaManager;

    void Start()
    {
        hasBeenHitCounter = 0;
        isCaptured = false;
        IsPlaying = false;
        willBeCapturedAfter = 3 + Random.Range(0,4);

        //this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsPlaying)
        {

        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                collision.gameObject.GetComponent<DialogueManager>().RpcShowDialogue("Waiting for the other player");
                isCaptured = true;
                IsPlaying = false;

                if (this.name == "ffL") collision.gameObject.GetComponent<PlayerSetup>().CmdTouchLeft();
                else collision.gameObject.GetComponent<PlayerSetup>().CmdTouchRight();

                Debug.Log($"{this.name} has been touched by the player");
            }
        }
    }

    

}
