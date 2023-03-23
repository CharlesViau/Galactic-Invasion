using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            PlayerInputEventHandler.Instance.OnLeftClickPerform += OnLeftClickStart;

        }

        private void OnLeftClickStart(InputAction.CallbackContext ctx)
        {
            _animator.SetBool(Attack, true);
        }
    }
}
