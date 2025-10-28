param(
  [string]$Tag = "lts-community",
  [int]$Port = 9000,
  [string]$Name = "sonarqube"
)

$ErrorActionPreference = "Stop"

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
  Write-Error "Docker not found. Install Docker Desktop or start SonarQube manually from a ZIP distribution."
  exit 1
}

# Compose image ref explicitly to avoid PS tokenization issues
$image = "sonarqube:$Tag"

# Check if a container with the given name exists
$existing = (docker ps -a --filter "name=^$Name$" --format "{{.ID}}")

if (-not $existing) {
  Write-Host "Pulling image $image ..."
  & docker pull $image | Out-Null

  Write-Host "Creating SonarQube container '$Name' on port $Port ..."
  $runArgs = @(
    "run",
    "-d",
    "--name", $Name,
    "-p", "$Port:9000",
    "-e", "SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true",
    $image
  )
  & docker @runArgs | Out-Null
} else {
  Write-Host "Starting existing SonarQube container '$Name'..."
  & docker start $Name | Out-Null
}

Write-Host "SonarQube starting at http://localhost:$Port"
Write-Host "Open the UI, log in (default admin/admin on first run), change the password, create a project and generate a token."
Write-Host "Then run scripts/run-sonar.ps1 -Token '<YOUR_TOKEN>'"
