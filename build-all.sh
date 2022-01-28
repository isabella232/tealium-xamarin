#!/bin/bash          
#Author:    James Keith (james.keith@tealium.com)
#Date:      18-10-2018

#Build properties
Configuration=Release
Platform=AnyCPU

#Library names for reuse
common=Tealium.Common
droid=Tealium.Droid
ios=Tealium.iOS
platform_ios=Tealium.Platform.iOS
platform_droid=Tealium.Platform.Droid
nuget=Tealium.Xamarin.NuGet

#Project filenames for cleaning/building
proj_common=APIs/$common/$common.csproj
proj_droid=APIs/$droid/$droid.csproj
proj_ios=APIs/$ios/$ios.csproj
proj_platform_ios=APIs/$platform_ios/$platform_ios.csproj
proj_platform_droid=APIs/$platform_droid/$platform_droid.csproj


# echo "Cleaning..."
msbuild $proj_common /t:Clean /p:Configuration=$Configuration
msbuild $proj_droid /t:Clean /p:Configuration=$Configuration
msbuild $proj_ios /t:Clean /p:Configuration=$Configuration
msbuild $proj_platform_droid /t:Clean /p:Configuration=$Configuration
msbuild $proj_platform_ios /t:Clean /p:Configuration=$Configuration
#msbuild $proj_nuget /t:Clean /p:Configuration=$Configuration /p:Platform=$Platform
# echo "Deleting old build directories for $Configuration"
# rm -r ./APIs/$common/bin/$Configuration
# rm -r ./APIs/$droid/bin/$Configuration
# rm -r ./APIs/$ios/bin/$Configuration
# rm -r ./APIs/$platform_droid/bin/$Configuration
# rm -r ./APIs/$platform_ios/bin/$Configuration
# rm -r ./APIs/$nuget/bin/$Configuration


# echo "Building..."
msbuild $proj_common /p:Configuration=$Configuration
msbuild $proj_droid /p:Configuration=$Configuration
msbuild $proj_ios /p:Configuration=$Configuration
msbuild $proj_platform_droid /p:Configuration=$Configuration
msbuild $proj_platform_ios /p:Configuration=$Configuration
#msbuild $proj_nuget /p:Configuration=$Configuration /p:Platform=$Platform


# echo "Copying..."
# cp APIs/$common/bin/$Configuration/$common.dll ./Binaries
# cp APIs/$droid/bin/$Configuration/$droid.dll ./Binaries
# cp APIs/$ios/bin/$Configuration/$ios.dll ./Binaries
# cp APIs/$platform_droid/bin/$Configuration/$platform_droid.dll ./Binaries
# cp APIs/$platform_ios/bin/$Configuration/$platform_ios.dll ./Binaries

echo "Copying..."
mkdir -p Distribution
cp APIs/$common/bin/$Configuration/$common.dll ./Distribution
cp APIs/$droid/bin/$Configuration/$droid.dll ./Distribution
cp APIs/$ios/bin/$Configuration/$ios.dll ./Distribution
cp APIs/$platform_droid/bin/$Configuration/$platform_droid.dll ./Distribution
cp APIs/$platform_ios/bin/$Configuration/$platform_ios.dll ./Distribution


cd NuGet
./nuget-pack.sh
./nuget-push-to-local.sh