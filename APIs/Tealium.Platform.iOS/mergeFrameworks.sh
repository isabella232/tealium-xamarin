rm -rf "fat"
cp -R "simulator" "fat"


cp -R "device/TealiumSwift.framework/Modules/TealiumSwift.swiftmodule/" "fat/TealiumSwift.framework/Modules/TealiumSwift.swiftmodule/"
cp -R "device/TealiumWrapperCocoapods.framework/Modules/TealiumWrapperCocoapods.swiftmodule/" "fat/TealiumWrapperCocoapods.framework/Modules/TealiumWrapperCocoapods.swiftmodule/"

lipo -create -output "fat/TealiumSwift.framework/TealiumSwift" "device/TealiumSwift.framework/TealiumSwift" "simulator/TealiumSwift.framework/TealiumSwift"
lipo -create -output "fat/TealiumWrapperCocoapods.framework/TealiumWrapperCocoapods" "device/TealiumWrapperCocoapods.framework/TealiumWrapperCocoapods" "simulator/TealiumWrapperCocoapods.framework/TealiumWrapperCocoapods"


sharpie bind --sdk=iphoneos14.5 --output="XamarinApiDef" --namespace="Tealium.Platform.iOS" --scope="fat/TealiumWrapperCocoapods.framework/Headers/" "fat/TealiumWrapperCocoapods.framework/Headers/TealiumWrapperCocoapods-Swift.h"
