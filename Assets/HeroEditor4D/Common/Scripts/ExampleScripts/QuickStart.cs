using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    /// <summary>
    /// A small helper used in Quick Start scene.
    /// </summary>
    public class QuickStart : MonoBehaviour
    {
        public List<Character4D> CharacterPrefabs;
        public ControlsExample ControlsExample;
        public EquipmentExample EquipmentExample;
        public AppearanceExample AppearanceExample;
        public InventoryExample InventoryExample;

        public static string ReturnSceneName;

        public void Awake()
        {
            var character = Instantiate(CharacterPrefabs.First(i => i != null));

            character.transform.position = Vector2.zero;

            ControlsExample.Character = character;
            EquipmentExample.Character = character;
            AppearanceExample.Character = character;
            InventoryExample.Character = character;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && ReturnSceneName != null)
            {
                SceneManager.LoadScene(ReturnSceneName);
            }
        }
    }
}