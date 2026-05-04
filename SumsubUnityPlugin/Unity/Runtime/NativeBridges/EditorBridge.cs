#if UNITY_EDITOR

using System;
using Cysharp.Threading.Tasks;

namespace NativePlugins.SumsubPlugin
{
    internal class EditorBridge : INativeBridge
    {
        public event Action OnAccessTokenExpired;

        public bool CreateInstance(string token) => true;
        public async UniTask OpenSdk() => await UniTask.WaitForSeconds(1);

        public SumsubStatus GetStatus() => SumsubStatus.Approved;
        public FailReason GetFailReason() => FailReason.Unknown;
        public string GetVerboseStatus() => "Approved";
    }
}

#endif