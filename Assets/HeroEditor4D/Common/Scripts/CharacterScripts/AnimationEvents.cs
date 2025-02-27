using System;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
	/// <summary>
	/// Animation events. If you want to get animation callback, use it.
	/// For example, if you want to know exact hit moment for attack animation, use custom event 'Hit' that is fired in most attack animations.
	/// </summary>
	public class AnimationEvents : MonoBehaviour
    {
		/// <summary>
		/// Subscribe it to get animation callback.
		/// </summary>
		public event Action<string> OnEvent = s => { };

        /// <summary>
        /// Set trigger.
        /// </summary>
        public void SetTrigger(string triggerName)
        {
            GetComponent<Animator>().SetTrigger(triggerName);
        }

        /// <summary>
        /// Set bool param, usage example: Idle=false
        /// </summary>
        public void SetBool(string value)
        {
            var parts = value.Split('=');

            GetComponent<Animator>().SetBool(parts[0], bool.Parse(parts[1]));
        }

		/// <summary>
		/// Set integer param, usage example: WeaponType=2
		/// </summary>
		public void SetInteger(string value)
        {
            var parts = value.Split('=');

            GetComponent<Animator>().SetInteger(parts[0], int.Parse(parts[1]));
        }

	    /// <summary>
	    /// Called from animation.
	    /// </summary>
	    public void CustomEvent(string eventName)
	    {
		    OnEvent(eventName);
	    }

	    /// <summary>
	    /// Set characters' expression. Called from animation.
	    /// </summary>
		public void SetExpression(string expression)
	    {
		    GetComponent<Character4D>().Parts.ForEach(i => i.SetExpression(expression));
		}
	}
}