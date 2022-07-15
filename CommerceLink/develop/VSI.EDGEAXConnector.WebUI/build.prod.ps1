.\build_script.ps1
Write-Host "Build : Removing Exisiting Build Code..."
Remove-Item .\dist -Recurse -ErrorAction Ignore
Write-Host "Build : Start."
ng build --env=prod --output-hashing none
Copy-Item .\Web.config .\dist
Write-Host "Build : Completed."
Copy-Item .\script.deploy.ps1 .\dist
# .\dist\script.deploy.ps1
