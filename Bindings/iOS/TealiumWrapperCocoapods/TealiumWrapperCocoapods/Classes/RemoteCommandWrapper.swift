//
//  RemoteCommandWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 27/07/21.
//

import Foundation
import TealiumSwift


@objc(RemoteCommandTypeWrapper)
open class RemoteCommandTypeWrapper: NSObject {
    
    public let type: RemoteCommandType
    @objc public let typeString: String
    @objc public var url: String? {
        if case .remote(let url) = type {
            return url
        }
        return nil
    }
    @objc public var path: String? {
        if case .local(let path, _) = type {
            return path
        }
        return nil
    }
    @objc override public init() {
        type = .webview
        typeString = "webview"
    }
    
    @objc public init(url: String) {
        type = .remote(url: url)
        typeString = "url"
    }
    
    @objc public init(file: String, bundle: Bundle? = nil) {
        type = .local(file: file, bundle: bundle)
        typeString = "local"
    }
}

@objc(RemoteCommandResponseWrapper)
public class RemoteCommandResponseWrapper: NSObject {
    
    var response: RemoteCommandResponseProtocol
    init(response: RemoteCommandResponseProtocol) {
        self.response = response
    }
    
    @objc public init(payload: [String: Any]) {
        response = JSONRemoteCommandResponse(with: payload)
    }
    
    @objc public var payload: [String: Any]? {
        get {
            response.payload
        }
        set {
            response.payload = newValue
        }
    }
    
    @objc public var error: Error? {
        get {
            response.error
        }
        set {
            response.error = newValue
        }
    }
    
    @objc public var status: NSNumber? {
        get {
            guard let responseStatus = response.status else {
                return nil
            }
            return NSNumber(value: responseStatus)
        }
        set {
            response.status = newValue?.intValue
        }
    }
    @objc public var data: Data? {
        get {
            response.data
        }
        set {
            response.data = newValue
        }
    }
    @objc public var hasCustomCompletionHandler: Bool {
        get {
            response.hasCustomCompletionHandler
        }
        set {
            response.hasCustomCompletionHandler = newValue
        }
    }
}

/// Designed to be subclassed. Allows Remote Commands to be created by host apps,
/// and called on-demand by the Tag Management module
@objc(RemoteCommandWrapper)
open class RemoteCommandWrapper: NSObject {

    private(set) var command: RemoteCommand!
    
    @objc public let type: RemoteCommandTypeWrapper
    
    @objc public var commandId: String {
        command.commandId
    }
    
    @objc public var commandDescription: String? {
        get {
            command.description
        }
        set {
            command.description = newValue
        }
    }
    
    @objc open var name: String {
        command.name
    }
    
    @objc open var version: String? {
        command.version
    }
    
    /// Just used to allow the return of the completion variable
    @objc public var completion: (_ response: RemoteCommandResponseWrapper) -> Void
    
    /// Constructor for a Tealium Remote Command.
    ///
    /// - Parameters:
    ///     - commandId: `String` identifier for command block.
    ///     - description: `String?` description of command.
    ///     - type: Type of remote command: webview, local or remote.
    ///     - completion: The completion block to run when this remote command is triggered.
    @objc public convenience init(commandId: String,
                                  description: String?,
                                  type: RemoteCommandTypeWrapper = RemoteCommandTypeWrapper(),
                                  completion : @escaping (_ response: RemoteCommandResponseWrapper) -> Void) {
        self.init(commandId: commandId,
                  description: description,
                  type: type,
                  name: nil,
                  version: nil,
                  completion: completion)
    }
    
    /// Constructor for a Tealium Remote Command.
    ///
    /// - Parameters:
    ///     - commandId: `String` identifier for command block.
    ///     - description: `String?` description of command.
    ///     - type: Type of remote command: webview, local or remote.
    ///     - name: The name of the remote command, for tracking purposes.
    ///     - version: The version of the remote command, for tracking purposes.
    ///     - completion: The completion block to run when this remote command is triggered.
    @objc public init(commandId: String,
                      description: String?,
                      type: RemoteCommandTypeWrapper = RemoteCommandTypeWrapper(),
                      name: String?,
                      version: String?,
                      completion : @escaping (_ response: RemoteCommandResponseWrapper) -> Void) {
        self.completion = completion
        self.type = type
        self.command = nil
        super.init()
        self.command = NameAndVersionRemoteCommand(commandId: commandId,
                                                   description: description,
                                                   type: type.type,
                                                   name: name,
                                                   version: version) {[weak self] res in
            guard let self = self else { return }
            self.completion(RemoteCommandResponseWrapper(response: res))
        }
    }
    
}

public class NameAndVersionRemoteCommand: RemoteCommand {
    
    private let _version: String?
    private let _name: String?
    
    override public var version: String? {
        return _version ?? super.version
    }
    override public var name: String {
        return _name ?? super.name
    }
    
    public init(commandId: String,
                description: String?,
                type: RemoteCommandType = .webview,
                name: String? = nil,
                version: String? = nil,
                completion : @escaping (_ response: RemoteCommandResponseProtocol) -> Void) {
        self._name = name
        self._version = version
        super.init(commandId: commandId, description: description, type: type, completion: completion)
    }
}
