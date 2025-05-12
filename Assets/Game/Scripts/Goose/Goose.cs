using System.Collections.Generic;
using UnityEngine;
using Figure;
using Common;

namespace Bird
{
    [RequireComponent(typeof(Mover), typeof(AnimationsGoose))]
    public class Goose : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _render;
        [SerializeField] private FigureDetector _figureDetector;
        [SerializeField] private float _attackDistance = 1.7f;
        [SerializeField] private AudioSource _soundBite;
        [SerializeField] private ParticleSystem _effectRemove;
        [SerializeField] private GooseConfig _config;

        private List<Transform> _movePoints = new List<Transform>();
        private ColorInitializer _colorInitializer;
        private Mover _mover;
        private AnimationsGoose _animations;
        private StartPositionCalculator _startPositionCalculator;
        private Attacker _attacker;
        private Stunner _stunner;

        private IPoolAdder<Goose> _poolAdder;
        private int _currentIndex;
        private int _correctionNumber = 1;
        private bool _isMovingForward = true;

        public Attacker Attacker => _attacker;

        private void Awake()
        {
            _colorInitializer = new ColorInitializer();
            _mover = GetComponent<Mover>();
            _animations = GetComponent<AnimationsGoose>();

            _startPositionCalculator = new StartPositionCalculator();
            _attacker = new Attacker(_attackDistance, _animations, _figureDetector, _mover, _config);
            _stunner = new Stunner(_mover, _animations, _config);
        }

        private void OnEnable()
        {
            _mover.TargetReached += OnSetNextTarget;
            _mover.Slowed += OnAnimationSlowSpeed;
            _attacker.Hited += PlaySoundAttack;
            _figureDetector.FigureRemoved += OnFigureRemoved;
        }

        private void OnDisable()
        {
            _mover.TargetReached -= OnSetNextTarget;
            _mover.Slowed -= OnAnimationSlowSpeed;
            _attacker.Hited -= PlaySoundAttack;
            _figureDetector.FigureRemoved -= OnFigureRemoved;
        }

        public void Remove()
        {
            Instantiate(_effectRemove, transform.position + new Vector3(0, _correctionNumber, 0), Quaternion.identity);
            _poolAdder.AddToPool(this);
        }

        public void Init(IPoolAdder<Goose> poolAdder) =>
            _poolAdder = poolAdder;

        public void SetColor(Color color) =>
            _colorInitializer.SetColor(_render, color);

        public void SetStartMovePoints(List<Transform> movePoints)
        {
            _movePoints = movePoints;
            _currentIndex = _startPositionCalculator.FindStartClosestPointIndex(_movePoints, transform.position);

            if (_currentIndex >= 0 && _currentIndex < _movePoints.Count)
                _mover.GoToTarget(_movePoints[_currentIndex]);

            _animations.TriggerRun();
        }

        public void TakeStun()
        {
            _attacker.StopAttack();
            _mover.StopBoostCoroutine();

            _stunner.Stun(() =>
            {
                _mover.GoToTarget(_movePoints[_currentIndex]);
                _animations.TriggerRun();
            });
        }

        private void OnFigureRemoved(TeleporterFigure figure)
        {
            if (_attacker.CurrentFigure != figure) 
                return;
            
            _attacker.StopAttack();

            if (_stunner.IsStunned) 
                return;
            
            _mover.GoToTarget(_movePoints[_currentIndex]);
            _animations.TriggerRun();
        }

        private void PlaySoundAttack()
        {
            _soundBite.Play();
        }

        private void OnAnimationSlowSpeed() =>
            _animations.TriggerRun();

        private void OnSetNextTarget()
        {
            _attacker.Attack(_colorInitializer.Color, transform, () => { MoveToNextPoint(); });
        }

        public void ApplySpeedBoost(int speedBoost, float duration)
        {
            _mover.ApplySpeedBoost(speedBoost, duration);
            _animations.TriggerRunFast();
        }

        public void ReverseDirection()
        {
            _isMovingForward = !_isMovingForward;
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            if (_isMovingForward)
                _currentIndex = (_currentIndex + _correctionNumber) % _movePoints.Count;
            else
                _currentIndex = (_currentIndex - _correctionNumber + _movePoints.Count) % _movePoints.Count;

            _mover.GoToTarget(_movePoints[_currentIndex]);
        }
    }
}