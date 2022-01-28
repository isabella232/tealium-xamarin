//
//  VisitorProfileTests.m
//  TealiumWrapperCocoapods_Tests
//
//  Created by Enrico Zannini on 25/08/21.
//  Copyright © 2021 CocoaPods. All rights reserved.
//

#import <XCTest/XCTest.h>
#import "TealiumWrapperCocoapods_Tests-Swift.h"

@interface VisitorProfileTests : XCTestCase

@end

@implementation VisitorProfileTests

- (void)setUp {
    // Put setup code here. This method is called before the invocation of each test method in the class.
}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
}

- (void)testExample {
    NSData* data = [TestHelper loadStubFrom:@"visitor" :self.class];
    TealiumVisitorProfileWrapper* profile = [[TealiumVisitorProfileWrapper alloc] initWithJsonData:data error:nil];
    XCTAssertNotNil(profile);
    XCTAssertNotNil(profile.currentVisit);
}


@end
