import Foundation
import IdensicMobileSDK
import UIKit

public class SumsubPlugin {

    private var _sdk: SNSMobileSDK?

    public func createInstance(token: String) -> Bool {
        _sdk = SNSMobileSDK(accessToken: token)
        guard let sdk = _sdk, sdk.isReady else {
            return false;
        }

        sdk.onDidDismiss { _ in
            Callbacks.onClose?()
        }

        sdk.tokenExpirationHandler { _ in
            Callbacks.onTokenExpired?()
        }

        return true;
    }

    public func openSdk() {
        guard let sdk = _sdk, sdk.isReady else {
            print("Sumsub not initialized")
            return
        }

        DispatchQueue.main.async {
            sdk.present()
        }
    }

    public func getVerboseStatus() -> String {
        guard let sdk = _sdk else {
            return "Sumsub not initialized!"
        }
        return sdk.verboseStatus
    }

    public func getStatus() -> (Int32) {
        guard let sdk = _sdk else {
            return -1
        }

        switch sdk.status {
        case .ready:
            return 0
        case .failed:
            return 1
        case .initial:
            return 2
        case .incomplete:
            return 3
        case .pending:
            return 4
        case .temporarilyDeclined:
            return 5
        case .finallyRejected:
            return 6
        case .approved:
            return 7
        case .actionCompleted:
            return 8
        default:
            return 100
        }
    }

    public func getFailReason() -> (Int32) {
        guard let sdk = _sdk else {
            return -1
        }

        switch sdk.failReason {
        case .unknown:
            return 0
        case .invalidParameters:
            return 1
        case .unauthorized:
            return 2
        case .initialLoadingFailed:
            return 3
        case .applicantNotFound:
            return 4
        case .applicantMisconfigured:
            return 5
        case .networkError:
            return 6
        case .unexpectedError:
            return 7
        case .initializationError:
            return 8
        default:
            return 100
        }
    }

}
