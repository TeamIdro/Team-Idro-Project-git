//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Input System/GamePlayInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GamePlayInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GamePlayInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GamePlayInputActions"",
    ""maps"": [
        {
            ""name"": ""Mage"",
            ""id"": ""34384080-95a8-4ee7-b837-6c9b9d4826ea"",
            ""actions"": [
                {
                    ""name"": ""Movimento"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2db25bd3-2c60-489d-8634-62826f44c8eb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""76cb9122-9b85-4e65-b315-4503e70a7c1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""fd0eb405-bdad-4e41-8dec-9bd7316b831e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsaElementoFuoco"",
                    ""type"": ""Button"",
                    ""id"": ""698cee77-4281-4660-8599-fc37e0f04b00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsaElementoAcqua"",
                    ""type"": ""Button"",
                    ""id"": ""30673b2a-710e-4b18-9a9b-c1dae185b0bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsaElementoTerra"",
                    ""type"": ""Button"",
                    ""id"": ""a65c7176-b7fa-4fdd-93c2-d8d9f5ddee0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsaElementoAria"",
                    ""type"": ""Button"",
                    ""id"": ""99f87ccf-e49f-4b2b-84e9-04b20b316c97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsaElementoFulmine"",
                    ""type"": ""Button"",
                    ""id"": ""7648c45d-18c2-4c31-8999-dd2f0cf14cf5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""2ad2c0c0-d11e-4854-9311-b83d123a5250"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3b215dc9-d36c-4847-89cd-92e10a0ff3aa"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""847c2256-8dbd-42ca-b1be-4212d1980f2e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a54890a0-643c-4927-b9e2-28006bbdf575"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""99cc971d-d9e8-4676-9424-350e64fc2390"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bae7dcc4-c315-47eb-8812-83096b75bcfc"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4d2af373-63be-41d3-b7e8-3f0b37c2fe5b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector GamePad"",
                    ""id"": ""74267e0d-90fe-4b88-916b-7f7aea0e0fd9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d60a1c31-1825-4e0c-9e06-aafade2b4381"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""821e25a6-2900-4184-a85c-7f3f93d370a2"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movimento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0c5e3a1e-11c3-49cf-9166-e8316334a058"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsaElementoFuoco"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be30a3ac-b061-464c-be8d-f39a3b49f80d"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsaElementoAcqua"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0449c9bb-0bb1-43a2-ae02-c8d75d1dd696"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsaElementoTerra"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ed9c37e-12ad-4a5b-b74d-fe32b1174c12"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsaElementoAria"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""573397a6-47e2-4560-ba63-a36ce9f6aa24"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsaElementoFulmine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a19c7354-dd7c-43ed-a93b-ebf7598ef0d8"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Kid"",
            ""id"": ""e7016efa-623c-4599-ac4a-2efcd36abc43"",
            ""actions"": [
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""1d5edb02-2c10-4d54-ab32-4f2d6ecd619e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""1389fd0e-c5f2-4818-a043-4c12c804665b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9c3bf6e4-0ba2-4c5c-888c-13a3b999cf20"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""217ad8e1-4a4d-4942-b356-1caebd9a16b7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mage
        m_Mage = asset.FindActionMap("Mage", throwIfNotFound: true);
        m_Mage_Movimento = m_Mage.FindAction("Movimento", throwIfNotFound: true);
        m_Mage_Interaction = m_Mage.FindAction("Interaction", throwIfNotFound: true);
        m_Mage_Jump = m_Mage.FindAction("Jump", throwIfNotFound: true);
        m_Mage_UsaElementoFuoco = m_Mage.FindAction("UsaElementoFuoco", throwIfNotFound: true);
        m_Mage_UsaElementoAcqua = m_Mage.FindAction("UsaElementoAcqua", throwIfNotFound: true);
        m_Mage_UsaElementoTerra = m_Mage.FindAction("UsaElementoTerra", throwIfNotFound: true);
        m_Mage_UsaElementoAria = m_Mage.FindAction("UsaElementoAria", throwIfNotFound: true);
        m_Mage_UsaElementoFulmine = m_Mage.FindAction("UsaElementoFulmine", throwIfNotFound: true);
        m_Mage_Fire = m_Mage.FindAction("Fire", throwIfNotFound: true);
        // Kid
        m_Kid = asset.FindActionMap("Kid", throwIfNotFound: true);
        m_Kid_MouseClick = m_Kid.FindAction("MouseClick", throwIfNotFound: true);
        m_Kid_MousePosition = m_Kid.FindAction("MousePosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Mage
    private readonly InputActionMap m_Mage;
    private List<IMageActions> m_MageActionsCallbackInterfaces = new List<IMageActions>();
    private readonly InputAction m_Mage_Movimento;
    private readonly InputAction m_Mage_Interaction;
    private readonly InputAction m_Mage_Jump;
    private readonly InputAction m_Mage_UsaElementoFuoco;
    private readonly InputAction m_Mage_UsaElementoAcqua;
    private readonly InputAction m_Mage_UsaElementoTerra;
    private readonly InputAction m_Mage_UsaElementoAria;
    private readonly InputAction m_Mage_UsaElementoFulmine;
    private readonly InputAction m_Mage_Fire;
    public struct MageActions
    {
        private @GamePlayInputActions m_Wrapper;
        public MageActions(@GamePlayInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movimento => m_Wrapper.m_Mage_Movimento;
        public InputAction @Interaction => m_Wrapper.m_Mage_Interaction;
        public InputAction @Jump => m_Wrapper.m_Mage_Jump;
        public InputAction @UsaElementoFuoco => m_Wrapper.m_Mage_UsaElementoFuoco;
        public InputAction @UsaElementoAcqua => m_Wrapper.m_Mage_UsaElementoAcqua;
        public InputAction @UsaElementoTerra => m_Wrapper.m_Mage_UsaElementoTerra;
        public InputAction @UsaElementoAria => m_Wrapper.m_Mage_UsaElementoAria;
        public InputAction @UsaElementoFulmine => m_Wrapper.m_Mage_UsaElementoFulmine;
        public InputAction @Fire => m_Wrapper.m_Mage_Fire;
        public InputActionMap Get() { return m_Wrapper.m_Mage; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MageActions set) { return set.Get(); }
        public void AddCallbacks(IMageActions instance)
        {
            if (instance == null || m_Wrapper.m_MageActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MageActionsCallbackInterfaces.Add(instance);
            @Movimento.started += instance.OnMovimento;
            @Movimento.performed += instance.OnMovimento;
            @Movimento.canceled += instance.OnMovimento;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @UsaElementoFuoco.started += instance.OnUsaElementoFuoco;
            @UsaElementoFuoco.performed += instance.OnUsaElementoFuoco;
            @UsaElementoFuoco.canceled += instance.OnUsaElementoFuoco;
            @UsaElementoAcqua.started += instance.OnUsaElementoAcqua;
            @UsaElementoAcqua.performed += instance.OnUsaElementoAcqua;
            @UsaElementoAcqua.canceled += instance.OnUsaElementoAcqua;
            @UsaElementoTerra.started += instance.OnUsaElementoTerra;
            @UsaElementoTerra.performed += instance.OnUsaElementoTerra;
            @UsaElementoTerra.canceled += instance.OnUsaElementoTerra;
            @UsaElementoAria.started += instance.OnUsaElementoAria;
            @UsaElementoAria.performed += instance.OnUsaElementoAria;
            @UsaElementoAria.canceled += instance.OnUsaElementoAria;
            @UsaElementoFulmine.started += instance.OnUsaElementoFulmine;
            @UsaElementoFulmine.performed += instance.OnUsaElementoFulmine;
            @UsaElementoFulmine.canceled += instance.OnUsaElementoFulmine;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
        }

        private void UnregisterCallbacks(IMageActions instance)
        {
            @Movimento.started -= instance.OnMovimento;
            @Movimento.performed -= instance.OnMovimento;
            @Movimento.canceled -= instance.OnMovimento;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @UsaElementoFuoco.started -= instance.OnUsaElementoFuoco;
            @UsaElementoFuoco.performed -= instance.OnUsaElementoFuoco;
            @UsaElementoFuoco.canceled -= instance.OnUsaElementoFuoco;
            @UsaElementoAcqua.started -= instance.OnUsaElementoAcqua;
            @UsaElementoAcqua.performed -= instance.OnUsaElementoAcqua;
            @UsaElementoAcqua.canceled -= instance.OnUsaElementoAcqua;
            @UsaElementoTerra.started -= instance.OnUsaElementoTerra;
            @UsaElementoTerra.performed -= instance.OnUsaElementoTerra;
            @UsaElementoTerra.canceled -= instance.OnUsaElementoTerra;
            @UsaElementoAria.started -= instance.OnUsaElementoAria;
            @UsaElementoAria.performed -= instance.OnUsaElementoAria;
            @UsaElementoAria.canceled -= instance.OnUsaElementoAria;
            @UsaElementoFulmine.started -= instance.OnUsaElementoFulmine;
            @UsaElementoFulmine.performed -= instance.OnUsaElementoFulmine;
            @UsaElementoFulmine.canceled -= instance.OnUsaElementoFulmine;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
        }

        public void RemoveCallbacks(IMageActions instance)
        {
            if (m_Wrapper.m_MageActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMageActions instance)
        {
            foreach (var item in m_Wrapper.m_MageActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MageActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MageActions @Mage => new MageActions(this);

    // Kid
    private readonly InputActionMap m_Kid;
    private List<IKidActions> m_KidActionsCallbackInterfaces = new List<IKidActions>();
    private readonly InputAction m_Kid_MouseClick;
    private readonly InputAction m_Kid_MousePosition;
    public struct KidActions
    {
        private @GamePlayInputActions m_Wrapper;
        public KidActions(@GamePlayInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseClick => m_Wrapper.m_Kid_MouseClick;
        public InputAction @MousePosition => m_Wrapper.m_Kid_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_Kid; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KidActions set) { return set.Get(); }
        public void AddCallbacks(IKidActions instance)
        {
            if (instance == null || m_Wrapper.m_KidActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_KidActionsCallbackInterfaces.Add(instance);
            @MouseClick.started += instance.OnMouseClick;
            @MouseClick.performed += instance.OnMouseClick;
            @MouseClick.canceled += instance.OnMouseClick;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
        }

        private void UnregisterCallbacks(IKidActions instance)
        {
            @MouseClick.started -= instance.OnMouseClick;
            @MouseClick.performed -= instance.OnMouseClick;
            @MouseClick.canceled -= instance.OnMouseClick;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
        }

        public void RemoveCallbacks(IKidActions instance)
        {
            if (m_Wrapper.m_KidActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IKidActions instance)
        {
            foreach (var item in m_Wrapper.m_KidActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_KidActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public KidActions @Kid => new KidActions(this);
    public interface IMageActions
    {
        void OnMovimento(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnUsaElementoFuoco(InputAction.CallbackContext context);
        void OnUsaElementoAcqua(InputAction.CallbackContext context);
        void OnUsaElementoTerra(InputAction.CallbackContext context);
        void OnUsaElementoAria(InputAction.CallbackContext context);
        void OnUsaElementoFulmine(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
    public interface IKidActions
    {
        void OnMouseClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
