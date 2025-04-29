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
        

        // Cache le Canvas au départ
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
            case "CRG": return "“Vernatius Willigher, 1355-1415 ;  Here lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom”";
            case "TAG": return "It is a Tall Grave";
            case "SHG": return "It is a Short Grave";

            // for 1srt enigma with pillar and cypher keys
            case "Stone 1 red": return "Amongst the stones, a soft heavenly voice whispers of guidance for the lost. Perhaps your companion has heard of a similar tale? But her guidance will only befall those who are worthy. \n ♖ = +3";
            case "Stone 2 red": return "“An ancient king, his face lost to the ages, is said to have mastered a secret language. He twisted words, rearranging their letters, to reveal a hidden truth only those loyal can decipher. Even now, the whispers of the wind carry echoes of his boast, 'Veni, Vidi, Vici'. It is said that the whisperer’s message, too, is veiled in this ancient tongue, its meaning concealed by a similar shift in the essence of the words itself” \n ♕ = +5";
            case "Stone 3 red": return "♘  = -4";
            case "Stone 4 red": return "♔ = -2";
            case "Stone 5 red": return "♘♖ = -1";

          /**  case "Stone 1 blue": return "“♘ SDANA | ♔ ♕ WKH | ♖GHDG | ♕ XQJJUX” \n “♕♖ AMMS |♘♖ NBY |♘♕ DPMVNOT | ♕♕ YP |♖♖ ROMNZ” \n
            A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";**/
            case "Stone 2 blue": return "g";

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
