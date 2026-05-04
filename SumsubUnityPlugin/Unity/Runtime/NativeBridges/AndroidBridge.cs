#if UNITY_EDITOR || UNITY_ANDROID

using System;
using Cysharp.Threading.Tasks;

namespace NativePlugins.SumsubPlugin
{
    internal class AndroidBridge : INativeBridge
    {
        public event Action OnAccessTokenExpired;

        public bool CreateInstance(string token) => throw new NotImplementedException();
        public UniTask OpenSdk() => throw new NotImplementedException();

        public SumsubStatus GetStatus() => throw new NotImplementedException();
        public FailReason GetFailReason() => throw new NotImplementedException();
        public string GetVerboseStatus() => throw new NotImplementedException();
    }
}

#endif