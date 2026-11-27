#!/usr/bin/env bash
set -euo pipefail

required_env() {
  local name="$1"

  if [ -z "${!name:-}" ]; then
    echo "ERROR: $name must be set"
    exit 1
  fi
}

required_env "PACKAGE_ID"
required_env "PACKAGE_VERSION"
required_env "GITHUB_OWNER"
required_env "GITHUB_TOKEN"

GITHUB_ACTOR="${GITHUB_ACTOR:-github-actions}"
GITHUB_FEED_URL="https://nuget.pkg.github.com/${GITHUB_OWNER}/index.json"
WORK_ROOT="${RUNNER_TEMP:-/tmp}/nuget-package-consumption"
SAFE_PACKAGE_ID="${PACKAGE_ID//[^A-Za-z0-9_.-]/_}"
SAFE_PACKAGE_VERSION="${PACKAGE_VERSION//[^A-Za-z0-9_.-]/_}"
WORK_DIR="${WORK_ROOT}/${SAFE_PACKAGE_ID}-${SAFE_PACKAGE_VERSION}"
CONSUMER_DIR="${WORK_DIR}/consumer"
NUGET_CONFIG="${WORK_DIR}/NuGet.config"
NUGET_PACKAGES="${WORK_DIR}/packages"

export NUGET_PACKAGES

echo "Validating NuGet package consumption"
echo "Package: ${PACKAGE_ID}"
echo "Version: ${PACKAGE_VERSION}"
echo "Feed: ${GITHUB_FEED_URL}"

rm -rf "$WORK_DIR"
mkdir -p "$WORK_DIR"

cat > "$NUGET_CONFIG" <<EOF
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="GitHub" value="${GITHUB_FEED_URL}" />
  </packageSources>

  <packageSourceMapping>
    <packageSource key="GitHub">
      <package pattern="ThunderbirdsBoardGameEngine.*" />
    </packageSource>

    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
  </packageSourceMapping>
</configuration>
EOF

dotnet nuget update source GitHub \
  --username "$GITHUB_ACTOR" \
  --password "$GITHUB_TOKEN" \
  --configfile "$NUGET_CONFIG" \
  --store-password-in-clear-text

dotnet new console \
  --framework net10.0 \
  --output "$CONSUMER_DIR"

PROJECT_FILE="${CONSUMER_DIR}/consumer.csproj"

pushd "$CONSUMER_DIR" > /dev/null

PACKAGE_RESTORED=false

dotnet add "$PROJECT_FILE" package "$PACKAGE_ID" \
  --version "[$PACKAGE_VERSION]" \
  --no-restore

for attempt in {1..12}; do
  echo "Restoring package from GitHub Packages (attempt ${attempt}/12)."

  if dotnet restore "$PROJECT_FILE" \
      --configfile "$NUGET_CONFIG" \
      --no-cache \
      --force-evaluate; then
    PACKAGE_RESTORED=true
    break
  fi

  if [ "$attempt" -eq 12 ]; then
    break
  fi

  echo "Package is not restorable yet; waiting before retry."
  sleep 10
done

if [ "$PACKAGE_RESTORED" != "true" ]; then
  echo "ERROR: ${PACKAGE_ID} ${PACKAGE_VERSION} could not be restored from GitHub Packages."
  exit 1
fi

cat > Program.cs <<EOF
Console.WriteLine("Consumed ${PACKAGE_ID} ${PACKAGE_VERSION}.");
EOF

dotnet build "$PROJECT_FILE" \
  --configuration Release \
  --no-restore

# Runtime and Testing must expose the exact snapshot passed between CI jobs.
if [ -n "${EXPECTED_SNAPSHOT_SHA256:-}" ] && {
  [ "$PACKAGE_ID" = "ThunderbirdsBoardGameEngine.ReferenceData.Runtime" ] ||
  [ "$PACKAGE_ID" = "ThunderbirdsBoardGameEngine.ReferenceData.Testing" ];
}; then
  cat > Program.cs <<'CS'
using System.Security.Cryptography;
using ThunderbirdsBoardGameEngine.ReferenceData.Runtime;

var assembly = typeof(ServiceCollectionExtensions).Assembly;
var resource = assembly.GetManifestResourceNames().Single(name => name.EndsWith(".snapshot.json", StringComparison.Ordinal));
using var stream = assembly.GetManifestResourceStream(resource)
    ?? throw new InvalidOperationException("Embedded snapshot is missing.");
var actualHash = Convert.ToHexString(SHA256.HashData(stream));
var expectedHash = Environment.GetEnvironmentVariable("EXPECTED_SNAPSHOT_SHA256");
if (!string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException("Published package embeds a different snapshot from the validated artifact.");
}
Console.WriteLine("Embedded snapshot matches the validated artifact.");
CS
  dotnet run --project "$PROJECT_FILE" --configuration Release --no-restore
fi

popd > /dev/null

echo "Package consumption validation passed."
