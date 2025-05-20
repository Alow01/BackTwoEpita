using UnityEngine;
using System;
using System.Collections.Generic;

public class EgypteEnigma2 : MonoBehaviour
{

    public Queue<string> q;
    public bool hasCompletedEnygma;
    private DialogueManager dialogueManager;

    public TPManager TPManager;

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
                    dialogueManager.RpcShowDialogue("SuccessEgypteEnigma2");
                }
                else
                {
                    dialogueManager.RpcShowDialogue("FailEgypteEnigma2");
                    Debug.Log("Failed enigma");
                }
                q = new Queue<string>();

                if (hasCompletedEnygma)
                {
                    TPManager.GoLobby();
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
