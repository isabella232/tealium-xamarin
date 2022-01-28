//
//  TrackingAndRemoteCommandAcceptanceTests.m
//  TealiumWrapperCocoapods_Tests
//
//  Created by Enrico Zannini on 24/08/21.
//  Copyright © 2021 CocoaPods. All rights reserved.
//

#define ACCOUNT @"tealiummobile"
#define PROFILE @"firebase-analytics"
#define TRIGGER_EVENT @"launch"
#define COMMAND_ID @"firebaseanalytics"
#import <XCTest/XCTest.h>
@import TealiumWrapperCocoapods;

@interface TrackingAndRemoteCommandAcceptanceTests : XCTestCase

@end

@implementation TrackingAndRemoteCommandAcceptanceTests

- (void)setUp {
    // Put setup code here. This method is called before the invocation of each test method in the class.
}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
}

- (void)testConfigRemoteCommand {
    XCTestExpectation * remoteCommandTrigger = [self expectationWithDescription:@"Command Triggered"];
    TealiumConfigWrapper* config = [[TealiumConfigWrapper alloc] initWithAccount:ACCOUNT profile: PROFILE environment:@"dev" dataSource:nil options:nil];
    config.remoteAPIEnabled = @YES;
    RemoteCommandTypeWrapper* type = [[RemoteCommandTypeWrapper alloc] initWithFile:@"stubs/mappings" bundle: [NSBundle bundleForClass:self.class]];
    RemoteCommandWrapper* rc = [[RemoteCommandWrapper alloc] initWithCommandId:COMMAND_ID
                                                                   description: @"TEST REMOTE COMMAND FOR"
                                                                          type:type
                                                                    completion:^(RemoteCommandResponseWrapper * _Nonnull res) {
        [remoteCommandTrigger fulfill];
    }];
    config.remoteCommands = @[rc];
    config.dispatchers = @[@(DispatcherTypeRemoteCommands)];
    config.logLevel = TealiumLogLevelWrapperInfo;
    __block TealiumWrapper* teal;
    teal = [[TealiumWrapper alloc] initWithConfig:config enableCompletion:^(BOOL success, NSError * _Nullable error) {
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(2 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            [teal trackWithTitle:TRIGGER_EVENT data:nil];
        });
    }];
    
    [self waitForExpectations:@[remoteCommandTrigger] timeout:20];
}


- (void)testRemoteCommand {
    XCTestExpectation * remoteCommandTrigger = [self expectationWithDescription:@"Command Triggered"];
    TealiumConfigWrapper* config = [[TealiumConfigWrapper alloc] initWithAccount:ACCOUNT profile: PROFILE environment:@"dev" dataSource:nil options:nil];
    config.remoteAPIEnabled = @YES;
    RemoteCommandTypeWrapper* type = [[RemoteCommandTypeWrapper alloc] initWithFile:@"stubs/mappings" bundle: [NSBundle bundleForClass:self.class]];
    __block RemoteCommandWrapper* rc = [[RemoteCommandWrapper alloc] initWithCommandId:COMMAND_ID
                                                                   description: @"TEST REMOTE COMMAND FOR"
                                                                          type:type
                                                                    completion:^(RemoteCommandResponseWrapper * _Nonnull res) {
        [remoteCommandTrigger fulfill];
    }];
    config.dispatchers = @[@(DispatcherTypeRemoteCommands)];
    config.logLevel = TealiumLogLevelWrapperInfo;
    __block TealiumWrapper* teal;
    teal = [[TealiumWrapper alloc] initWithConfig:config enableCompletion:^(BOOL success, NSError * _Nullable error) {
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(2 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            [teal addRemoteCommand:rc];
            [teal trackWithTitle:TRIGGER_EVENT data:nil];
        });
    }];
    
    [self waitForExpectations:@[remoteCommandTrigger] timeout:20];
}




@end
