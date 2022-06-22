//
//  NameAndVersionRemoteCommandTests.swift
//  TealiumWrapperCocoapods_Tests
//
//  Created by Enrico Zannini on 26/04/22.
//  Copyright © 2022 CocoaPods. All rights reserved.
//

import XCTest
@testable import TealiumWrapperCocoapods
@testable import TealiumSwift

private let testName = "testName"
private let testVersion = "X.Y.Z"

class SomeRemoteCommandWrapper: RemoteCommandWrapper {
    override var name: String {
        testName
    }
    
    override var version: String? {
        testVersion
    }
}

class NameAndVersionRemoteCommandTests: XCTestCase {
    
    override func setUpWithError() throws {
        // Put setup code here. This method is called before the invocation of each test method in the class.
    }

    override func tearDownWithError() throws {
        // Put teardown code here. This method is called after the invocation of each test method in the class.
    }
    
    func testCommandCreation() {
        
        let command = NameAndVersionRemoteCommand(commandId: "id", description: "", type: .webview, name: testName, version: testVersion) { response in
            print(response)
        }
        XCTAssertEqual(testName, command.name)
        XCTAssertEqual(testVersion, command.version)
        print(command.nameAndVersion)
    }
    
    func testWrapper() {
        let command = SomeRemoteCommandWrapper(commandId: "id", description: "", type: RemoteCommandTypeWrapper()) { response in
            print(response)
        }
        XCTAssertEqual(testName, command.command.name)
        XCTAssertEqual(testVersion, command.command.version)
    }
}


