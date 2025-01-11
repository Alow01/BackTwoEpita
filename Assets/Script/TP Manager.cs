using UnityEngine;

public class TPManager : MonoBehaviour
{
    GameObject[] players;

    public void GoMedieval()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 1")
            {
                p.transform.position = new Vector2(0, 0);
            }
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 2")
            {
                p.transform.position = new Vector2(70, 0);
            }
        }
    }

    public void GoEgypte()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 1")
            {
                p.transform.position = new Vector2(0, -106);
            }
            if (p.GetComponent<PlayerSetup>().playerRole == "Player 2")
            {
                p.transform.position = new Vector2(70, -115);
            }
        }
    }
}
