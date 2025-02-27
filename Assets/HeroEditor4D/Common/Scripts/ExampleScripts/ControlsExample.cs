using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
	/// <summary>
	/// An example of how to handle user input, play animations and move a character.
	/// </summary>
	public class ControlsExample : MonoBehaviour
	{
        public Character4D Character;
        public FirearmFxExample FirearmFx;
        public bool InitDirection;
        public int MovementSpeed;

        private bool _moving;

        public void Start()
        {
            Character.AnimationManager.SetState(CharacterState.Idle);

            if (InitDirection)
            {
                Character.SetDirection(Vector2.down);
            }
        }

        public void Update()
        {
            SetDirection();
            Move();
            ChangeState();
            Actions();
        }

        private void SetDirection()
        {
            Vector2 direction;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector2.down;
            }
            else return;

            Character.SetDirection(direction);
        }

        private void Move()
        {
            if (MovementSpeed == 0) return;

            var direction = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction += Vector2.left;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                direction += Vector2.right;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                direction += Vector2.up;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                direction += Vector2.down;
            }

            if (direction == Vector2.zero)
            {
                if (_moving)
                {
                    Character.AnimationManager.SetState(CharacterState.Idle);
                    _moving = false;
                }
            }
            else
            {
                Character.AnimationManager.SetState(CharacterState.Run);
                Character.transform.position += (Vector3) direction.normalized * MovementSpeed * Time.deltaTime;
                _moving = true;
            }
        }

        private void Actions()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                switch (Character.WeaponType)
                {
                    case WeaponType.Melee1H:
                    case WeaponType.Paired:
                        Character.AnimationManager.Slash(twoHanded: false);
                        break;
                    case WeaponType.Melee2H:
                        Character.AnimationManager.Slash(twoHanded: true);
                        break;
                    case WeaponType.Bow:
                        Character.AnimationManager.ShotBow();
                        break;
                    case WeaponType.Crossbow:
                        Character.AnimationManager.CrossbowShot();
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                Character.AnimationManager.Jab();
            }
            else if (Input.GetKey(KeyCode.F))
            {
                if (Character.AnimationManager.IsAction) return;

                switch (Character.WeaponType)
                {
                    case WeaponType.Crossbow:
                        Character.AnimationManager.CrossbowShot();
                        break;
                    case WeaponType.Firearm1H:
                    case WeaponType.Firearm2H:
                    {
                        Character.AnimationManager.Fire();

                        if (Character.Parts[0].PrimaryWeapon != null)
                        {
                            var firearm = Character.SpriteCollection.Firearm1H.SingleOrDefault(i => i.Sprites.Contains(Character.Parts[0].PrimaryWeapon))
                                ?? Character.SpriteCollection.Firearm2H.SingleOrDefault(i => i.Sprites.Contains(Character.Parts[0].PrimaryWeapon));

                            if (firearm != null)
                            {
                                FirearmFx.CreateFireMuzzle(firearm.Name, firearm.Collection);
                            }
                        }

                        break;
                    }
                    case WeaponType.Paired:
                        Character.AnimationManager.SecondaryShot();
                        break;
                }
            }
        }

        private void ChangeState()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Character.AnimationManager.SetState(CharacterState.Idle);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Character.AnimationManager.SetState(CharacterState.Ready);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Character.AnimationManager.SetState(CharacterState.Walk);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Character.AnimationManager.SetState(CharacterState.Run);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                Character.AnimationManager.SetState(CharacterState.Jump);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Character.AnimationManager.SetState(CharacterState.Climb);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Character.AnimationManager.Die();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Character.AnimationManager.Hit();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                Character.AnimationManager.SetState(CharacterState.ShieldBlock);
            }
            else if (Input.GetKeyUp(KeyCode.B))
            {
                Character.AnimationManager.SetState(CharacterState.Idle);
            }
        }

        public void TurnLeft()
		{
            Character.SetDirection(Vector2.left);
		}

		public void TurnRight()
		{
            Character.SetDirection(Vector2.right);
		}

		public void TurnUp()
		{
            Character.SetDirection(Vector2.up);
		}

		public void TurnDown()
		{
            Character.SetDirection(Vector2.down);
		}

        public void Show4Directions()
        {
            Character.SetDirection(Vector2.zero);
		}

        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }
	}
}