//
//  TealiumWrapperCocoapodsTests.m
//  TealiumWrapperCocoapods
//
//  Created by Enrico Zannini on 21/07/21.
//

#import <XCTest/XCTest.h>
@import TealiumWrapperCocoapods;

@interface TealiumWrapperCocoapodsTests : XCTestCase

@end

@implementation TealiumWrapperCocoapodsTests {
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

- (void)testExample {
    TealiumWrapper* teal = [[TealiumWrapper alloc] initWithConfig:config enableCompletion:^(BOOL success, NSError * _Nullable error) {
        NSLog(@"%i, %@", success, error);
    } ];

    XCTAssertNotNil(teal);
    // This is an example of a functional test case.
    // Use XCTAssert and related functions to verify your tests produce the correct results.
}

- (void)testConsentManager {
    XCTestExpectation * tealiumInitExpectation = [self expectationWithDescription:@"Tealium Init"];
    __block TealiumWrapper* teal;
    config.consentPolicy = TealiumConsentPolicyWrapperGdpr;
    teal = [[TealiumWrapper alloc] initWithConfig:config enableCompletion:^(BOOL success, NSError * _Nullable error) {
        
        ConsentManagerWrapper * manager = [teal consentManager];
        ConsentManagerWrapper * manager2 = [teal consentManager];
        XCTAssertNotNil(manager);
        XCTAssertEqual(manager, manager2);
        
        [tealiumInitExpectation fulfill];
    } ];
    [self waitForExpectations:@[tealiumInitExpectation] timeout:2];
}

- (void)testNSNumbersToDataLayer {
    XCTestExpectation * tealiumInitExpectation = [self expectationWithDescription:@"Tealium Init"];
    __block TealiumWrapper* teal;
    config.consentPolicy = TealiumConsentPolicyWrapperGdpr;
    teal = [[TealiumWrapper alloc] initWithConfig:config enableCompletion:^(BOOL success, NSError * _Nullable error) {
        __block NSNumber* intNum = [[NSNumber alloc] initWithInt:(int)2];
        __block NSNumber* floatNum = [[NSNumber alloc] initWithFloat:2.2f];
        [teal addToDataLayerWithData:@{@"intNum": intNum, @"floatNum": floatNum} expiry:ExpiryWrapperUntilRestart];
        
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(1 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            NSNumber* intRes = [teal getFromDataLayerWithKey:@"intNum"];
            NSNumber* floatRes = [teal getFromDataLayerWithKey:@"floatNum"];
            XCTAssertEqual(intNum, intRes);
            XCTAssertEqual(intNum.objCType, intRes.objCType);
            XCTAssertEqual(floatNum, floatRes);
            XCTAssertEqual(floatNum.objCType, floatRes.objCType);
            [tealiumInitExpectation fulfill];
        });
        
    } ];
    [self waitForExpectations:@[tealiumInitExpectation] timeout:2];
}


@end
