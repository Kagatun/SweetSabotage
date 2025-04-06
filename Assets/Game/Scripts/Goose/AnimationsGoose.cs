using UnityEngine;

public class AnimationsGoose : MonoBehaviour
{
    private static class AnimationParams
    {
        public static readonly int Run = Animator.StringToHash("Run");
        public static readonly int Run2 = Animator.StringToHash("Run2");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Stun = Animator.StringToHash("Stun");
    }

    [SerializeField] private Animator _animator;

    public void TriggerRun() =>
        _animator.SetTrigger(AnimationParams.Run);

    public void TriggerRunFast() =>
        _animator.SetTrigger(AnimationParams.Run2);

    public void TriggerStun() =>
        _animator.SetTrigger(AnimationParams.Stun);

    public void TriggerAttack() =>
        _animator.SetTrigger(AnimationParams.Hit);
}
