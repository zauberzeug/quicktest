#!/bin/bash
set -x

if [[ $(git status -s) ]]; then
    echo "You have uncommitted files. Commit and push them before running this script."
    exit 1
fi

git fetch --tags

# get latest git tag and increase by one (see https://stackoverflow.com/questions/4485399/how-can-i-bump-a-version-number-using-bash)
VERSION=`git describe --abbrev=0 | awk -F. '/[0-9]+\./{$NF+=1;OFS=".";print}'`

echo "setting version to $VERSION"

function setVersion_Nupkg {
  sed -i '' "s/\(<version>\).*\(<\/version>\)/\1$VERSION\2/" $1
}

function packNuGet {
	setVersion_Nupkg $1
	nuget pack $1 || exit 1
}

function publishNuGet {
  git tag -a $VERSION -m ''  || exit 1
  git push --tags

  nuget push -Source https://www.nuget.org/api/v2/package $1
}

nuget restore QuickTest.sln || exit 1

msbuild /p:Configuration=Release QuickTest/QuickTest.csproj || exit 1
msbuild /p:Configuration=Release Tests/Tests.csproj || exit 1

pushd packages && nuget install Nunit.Runners && popd
export MONO_IOMAP=all # this fixes slash, backslash path seperator problems within nunit test runner
NUNIT=(packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe)
mono ${NUNIT[0]} --config=Release "Tests/Tests.csproj" || exit 1

packNuGet Xamarin.Forms.QuickTest.nuspec
publishNuGet Xamarin.Forms.QuickTest.$VERSION.nupkg
