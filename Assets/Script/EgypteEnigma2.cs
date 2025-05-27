using UnityEngine;
using System;
using System.Collections.Generic;
using Mirror;

public class EgypteEnigma2 : NetworkBehaviour
{

    public Queue<string> q;
    [SyncVar]
    public bool hasCompletedEnygma;
    private DialogueManager dialogueManager;

    

    void Start()
    {
        q = new Queue<string>();
        hasCompletedEnygma = false;
    }

    void Update()
    {
        if (!hasCompletedEnygma)
        {
            if (q.Count >= 3)
            {
                dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
                if (q.Dequeue() == "WellTambourin" && q.Dequeue() == "WellFlute" && q.Dequeue() == "WellHarpe")
                {
                    hasCompletedEnygma = true;
                }
                else
                {
                    dialogueManager.RpcShowDialogue("FailEgypteEnigma2");
                    Debug.Log("Failed enigma");
                }
                q = new Queue<string>();

                if (hasCompletedEnygma)
                {

                    Debug.Log("Succed Egypte Enigma 2 !");


                    AudioSource audioSource = GetComponent<AudioSource>();
                    if (audioSource != null) audioSource.Play();

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

    public void AddToQueue(string str)
    {
        Debug.Log($"{str} was added to the queue");
        q.Enqueue(str);
    }
}
