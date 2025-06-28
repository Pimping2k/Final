using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = Paths.PLAYER, fileName = "Stamina Config")]
    public class StaminaConfig : ScriptableObject
    {
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _sprintStaminaReduce;
        [SerializeField] private float _climbingStaminaReduce;
        [SerializeField] private float _badWeatherStaminaReduce;
        [SerializeField] private float _staminaRegenerationDelay;

        public float MaxStamina => _maxStamina;
        public float SprintStaminaReduce => _sprintStaminaReduce;
        public float ClimbingStaminaReduce => _climbingStaminaReduce;
        public float BadWeatherStaminaReduce => _badWeatherStaminaReduce;
        public float StaminaRegenerationDelay => _staminaRegenerationDelay;
    }
}