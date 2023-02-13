//
//  TimeWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 22/07/21.
//

import Foundation
import TealiumSwift

@objc public enum TimeUnitWrapper: Int {
    case minutes
    case hours
    case days
    case months
    case years

    init(unit: TimeUnit) {
        switch unit {
        case .minutes:
            self = .minutes
        case .hours:
            self = .hours
        case .days:
            self = .days
        case .months:
            self = .months
        case .years:
            self = .years
        @unknown default:
            fatalError()
        }
    }
    
    var unit: TimeUnit {
        switch self {
        case .minutes:
            return .minutes
        case .hours:
            return .hours
        case .days:
            return .days
        case .months:
            return .months
        case .years:
            return .years
        }
    }
}

@objc(ExpirationTime)
public class ExpirationTime: NSObject {
    @objc public let time: Int
    @objc public let unit: TimeUnitWrapper
    
    @objc public init(time: Int, unit: TimeUnitWrapper) {
        self.time = time
        self.unit = unit
    }
    
}



@objc public enum RefreshTimeWrapper: Int {
    case seconds
    case minutes
    case hours
    
    init(refreshTime: RefreshTime) {
        switch refreshTime {
        case .seconds:
            self = .seconds
        case .minutes:
            self = .minutes
        case .hours:
            self = .hours
        @unknown default:
            fatalError()
        }
    }
    
    var refreshTime: RefreshTime {
        switch self {
        case .seconds:
            return .seconds
        case .minutes:
            return .minutes
        case .hours:
            return .hours
        }
    }
}

@objc(TealiumRefreshIntervalWrapper)
public class TealiumRefreshIntervalWrapper: NSObject {
    @objc public let amount: Int
    @objc public let unit: RefreshTimeWrapper
    
    @objc public init(amount: Int, unit: RefreshTimeWrapper) {
        self.amount = amount
        self.unit = unit
    }
    
    convenience init(refreshInterval: TealiumRefreshInterval) {
        switch refreshInterval {
        case .every(let amount, let unit):
            self.init(amount: amount, unit: RefreshTimeWrapper(refreshTime: unit))
        @unknown default:
            fatalError()
        }
    }
    
    var refreshInterval: TealiumRefreshInterval {
        return .every(amount, unit.refreshTime)
    }
}


@objc public enum ExpiryWrapper: Int {
    case session
    case untilRestart
    case forever
    
    var expiry: Expiry {
        switch self {
        case .session:
            return .session
        case .untilRestart:
            return .untilRestart
        case .forever:
            return .forever
        }
    }
}
