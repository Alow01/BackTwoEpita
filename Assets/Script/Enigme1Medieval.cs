using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class Enigme1Medieval : NetworkBehaviour
{
    public float lerpSpeed;
    public GameObject porte;
    public List<GameObject> Lanterns;
#pragma warning disable CS0108 // Un membre masque un membre hérité ; le mot clé new est manquant
    public ParticleSystem particleSystem;
#pragma warning restore CS0108 // Un membre masque un membre hérité ; le mot clé new est manquant
    public List<GameObject> Pilliers;
    public ParticleSystem particleConsigne1;
    public ParticleSystem particleConsigne2;

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
    public AudioClip successAudio = null;
    public AudioClip failureAudio = null;
    private bool fini;
    public AudioSource audioSource;

    private void Start()
    {
        ordrePilliers = ordrePilliersRandom[Random.Range(0, ordrePilliersRandom.Count)];
        PilliersActive = new List<int>();
        fini = false;
        particleSystem.gameObject.SetActive(false);
        Debug.Log($"Ordre des pilliers : {string.Join(" ",ordrePilliers)}");
    }

   

    void Update()
    {
        if (!fini)
        {
            

            if (PilliersActive.Count >= 7)
            {
                if (PilliersActive.SequenceEqual(ordrePilliers)) // ordre bon : reussite de l'énigme
                {
                    //StartCoroutine(ClignoterPilliers()); // fait clignoter les pilliers 

                    RpcMoveDoor(); // ouvre la porte

                    Debug.Log("Succed enigma");
                    fini = true;
                    if (successAudio!=null)
                    {
                        audioSource.resource = successAudio;
                        //audioSource.Play(); // joue le son de reussite
                        //audioSource.Stop();
                    }

                    particleConsigne1.gameObject.SetActive(false); // desactive les systemes de particules des consignes
                    particleConsigne2.gameObject.SetActive(false);
                    particleSystem.gameObject.SetActive(true); // active le systeme de particule
                }
                else // ordre d'allumage faux
                {
                    Debug.Log("Failed enigma");
                    if (failureAudio != null)
                    {
                        audioSource.resource = failureAudio;
                        //audioSource.Play(); // joue le son d'echec
                        //audioSources.Stop();
                    } 
                    foreach (GameObject Pillier in Pilliers) // éteint les pilliers
                    {
                        Transform glowTransform = Pillier.transform.Find("Glow");
                        glowTransform.gameObject.SetActive(false);
                    }
                    PilliersActive = new List<int>();

                }
            }

          
        }
    }

    public void LightAPillar(string tag)
    {
        if (!fini)
        {
            foreach (GameObject Pillier in Pilliers)
            {
                if (Pillier.tag == tag)
                {
                    Transform glowTransform = Pillier.transform.Find("Glow");
                    int indexPillier = int.Parse(Pillier.tag.Split("Stone ")[1].Split(" ")[0]);
                    if (!glowTransform.gameObject.activeSelf)
                    {
                        Debug.Log($"Pillier {indexPillier} has been actived");
                        glowTransform.gameObject.SetActive(true);
                        PilliersActive.Add(indexPillier);
                    }
                    else
                    {
                        Debug.Log($"Pillier {indexPillier} has been desactived");
                        glowTransform.gameObject.SetActive(false);
                        PilliersActive.Remove(indexPillier);
                    }
                    Debug.Log($"The Pillar list is now : {string.Join(" ", PilliersActive)}");
                    return;
                }

            }
        }
    }

    [ClientRpc]
    void RpcMoveDoor()
    {
        if (porte != null)
        {
            porte.transform.position = new Vector3(7, 50, 0);
        }
    }
    /*
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
    */
}
