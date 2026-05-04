using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NativePlugins.SumsubPlugin
{
    public static class Sumsub
    {
        /// <summary>
        /// Called when access token expires on the current instance <br/>
        /// Use this callback to request a new access token (e.g. from backend), then call <see cref="CreateInstance(string)"/> and pass the new token in.
        /// </summary>
        public static event Action OnAccessTokenExpired;

        private static readonly INativeBridge _bridge;

        static Sumsub()
        {
#if UNITY_EDITOR
            _bridge = new EditorBridge();
#elif UNITY_IOS
            _bridge = new IosBridge();
#elif UNITY_ANDROID
            _bridge = new AndroidBridge(); // todo
#endif

            _bridge.OnAccessTokenExpired += () => OnAccessTokenExpired?.Invoke();
        }

        /// <summary>
        /// Creates a new instance of the Sumsub SDK <br/>
        /// This can be called repeatedly and will replace the old instance using the new access token <br/>
        /// Will reset all callbacks
        /// </summary>
        /// <returns>
        /// 'true' if an instance is created successfully, otherwise 'false' <br/>
        /// Use <see cref="GetStatusInfo"/> to get the fail message.
        /// </returns>
        public static bool CreateInstance(string accessToken)
        {
            OnAccessTokenExpired = null;
            return _bridge.CreateInstance(accessToken);
        }

        /// <summary>
        /// Opens the Sumsub SDK. This is an async method, `await` will return when the SDK closes.
        /// </summary>
        public static UniTask OpenSdk() => _bridge.OpenSdk();

        /// <summary>
        /// Gets the current SDK status.
        /// </summary>
        public static SumsubStatus GetStatus()
        {
            var status = _bridge.GetStatus();
            Debug.Log($"[Sumsub] Current status: {status} - {GetDescriptionForStatus(status)}");
            return status;
        }

        /// <summary>
        /// Provides default description for the current SDK status
        /// </summary>
        public static string GetDescriptionForStatus(SumsubStatus? status)
        {
            status ??= GetStatus();
            return status switch
            {
                SumsubStatus.Invalid => "The SDK is not initialized",
                SumsubStatus.Failed => $"Verification failed: [{GetFailReason()}] - {GetVerboseStatus()}",
                SumsubStatus.Ready => "The SDK is ready",
                SumsubStatus.Initial => "No verification steps are passed yet",
                SumsubStatus.Incomplete => "Some but not all of the verification steps have been passed over",
                SumsubStatus.Pending => "Verification is pending",
                SumsubStatus.TemporarilyDeclined => "Applicant has been declined temporarily",
                SumsubStatus.FinallyRejected => "Applicant has been finally rejected",
                SumsubStatus.Approved => "Applicant has been approved.",
                SumsubStatus.ActionCompleted => "Face Auth action has been completed",
                _ => $"Unknown status ({(int)status})",
            };
        }

        /// <summary>
        /// Gets the reason if <see cref="GetStatus"/> returns <see cref="SumsubStatus.Failed"/>.
        /// </summary>
        public static FailReason GetFailReason() => _bridge.GetFailReason();

        /// <summary>
        /// Provides default description for the current fail reason.
        /// </summary>
        public static string GetDescriptionForFailReason(FailReason? failReason = null)
        {
            failReason ??= GetFailReason();
            return failReason switch
            {
                FailReason.Unknown => "Unknown or no fail",
                FailReason.InvalidParameters => "An attempt to setup with invalid parameters",
                FailReason.Unauthorized => "Unauthorized access detected (most likely `accessToken` is invalid or expired and had failed to be refreshed)",
                FailReason.InitialLoadingFailed => "Initial loading from backend is failed",
                FailReason.ApplicantNotFound => "No applicant is found for the given parameters",
                FailReason.ApplicantMisconfigured => "Applicant is found, but is misconfigured (most likely lacks of idDocs)",
                FailReason.NetworkError => "A network error occurred (the user will be presented with Network Oops screen)",
                FailReason.UnexpectedError => "Some unexpected error occurred (the user will be presented with Fatal Oops screen)",
                FailReason.InitializationError => "An initialization error occurred",
                _ => $"Invalid fail reason ({(int)failReason})",
            };
        }

        /// <summary>
        /// Returns SDK status as a text. Could contain detailed info for `Failed` case.
        /// </summary>
        public static string GetVerboseStatus() => _bridge.GetVerboseStatus();
    }

    public static class SumsubStatusExtensions
    {
        public static bool IsInitialized(this SumsubStatus status) => status is not (SumsubStatus.Invalid or SumsubStatus.Ready or SumsubStatus.Failed);
        public static bool IsFailed(this SumsubStatus status) => status is SumsubStatus.Failed;
        public static bool IsInProgress(this SumsubStatus status) => status is SumsubStatus.Ready or SumsubStatus.Initial or SumsubStatus.Incomplete or SumsubStatus.Pending or SumsubStatus.ActionCompleted;
        public static bool IsRejected(this SumsubStatus status) => status is SumsubStatus.TemporarilyDeclined or SumsubStatus.FinallyRejected;
        public static bool IsApproved(this SumsubStatus status) => status is SumsubStatus.Approved;
    }
}
