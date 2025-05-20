using Cainos.PixelArtTopDown_Basic;
using Mirror;
using UnityEngine;

public class TPManager : MonoBehaviour
{
    GameObject[] players;
    public GameObject tilemapObject;

    public void GoLobby()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 1")
            {
                p.transform.position = new Vector2(114, -28);
            }
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 2")
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
        }
        

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(this.name == "ToE"){

            if (collision.gameObject.TryGetComponent<PlayerSetup>(out var T)) {

                if (T.playerRole == "Player 1") T.transform.position = new Vector2(2, -105);
                else T.transform.position = new Vector2(70, -130);
            }
        }

        if (this.name == "ToM")
        {

            if (collision.gameObject.TryGetComponent<PlayerSetup>(out var T)) {

                if (T.playerRole == "Player 2") T.transform.position = new Vector2(0, 0);
                else T.transform.position = new Vector2(70, 0);
            }
        }

    }


}
