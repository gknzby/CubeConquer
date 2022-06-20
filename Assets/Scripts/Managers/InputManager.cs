using CubeConquer.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Managers
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        #region Class Variables
        private enum InputState
        {
            Stopped,
            Idle,
            Sending,
            Canceled
        }
        private InputState inputState;

        private Coroutine tickCoroutine;
        private IInputReceiver defaultReceiver;
        private IInputReceiver activeReceiver;
        private IInputReceiver ActiveReceiver
        {
            get
            {
                if (activeReceiver == null)
                {
                    activeReceiver = defaultReceiver;
                }
                return activeReceiver;
            }
            set
            {
                activeReceiver = value;
            }
        }
        #endregion

        #region IInputManager
        public void SetDefaultReceiver(IInputReceiver inputReceiver)
        {
            defaultReceiver = inputReceiver;
        }

        public void RemoveDefaultReceiver(IInputReceiver inputReceiver)
        {
            defaultReceiver = defaultReceiver == inputReceiver ? null : defaultReceiver;
        }

        public void StartSendingInputs()
        {
            //Debug.Log("Start sending inputs");
            switch (inputState)
            {
                case InputState.Sending:
                    SendCancel();
                    inputState = InputState.Idle;
                    break;
                default:
                    inputState = InputState.Idle;
                    break;
            }

            if (tickCoroutine == null)
            {
                tickCoroutine = StartCoroutine(Tick());
            }
        }

        public void StopSendingInputs()
        {
            switch (inputState)
            {
                case InputState.Sending:
                    SendCancel();
                    inputState = InputState.Stopped;
                    break;
                default:
                    break;
            }

            if(tickCoroutine != null)
            {
                StopCoroutine(tickCoroutine);
            }
        }
        #endregion

        #region Class Functions
        private IEnumerator Tick()
        {
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    OnRelease();
                }
                yield return null;
            }
        }

        private void OnClick()
        {
            if (inputState == InputState.Idle)
            {
                SendClick();
                inputState = InputState.Sending;
            }
        }

        private void OnRelease()
        {
            if (inputState == InputState.Sending)
            {
                //Debug.Log("send release");
                SendRelease();
                inputState = InputState.Idle;
            }
        }

        private void SendClick()
        {
            //Debug.Log("Send Click");
            ActiveReceiver?.Click();
        }
        private void SendRelease()
        {
            ActiveReceiver?.Release();
        }
        private void SendCancel()
        {
            ActiveReceiver?.Cancel();
        }
        #endregion

        #region Unity Functions => Awake, OnDestroy
        private void Awake()
        {
            ManagerProvider.AddManager<IInputManager>(this);
        }

        private void OnDestroy()
        {
            ManagerProvider.RemoveManager<IInputManager>();
        }
        #endregion
    }
}
