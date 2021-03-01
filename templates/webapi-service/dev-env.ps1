param (
  $command
)

function Main() {  
  if ($command -eq "start") {
    Write-Host "Starting development environment"
    docker-compose -f docker-compose.dev-env.yml up -d
  }
  elseif ($command -eq "stop") {
    Write-Host "Stoping development environment"
    docker-compose -f docker-compose.dev-env.yml down -v --rmi local --remove-orphans
  }  
}

Main