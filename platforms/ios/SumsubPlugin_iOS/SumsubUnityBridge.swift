import Foundation

public final class Callbacks {
    public static var onClose: VoidDelegate?
    public static var onTokenExpired: VoidDelegate?
}

let _plugin = SumsubPlugin()

@_cdecl("_CreateInstance")
public func _CreateInstance(token: CStringPtr) -> Bool {
    _plugin.createInstance(token: token.toString());
}

@_cdecl("_OpenSdk")
public func _OpenSdk() {
    _plugin.openSdk()
}

@_cdecl("_GetVerboseStatus")
public func _GetVerboseStatus() -> CStringPtr? {
    let desc = _plugin.getVerboseStatus()
    return UnsafePointer(strdup(desc))
}

@_cdecl("_GetStatus")
public func _GetStatus() -> Int32 {
    return _plugin.getStatus()
}

@_cdecl("_GetFailReason")
public func _GetFailReason() -> Int32 {
    return _plugin.getFailReason()
}

@_cdecl("_SetOnCloseCallback")
public func _SetOnCloseCallback(callback: @escaping VoidDelegate) {
    Callbacks.onClose = callback
}

@_cdecl("_SetAccessTokenExpiredCallback")
public func _SetAccessTokenExpiredCallback(callback: @escaping VoidDelegate) {
    Callbacks.onTokenExpired = callback
}
