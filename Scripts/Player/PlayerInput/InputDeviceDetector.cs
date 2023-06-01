using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.XInput;

namespace ChittaExorcist.PlayerSettings.InputHandler
{
    public class InputDeviceDetector : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] bool detectUIInputOnly = true;
        [SerializeField] bool hideCursorAtBeginning = false;

        [Space(10f)][Header("Device Switch Event Triggers")]
        [SerializeField] UnityEvent onSwitchToMouse = default;
        [SerializeField] UnityEvent onSwitchToKeyboard = default;
        // [SerializeField] UnityEvent onSwitchToGamepad = default;
        [SerializeField] UnityEvent onSwitchToGamepadPS = default;
        [SerializeField] UnityEvent onSwitchToGamepadXbox = default;

        Dictionary<InputDevice, UnityEvent> deviceSwitchTable = new Dictionary<InputDevice, UnityEvent>(); 

        InputDevice currentDevice;

        Mouse mouse;

        Keyboard keyboard;

        Gamepad gamepad;

        InputSystemUIInputModule UIInputModule;

        static InputDeviceDetector instance;

        public static UnityEvent OnSwitchToMouse => instance.onSwitchToMouse;
        public static UnityEvent OnSwitchToKeyboard => instance.onSwitchToKeyboard;
        // public static UnityEvent OnSwitchToGamepad => instance.onSwitchToGamepad;
        public static UnityEvent OnSwitchToGamepadPS => instance.onSwitchToGamepadPS;
        public static UnityEvent OnSwitchToGamepadXbox => instance.onSwitchToGamepadXbox;

        // private bool _hasGamepad = false;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            mouse = Mouse.current;
            keyboard = Keyboard.current;
            gamepad = Gamepad.current;

            if (mouse != null) deviceSwitchTable.Add(mouse, OnSwitchToMouse);
            if (keyboard != null) deviceSwitchTable.Add(keyboard, onSwitchToKeyboard);
            // if (gamepad != null) deviceSwitchTable.Add(gamepad, onSwitchToGamepad);
            if (gamepad != null)
            {
                if (gamepad is XInputController)
                {
                    deviceSwitchTable.Add(gamepad, onSwitchToGamepadXbox);
                    // Debug.Log("Xbox: " + gamepad);
                    // _hasGamepad = true;
                }
                else if (gamepad is DualShockGamepad)
                {
                    deviceSwitchTable.Add(gamepad, onSwitchToGamepadPS);
                    // Debug.Log("Dual: " + gamepad);
                    // _hasGamepad = true;
                }
                // deviceSwitchTable.Add(gamepad, onSwitchToGamepad);
            }

            if (hideCursorAtBeginning)
            {
                HideCursor();
            }

            UIInputModule = FindObjectOfType<InputSystemUIInputModule>(true);

        #if UNITY_EDITOR
            if (UIInputModule == null && detectUIInputOnly)
            {
                Debug.LogError("Can NOT find UI Input Module! Please check there is a Event System in the scene and is currently using the UI Input Module!");
            }
        #endif
        }

        void OnEnable()
        {
            InputSystem.onActionChange += DetectCurrentInputDevice;
        }

        void OnDisable()
        {
            InputSystem.onActionChange -= DetectCurrentInputDevice;

            onSwitchToMouse?.RemoveAllListeners();
            onSwitchToKeyboard?.RemoveAllListeners();
            // onSwitchToGamepad?.RemoveAllListeners();
            onSwitchToGamepadPS?.RemoveAllListeners();
            onSwitchToGamepadXbox?.RemoveAllListeners();
        }

        void DetectCurrentInputDevice(object obj, InputActionChange change)
        {
            if (detectUIInputOnly && !UIInputModule.isActiveAndEnabled) return;

            if (change == InputActionChange.ActionPerformed)
            {
                currentDevice = ((InputAction)obj).activeControl.device;
                // if (currentDevice is XInputController)
                // {
                //     // XBOX
                //     Debug.Log("XBOX");
                // }
                // else if (currentDevice is DualShockGamepad)
                // {
                //     // PS
                //     Debug.Log("PS");
                // }
                
                // if (!_hasGamepad)
                // {
                //     gamepad = Gamepad.current;
                //     if (gamepad != null)
                //     {
                //         if (gamepad is XInputController)
                //         {
                //             deviceSwitchTable.Add(gamepad, onSwitchToGamepadXbox);
                //             _hasGamepad = true;
                //         }
                //         else if (gamepad is DualShockGamepad)
                //         {
                //             deviceSwitchTable.Add(gamepad, onSwitchToGamepadPS);
                //             _hasGamepad = true;
                //         }                        
                //     }
                // }
                deviceSwitchTable[currentDevice].Invoke();
            }
        }

        public static void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            // if (!_hasGamepad)
            // {
            //     gamepad = Gamepad.current;
            //     if (gamepad != null)
            //     {
            //         if (gamepad is XInputController)
            //         {
            //             deviceSwitchTable.Add(gamepad, onSwitchToGamepadXbox);
            //             _hasGamepad = true;
            //         }
            //         else if (gamepad is DualShockGamepad)
            //         {
            //             deviceSwitchTable.Add(gamepad, onSwitchToGamepadPS);
            //             _hasGamepad = true;
            //         }                        
            //     }
            // }
            // Debug.Log(_hasGamepad);
        }
    }
}