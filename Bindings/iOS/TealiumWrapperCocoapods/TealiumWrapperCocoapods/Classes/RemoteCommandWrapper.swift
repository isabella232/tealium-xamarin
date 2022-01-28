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
    
    let type: RemoteCommandType
    @objc public let typeString: String
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
    
    /// Just used to allow the return of the completion variable
    @objc public var completion: (_ response: RemoteCommandResponseWrapper) -> Void

    /// Constructor for a Tealium Remote Command.
    ///
    /// - Parameters:
    ///     - commandId: `String` identifier for command block.
    ///     - description: `String?` description of command.
    ///     - completion: The completion block to run when this remote command is triggered.
    @objc public init(commandId: String,
                      description: String?,
                      type: RemoteCommandTypeWrapper = RemoteCommandTypeWrapper(),
                      completion : @escaping (_ response: RemoteCommandResponseWrapper) -> Void) {
        self.completion = completion
        self.command = nil
        super.init()
        self.command = RemoteCommand(commandId: commandId,
                                     description: description,
                                     type: type.type) {[weak self] res in
            guard let self = self else { return }
            self.completion(RemoteCommandResponseWrapper(response: res))
        }
    }
    
}
