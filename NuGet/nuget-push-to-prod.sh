#!/bin/bash          
#Author:    James Keith (james.keith@tealium.com)
#Date:      18-10-2018


## now loop through the above array
for i in $(find Packs -name '*.nupkg')
do
   echo "$i"
   # or do whatever with individual element of the array
    #cp $i ./Packs
    #nuget push $1 -src https://www.nuget.org

done