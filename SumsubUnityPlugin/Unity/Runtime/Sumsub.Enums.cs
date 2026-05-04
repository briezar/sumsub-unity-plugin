namespace NativePlugins.SumsubPlugin
{
    public enum SumsubStatus
    {
        Invalid = -1,

        /// SDK is initialized and ready to be presented
        Ready,

        /// SDK fails for some reasons (see `failReason` and `verboseStatus` for details)
        Failed,

        /// No verification steps are passed yet
        Initial,

        /// Some but not all of the verification steps have been passed over
        Incomplete,

        /// Verification is pending
        Pending,

        /// Applicant has been declined temporarily
        TemporarilyDeclined,

        /// Applicant has been finally rejected
        FinallyRejected,

        /// Applicant has been approved
        Approved,

        /// Face Auth action has been completed (see `sdk.actionResult` for details)
        ActionCompleted,
    }

    public enum FailReason
    {
        /// Unknown or no fail
        Unknown,

        /// An attempt to setup with invalid parameters
        InvalidParameters,

        /// Unauthorized access detected (most likely `accessToken` is invalid or expired and had failed to be refreshed)
        Unauthorized,

        /// Initial loading from backend is failed
        InitialLoadingFailed,

        /// No applicant is found for the given parameters
        ApplicantNotFound,

        /// Applicant is found, but is misconfigured (most likely lacks of idDocs)
        ApplicantMisconfigured,

        /// A network error occurred (the user will be presented with Network Oops screen)
        NetworkError,

        /// Some unexpected error occurred (the user will be presented with Fatal Oops screen)
        UnexpectedError,

        /// An initialization error occurred
        InitializationError,
    }
}
