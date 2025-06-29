using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = Paths.PLAYER + "Stamina Config", fileName = "Stamina Config")]
    public class StaminaConfig : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _reduceTiming;
        [SerializeField] private float _regenerationTiming;
        [Header("Reduce settings")]
        [SerializeField] private float _sprintStaminaReduce;
        [SerializeField] private float _climbingStaminaReduce;
        [SerializeField] private float _badWeatherStaminaReduce;
        [Header("Regeneration settings")]
        [SerializeField] private float _staminaRegenerationValue;
        [SerializeField] private float _staminaRegenerationDelay;

        public float MaxStamina => _maxStamina;
        public float SprintStaminaReduce => _sprintStaminaReduce;
        public float ClimbingStaminaReduce => _climbingStaminaReduce;
        public float BadWeatherStaminaReduce => _badWeatherStaminaReduce;
        public float StaminaRegenerationValue => _staminaRegenerationValue;
        public float StaminaRegenerationDelay => _staminaRegenerationDelay;
        public float ReduceTiming => _reduceTiming;
        public float RegenerationTiming => _regenerationTiming;
    }
}
