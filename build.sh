  #!/bin/bash
set -x

# get latest git tag and increase by one (see https://stackoverflow.com/questions/4485399/how-can-i-bump-a-version-number-using-bash)
VERSION=`git describe --abbrev=0 | awk -F. '/[0-9]+\./{$NF+=1;OFS=".";print}'`

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

nuget restore UserFlow.sln || exit 1
updateAssemblyInfos .

xbuild /p:Configuration=Release UserFlow.sln || exit 1

pushd packages && nuget install Nunit.Runners && popd
export MONO_IOMAP=all # this fixes slash, backslash path seperator problems within nunit test runner
NUNIT="mono packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe"
#$NUNIT -config=Release "Tests/Tests.csproj" || exit 1


packNuGet userflow.nuspec
publishNuGet UserFlow.$VERSION.nupkg

git commit -am "nuget package v${VERSION}" || exit 1
git tag -a $VERSION -m ''  || exit 1
