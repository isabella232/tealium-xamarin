//
//  ConsentManagerWrapperTests.m
//  TealiumSwiftObjCWrapperTests
//
//  Created by Enrico Zannini on 23/07/21.
//

#import <XCTest/XCTest.h>
@import TealiumWrapperCocoapods;
#import "TealiumWrapperCocoapods_Tests-Swift.h"

@interface ConsentManagerWrapperTests : XCTestCase

@end

@implementation ConsentManagerWrapperTests {
    TealiumConfigWrapper * config;
}

- (void)setUp {
    config = [[TealiumConfigWrapper alloc] initWithAccount:@"testaccount"
                                                   profile:@"testprofile"
                                               environment:@"dev"
                                                dataSource:nil
                                                   options:nil];
    config.consentPolicy = TealiumConsentPolicyWrapperGdpr;

}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
}

- (void)testUserConsentStatus {
    ConsentManagerWrapper * manager = [ConsentManagerWrapper createMockManagerWithConfig: config];
    manager.userConsentStatus = TealiumConsentStatusWrapperConsented;
    XCTAssertEqual(manager.userConsentStatus, TealiumConsentStatusWrapperConsented);
}

- (void)testUserConsentCategories {
    ConsentManagerWrapper * manager = [ConsentManagerWrapper createMockManagerWithConfig: config];
    NSArray * categories = @[@(TealiumConsentCategoriesWrappersAnalytics),
                             @(TealiumConsentCategoriesWrappersCdp)];
    manager.userConsentCategories = categories;
    XCTAssertEqualObjects(manager.userConsentCategories, categories);
}

- (void)testSetUserConsentStatusAndCategories {
    ConsentManagerWrapper * manager = [ConsentManagerWrapper createMockManagerWithConfig: config];
    NSArray * categories = @[@(TealiumConsentCategoriesWrappersAnalytics),
                             @(TealiumConsentCategoriesWrappersCdp)];
    [manager setUserConsentStatus:TealiumConsentStatusWrapperConsented withCategories:categories];
    XCTAssertEqualObjects(manager.userConsentCategories, categories);
    XCTAssertEqual(manager.userConsentStatus, TealiumConsentStatusWrapperConsented);
    [manager resetUserConsentPreferences];
    XCTAssertNil(manager.userConsentCategories);
    XCTAssertEqual(manager.userConsentStatus, TealiumConsentStatusWrapperUnknown);
    
}

@end
