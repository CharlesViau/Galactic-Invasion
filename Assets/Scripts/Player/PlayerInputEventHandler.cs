using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerInputEventHandler : MonoBehaviour
    {

        #region Properties and Variables
        #region InputActionProperty
        //Gameplay Action
        //WASD Composite Vector2
        [SerializeField] private InputActionProperty _leftJoystickAction;
        //Mouse stuff
        [SerializeField] private InputActionProperty _mouseDeltaAction;
        [SerializeField] private InputActionProperty _leftMouseClickAction;
        [SerializeField] private InputActionProperty _rightMouseClickAction;
        //Button
        [SerializeField] private InputActionProperty _ability1Action;
        [SerializeField] private InputActionProperty _ability2Action;
        [SerializeField] private InputActionProperty _ability3Action;
        [SerializeField] private InputActionProperty _ability4Action;

        //Other Action
        [SerializeField] private InputActionProperty _gamePauseAction;
        #endregion

        #region Public Events
        //Gameplay
        public event Action<Vector2> OnLeftJoystickPerform;
        public event Action OnLeftJoystick1Cancel;
        public event Action<InputAction.CallbackContext> OnRightClickPerform;
        public event Action<InputAction.CallbackContext> OnLeftClickPerform;
        

        //Other
        public event Action<InputAction.CallbackContext> OnGamePausePerform;
        #endregion

        #region Singleton Stuff

        public static PlayerInputEventHandler Instance { get; private set; }

        #endregion
        #endregion

        private void Awake()
        {
            InitializeSingleton();
        }
        private void OnEnable()
        {
            EnableAllInputAction();

            _leftJoystickAction.reference.action.performed += OnMovePerfo;
            _leftJoystickAction.reference.action.canceled += OnMoveStop;
            //_lookAction.reference.action.performed += OnLookPerfo;
            //_basicAttackAction.reference.action.performed += OnAttackPerfo;
            /*_ability1Action.reference.action.performed += OnAbility1Perfo;
        _ability2Action.reference.action.performed += OnAbility2Perfo;
        _ability3Action.reference.action.performed += OnAbility3Perfo;
        _ability4Action.reference.action.performed += OnAbility4Perfo;
        */
            //_gamePauseAction.reference.action.performed += OnGamePausePerfo;
        

        }
        private void OnDisable()
        {
            DisableAllInputAction();

            _leftJoystickAction.reference.action.performed -= OnMovePerfo;
            _leftJoystickAction.reference.action.canceled -= OnMoveStop;
            //_lookAction.action.performed -= OnLookPerfo;
            //_basicAttackAction.action.performed -= OnAttackPerfo;
            /*_ability1Action.reference.action.performed -= OnAbility1Perfo;
        _ability2Action.reference.action.performed -= OnAbility2Perfo;
        _ability3Action.reference.action.performed -= OnAbility3Perfo;
        _ability4Action.reference.action.performed -= OnAbility4Perfo;*/
            //_gamePauseAction.reference.action.performed -= OnGamePausePerfo;
        }

        #region Hidden Complexity (Private Methods)
        #region Called in Unity Standard Method
        private void InitializeSingleton()
        {
            if (Instance != null && Instance != this &&
                FindObjectOfType<PlayerInputEventHandler>() != this)
            {
                Destroy(this);
            }
            Instance = this;
        }
        private void EnableAllInputAction()
        {
            foreach (var action in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                         .Where(field => field.FieldType == typeof(InputActionProperty)))
            {
                ((InputActionProperty)action.GetValue(this)).action.Enable();
            }
        }
        private void DisableAllInputAction()
        {
            foreach (var action in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                         .Where(field => field.FieldType == typeof(InputActionProperty)))
            {
                ((InputActionProperty)action.GetValue(this)).action.Disable();
            }
        }
        #endregion

        #region Event Invoke Encapsulation In Methods
        #region Move
        private void OnMovePerfo(InputAction.CallbackContext context)
        {
            OnLeftJoystickPerform?.Invoke(context.ReadValue<Vector2>());
        }
        private void OnMoveStop(InputAction.CallbackContext context)
        {
            OnLeftJoystick1Cancel?.Invoke();
        }
        #endregion
        #region Look
        private void OnLookPerfo(InputAction.CallbackContext context)
        {
            OnRightClickPerform?.Invoke(context);
        }
        #endregion
        #region Attack
        private void OnAttackPerfo(InputAction.CallbackContext context)
        {
            OnLeftClickPerform?.Invoke(context);
        }
        #endregion
        #region Abilities
        #region Ability1
        private void OnAbility1Perfo(InputAction.CallbackContext context)
        {
            //OnAbility1Perform?.Invoke(context);
        }
        #endregion
        #region Ability2
        private void OnAbility2Perfo(InputAction.CallbackContext context)
        {
            //OnAbility2Perform?.Invoke(context);
        }
        #endregion
        #region Ability3
        private void OnAbility3Perfo(InputAction.CallbackContext context)
        {
            //OnAbility3Perform?.Invoke(context);
        }
        #endregion
        #region Ability4
        private void OnAbility4Perfo(InputAction.CallbackContext context)
        {
            //OnAbility4Perform?.Invoke(context);
        }
        #endregion
        #endregion
        #region Other
        #region GamePause
        private void OnGamePausePerfo(InputAction.CallbackContext context)
        {
            OnGamePausePerform?.Invoke(context);
        }
        #endregion

        #endregion
        #endregion
        #endregion
    }
}
