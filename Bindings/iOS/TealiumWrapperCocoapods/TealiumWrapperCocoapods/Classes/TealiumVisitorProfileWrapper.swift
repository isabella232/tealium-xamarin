//
//  TealiumVisitorProfileWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 28/07/21.
//

import Foundation
import TealiumSwift


@objc(TealiumVisitorProfileWrapper)
public class TealiumVisitorProfileWrapper: NSObject {
    
    @objc public var totalEventsCount: Int {
        Int(self.numbers?["22"] ?? 0)
    }
    
    @objc public var audiences: [String: String]? {
        visitorProfile.audiences
    }
    @objc public var badges: [String: Bool]? {
        visitorProfile.badges
    }
    @objc public var dates: [String: Int64]? {
        visitorProfile.dates
    }
    @objc public var booleans: [String: Bool]? {
        visitorProfile.booleans
    }
    @objc public var arraysOfBooleans: [String: [Bool]]? {
        visitorProfile.arraysOfBooleans
    }
    @objc public var numbers: [String: Double]? {
        visitorProfile.numbers
    }
    @objc public var arraysOfNumbers: [String: [Double]]? {
        visitorProfile.arraysOfNumbers
    }
    @objc public var tallies: [String: [String: Double]]? {
        visitorProfile.tallies
    }
    @objc public var strings: [String: String]? {
        visitorProfile.strings
    }
    @objc public var arraysOfStrings: [String: [String]]? {
        visitorProfile.arraysOfStrings
    }
    @objc public var setsOfStrings: [String: Set<String>]? {
        visitorProfile.setsOfStrings
    }
    @objc public let currentVisit: TealiumCurrentVisitProfileWrapper?
    
    let visitorProfile: TealiumVisitorProfile
    
    @objc public init(jsonData: Data) throws {
        visitorProfile = try JSONDecoder().decode(TealiumVisitorProfile.self, from: jsonData)
        currentVisit = TealiumCurrentVisitProfileWrapper(visitProfile: visitorProfile.currentVisit)
    }
    
    init(visitorProfile: TealiumVisitorProfile) {
        self.visitorProfile = visitorProfile
        self.currentVisit = TealiumCurrentVisitProfileWrapper(visitProfile: visitorProfile.currentVisit)
    }
    
    @objc public var isEmpty: Bool {
        return self.audiences == nil &&
            self.badges == nil &&
            self.currentVisit == nil &&
            self.dates == nil &&
            self.booleans == nil &&
            self.arraysOfBooleans == nil &&
            self.numbers == nil &&
            self.arraysOfNumbers == nil &&
            self.tallies == nil &&
            self.strings == nil &&
            self.arraysOfStrings == nil &&
            self.setsOfStrings == nil
    }
    
}

@objc(TealiumCurrentVisitProfileWrapper)
public class TealiumCurrentVisitProfileWrapper: NSObject {
    
    @objc public let createdAt: Int64
    
    @objc public var dates: [String: Int64]? {
        visitProfile.dates
    }
    @objc public var booleans: [String: Bool]? {
        visitProfile.booleans
    }
    @objc public var arraysOfBooleans: [String: [Bool]]? {
        visitProfile.arraysOfBooleans
    }
    @objc public var numbers: [String: Double]? {
        visitProfile.numbers
    }
    @objc public var arraysOfNumbers: [String: [Double]]? {
        visitProfile.arraysOfNumbers
    }
    @objc public var tallies: [String: [String: Double]]? {
        visitProfile.tallies
    }
    @objc public var strings: [String: String]? {
        visitProfile.strings
    }
    @objc public var arraysOfStrings: [String: [String]]? {
        visitProfile.arraysOfStrings
    }
    @objc public var setsOfStrings: [String: Set<String>]? {
        visitProfile.setsOfStrings
    }
    
    let visitProfile: TealiumCurrentVisitProfile
    init?(visitProfile: TealiumCurrentVisitProfile?) {
        guard let visitProfile = visitProfile else {
            return nil
        }
        self.visitProfile = visitProfile
        self.createdAt = Int64((Date().timeIntervalSince1970 * 1000.0).rounded())
    }
}
