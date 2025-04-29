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
            case "Gravestone 1": return "Vernatius Willigher, 1355-1415 :\n  Here lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
            case "Gravestone 2": return "General Radanh, 1366-1415 :\n Stalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
            case "Gravestone 3": return "Lord Aldric Blackthorn, 1363-1415 :\n Royal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
            case "Gravestone 4": return "Lord Arin, 1365-1415 :\n Royal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
            case "Gravestone 5": return "Prince Edric, 1390-1415 :\n Crown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
            case "Gravestone 6": return "Mastein Blackwood, 1368-141 :\n Chronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
            case "Gravestone 7": return "Luther Esso, 1372-1415 :\n Here lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
            case "Gravestone 8": return "Princess Elara, 1375-1415 :\n Sister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

            // for 1srt enigma with pillar 
            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness.";
            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow";
            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand";
            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch";
            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh";
            case "Stone 5 red": return "???";
            case "Stone 6 red": return "???";


            case "EmptyChest": return "This Chest seems empty...";

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
