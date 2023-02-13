#
# Be sure to run `pod lib lint TealiumWrapperCocoapods.podspec' to ensure this is a
# valid spec before submitting.
#
# Any lines starting with a # are optional, but their use is encouraged
# To learn more about a Podspec see https://guides.cocoapods.org/syntax/podspec.html
#

Pod::Spec.new do |s|
  s.name             = 'TealiumWrapperCocoapods'
  s.version          = '0.1.0'
  s.summary          = 'A short description of TealiumWrapperCocoapods.'

# This description is used to generate tags and improve search results.
#   * Think: What does it do? Why did you write it? What is the focus?
#   * Try to keep it short, snappy and to the point.
#   * Write the description between the DESC delimiters below.
#   * Finally, don't worry about the indent, CocoaPods strips it!

  s.description      = <<-DESC
TODO: Add long description of the pod here.
                       DESC

  s.homepage         = 'https://github.com/Enrico Zannini/TealiumWrapperCocoapods'
  # s.screenshots     = 'www.example.com/screenshots_1', 'www.example.com/screenshots_2'
  s.license          = { :type => 'MIT', :file => 'LICENSE' }
  s.author           = { 'Enrico Zannini' => 'enricozannini93@gmail.com' }
  s.source           = { :git => 'https://github.com/Enrico Zannini/TealiumWrapperCocoapods.git', :tag => s.version.to_s }
  # s.social_media_url = 'https://twitter.com/<TWITTER_USERNAME>'

  s.ios.deployment_target = '11.0'

  s.source_files = 'TealiumWrapperCocoapods/Classes/**/*'
  
  # s.resource_bundles = {
  #   'TealiumWrapperCocoapods' => ['TealiumWrapperCocoapods/Assets/*.png']
  # }

  # s.public_header_files = 'Pod/Classes/**/*.h'
  # s.frameworks = 'UIKit', 'MapKit'
  s.dependency 'tealium-swift/Core', '~> 2.9'
  s.dependency 'tealium-swift/TagManagement', '~> 2.9'
  s.dependency 'tealium-swift/Lifecycle', '~> 2.9'
  s.dependency 'tealium-swift/VisitorService', '~> 2.9'
  s.dependency 'tealium-swift/RemoteCommands', '~> 2.9'
  s.dependency 'tealium-swift/Collect', '~> 2.9'
  
end
