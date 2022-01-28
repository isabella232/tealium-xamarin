#!/bin/bash          
#Author:    James Keith (james.keith@tealium.com)
#Date:      18-10-2018

#global vars for later
#cd ..

#nupkgs=find "../APIs" -name '*.nupkg'

for i in $(find Specs -name '*.nuspec')
do
   echo "Packaging $i"
   # or do whatever with individual element of the array
    #cp $i ./Packs
    nuget pack $i -OutputDirectory ./Packs

done