//
//  empty.swift
//  TealiumSwiftObjCWrapperTests
//
//  Created by Enrico Zannini on 21/07/21.
//

import Foundation
@testable import TealiumSwift
@testable import TealiumWrapperCocoapods

@objc
public extension ConsentManagerWrapper {
    
    @objc static func createMockManager(config: TealiumConfigWrapper) -> ConsentManagerWrapper {
        let consent = ConsentManager(config: config.config,
                                     delegate: nil,
                                     diskStorage: ConsentMockDiskStorage(),
                                     dataLayer: nil)
        return ConsentManagerWrapper(consentManager: consent, config: config)
    }
}


@objc class TestHelper: NSObject {
    
    @objc class func loadStub(from file: String,
                        _ cls: AnyClass) -> Data {
      let bundle = Bundle(for: cls)
      let url = bundle.url(forResource: "stubs/" + file, withExtension: "json")
      return try! Data(contentsOf: url!)
    }
    
}
