//
//  TealiumLogLevelWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 27/07/21.
//

import Foundation
import TealiumSwift


@objc public enum TealiumLogLevelWrapper: Int {
    
    case info = 0
    case debug = 100
    case error = 200
    case fault = 300
    case silent = -9999
    case undefined = -99999
    
    init(level: TealiumLogLevel?) {
        switch level {
        case .info:
            self = .info
        case .debug:
            self = .debug
        case .error:
            self = .error
        case .fault:
            self = .fault
        case .silent:
            self = .silent
        case .none, .some:
            self = .undefined
        }
    }
    
    var logLevel: TealiumLogLevel? {
        switch self {
        case .info:
            return .info
        case .debug:
            return .debug
        case .error:
            return .error
        case .fault:
            return .fault
        case .silent:
            return .silent
        case .undefined:
            return nil
        }
    }
}
