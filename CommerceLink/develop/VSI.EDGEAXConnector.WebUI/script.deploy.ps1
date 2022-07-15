

function Is-Installed( $program ) {
    
    $x86 = ((Get-ChildItem "HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall") |
        Where-Object { $_.GetValue( "DisplayName" ) -like "*$program*" } ).Length -gt 0;

    $x64 = ((Get-ChildItem "HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall") |
        Where-Object { $_.GetValue( "DisplayName" ) -like "*$program*" } ).Length -gt 0;

    return $x86 -or $x64;
}




$exist = Is-Installed "IIS URL Rewrite Module 2"
if (!$exist) {
    Remove-Item .\iisPackage -Recurse -ErrorAction Ignore
    New-Item -Path './iisPackage' -ItemType directory
    Invoke-WebRequest 'https://download.microsoft.com/download/C/9/E/C9E8180D-4E51-40A6-A9BF-776990D8BCA9/rewrite_amd64.msi' -OutFile ./iisPackage/iisrewrite.msi
    ./iisPackage/iisrewrite.msi
}

Import-Module WebAdministration
$appPath = (Get-Item -Path ".\" -Verbose).FullName +"\"
New-WebSite -Name DashboardApp -Port 5680 -PhysicalPath $appPath -Force