using System;
using System.Collections;
using Common;
using UnityEngine;
using YG;

namespace Bird
{
    public class Stunner
    {
        private GooseConfig _config;
        private Mover _mover;
        private AnimationsGoose _animations;
        private float _stun;
        private WaitForSeconds _stunTime;
        private Coroutine _stunCoroutine;
        
        public Stunner(Mover mover, AnimationsGoose animations, GooseConfig config)
        {
            _config = config;
            int extraTime = YandexGame.savesData.ExtraStun * _config.MultiplierStun;
            _mover = mover;
            _animations = animations;
            
            _stun = _config.Stun;
            _stun += extraTime;
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