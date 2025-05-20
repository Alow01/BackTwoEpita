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
    public List<List<GameObject>> Zones_feux_follets;
    public int NumeroTentativeToucherFeuFollet;
    public bool IsPlay;
    public GameObject feu_follet_de_depart; // contient le 1er feu follet qui va apparaitre pour le joueur
    private GameObject feu_follet_actif;
    private int ZoneFeuFolletActif;
    public float DistanceFuite = 1.5f;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NumeroTentativeToucherFeuFollet = 0;
        IsPlay = false;

        foreach(List<GameObject> zone in Zones_feux_follets) // éteind initialement tous les feux follets
        {
            foreach (GameObject feu_follet in zone)
            {
                feu_follet.SetActive(false);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsPlay = true;
        feu_follet_actif = feu_follet_de_depart;
        feu_follet_actif.SetActive(true);
        ZoneFeuFolletActif = 0;
    }



    void Update()
    {
        if (IsPlay)
        {
            if (player != null && feu_follet_actif != null)
            {
                float distance = Vector3.Distance(feu_follet_actif.transform.position, player.transform.position);
               
                if (distance <= DistanceFuite)
                {
                    feu_follet_actif.SetActive(false);
                    NumeroTentativeToucherFeuFollet++;
                    if (NumeroTentativeToucherFeuFollet >= 7)
                    {
                        IsPlay = false;
                        Debug.Log("Fin de l'énigme");
                        return;
                    }

                    List<int> ZonesPossibles= new List<int>();
                    switch (ZoneFeuFolletActif)
                    {
                        case 0:
                            ZonesPossibles = new List<int>() { 3, 2, 0 };
                            break;
                        case 1:
                            ZonesPossibles = new List<int>() { 4, 5, 6, 7 };
                            break;
                        case 2:
                            ZonesPossibles = new List<int>() { 3, 4, 5 };
                            break;
                        case 3:
                            ZonesPossibles = new List<int>() { 4, 5, 6, 7 };
                            break;
                        case 4:
                            ZonesPossibles = new List<int>() { 4, 5, 6, 7 };
                            break;
                        case 5:
                            ZonesPossibles = new List<int>() { 1, 2, 6 };
                            break;
                        case 6:
                            ZonesPossibles = new List<int>() { 8, 3, 2,0 };
                            break;
                        case 7:
                            ZonesPossibles = new List<int>() { 7,1,4,0,2 };
                            break;
                        case 8:
                            ZonesPossibles = new List<int>() { 0,3,4,6 };
                            break;
                        case 9:
                            ZonesPossibles = new List<int>() { 5,6,4,3 };
                            break;
                    }
                    ZoneFeuFolletActif = ZonesPossibles[Random.Range(0, ZonesPossibles.Count)]; // zone d'apparition élougnée aléatoire 
                    int randomFeuFollet = Random.Range(0, Zones_feux_follets[ZoneFeuFolletActif].Count);
                    feu_follet_actif = Zones_feux_follets[ZoneFeuFolletActif][randomFeuFollet];
                    feu_follet_actif.SetActive(true);

                }
            }
        }
    }
}
