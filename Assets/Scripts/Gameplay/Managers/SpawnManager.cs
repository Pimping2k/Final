using UnityEngine;

namespace Gameplay.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        
        private void Start()
        {
            PlayerManager.Instance.transform.position = _spawnPoint.position;
        }
    }
}