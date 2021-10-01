using Unity.Mathematics;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerVfx : MonoBehaviour
    {
        public GameObject AttackVfx;
        public GameObject Shield;
        public GameObject UnableShieldVfx;
        public GameObject InvulnerableItemShield;
        public GameObject RunVfx;
        public GameObject ExtraJumpVfx;
        public GameObject DustFromRun;

        private PlayerAnimationController playerAnimationController;

        private void Start()
        {
            playerAnimationController = GetComponent<PlayerAnimationController>();
        }

        public void SetActiveDustFromRun()
        {
            if (DustFromRun != null)
            {
                DustFromRun.SetActive(
                    playerAnimationController.GetSpeed() > 0.5f && playerAnimationController.GetGrounded());
            }
        }

        public void CreateJumpVfx()
        {
            if (ExtraJumpVfx != null)
            {
                Instantiate(ExtraJumpVfx, transform.position, quaternion.identity);
            }
        }
    }
}
