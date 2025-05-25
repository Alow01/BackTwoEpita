using Mirror;
using UnityEngine;
using UnityEngine.Audio;

public class Enigme2Medieval : NetworkBehaviour
{
    public GameObject feuFL;
    public GameObject feuFR;


    public AudioClip successAudio = null;
    public AudioClip failureAudio = null;

    private bool IsPlaying;

    [SyncVar] public bool isCapturedfeuFL = false;
    [SyncVar] public bool isCapturedfeuFR = false;

    [Server]
    public void SetLeftTrue()
    {
        isCapturedfeuFL = true;
    }

    [Server]
    public void SetRightTrue()
    {
        isCapturedfeuFR = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsPlaying = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPlaying)
        {
            if (collision.gameObject.tag == "Player")
            {
                IsPlaying = true;
                feuFL.transform.position = new Vector3 (4, 14, 0);
                feuFR.transform.position = new Vector3(66, 12,0);


                feuFL.GetComponent<FeuFollet>().IsPlaying = true;
                feuFR.GetComponent<FeuFollet>().IsPlaying = true;

                Debug.Log("Medieval enigma 2 is starting");
            }
        }

    }



    void Update()
    {
        if (IsPlaying)
        {

            if(isCapturedfeuFL && isCapturedfeuFR)
            {
                Debug.Log("Succed Medieval Enigma 2 !");
                
                IsPlaying=false;

                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource!=null)audioSource.Play();

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject p in players)
                {
                    if (p.GetComponent<PlayerSetup>().playerRole == "Player 1")
                    {
                        p.transform.position = new Vector2(114, -28);
                    }
                    else
                    {
                        p.transform.position = new Vector2(117, -28);
                    }
                    //change le layer du joueur en layer 1
                    p.gameObject.layer = LayerMask.NameToLayer("Layer 1");

                    p.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 1";
                    SpriteRenderer[] srs = p.gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sr in srs)
                    {
                        sr.sortingLayerName = "Layer 1";
                    }
                    DialogueManager dialogueManager = p.GetComponent<DialogueManager>();
                    dialogueManager.RpcShowDialogue("SuccessEgypteEnigma2");
                }
                
            }
        }
    }

}
