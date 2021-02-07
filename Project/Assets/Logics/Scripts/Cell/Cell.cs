using System;
using UnityEngine;

namespace Combine
{
    public abstract class Cell : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            [Header("Edge length of quad cell.")]
            public float CellSize;
        }

        // Biome visible in inspector.
        [SerializeField] private BiomeType _biome;

        public abstract CellType Type { get; }

        public BiomeType Biome => _biome;

        public void ResetObject()
        {
            OnResetObject();
        }

        public virtual void PlayerCollisionEnter(PlayerCharacter player) { }

        public virtual void PursuerCollisionEnter(Pursuer pursuer) { }

        protected virtual void OnResetObject() { }
    }
}