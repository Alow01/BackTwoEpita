using System;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Common.Springs;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
	/// <summary>
	/// Used to play animations.
	/// </summary>
	public class AnimationManager : MonoBehaviour
	{
		public Character4D Character;
		public Animator Animator;

        public bool IsAction
        {
            get => Animator.GetBool("Action");
            set => Animator.SetBool("Action", value);
        }

		/// <summary>
		/// Set animation parameter State that controls transition.
		/// </summary>
		public void SetState(CharacterState state)
		{
			Animator.SetInteger("State", (int) state);
		}

	    /// <summary>
        /// Play Attack animation according to selected weapon.
        /// </summary>
		public void Attack()
		{
			switch (Character.WeaponType)
			{
				case WeaponType.Melee1H:
				case WeaponType.Melee2H:
					Slash1H();
					break;
				case WeaponType.Bow:
					ShotBow();
					break;
				default:
					//throw new NotImplementedException("This feature may be implemented in next updates.");
                    Slash1H();
                    break;
			}
		}

        /// <summary>
        /// Play Slash1H animation.
        /// </summary>
		public void Slash1H()
		{
			Animator.SetTrigger("Slash1H");
            IsAction = true;
        }

	    /// <summary>
	    /// Play Slash2H animation.
	    /// </summary>
	    public void Slash2H()
	    {
	        Animator.SetTrigger("Slash2H");
            IsAction = true;
		}

	    public void Slash(bool twoHanded)
	    {
	        Animator.SetTrigger(twoHanded ? "Slash2H" : "Slash1H");
            IsAction = true;
		}

        /// <summary>
        /// Play Jab animation.
        /// </summary>
        public void Jab()
        {
            Animator.SetTrigger("Jab");
            IsAction = true;
		}

        /// <summary>
        /// Play Slash1H animation.
        /// </summary>
        public void HeavySlash1H()
        {
            Animator.SetTrigger("HeavySlash1H");
            IsAction = true;
		}
        
	    /// <summary>
	    /// Play PowerStab animation.
	    /// </summary>
	    public void FastStab()
	    {
	        Animator.SetTrigger("FastStab");
            IsAction = true;
		}

        /// <summary>
        /// Play Shot animation (bow).
        /// </summary>
		public void ShotBow()
		{
			Animator.SetTrigger("ShotBow");
            IsAction = true;
		}

        /// <summary>
        /// Play Death animation.
        /// </summary>
	    public void Die()
	    {
	        SetState(CharacterState.Death);
	    }

        /// <summary>
        /// Play Hit animation. This will just scale character up and down.
        /// Hit will not break currently playing animation, for example you can Hit character while it's playing Attack animation.
        /// </summary>
	    public void Hit()
	    {
	        Animator.SetTrigger("Hit");
        }

	    public void ShieldBlock()
	    {
	        SetState(CharacterState.ShieldBlock);
        }

	    public void WeaponBlock()
	    {
	        SetState(CharacterState.WeaponBlock);
	    }

	    public void Evade()
	    {
	        Animator.SetTrigger("Evade");
        }

	    public void SetTwoHanded(bool twoHanded)
	    {
	        Animator.SetBool("TwoHanded", twoHanded);
        }

	    public void SetWeaponType(WeaponType weaponType)
	    {
	        Animator.SetInteger("WeaponType", (int) weaponType);
	    }

        public void Fire()
        {
            Animator.SetTrigger("Fire");
            IsAction = true;
        }

		public void SecondaryShot()
        {
            Animator.SetTrigger("SecondaryShot");
            IsAction = true;
        }

        public void CrossbowShot()
        {
            Animator.SetTrigger("Fire");
            IsAction = true;

            if (Character.Parts[0].PrimaryWeapon != null)
            {
                var loaded = Character.Front.CompositeWeapon.Single(i => i.name == "Gun");
                var empty = Character.Front.CompositeWeapon.Single(i => i.name == "GunEmpty");

                foreach (var part in Character.Parts)
                {
                    part.PrimaryWeaponRenderer.sprite = part.PrimaryWeaponRenderer.sprite == loaded ? empty : loaded;
                }
            }
		}

		/// <summary>
		/// Alternative way to Hit character (with a script).
		/// </summary>
		public void Spring()
	    {
            ScaleSpring.Begin(this, 1f, 1.1f, 40, 2);
        }
    }
}