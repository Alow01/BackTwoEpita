using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Mirror;
using Unity.VisualScripting;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : NetworkBehaviour
    {
        public float speed;

        private Animator animator;
        public InputAction MoveAction;
        public InputAction InteractAction;
        public GameObject projectilePrefab;
        Vector2 direction = new Vector2(0,0);
        
        Rigidbody2D rigidbody2d;

        public DialogueManager dialogueManager; // Référence au DialogueManager
        public string dialogueMessage = "Test";
        
        EgypteEnigma2 wellM;

        void FindObject()
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, direction, 0.5f, LayerMask.GetMask("Layer 4"));
            
            if (hit.collider != null)
            {
                //Debug.Log("Raycast has hit the object " + hit.collider.gameObject.tag);
                OpenDialogue(hit.collider.gameObject.tag);
            }
            
          
        }

       

        private void Start()
        {
            animator = GetComponent<Animator>();
            InteractAction.Enable();
            MoveAction.Enable();
            rigidbody2d = GetComponent<Rigidbody2D>();
            wellM = GameObject.FindGameObjectWithTag("WellM").GetComponent<EgypteEnigma2>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                dir.x = -1;
                direction=new Vector2(-1,0);
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                dir.x = 1;
                direction=new Vector2(1,0);
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                dir.y = 1;
                direction=new Vector2(0,1);
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                dir.y = -1;
                direction=new Vector2(0,-1);
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().linearVelocity = speed * dir;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindObject();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CloseDialogue();
            }
        }

        private void OpenDialogue(string tag)
        {
            if (isLocalPlayer)
            {
                if (tag == "EgypteEnigma1Map")
                {
                    dialogueManager.ShowMapGride();
                    return;
                }
                if (tag.Contains("Well")) WellM(tag);
                dialogueManager.RpcShowDialogue(tag);
            }
        }

        private void CloseDialogue()
        {
            if (isLocalPlayer)
            {
                dialogueManager.HideDialogue();
            }
        }

        

        public void WellM(string tag)
        {
            wellM.AddToQueue(tag);
        }

        private void OnDisable()
        {
            dialogueManager.OnQuitMapDBox();
        }
    }
}
