//
//  ConsentManagerWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 23/07/21.
//

import Foundation
import TealiumSwift

@objc(ConsentManagerWrapper)
public class ConsentManagerWrapper: NSObject {
    
    var consentManager: ConsentManager
    let config: TealiumConfigWrapper
    
    init(consentManager: ConsentManager, config: TealiumConfigWrapper) {
        self.consentManager = consentManager
        self.config = config
    }
    
    @objc public var onConsentExpiration: (() -> Void)? {
        get {
            consentManager.onConsentExpiraiton
        }
        set {
            consentManager.onConsentExpiraiton = newValue
        }
    }
    
    /// Returns current consent status
    @objc public var userConsentStatus: TealiumConsentStatusWrapper {
        get {
            return TealiumConsentStatusWrapper(consentStatus: consentManager.userConsentStatus)
        }
        set {
            consentManager.userConsentStatus = newValue.consentStatus
        }
    }
    
    /// Returns current consent categories, if applicable
    @objc public var userConsentCategories: [Int]? {
        get {
            consentManager.userConsentCategories?.compactMap { TealiumConsentCategoriesWrappers(category: $0).rawValue }
        }
        set {
            consentManager.userConsentCategories = newValue?.compactMap{ TealiumConsentCategoriesWrappers(rawValue: $0)?.category }
        }
    }
    
    @objc public var consentLoggingEnabled: Bool {
        config.consentLoggingEnabled
    }

    @objc public var consentManagerEnabled: Bool {
        config.consentPolicy != .none
    }
    
    @objc public var policy: TealiumConsentPolicyWrapper {
        config.consentPolicy
    }
    
    @objc public func resetUserConsentPreferences() {
        consentManager.resetUserConsentPreferences()
    }
    
    
    // This is not public in the native class so it's not very useful
    @objc public func setUserConsentStatus(_ status: TealiumConsentStatusWrapper, withCategories categories: [Int]) {
        self.userConsentStatus = status
        self.userConsentCategories = categories
    }
    
}
