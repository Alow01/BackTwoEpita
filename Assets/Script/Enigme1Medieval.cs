using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Enigme1Medieval : MonoBehaviour
{
    public float lerpSpeed;
    public GameObject porte;


    public List<GameObject> Pilliers;

    public List<List<int>> ordrePilliersRandom = new List<List<int>> {new List<int> {0,6,3,5,1,2,4},
                                                                      new List<int> {6,5,2,4,0,3,1},
                                                                      new List<int> {3,1,4,6,2,0,5},
                                                                      new List<int> {5,0,3,6,1,4,2},
                                                                      new List<int> {2,4,6,1,3,5,0},
                                                                      new List<int> {1,3,5,0,6,4,2},
                                                                      new List<int> {4,6,0,2,5,1,3},
                                                                     };

    public List<int> ordrePilliers;
    public List<int> PilliersActive;

    private bool fini;

    private void Start()
    {
        ordrePilliers = ordrePilliersRandom[Random.Range(0, ordrePilliersRandom.Count)];
        PilliersActive= new List<int>();
        fini = false;
        

    }

   

    void Update()
    {
        if (!fini)
        {
            int i = 0;
            foreach (GameObject Pillier in Pilliers) // ajoute a la liste les pilliers dans l'ordre de leur activation
            {
                Transform glowTransform = Pillier.transform.Find("Glow");
                if ( glowTransform.gameObject.activeSelf)   
                {
                    if (!PilliersActive.Contains(i))
                    {
                        Debug.Log("Pillier " + i + " is active");
                        PilliersActive.Add(i);
                    }
                }
                else
                {
                    if (PilliersActive.Contains(i)) PilliersActive.Remove(i);
                }
                i++;
            }

            if (PilliersActive.Count >= 7)
            {
                if (PilliersActive.SequenceEqual(ordrePilliers)) // ordre bon : reussite de l'énigme
                {
                    StartCoroutine(ClignoterPilliers()); // fait clignoter les pilliers 
                    porte.SetActive(false); // ouvre la porte
                    fini = true;
                }
                else // ordre d'allumage faux
                {
                    foreach (GameObject Pillier in Pilliers) // éteint les pilliers
                    {
                        Transform glowTransform = Pillier.transform.Find("Glow");
                        glowTransform.gameObject.SetActive(false);
                    }
                }
            }

          
        }
    }

    private IEnumerator ClignoterPilliers()
    {
        yield return new WaitForSeconds(5f);
        for (int j = 0; j < 5; j++) // fait clignoter 5 fois
        {
            foreach (GameObject Pillier in Pilliers)
            {
                Transform glowTransform = Pillier.transform.Find("Glow");
                glowTransform.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.5f); // attend 0.5 seconde

            foreach (GameObject Pillier in Pilliers)
            {
                Transform glowTransform = Pillier.transform.Find("Glow");
                glowTransform.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f); // attend 0.5 seconde
        }
    }
}
