using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementHandler : MonoBehaviour
    {
        [SerializeField] private float speed;
        public float Speed => speed;

        private Vector3 _moveDirection;
        private IEnumerator _moveRoutine;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            _moveRoutine = MoveCoroutine();

            PlayerInputEventHandler.Instance.OnLeftJoystickPerform += StartLeftJoystickCoroutine;
            PlayerInputEventHandler.Instance.OnLeftJoystick1Cancel += StopLeftJoystick1Coroutine;
        }

        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                _rb.MovePosition(_rb.position + _moveDirection * speed);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void StartLeftJoystickCoroutine(Vector2 context)
        {
            _moveDirection = new Vector3(context.x, 0, context.y);
            StartCoroutine(_moveRoutine);
        }
        private void StopLeftJoystick1Coroutine()
        {
            _moveDirection = Vector3.zero;
            //_rb.velocity = Vector3.zero;
            StopCoroutine(_moveRoutine);
        }
    }
}

