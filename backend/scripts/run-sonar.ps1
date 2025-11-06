param(
  [Parameter(Mandatory=$true)][string]$Token,
  [string]$ProjectKey = "DealersAndDistributors",
  [string]$ProjectName = "DealersAndDistributors",
  [string]$HostUrl = "http://localhost:9000",
  [string]$Configuration = "Release",
  [switch]$IncludeCoverage,
  [string]$BuildCommand
)

$ErrorActionPreference = "Stop"

# Ensure scanner is available
if (-not (Get-Command dotnet-sonarscanner -ErrorAction SilentlyContinue)) {
  Write-Error "dotnet-sonarscanner not found. Install with: dotnet tool update --global dotnet-sonarscanner"
  exit 1
}

# Begin analysis
$beginArgs = @(
  "begin",
  "/k:$ProjectKey",
  "/n:$ProjectName",
  "/d:sonar.host.url=$HostUrl",
  "/d:sonar.login=$Token"
)

if ($IncludeCoverage) {
  # TRX test results and OpenCover coverage report paths
  $beginArgs += "/d:sonar.cs.vstest.reportsPaths=**/TestResults/*.trx"
  $beginArgs += "/d:sonar.cs.opencover.reportsPaths=**/TestResults/**/coverage.opencover.xml"
}

& dotnet-sonarscanner @beginArgs

# Restore and build
& dotnet restore

if ([string]::IsNullOrWhiteSpace($BuildCommand)) {
  & dotnet build --no-incremental --configuration $Configuration
} else {
  Write-Host "Running custom build: $BuildCommand"
  Invoke-Expression $BuildCommand
}

# Optional tests with coverage (Coverlet -> OpenCover)
if ($IncludeCoverage) {
  & dotnet test --configuration $Configuration --logger "trx;LogFileName=tests.trx" /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage/
}

# End analysis
& dotnet-sonarscanner end "/d:sonar.login=$Token"
