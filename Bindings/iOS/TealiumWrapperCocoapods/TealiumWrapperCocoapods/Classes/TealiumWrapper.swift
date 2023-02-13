//
//  TealiumWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 19/07/21.
//

import TealiumSwift

@objc(TealiumWrapper)
public class TealiumWrapper: NSObject {
    
    private let tealium: Tealium
    let config: TealiumConfigWrapper
    private var _consentManager: ConsentManagerWrapper?
    /// - Returns: `ConsentManager` instance
    @objc public var consentManager: ConsentManagerWrapper? {
        if let wrapper = _consentManager {
            return wrapper
        }
        if let manager = tealium.consentManager {
            _consentManager = ConsentManagerWrapper(consentManager: manager, config: config)
            return _consentManager
        }
        return nil
    }
    
    @objc public var instanceId: String {
        "\(config.account).\(config.profile).\(config.environment)"
    }
    
    @objc public var visitorId: String? {
        tealium.visitorId
    }
    
    @objc public var userConsentStatus: TealiumConsentStatusWrapper {
        get {
            consentManager?.userConsentStatus ?? .unknown
        }
        set {
            consentManager?.userConsentStatus = newValue
        }
    }
    
    @objc public var userConsentCategories: [Int]? {
        get {
            consentManager?.userConsentCategories
        }
        set {
            consentManager?.userConsentCategories = newValue
        }
    }
    
    @objc public var onConsentExpiration: (() -> Void)? {
        get {
            consentManager?.onConsentExpiration
        }
        set {
            consentManager?.onConsentExpiration = newValue
        }
    }
    
    @objc public var onVisitorProfileUpdate: ((TealiumVisitorProfileWrapper) -> Void)? {
        get {
            eventsHandler.onVisitorProfileUpdate
        }
        set {
            eventsHandler.onVisitorProfileUpdate = newValue
        }
    }

    let eventsHandler: EventsHandler
    
    @objc public var onVisitorIdUpdate: ((String) -> Void)? {
        get {
            eventsHandler.onVisitorIdUpdate
        }
        set {
            eventsHandler.onVisitorIdUpdate = newValue
        }
    }
    
    @objc public init(config: TealiumConfigWrapper, enableCompletion: ((_ success: Bool,_ error: Error?) -> Void)?) {
        self.config = config
        let eventsHandler = EventsHandler()
        self.eventsHandler = eventsHandler
        config.config.visitorServiceDelegate = eventsHandler
        var tealium: Tealium!
        tealium = Tealium(config: config.config) { result in
            eventsHandler.subscribe(onVisitorId: tealium?.onVisitorId)
            switch result {
                case .success(let success):
                    enableCompletion?(success, nil)
                case .failure(let error):
                    enableCompletion?(false, error)
            }
        }
        self.tealium = tealium
    }
    
    @objc public func resetConsentPreferences() {
        tealium.consentManager?.resetUserConsentPreferences()
    }
    
    @objc public func track(title: String, data: [String: Any]?) {
        let dispatch = TealiumEvent(title, dataLayer: data)
        tealium.track(dispatch)
    }

    @objc public func trackView(title: String, data: [String: Any]?) {
        let dispatch = TealiumView(title, dataLayer: data)
        tealium.track(dispatch)
    }

    @objc public func joinTrace(_ traceID: String) {
        tealium.joinTrace(id: traceID)
    }

    @objc public func leaveTrace() {
        tealium.leaveTrace()
    }
    
    @objc public func addToDataLayer(data: [String:Any], expiry: ExpiryWrapper) {
        tealium.dataLayer.add(data: data, expiry: expiry.expiry)
    }
    
    @objc public func getFromDataLayer(key: String) -> Any? {
        return tealium.dataLayer.all[key]
    }
    
    @objc public func removeFromDataLayer(keys: [String]) {
        tealium.dataLayer.delete(for: keys)
    }
    
    var commands = [RemoteCommandWrapper]()
    
    @objc public func addRemoteCommand(_ remoteCommand: RemoteCommandWrapper) {
        guard let commandsModule = tealium.remoteCommands else {
            return
        }
        commands.append(remoteCommand)
        commandsModule.add(remoteCommand.command)
    }
    
    @objc public func removeRemoteCommand(id: String) {
        guard let commandsModule = tealium.remoteCommands else {
            return
        }
        commands.removeAll { command in
            return command.commandId == id
        }
        commandsModule.remove(commandWithId: id)
    }
    
    /// Just to remove commands in case there is some involuntary leak in the completions
    @objc public func destroy() {
        commands = []
        tealium.remoteCommands?.removeAll()
    }
    
    @objc public func resetVisitorId() {
        tealium.resetVisitorId()
    }

    @objc public func clearStoredVisitorIds() {
        tealium.clearStoredVisitorIds()
    }
    
    deinit {
        // This is just added to allow for tealium instance to be deallocated
        // Instance manager will be entirely written in the Xamarin project
        TealiumInstanceManager.shared.removeInstance(config: self.config.config)
    }
    
}

class EventsHandler: VisitorServiceDelegate {
    let bag = TealiumDisposeBag()
    var onVisitorIdUpdate: ((String) -> Void)?
    var onVisitorProfileUpdate: ((TealiumVisitorProfileWrapper) -> Void)?
    
    func didUpdate(visitorProfile: TealiumVisitorProfile) {
        onVisitorProfileUpdate?(TealiumVisitorProfileWrapper(visitorProfile: visitorProfile))
    }
    
    func subscribe(onVisitorId: TealiumObservable<String>?) {
        onVisitorId?.subscribe({ [weak self] newId in
            self?.onVisitorIdUpdate?(newId)
        }).toDisposeBag(bag)
    }
}
