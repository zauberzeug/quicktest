  #!/bin/bash
set -x

# get latest git tag and increase by one (see https://stackoverflow.com/questions/4485399/how-can-i-bump-a-version-number-using-bash)
VERSION=`git describe --abbrev=0 | awk -F. '/[0-9]+\./{$NF+=1;OFS=".";print}'`

echo "setting version to $VERSION"

function setVersion_Nupkg {
  sed -i '' "s/\(<version>\).*\(<\/version>\)/\1$VERSION-pre\2/" $1
}

function packNuGet {
	setVersion_Nupkg $1
	nuget pack $1 || exit 1
}

function publishNuGet {
  git add $1
  git commit -am "nuget package ${VERSION}" || exit 1
  git tag -a $VERSION -m ''  || exit 1

  git push
  git push --tags

  echo "not publishing to nuget.org jet"
  #nuget push $1
}

nuget restore UserFlow.sln || exit 1

xbuild /p:Configuration=Release UserFlow/UserFlow.csproj || exit 1
xbuild /p:Configuration=Release Tests/Tests.csproj || exit 1

pushd packages && nuget install Nunit.Runners && popd
export MONO_IOMAP=all # this fixes slash, backslash path seperator problems within nunit test runner
NUNIT="mono packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe"
$NUNIT -config=Release "Tests/Tests.csproj" || exit 1

packNuGet userflow.nuspec
publishNuGet UserFlow.$VERSION-pre.nupkg
