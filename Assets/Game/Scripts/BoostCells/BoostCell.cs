using Bird;
using UnityEngine;

namespace Boost
{
    public abstract class BoostCell : MonoBehaviour
    {
        private int _minNumber = 0;
        private int _maxNumber = 100;
        private int _percent = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Goose goose))
            {
                int random = Random.Range(_minNumber, _maxNumber);

                if (random <= _percent)
                    ApplyBoost(goose);
            }
        }

        public abstract void ApplyBoost(Goose goose);
    }
}
