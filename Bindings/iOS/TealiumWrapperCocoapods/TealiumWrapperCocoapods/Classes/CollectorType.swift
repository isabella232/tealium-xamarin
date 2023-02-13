//
//  CollectorType.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 22/07/21.
//

import Foundation
import TealiumSwift

@objc public enum CollectorType: Int {
    case appData
    case connectivity
    case deviceData
    case lifecycle
    case visitorService
    
    var collector: Collector.Type {
        switch self {
        case .appData:
            return AppDataModule.self
        case .connectivity:
            return ConnectivityModule.self
        case .deviceData:
            return DeviceDataModule.self
        case .lifecycle:
            return LifecycleModule.self
        case .visitorService:
            return VisitorServiceModule.self
        }
    }
    
    init?(collector: Collector.Type) {
        switch collector {
        case is AppDataModule.Type:
            self = .appData
        case is ConnectivityModule.Type:
            self = .connectivity
        case is DeviceDataModule.Type:
            self = .deviceData
        case is LifecycleModule.Type:
            self = .lifecycle
        case is VisitorServiceModule.Type:
            self = .visitorService
        default:
            return nil
        }
    }
    
}
