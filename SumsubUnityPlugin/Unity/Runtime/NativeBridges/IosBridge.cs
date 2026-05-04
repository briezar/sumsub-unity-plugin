#if UNITY_EDITOR || UNITY_IOS

using System;
using System.Runtime.InteropServices;
using AOT;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NativePlugins.SumsubPlugin
{
    internal class IosBridge : INativeBridge
    {
        private static UniTaskCompletionSource _onCloseSdkTcs;

        public event Action OnAccessTokenExpired;

        private static IosBridge _instance;

        internal IosBridge()
        {
            if (_instance != null)
            {
                Debug.LogError($"Instantiating multiple instances of {nameof(IosBridge)} is not allowed!");
                return;
            }
            _instance = this;
        }

        public bool CreateInstance(string token)
        {
            _SetOnCloseCallback(InvokeOnCloseSdk);
            _SetAccessTokenExpiredCallback(InvokeOnAccessTokenExpired);
            return _CreateInstance(token);
        }

        public async UniTask OpenSdk()
        {
            _onCloseSdkTcs = new();
            _OpenSdk();
            await _onCloseSdkTcs.Task;
        }

        public SumsubStatus GetStatus() => (SumsubStatus)_GetStatus();
        public FailReason GetFailReason() => (FailReason)_GetFailReason();
        public string GetVerboseStatus() => GetStringFromIntPtr(_GetVerboseStatus);

        private string GetStringFromIntPtr(Func<IntPtr> func)
        {
            var ptr = func();
            if (ptr == IntPtr.Zero) { return null; }

            var stringValue = Marshal.PtrToStringAnsi(ptr);
            Marshal.FreeHGlobal(ptr);
            return stringValue;
        }

        #region Native

        private delegate void VoidDelegate();

        [MonoPInvokeCallback(typeof(VoidDelegate))]
        private static void InvokeOnCloseSdk() => _onCloseSdkTcs?.TrySetResult();

        [MonoPInvokeCallback(typeof(VoidDelegate))]
        private static void InvokeOnAccessTokenExpired() => _instance.OnAccessTokenExpired?.Invoke();

        [DllImport("__Internal")] private static extern bool _CreateInstance(string token);
        [DllImport("__Internal")] private static extern void _OpenSdk();
        [DllImport("__Internal")] private static extern IntPtr _GetVerboseStatus();
        [DllImport("__Internal")] private static extern int _GetStatus();
        [DllImport("__Internal")] private static extern int _GetFailReason();
        [DllImport("__Internal")] private static extern void _SetOnCloseCallback(VoidDelegate onClose);
        [DllImport("__Internal")] private static extern void _SetAccessTokenExpiredCallback(VoidDelegate onTokenExpired);

        #endregion

    }
}

#endif