using UnityEngine;

namespace InterfaceUI
{
    public class AnimationsScene : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        public void TriggerEntryScene() =>
            _animator.SetTrigger(AnimationParams.Entry);

        public void TriggerExitScene() =>
            _animator.SetTrigger(AnimationParams.Exit);
        
        private static class AnimationParams
        {
            public static readonly int Entry = Animator.StringToHash("Entry");
            public static readonly int Exit = Animator.StringToHash("Exit");
        }
    }
}
