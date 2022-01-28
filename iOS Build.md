# iOS Build

In order to make this project work for iOS, a TealiumSwift and a TealiumWrapperCocoapods framework need to be in the `APIs/Tealium.Platform.iOS/Release-fat` folder.
Installing them is as easy as installing via cocoapods the Example workspace `Bindings/iOS/TealiumWrapperCocoapods/Example/TealiumWrapperCocoapods.xcworkspace` and build the `TealiumWrapperSample` scheme in release mode.
This will compile both frameworks for device and simulator, merge them in two fat frameworks and place them in the correct place.

## Post Install Script algorithm

The post install script should first create a Generated-Frameworks in the cocoapod Example folder.
Then TealiumSwift and TealiumWrapperCocoapods frameworks should be created for Simulator and Device, and then merged in the Fat folder.
This is then copied in the `APIs/Tealium.Platform.iOS/Release-fat` folder, where it can be consumed by our project.

## Troubleshooting

In case you see some errors in Visual Studio, be sure to clean and rebuild `Tealium.Platform.iOS` and then `Tealium.iOS` after you generated the native libraries, just to be sure that they are being correctly referenced by those libraries.