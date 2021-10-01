using System;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerPowerUp : MonoBehaviour
    {
        public static event Action OnAddSpeedPowerUp;
        public static event Action OnRunPowerUp;
        public static event Action OnJumpPowerUp;
        public static event Action OnExtraJumpPowerUp;
        public static event Action OnRangePowerUp;
        public static event Action OnMeleePowerUp;
        public static event Action OnPotionPowerUp;

        public void AddSpeed()
        {
            OnAddSpeedPowerUp?.Invoke();
        }

        public void RunPowerUp()
        {
            OnRunPowerUp?.Invoke();
        }

        public void AddJumpForce()
        {
            OnJumpPowerUp?.Invoke();
        }

        public void AdditionalJumpsPowerUp()
        {
            OnExtraJumpPowerUp?.Invoke();
        }

        public void RangedPowerUp()
        {
            OnRangePowerUp?.Invoke();
        }

        public void AddAttackValue()
        {
            OnMeleePowerUp?.Invoke();
        }

        public void HealthPowerUp()
        {
            OnPotionPowerUp?.Invoke();
        }
    }
}
