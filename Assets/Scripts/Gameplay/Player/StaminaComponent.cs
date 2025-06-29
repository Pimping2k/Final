using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Configs;

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
        
        private UniTaskCompletionSource<bool> _reductionCompletionSource;
        private UniTaskCompletionSource<bool> _regenerationCompletionSource;
        
        private CancellationTokenSource _reducingCancellationTokenSource;
        private CancellationTokenSource _regenerationCancellationTokenSource;
        
        public float CurrentStamina => _currentStamina;
        public StaminaConfig Config => _config;
        
        public event Action<float> ValueChanged;
        
        private void Awake()
        {
            _currentStamina = _config.MaxStamina;
            _reductionCompletionSource = new UniTaskCompletionSource<bool>();
            _regenerationCompletionSource = new UniTaskCompletionSource<bool>();
        }

        public async UniTask<bool> TryReduceStamina(StaminaReduceType type)
        {
            if (_reductionCompletionSource.Task.Status == UniTaskStatus.Pending)
            {
                _reductionCompletionSource.TrySetResult(false);
            }
            
            _reductionCompletionSource = new UniTaskCompletionSource<bool>();
            _reducingCancellationTokenSource = new CancellationTokenSource();
            var token = _reducingCancellationTokenSource.Token;
            
            _regenerationCompletionSource.TrySetResult(false);

            float reduceValue = GetStaminaReductionValue(type);
            try
            {
                while (_currentStamina > 0 && !_reductionCompletionSource.Task.Status.IsCompleted())
                {
                    ChangeStamina(-reduceValue);
                    if (_currentStamina <= 0)
                    {
                        _reductionCompletionSource.TrySetResult(false);
                        break;
                    }

                    await UniTask.WaitForSeconds(_config.ReduceTiming, cancellationToken: token);
                }
            }
            catch (OperationCanceledException e)
            {
            }

            return _currentStamina > 0;
        }

        public async void RegenerateStamina()
        {
            if (_regenerationCompletionSource.Task.Status == UniTaskStatus.Pending)
            {
                _regenerationCompletionSource.TrySetResult(false);
            }
            
            _regenerationCompletionSource = new UniTaskCompletionSource<bool>();
            _regenerationCancellationTokenSource = new CancellationTokenSource();
            var token = _regenerationCancellationTokenSource.Token;
            
            _reductionCompletionSource.TrySetResult(false);

            try
            {
                await UniTask.WaitForSeconds(_config.StaminaRegenerationDelay, cancellationToken: token);
            
                if (_regenerationCompletionSource.Task.Status.IsCompleted())
                    return;

                while (_currentStamina < _config.MaxStamina && !_regenerationCompletionSource.Task.Status.IsCompleted())
                {
                    ChangeStamina(_config.StaminaRegenerationValue);
                    if (_currentStamina >= _config.MaxStamina)
                    {
                        _regenerationCompletionSource.TrySetResult(true);
                        break;
                    }
                    await UniTask.WaitForSeconds(_config.RegenerationTiming, cancellationToken:token);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }

        public void CancelReducingStamina()
        {
            _reducingCancellationTokenSource?.Cancel();
            _reducingCancellationTokenSource?.Dispose();
            _reducingCancellationTokenSource = null;
        }

        public void CancelRegenerationStamina()
        {
            _regenerationCancellationTokenSource?.Cancel();
            _regenerationCancellationTokenSource?.Dispose();
            _regenerationCancellationTokenSource = null;
        }
        
        private float GetStaminaReductionValue(StaminaReduceType type)
        {
            return type switch
            {
                StaminaReduceType.Sprint => _config.SprintStaminaReduce,
                StaminaReduceType.Climbing => _config.ClimbingStaminaReduce,
                StaminaReduceType.BadWeather => _config.BadWeatherStaminaReduce,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void ChangeStamina(float value)
        {
            _currentStamina = Mathf.Clamp(_currentStamina + value, 0, _config.MaxStamina);
            ValueChanged?.Invoke(_currentStamina);
        }
    }
}