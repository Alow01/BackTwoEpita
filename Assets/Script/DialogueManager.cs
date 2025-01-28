using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : NetworkBehaviour
{
    GameObject dialogueBox; // Le canvas de la bo�te de dialogue
    TMP_Text dialogueText;  // Le texte affich� dans la bo�te de dialogue

    private void Start()
    {
        if(dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");

        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants
        

        // Cache le Canvas au d�part
        dialogueBox.SetActive(false);
    }

    // M�thode RPC pour afficher le dialogue
    [ClientRpc]
    public void RpcShowDialogue(string tag)
    {
        if (dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");

        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants
        

        if (isLocalPlayer)
        {
            dialogueBox.SetActive(true);
            dialogueText.text = SelectMessage(tag);    // Affiche le message dans le texte
        }
    }

    private string SelectMessage(string tag)
    {
        string res = "";
        switch (tag)
        {
            case "CRG": return "It is a Cross Grave";
            case "TAG": return "It is a Tall Grave";
            case "SHG": return "It is a Short Grave";
        }

        return res;
    }

    // M�thode pour cacher le dialogue
    public void HideDialogue()
    {
        if (dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        if (!dialogueBox.activeSelf) return;
        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants
        

        if (isLocalPlayer)
        {
            dialogueBox.SetActive(false);
        }
    }
}
