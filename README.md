# Sumsub Unity Plugin
Adds Sumsub KYC functionality to your Unity project.

## Dependencies
- UniTask (for asynchronous operations)
- EDM4U (External Dependency Manager)

## Installation
- Drag the `project-root/SumsubUnityPlugin` into Unity at `Assets/Plugins/`
- `Assets/External Dependency Manager/iOS Resolver/Settings` -> Untick `Link Frameworks statically`

## Usage
- Call `Sumsub.CreateInstance(string accessToken)` and pass in the access token to setup the SDK.
  - Returns true if an instance is created successfully, otherwise false.
  - This can be called repeatedly and will replace the old instance using the new access token. Use together with `Sumsub.OnAccessTokenExpired` to re-initialize the SDK with a new token.
- Use `Sumsub.OpenSdk` to open the SDK interface.
  - This is an async method, `await` will return when the SDK closes.
- Use `Sumsub.GetStatus` to get the current Sumsub status.

Callbacks:
- `Sumsub.OnAccessTokenExpired`: Called when access token expires on the current instance. Use this callback to request a new access token (e.g. from your backend), then call `Sumsub.CreateInstance(string)` and pass the new token in.

## Modify/Update Project
### iOS
- If you need to change the version of Sumsub dependencies, you need to update both `./platforms/ios/Podfile` and `./SumsubUnityPlugin/Unity/Editor/SumsubDependencies.xml`
- Right-click `./platforms/ios` -> `New Terminal at Folder`, run `pod install` (optionally ` --repo-update` to update pods).
- Open `./platforms/ios/SumsubPlugin_iOS.xcworkspace` and modify the project.
- From top menu choose `Product/Archive`, then choose `Product/Show Build Folder in Finder`, and navigate to `.../Build/Intermediates.noindex/ArchiveIntermediates/SumsubPlugin_iOS/BuildProductsPath/Release-iphoneos/SumsubPlugin_iOS.framework`. Right-click -> Show Original, then Copy (and replace) `SumsubPlugin_iOS.framework` to `./SumsubUnityPlugin/iOS/`.

### Android
- N/A
