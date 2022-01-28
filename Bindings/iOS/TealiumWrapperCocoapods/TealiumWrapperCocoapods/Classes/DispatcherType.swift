//
//  DispatcherType.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 22/07/21.
//

import Foundation
import TealiumSwift



@objc public enum DispatcherType: Int {
    case collect
    case remoteCommands
    case tagManagement
    
    var dispatcher: Dispatcher.Type {
        switch self {
        case .collect:
            return CollectModule.self
        case .remoteCommands:
            return RemoteCommandsModule.self
        case .tagManagement:
            return TagManagementModule.self
        }
    }
    
    init?(dispatcher: Dispatcher.Type) {
        switch dispatcher {
        case is CollectModule.Type:
            self = .collect
        case is RemoteCommandsModule.Type:
            self = .remoteCommands
        case is TagManagementModule.Type:
            self = .tagManagement
        default:
            return nil
        }
    }
}

/**
 * Override this class to provide the implementation of the wrapped methods
 */
@objc(DispatchValidatorWrapper)
open class DispatchValidatorWrapper: NSObject, DispatchValidator {
    public func shouldQueue(request: TealiumRequest) -> (Bool, [String : Any]?) {
        let res = shouldQueue(request: TealiumRequestWrapper(trackRequest: request as! TealiumTrackRequest))
        return (res.shouldQueue, res.data)
    }
    
    public func shouldDrop(request: TealiumRequest) -> Bool {
        return shouldDrop(request: TealiumRequestWrapper(trackRequest: request as! TealiumTrackRequest))
    }
    
    public func shouldPurge(request: TealiumRequest) -> Bool {
        return shouldPurge(request: TealiumRequestWrapper(trackRequest: request as! TealiumTrackRequest))
    }
    
    @objc open var id: String
    @objc public init(id: String) {
        self.id = id
    }
    
    @objc open func shouldQueue(request: TealiumRequestWrapper) -> QueueRequestResponse {
        return QueueRequestResponse(shouldQueue: false, data: nil)
    }
    @objc open func shouldDrop(request: TealiumRequestWrapper) -> Bool {
        return false
    }
    @objc open func shouldPurge(request: TealiumRequestWrapper) -> Bool {
        return false
    }
}
/**
 * Override this class to provide the implementation of the wrapped methods
 */
@objc(DispatchListenerWrapper)
open class DispatchListenerWrapper: NSObject, DispatchListener {
    
    public func willTrack(request: TealiumRequest) {
        willTrack(request: request as! TealiumRequestWrapper)
    }
    
    /// Empty method, just override it to implement the dispatch
    @objc public func willTrack(request: TealiumRequestWrapper) {
        
    }
}
