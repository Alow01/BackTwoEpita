using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Enigme2Medieval : MonoBehaviour
{
    public List<GameObject> feu_foleys;
    public int i;
    public bool IsPlay;
    private Stopwatch chrono;
    public long interval = 10000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        i = 0;
        IsPlay = false;
        chrono = new Stopwatch();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // afficher lancement enigme

        chrono.Start();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (chrono.ElapsedMilliseconds >= 1000)
        {
            IsPlay = true; // si le perso est resté + de 2 sec sur le trigger, on lance l'énigme
            ResetChrono();
            i = 0;
            feu_foleys[i].gameObject.SetActive(true); // on active le premier feu foley
        }
        else
        {
            IsPlay = false; // sinon on ne lance pas l'énigme
            chrono.Stop();
        }
    }

    public void ResetChrono()
    {
        chrono.Reset(); // Reset the chrono
        chrono.Start(); // Restart the chrono
    }

    void Update()
    {
        if (IsPlay)
        {
            if (chrono.ElapsedMilliseconds > interval) // trop long à touché le feu foley : perdu
            {
                IsPlay = false; // échec de l'enigme
                feu_foleys[i].gameObject.SetActive(false); // on désactive le feu foley
                i = 0;
                chrono.Stop();
                foreach(GameObject feu in feu_foleys)
                {
                    feu.gameObject.GetComponentInChildren<Feu_foley>().isTouch = false ; // on désactive tous les feux foleys
                }
            }
            else if (feu_foleys[i].gameObject.GetComponentInChildren<Feu_foley>().isTouch)
            {
                chrono.Stop();
                feu_foleys[i].gameObject.SetActive(false);
                i++;
                if (i >= feu_foleys.Count)
                {
                    IsPlay = false; // fin de l'enigme
                }
                else
                {
                    feu_foleys[i].gameObject.SetActive(true);
                    ResetChrono(); // Reset the chrono
                }
            }
        }
    }
    
}
