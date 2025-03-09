using Mirror;
using UnityEngine;

public class EgSymbManager : MonoBehaviour
{
    public GameObject case1;
    public GameObject case2;
    public GameObject case3;
    public GameObject case4;
    public GameObject case5;

    public GameObject door;
    private bool hasBeenOpen = false;

    BoxCollider2D col1;
    BoxCollider2D col2;
    BoxCollider2D col3;
    BoxCollider2D col4;
    BoxCollider2D col5;

    private void Start()
    {
        

    }
    void Update()
    {
        if (!hasBeenOpen) {

            col1 = case1.GetComponent<BoxCollider2D>();
            col2 = case2.GetComponent<BoxCollider2D>();
            col3 = case3.GetComponent<BoxCollider2D>();
            col4 = case4.GetComponent<BoxCollider2D>();
            col5 = case5.GetComponent<BoxCollider2D>();

            Collider2D hit1 = Physics2D.OverlapBox(col1.bounds.center, col1.bounds.size, 0f);
            Collider2D hit2 = Physics2D.OverlapBox(col2.bounds.center, col2.bounds.size, 0f);
            Collider2D hit3 = Physics2D.OverlapBox(col3.bounds.center, col3.bounds.size, 0f);
            Collider2D hit4 = Physics2D.OverlapBox(col4.bounds.center, col4.bounds.size, 0f);
            Collider2D hit5 = Physics2D.OverlapBox(col5.bounds.center, col5.bounds.size, 0f);

            bool condition = hit1 != null && hit1.gameObject != case1
                && hit2 != null && hit2.gameObject != case2
                && hit3 != null && hit3.gameObject != case3
                && hit4 != null && hit4.gameObject != case4
                && hit5 != null && hit5.gameObject != case5;

            if (condition) {
                Debug.Log("All conditions are reunite !");
                hasBeenOpen = true;
                door.SetActive(false);
            }
        }
    }
}
