  #!/bin/bash
set -x

VERSION="0.1"

# XAMARIN_TOOLS=/Library/Frameworks/Mono.framework/Versions/Current/Commands/
# NUGET="$XAMARIN_TOOLS/nuget"
# XBUILD="$XAMARIN_TOOLS/xbuild"
# MONO="$XAMARIN_TOOLS/mono"

if [[ $1 ]]; then
  echo "$1 was provided as a build number -- assuming we are executed on jenkins -> appending to version string"
  VERSION=$VERSION.$1
fi

echo "setting version to $VERSION"

function setVersion_Nupkg {
  sed -i '' "s/\(<version>\).*\(<\/version>\)/\1$VERSION\2/" $1
}

function setGitHead_Nupkg {
  HEAD=$( git rev-parse HEAD )
  HEAD=" (HEAD: $HEAD)"
  sed -i '' "s/\(<description>\)\(.*\)\(<\/description>\)/\1\2$HEAD\3/" $1
}

function packNuGet {
	setVersion_Nupkg $1
	setGitHead_Nupkg $1
	nuget pack $1 || exit 1
}

function publishNuGet {
  echo "not publishing nuget jet"
  #nuget push $1
}

function updateAssemblyInfos {
    DIRECTORY=$1
    echo "Update AssemblyInfo.cs files:"
    ASSEMBLY_INFOS=$(find $DIRECTORY -iname "assemblyinfo.cs")
    for ASSEMBLY_INFO in $ASSEMBLY_INFOS;
    do
        echo "Updating $ASSEMBLY_INFO"
        sed -E -i '' "s/AssemblyVersion.*\(.*\)/AssemblyVersion\(\"$VERSION\"\)/" $ASSEMBLY_INFO
        sed -E -i '' "s/AssemblyFileVersion.*\(.*\)/AssemblyFileVersion\(\"$VERSION\"\)/" $ASSEMBLY_INFO
    done
}

TAG="v$VERSION"
#git tag -a $TAG -m '' $GIT_COMMIT || exit 1
#git push origin $TAG || exit 1

nuget restore FormsTest.sln || exit 1
updateAssemblyInfos .

#xbuild /p:Configuration=Release FormsTest.sln || exit 1

pushd packages && nuget install Nunit.Runners && popd
export MONO_IOMAP=all # this fixes slash, backslash path seperator problems within nunit test runner
NUNIT="mono packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe"
$NUNIT -config=Release "NUnitTest/NUnitTest.csproj" || exit 1

rm *.nupkg
packNuGet userflow.nuspec
#publishNuGet *.nupkg
