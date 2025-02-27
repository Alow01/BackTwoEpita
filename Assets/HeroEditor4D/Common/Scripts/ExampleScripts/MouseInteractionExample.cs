using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    public class MouseInteractionExample : MonoBehaviour
    {
        public Character4D Character;

        public void Start()
        {
            var firearm = Character.SpriteCollection.Firearm2H.Single(i => i.Name == "MP5L");

            Character.Equip(firearm, EquipmentPart.Firearm2H);
            Character.AnimationManager.SetState(CharacterState.Ready);
            Character.SetDirection(Vector2.down);
        }

        // We need LateUpdate() to override Animation transitions.
        public void LateUpdate()
        {
            SetDirection();
            RotateFirearm();
        }

        private void SetDirection()
        {
            var position = (Vector2) Camera.main.WorldToScreenPoint(Character.Active.AnchorBody.position);
            var angle = Vector2.SignedAngle((Vector2) Input.mousePosition - position, Vector2.up);
            var direction = Vector2.down;

            if (angle > -45 && angle <= 45) direction = Vector2.up;
            else if (angle > 45 && angle <= 135) direction = Vector2.right;
            else if (angle > -135 && angle <= 45) direction = Vector2.left;

            Character.SetDirection(direction);
        }

        private void RotateFirearm()
        {
            var armL = Character.Active.BodyRenderers.Single(i => i.name == "ArmL").transform;
            var armR = Character.Active.BodyRenderers.Single(i => i.name == "ArmR").transform;
            var weapon = Character.Active.PrimaryWeaponRenderer.transform;
            var rotation = GetArmRotation(armR);

            weapon.Rotate(0, 0, rotation * 2 / 4);
            rotation = GetArmRotation(armR);
            armL.Rotate(0, 0, rotation);
            armR.Rotate(0, 0, rotation);
        }

        private float GetArmRotation(Transform armR) // You should be a geometry ninja to understand this.
        {
            var muzzle = Character.Active.AnchorFireMuzzle;
            var alpha = Vector2.SignedAngle(armR.position - muzzle.position, muzzle.up);
            var magnitude = muzzle.InverseTransformPoint(armR.position).magnitude;
            var elbow = (Vector2) muzzle.TransformPoint(0, magnitude * Mathf.Cos(alpha * Mathf.Deg2Rad), 0);
            var angle = Vector2.SignedAngle((Vector2) muzzle.position - elbow, (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - elbow);

            return angle;
        }
    }
}