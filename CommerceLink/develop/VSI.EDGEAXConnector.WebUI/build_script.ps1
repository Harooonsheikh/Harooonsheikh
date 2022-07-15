Write-Host "Modules : Cleaning..."
Remove-Item .\node_modules -Recurse -Force -ErrorAction Ignore 
npm cache verify
Write-Host "Modules : Installing..."
npm install --global --production windows-build-tools
npm install --global node-gyp
setx PYTHON "%USERPROFILE%\.windows-build-tools\python27\python.exe"
npm install
Write-Host "Modules : Installation Completed..."