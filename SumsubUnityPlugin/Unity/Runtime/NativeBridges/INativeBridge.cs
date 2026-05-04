using System;
using Cysharp.Threading.Tasks;

namespace NativePlugins.SumsubPlugin
{
    internal interface INativeBridge
    {
        event Action OnAccessTokenExpired;

        bool CreateInstance(string token);
        UniTask OpenSdk();

        SumsubStatus GetStatus();
        FailReason GetFailReason();
        string GetVerboseStatus();
    }
}
