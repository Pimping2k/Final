using System;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public enum StaminaReduceType
    {
        Sprint,
        Climbing,
        BadWeather
    }

    public class StaminaComponent : MonoBehaviour
    {
        [SerializeField] private StaminaConfig _config;
        
        private float _currentStamina;

        private void Awake()
        {
            _currentStamina = _config.MaxStamina;
        }

        public async void ReduceStamina(StaminaReduceType type)
        {
            switch (type)
            {
                case StaminaReduceType.Sprint:
                    ChangeStamina(_config.SprintStaminaReduce, false);
                    break;
                case StaminaReduceType.Climbing:
                    ChangeStamina(_config.ClimbingStaminaReduce, false);
                    break;
                case StaminaReduceType.BadWeather:
                    ChangeStamina(_config.ClimbingStaminaReduce, false);
                    break;
            }

            await UniTask.Yield();
        }

        private void ChangeStamina(float value, bool isIncreasing)
        {
            _currentStamina = isIncreasing
                ? Mathf.Clamp(_currentStamina + value, 0, _config.MaxStamina)
                : Mathf.Clamp(_currentStamina - value, 0, _config.MaxStamina);
        }
    }
}