using Common;
using System;
using System.Collections;
using UnityEngine;
using YG;

namespace Bird
{
    public class Stunner
    {
        private Mover _mover;
        private AnimationsGoose _animations;
        private float _stun = 5;
        private WaitForSeconds _stunTime;
        private Coroutine _stunCoroutine;

        public Stunner(Mover mover, AnimationsGoose animations)
        {
            int multiplier = 2;
            int extraTime = YandexGame.savesData.ExtraStun * multiplier;
            _stun += extraTime;
            _mover = mover;
            _animations = animations;

            _stunTime = new WaitForSeconds(_stun);
        }

        public bool IsStunned { get; private set; }

        public void Stun(Action onStunComplete)
        {
            StopCoroutineStun();

            IsStunned = true;
            _mover.GoToTarget(null);
            _animations.TriggerStun();

            _stunCoroutine = _mover.StartCoroutine(ResumeMovementAfterStun(onStunComplete));
        }

        private void StopCoroutineStun()
        {
            if (_stunCoroutine != null)
            {
                _mover.StopCoroutine(_stunCoroutine);
                _stunCoroutine = null;
            }
        }

        private IEnumerator ResumeMovementAfterStun(Action onStunComplete)
        {
            yield return _stunTime;

            IsStunned = false;
            onStunComplete?.Invoke();
        }
    }
}