using Bird;
using UnityEngine;

namespace Boost
{
    public abstract class BoostCell : MonoBehaviour
    {
       [SerializeField] protected BoostConfig Config;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Goose goose))
            {
                int random = Random.Range(Config.MinNumber, Config.MaxNumber);

                if (random <= Config.Percent)
                    ApplyBoost(goose);
            }
        }

        public abstract void ApplyBoost(Goose goose);
    }
}
