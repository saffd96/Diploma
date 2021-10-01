using System;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(PlayerVfx))]
    public class PlayerItemEffects : MonoBehaviour
    {
        public bool IsInvulnerableItem;
        public bool IsShieldEnabled;

        private PlayerVfx playerVfx;

        public static event Action<int> OnStonesPickup;
        public static event Action<int> OnPotionPickup;
        public static event Action<float> OnInvulnerablePickup;

        private void Start()
        {
            playerVfx = GetComponent<PlayerVfx>();
        }

        public void CheckShield()
        {
            playerVfx.Shield.SetActive(IsShieldEnabled);
            playerVfx.InvulnerableItemShield.SetActive(IsInvulnerableItem);
        }

        public void AddShieldItem()
        {
            IsShieldEnabled = true;
            AudioManager.Instance.PLaySfx(SfxType.ShieldActive);
        }

        public void AddStonesItem(int amount)
        {
            OnStonesPickup?.Invoke(amount);
        }

        public void PotionItem(int amount)
        {
            OnPotionPickup?.Invoke(amount);
        }

        public void InvulnerableItem(float time)
        {
            IsInvulnerableItem = true;
            OnInvulnerablePickup?.Invoke(time);
        }
    }
}
