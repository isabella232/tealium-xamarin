//
//  TealiumConfigWrapperTests.m
//  TealiumSwiftObjCWrapperTests
//
//  Created by Enrico Zannini on 23/07/21.
//

#import <XCTest/XCTest.h>
@import TealiumWrapperCocoapods;

@interface TealiumConfigWrapperTests : XCTestCase

@end

@implementation TealiumConfigWrapperTests {
    TealiumConfigWrapper * config;
}

- (void)setUp {
    config = [[TealiumConfigWrapper alloc] initWithAccount:@"testaccount"
                                                   profile:@"testprofile"
                                               environment:@"dev"
                                                dataSource:nil
                                                   options:nil];
}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
}

- (void)testCollectors {
    NSMutableArray * collectors = [NSMutableArray arrayWithArray:@[@(CollectorTypeLifecycle)]];
    [config setCollectors: collectors];
    XCTAssertEqualObjects(config.collectors, collectors);
    [collectors addObject:@(CollectorTypeConnectivity)];
    XCTAssertNotEqualObjects(config.collectors, collectors);
    [config setCollectors: collectors];
    XCTAssertEqualObjects(config.collectors, collectors);
}

- (void)testDisaptchers {
    NSMutableArray * dispatchers = [NSMutableArray arrayWithArray:@[@(DispatcherTypeCollect)]];
    [config setDispatchers:dispatchers];
    XCTAssertEqualObjects(config.dispatchers, dispatchers);
    [dispatchers addObject: @(DispatcherTypeTagManagement)];
    XCTAssertNotEqualObjects(config.dispatchers, dispatchers);
    [config setDispatchers:dispatchers];
    XCTAssertEqualObjects(config.dispatchers, dispatchers);
}

-(void)testRemoteCommands {
    void (^completion)(RemoteCommandResponseWrapper * _Nonnull) = ^void(RemoteCommandResponseWrapper * _Nonnull response) {
        
    };
    RemoteCommandWrapper * remoteCommand = [[RemoteCommandWrapper alloc] initWithCommandId:@"id"
                                                                               description:@"description"
                                                                                      type:[[RemoteCommandTypeWrapper alloc] init]
                                                                                completion: completion];
    NSMutableArray *commands = [NSMutableArray arrayWithArray: @[ remoteCommand ]];
    [config setRemoteCommands: commands];
    
    XCTAssertEqualObjects(config.remoteCommands, commands);
    RemoteCommandWrapper * remoteCommand2 = [[RemoteCommandWrapper alloc] initWithCommandId:@"id2"
                                                                               description:@"description2"
                                                                                      type:[[RemoteCommandTypeWrapper alloc] init]
                                                                                completion: completion];
    [commands addObject:remoteCommand2];
    XCTAssertNotEqualObjects(config.remoteCommands, commands);
    [config setRemoteCommands: commands];
    XCTAssertEqualObjects(config.remoteCommands, commands);
}

@end
