using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = Paths.PLAYER +"Movement Config", fileName = "Movement Config")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _cameraClampValue;
        
        public float CameraClampValue => _cameraClampValue;
        public float RotationSpeed => _rotationSpeed;
        public float MovementSpeed => _movementSpeed;
    }
}