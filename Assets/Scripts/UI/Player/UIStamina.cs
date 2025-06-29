using System;
using Gameplay.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player
{
    public class UIStamina : MonoBehaviour
    {
        [SerializeField] private Image _staminaBar;
        
        private PlayerManager _player;

        private void Start()
        {
            _player = PlayerManager.Instance;
            _player.StaminaComponent.ValueChanged += OnStaminaValueChanged;
         
            OnStaminaValueChanged(_player.StaminaComponent.CurrentStamina);
        }

        private void OnDestroy()
        {
            _player.StaminaComponent.ValueChanged -= OnStaminaValueChanged;
        }

        private void OnStaminaValueChanged(float value)
        {
            _staminaBar.fillAmount = value / _player.StaminaComponent.Config.MaxStamina;
        }
    }
}