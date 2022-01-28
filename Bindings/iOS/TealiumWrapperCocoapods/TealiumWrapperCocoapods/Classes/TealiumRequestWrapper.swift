//
//  TealiumProtocols.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 22/07/21.
//

import Foundation
import TealiumSwift

@objc(QueueRequestResponse)
public class QueueRequestResponse: NSObject {
    @objc public let shouldQueue: Bool
    @objc public let data: [String: Any]?
    @objc public init(shouldQueue: Bool, data: [String: Any]?) {
        self.shouldQueue = shouldQueue
        self.data = data
    }
    
}

/// Request protocol
@objc(TealiumRequestWrapper)
open class TealiumRequestWrapper: NSObject, TealiumRequest {
    @objc public var typeId: String {
        get {
            trackRequest.typeId
        }
        set {
            trackRequest.typeId = newValue
        }
    }
    
    @objc open class func instanceTypeId() -> String {
        return TealiumTrackRequest.instanceTypeId()
    }
    
    var trackRequest: TealiumTrackRequest
    init(trackRequest: TealiumTrackRequest) {
        self.trackRequest = trackRequest
    }
    
    @objc public var trackDictionary: [String:Any] {
        return trackRequest.trackDictionary
    }
    
    
}
