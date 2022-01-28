//
//  ConsentTypes.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 27/07/21.
//

import Foundation
import TealiumSwift


@objc public enum TealiumConsentStatusWrapper: Int {
    case unknown
    case consented
    case notConsented
    
    init(consentStatus: TealiumConsentStatus) {
        switch consentStatus {
        case .unknown:
            self = .unknown
        case .consented:
            self = .consented
        case .notConsented:
            self = .notConsented
        @unknown default:
            fatalError()
        }
    }
    
    var consentStatus: TealiumConsentStatus {
        switch self {
        case .unknown:
            return .unknown
        case .consented:
            return .consented
        case .notConsented:
            return .notConsented
        }
    }
}

@objc public enum TealiumConsentTrackAction: Int {
    case trackingAllowed
    case trackingForbidden
    case trackingQueued
}

@objc public enum TealiumConsentCategoriesWrappers: Int {

    case analytics
    case affiliates
    case displayAds
    case email
    case personalization
    case search
    case social
    case bigData
    case mobile
    case engagement
    case monitoring
    case crm
    case cdp
    case cookieMatch
    case misc
    
    init(category: TealiumConsentCategories) {
        switch category {
        case .analytics:
            self = .analytics
        case .affiliates:
            self = .affiliates
        case .displayAds:
            self = .displayAds
        case .email:
            self = .email
        case .personalization:
            self = .personalization
        case .search:
            self = .search
        case .social:
            self = .social
        case .bigData:
            self = .bigData
        case .mobile:
            self = .mobile
        case .engagement:
            self = .engagement
        case .monitoring:
            self = .monitoring
        case .crm:
            self = .crm
        case .cdp:
            self = .cdp
        case .cookieMatch:
            self = .cookieMatch
        case .misc:
            self = .misc
        @unknown default:
            fatalError()
        }
    }
    
    var category: TealiumConsentCategories {
        switch self {
        case .analytics:
            return .analytics
        case .affiliates:
            return .affiliates
        case .displayAds:
            return .displayAds
        case .email:
            return .email
        case .personalization:
            return .personalization
        case .search:
            return .search
        case .social:
            return .social
        case .bigData:
            return .bigData
        case .mobile:
            return .mobile
        case .engagement:
            return .engagement
        case .monitoring:
            return .monitoring
        case .crm:
            return .crm
        case .cdp:
            return .cdp
        case .cookieMatch:
            return .cookieMatch
        case .misc:
            return .misc
        }
    }

}

@objc public enum TealiumConsentPolicyWrapper: Int {
    case ccpa
    case gdpr
    case none
    
    init(policy: TealiumConsentPolicy?) {
        switch policy {
        case .ccpa:
            self = .ccpa
        case .gdpr:
            self = .gdpr
        default:
            self = .none
        }
    }
    
    var policy: TealiumConsentPolicy? {
        switch self {
        case .ccpa:
            return .ccpa
        case .gdpr:
            return .gdpr
        case .none:
            return nil
        }
    }
}
