//
//  RemoteCommandWrapperTests.m
//  TealiumSwiftObjCWrapperTests
//
//  Created by Enrico Zannini on 27/07/21.
//

#import <XCTest/XCTest.h>
@import TealiumWrapperCocoapods;

@interface RemoteCommandWrapperTests : XCTestCase

@end

@implementation RemoteCommandWrapperTests

- (void)setUp {
    // Put setup code here. This method is called before the invocation of each test method in the class.
}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
}

- (void)testCommandCreation {
    __block NSInteger i = 0;
    void (^completion)(RemoteCommandResponseWrapper * _Nonnull) = ^void(RemoteCommandResponseWrapper * _Nonnull response) {
        i++;
    };
    RemoteCommandWrapper * remoteCommand = [[RemoteCommandWrapper alloc] initWithCommandId:@"id"
                                                                               description:@"description"
                                                                                      type:[[RemoteCommandTypeWrapper alloc] init]
                                                                                completion: completion];
    remoteCommand.completion((RemoteCommandResponseWrapper*)[NSObject new]);
    XCTAssertEqual(i, 1);
    
    XCTAssertEqual(remoteCommand.commandId, @"id");
    XCTAssertEqual(remoteCommand.commandDescription, @"description");
}

- (void)testChangeDescription {
    void (^completion)(RemoteCommandResponseWrapper * _Nonnull) = ^void(RemoteCommandResponseWrapper * _Nonnull response) {
        
    };
    RemoteCommandWrapper * remoteCommand = [[RemoteCommandWrapper alloc] initWithCommandId:@"id"
                                                                               description:@"description"
                                                                                      type:[[RemoteCommandTypeWrapper alloc] init]
                                                                                completion: completion];
    XCTAssertEqual(remoteCommand.commandDescription, @"description");
    remoteCommand.commandDescription = @"description2";
    XCTAssertEqual(remoteCommand.commandDescription, @"description2");
}

- (void)testChangeCompletion {
    __block NSInteger i = 0;
    void (^completion)(RemoteCommandResponseWrapper * _Nonnull) = ^void(RemoteCommandResponseWrapper * _Nonnull response) {
        i++;
    };
    void (^completion2)(RemoteCommandResponseWrapper * _Nonnull) = ^void(RemoteCommandResponseWrapper * _Nonnull response) {
        i+=10;
    };
    RemoteCommandWrapper * remoteCommand = [[RemoteCommandWrapper alloc] initWithCommandId:@"id"
                                                                               description:@"description"
                                                                                      type:[[RemoteCommandTypeWrapper alloc] init]
                                                                                completion: completion];
    remoteCommand.completion((RemoteCommandResponseWrapper*)[NSObject new]);
    XCTAssertEqual(i, 1);
    
    remoteCommand.completion = completion2;
    remoteCommand.completion((RemoteCommandResponseWrapper*)[NSObject new]);
    XCTAssertEqual(i, 11);
}

@end
