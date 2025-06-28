using System;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Managers
{
    public class PlayerManager: MonoBehaviour
    {
        public static PlayerManager Instance;

        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private StaminaComponent _staminaComponent;
        [SerializeField] private MovementComponent _movementComponent;

        public HealthComponent HealthComponent => _healthComponent;
        public StaminaComponent StaminaComponent => _staminaComponent;
        public MovementComponent MovementComponent => _movementComponent;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}