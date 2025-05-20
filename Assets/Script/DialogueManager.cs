using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : NetworkBehaviour
{
    GameObject dialogueBox; // Le canvas de la bo�te de dialogue
    TMP_Text dialogueText;  // Le texte affich� dans la bo�te de dialogue
    GameObject objetAvecEnigme;
    List<int> OrdrePilliers;
    public List<List<int>> ordrePilliersRandom = new List<List<int>> {new List<int> {0,6,3,5,1,2,4},
                                                                      new List<int> {6,5,2,4,0,3,1},
                                                                      new List<int> {3,1,4,6,2,0,5},
                                                                      new List<int> {5,0,3,6,1,4,2},
                                                                      new List<int> {2,4,6,1,3,5,0},
                                                                      new List<int> {1,3,5,0,6,4,2},
                                                                      new List<int> {4,6,0,2,5,1,3},
                                                                     };

    GameObject MapGride;


    private void Start()
    {
        objetAvecEnigme= GameObject.FindWithTag("Enigme1"); 
        if (dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants
        // Cache le Canvas au départ
        

        MapGride = GameObject.FindGameObjectWithTag("MapGride");
        if (isLocalPlayer)
        {
            dialogueBox.SetActive(false);
            MapGride.SetActive(false);
        }
    }

    // M�thode RPC pour afficher le dialogue
    //[ClientRpc]
    public void RpcShowDialogue(string tag)
    {
        if (dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");

        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants
        

        if (isLocalPlayer && tag!="Untagged")
        {
            dialogueBox.SetActive(true);
            dialogueText.text = SelectMessage(tag);    // Affiche le message dans le texte
        }
    }

    // M�thode pour cacher le dialogue
    public void HideDialogue()
    {
        if (dialogueBox == null) dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        if (!dialogueBox.activeSelf && !MapGride.activeSelf) return;
        if (dialogueText == null) dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>(); // Recherche le TMP_Text dans les enfants


        if (isLocalPlayer)
        {
            dialogueBox.SetActive(false);
            if (MapGride == null) MapGride = GameObject.FindGameObjectWithTag("MapGride");
            MapGride.SetActive(false);
        }
    }

    private string SelectMessage(string tag)
    {
        
        if (OrdrePilliers == null)  OrdrePilliers = objetAvecEnigme.GetComponentInChildren<Enigme1Medieval>().ordrePilliers; 
       
        
        if (tag.Contains("Stone") || tag.Contains("stone")) // à afficher pour l'énigme 1 du medieval
        {
            switch (OrdrePilliers)  
            {
                 
                case List<int> l when l[0] == 0: // {0,6,3,5,1,2,4}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1369-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1362-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1363-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1372-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1359-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1370-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1356-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1352-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  // He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown. : prince
                        break;
                    }
                case List<int> l when l[0] == 1: //{1,3,5,0,6,4,2}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1355-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1358-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1363-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1371-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1390-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1374-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1370-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1364-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  // He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown. : prince
                        break;
                    }

                case List<int> l when l[0] == 2: // {2,4,6,1,3,5,0}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1369-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1366-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1370-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1365-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1370-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1363-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1366-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1375-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown."; // Prince Edric
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  
                        break;
                    }
                case List<int> l when l[0] == 3: // { 3,1,4,6,2,0,5}
                    { 
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1355-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1350-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1363-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1362-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1380-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1368-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1365-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1376-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown."; // Prince Edric
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  // 
                        break;
                    }
                case List<int> l when l[0] == 4: // {4,6,0,2,5,1,3}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1372-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1375-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1370-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1358-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1366-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1368-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1360-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1375-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown."; // Prince Edric
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  //  
                        break;
                    }
                case List<int> l when l[0] == 5: // {5,0,3,6,1,4,2}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1355-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1364-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1355-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1368-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1367-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1370-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1366-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1360-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown."; // Prince 
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "Ever faithful, he kept the king’s house safe-until betrayal dimmed his steadfast ligh"; // Lord Arin
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        } 
                        break;
                    }
                case List<int> l when l[0] == 6: // {6,5,2,4,0,3,1}
                    {
                        switch (tag)
                        {
                            case "Gravestone 6": return "Vernatius Willigher, 1377-1415 :\nHere lies Vernatius Willigher, Keeper of the Royal Library, Scholar at the Royal Academy and Seeker of Knowledge. His endless quest for wisdom illuminated the realm, yet even his vast knowledge could not uncover the betrayal that doomed the Kingdom";
                            case "Gravestone 5": return "General Radanh, 1375-1415 :\nStalwart Guardian of the Realm, Master Tactician of the Royal Army, whose wisdom and courage were felled by treachery's poison.";
                            case "Gravestone 2": return "Lord Aldric Blackthorn, 1361-1415 :\nRoyal Advisor to the Crown, Keeper of the King's Seal, whose wisdom was lost to the shadows of betrayal.";
                            case "Gravestone 8": return "Lord Arin, 1365-1415 :\nRoyal Steward to the Crown, Keeper of the King's Household, whose loyalty was unwavering until the shadows of betrayal claimed him.";
                            case "Gravestone 4": return "Prince Edric, 1371-1415 :\nCrown Prince and Knight of the Realm, whose bright future was extinguished in the flames of rebellion.";
                            case "Gravestone 7": return "Mastein Blackwood, 1365-1415 :\nChronicler of the King’s Deeds and his Kingdom’s Prosperity. He recorded an honorable and glorious reign but could not write its end";
                            case "Gravestone 1": return "Luther Esso, 1359-1415 :\nHere lies Luther Esso, Master of Treaties and Builder of Bridge. His words quelled storms and forged iron bonds, yet even his eloquence could not mend a kingdom torn apart.";
                            case "Gravestone 3": return "Princess Elara, 1372-1415 :\nSister to the King, Heart of the Realm, whose love for her family and people was boundless, even in death.";

                            // for 1srt enigma with pillar 
                            case "Stone 0 red": return "She was the kingdom’s heart, her love enduring beyond the grave, a beacon in the darkness."; // Princess Elara
                            case "Stone 1 red": return "His quest for truth lit the halls of learning, yet treachery flourished unseen in his shadow"; // Vernatius Willigher
                            case "Stone 2 red": return "He chronicled glory and sorrow, but the last page was stolen by fate’s cruel hand"; // Mastein Blackwood
                            case "Stone 3 red": return "His courage steered armies, but poison, not blade, ended his watch"; // General Radanh
                            case "Stone 4 red": return "He was the future of the realm, brave and noble—but rebellion claimed him before he could wear the crown."; // Prince
                            case "Stone 5 red": return "His words shaped the crown’s will, his wisdom a pillar of rule—yet even he could not advise against the shadow within."; // Lord Aldric Blackthorn
                            case "Stone 6 red": return "He forged peace with silvered tongue and steady hand, yet all his treaties turned to dust when unity crumbled."; // Luther Esso
                        }  
                        break;
                    }
            }
        }
        switch (tag)
        {
            case "EmptyChest": return "This Chest seems empty...";
            case "FailEgypteEnigma2": return "You FAILED, you must select each well in the correct order !";
            case "SuccessEgypteEnigma2": return "Congratulations, you achieved the enigma ! You have been teleported in the Medieval era !";
            case "ConsigneEgypteEnigma2": return "Your team-mate must find a magic well which will played different sound, you must select each well in the right order !";
            case "ConsigneMedievalEnigma1Perso1": return "A thick mist hangs over an old, forgotten graveyard. Weathered tombstones still bear the traces of lives long past. Names and dates carved into stone wait to be uncovered. You hear a distant voice echoes:\n“The past whispers its secrets. Unravel the threads of time… and help your partner awaken the forgotten.”";
            case "ConsigneMedievalEnigma1Perso2": return "In a mysterious sanctuary, glowing pillars lie dormant, waiting to be awakened. At their base, faint lanterns flicker, ready to be used. Each pillar bears a description of someone long gone — but their identity is incomplete. A soft voice whispers you:\n“Only the order of their arrival into the world will reignite their memory. But you cannot do it alone…”";
            case "ConsigneMedievalEnigma2": return "When the stone glows, the spirits awaken.\nTry to catch them — patience is the key.";
            case "Sign Egypte": return "Teleportation point to ancient Egypt";
            case "Sign Medieval": return "Teleportation point to the Middle Ages";
        }
        if (tag.Contains("Well")) return "You added this sound in the queue.";
        return "";

    }

 
    public void ShowMapGride()
    {
        if (MapGride == null) MapGride = GameObject.FindGameObjectWithTag("MapGride");
        if (isLocalPlayer) MapGride.SetActive(true);
    }

    public void OnQuitMapDBox()
    {
        if (isLocalPlayer)
        { 
            MapGride.SetActive(true);
            dialogueBox.SetActive(true);
        }
    }

}
