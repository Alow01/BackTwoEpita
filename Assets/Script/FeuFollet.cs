using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class FeuFollet : NetworkBehaviour
{
    public int hasBeenHitCounter;
    public bool isCaptured;
    public bool IsPlaying;
    public int willBeCapturedAfter;

    public float detectionRange;

    public Enigme2Medieval enigmaManager;

    private List<(Vector2,uint)> posL = new List<(Vector2,uint)>
    {
        (new Vector2(-4,12),3),
        (new Vector2(-6,-4),2),
        (new Vector2(4,-4),2),
        (new Vector2(-9,-6),1),
        (new Vector2(5,-12),1),
        (new Vector2(10,9),2),
        (new Vector2(13,1),1),
        (new Vector2(-2,16),1),
        (new Vector2(-2,0),1),
        (new Vector2(12,-8),1)
    };

    private List<(Vector2, uint)> posR = new List<(Vector2, uint)>
    {
        (new Vector2(55,5),2),
        (new Vector2(64,-4),2),
        (new Vector2(61,-8),1),
        (new Vector2(77,-8),1),
        (new Vector2(81,-1),2),
        (new Vector2(85,3),1),
        (new Vector2(79,10),2),
        (new Vector2(76,3),1),
        (new Vector2(67,1),1),
    };

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
        if (!isServer || !IsPlaying) return;

        if (hasBeenHitCounter >= willBeCapturedAfter)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            float distance = Vector2.Distance(transform.position, p.transform.position);
            
            if (distance <= detectionRange)
            {
                hasBeenHitCounter++;
                Debug.Log("Detected player: " + p.name);
                TeleportAway();

                // Call Rpc on the player’s DialogueManager
                DialogueManager dm = p.GetComponent<DialogueManager>();
                if (dm != null)
                {
                    dm.RpcShowDialogue("ff enfui");
                }

            }
        }
    }

    private void TeleportAway()
    {
        if(this.name == "ffL")
        {
            (Vector2,uint) coord = posL[Random.Range(0,posL.Count)];
            while(new Vector3(coord.Item1.x,coord.Item1.y,0) == this.transform.position) coord = posL[Random.Range(0, posL.Count)];

            this.transform.position = coord.Item1;

            this.gameObject.layer = LayerMask.NameToLayer($"Layer {coord.Item2}");

            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = $"Layer {coord.Item2}";
            SpriteRenderer[] srs = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = $"Layer {coord.Item2}";
            }
        }
        else
        {
            (Vector2, uint) coord = posR[Random.Range(0, posR.Count)];
            while (new Vector3(coord.Item1.x, coord.Item1.y, 0) == this.transform.position) coord = posR[Random.Range(0, posR.Count)];

            this.transform.position = coord.Item1;

            this.gameObject.layer = LayerMask.NameToLayer($"Layer {coord.Item2}");

            this.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = $"Layer {coord.Item2}";
            SpriteRenderer[] srs = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                sr.sortingLayerName = $"Layer {coord.Item2}";
            }
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
