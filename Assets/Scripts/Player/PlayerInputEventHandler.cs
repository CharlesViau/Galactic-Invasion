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
        [SerializeField] private InputActionProperty leftJoystickAction;
        //Mouse stuff
        [SerializeField] private InputActionProperty mouseDeltaAction;
        [SerializeField] private InputActionProperty leftMouseClickAction;
        [SerializeField] private InputActionProperty rightMouseClickAction;
        //Button
        [SerializeField] private InputActionProperty ability1Action;
        [SerializeField] private InputActionProperty ability2Action;
        [SerializeField] private InputActionProperty ability3Action;
        [SerializeField] private InputActionProperty ability4Action;

        //Other Action
        [SerializeField] private InputActionProperty gamePauseAction;
        #endregion

        #region Public Events
        //Gameplay
        public event Action<Vector2> OnLeftJoystickPerform;
        public event Action OnLeftJoystick1Cancel;
        public event Action<InputAction.CallbackContext> OnRightClickPerform;
        public event Action OnLeftClickPerform;
        public event Action OnLeftClickCancel;

        public event Action<int> OnAbilityPerform;
        public event Action<int> OnAbilityCancel;
        

        //Other
        public event Action OnGamePausePerform;
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

            leftJoystickAction.reference.action.performed += OnMovePerfo;
            leftJoystickAction.reference.action.canceled += OnMoveStop;
            leftMouseClickAction.reference.action.performed += OnLeftClickPerfo;
            leftMouseClickAction.reference.action.canceled += OnLeftClickStop;
            ability1Action.reference.action.performed += OnAbility1Perfo;
            ability2Action.reference.action.performed += OnAbility2Perfo;
            ability3Action.reference.action.performed += OnAbility3Perfo;
            //ability4Action.reference.action.performed += OnAbility4Perfo;
            
            gamePauseAction.reference.action.performed += OnGamePausePerfo;
        

        }
        private void OnDisable()
        {
            DisableAllInputAction();

            leftJoystickAction.reference.action.performed -= OnMovePerfo;
            leftJoystickAction.reference.action.canceled -= OnMoveStop;
            leftMouseClickAction.reference.action.performed -= OnLeftClickPerfo;
            leftMouseClickAction.reference.action.canceled -= OnLeftClickStop;
            ability1Action.reference.action.performed -= OnAbility1Perfo;
            ability2Action.reference.action.performed -= OnAbility2Perfo;
            ability3Action.reference.action.performed -= OnAbility3Perfo;
            //ability4Action.reference.action.performed -= OnAbility4Perfo;
            gamePauseAction.reference.action.performed -= OnGamePausePerfo;
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
        private void OnLeftClickPerfo(InputAction.CallbackContext context)
        {
            OnLeftClickPerform?.Invoke();
        }

        private void OnLeftClickStop(InputAction.CallbackContext context)
        {
            OnLeftClickCancel?.Invoke();
        }
        #endregion
        #region Abilities
        #region Ability1
        private void OnAbility1Perfo(InputAction.CallbackContext context)
        {
            OnAbilityPerform?.Invoke(1);
        }
        #endregion
        #region Ability2
        private void OnAbility2Perfo(InputAction.CallbackContext context)
        {
            OnAbilityPerform?.Invoke(2);
        }
        #endregion
        #region Ability3
        private void OnAbility3Perfo(InputAction.CallbackContext context)
        {
            OnAbilityPerform?.Invoke(3);
        }
        #endregion
        #region Ability4
        private void OnAbility4Perfo(InputAction.CallbackContext context)
        {
            OnAbilityPerform?.Invoke(4);
        }
        #endregion
        #endregion
        #region Other
        #region GamePause
        private void OnGamePausePerfo(InputAction.CallbackContext context)
        {
            OnGamePausePerform?.Invoke();
        }
        #endregion

        #endregion
        #endregion
        #endregion
    }
}
