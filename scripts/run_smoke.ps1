param([switch]$Fallback,[string]$Tag='final',[string]$Profile='',[string]$RuntimeDir='',[switch]$WaitForExit,[string]$GameDir=$env:KSA_GAME_DIR,[string]$LoaderDir=$env:KSA_LOADER_DIR)
$ErrorActionPreference='Stop'
$taskRoot=Split-Path $PSScriptRoot -Parent
if([string]::IsNullOrWhiteSpace($GameDir)) { throw 'Specify -GameDir or set KSA_GAME_DIR to the local KSA installation.' }
$GameDir=[IO.Path]::GetFullPath($GameDir)
if(!(Test-Path -LiteralPath (Join-Path $GameDir 'KSA.dll') -PathType Leaf)) { throw 'GameDir must contain KSA.dll.' }
if(!$LoaderDir) { $LoaderDir=Join-Path $taskRoot '.tools\StarMap-0.4.8' }
$taskWorkPrefix=(Join-Path $taskRoot 'work')+[IO.Path]::DirectorySeparatorChar
function Assert-WorkspacePath([string]$TaskPath) {
    if(!$TaskPath.StartsWith($taskWorkPrefix,[StringComparison]::OrdinalIgnoreCase)) { throw 'Smoke paths must be inside the project work directory.' }
    $taskCurrent=$TaskPath
    while($taskCurrent) {
        if(Test-Path -LiteralPath $taskCurrent) {
            if(((Get-Item -LiteralPath $taskCurrent -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { throw 'Smoke paths must not traverse junctions or symbolic links.' }
        }
        if($taskCurrent.Equals($taskRoot,[StringComparison]::OrdinalIgnoreCase)) { return }
        $taskCurrent=[IO.Path]::GetDirectoryName($taskCurrent)
    }
    throw 'Smoke path did not resolve inside the project root.'
}
if(!$Profile) { $Profile=Join-Path $taskRoot 'work\test-profile' }
$Profile=[IO.Path]::GetFullPath($Profile)
Assert-WorkspacePath $Profile
$taskProfileName=($Profile.Substring($taskWorkPrefix.Length) -split '[\\/]')[0]
if($taskProfileName -ne 'test-profile' -and !$taskProfileName.StartsWith('test-profile-',[StringComparison]::OrdinalIgnoreCase)) { throw 'Smoke is restricted to work/test-profile or work/test-profile-* directories.' }
if($Tag -notmatch '^[a-zA-Z0-9-]+$') { throw 'Invalid log tag' }
if(!$RuntimeDir) { $RuntimeDir=Join-Path $taskRoot 'work\runtime' }
$RuntimeDir=[IO.Path]::GetFullPath($RuntimeDir)
Assert-WorkspacePath $RuntimeDir
& dotnet build (Join-Path $taskRoot 'src\SmokeDriver\SmokeDriver.csproj') -c Release --nologo "-p:KsaDir=$GameDir" "-p:LoaderDir=$LoaderDir" "-p:BaseIntermediateOutputPath=$(Join-Path $taskRoot 'work\obj\SmokeDriver\')" "-p:OutputPath=$(Join-Path $taskRoot 'work\build\SmokeDriver\')"
if($LASTEXITCODE -ne 0) { throw 'Smoke driver build failed' }
$taskMods=Join-Path $Profile 'mods'
New-Item -ItemType Directory -Force -Path (Join-Path $taskMods 'SmokeDriver') | Out-Null
Copy-Item -LiteralPath (Join-Path $taskRoot 'work\build\SmokeDriver\SmokeDriver.dll') -Destination (Join-Path $taskMods 'SmokeDriver') -Force
[IO.File]::WriteAllText((Join-Path $taskMods 'SmokeDriver\mod.toml'),"name = `"5602 Workspace Smoke Driver`"`nassets = []`n[StarMap]`nEntryAssembly = `"SmokeDriver`"`n",[Text.UTF8Encoding]::new($false))
$taskManifest="[[mods]]`nid = `"Core`"`nenabled = true`n"
if(!$Fallback) {
    Copy-Item -LiteralPath (Join-Path $taskRoot 'work\build\KsaBabel') -Destination $taskMods -Recurse -Force
    $taskManifest+="`n[[mods]]`nid = `"KsaBabel`"`nenabled = true`n"
}
$taskManifest+="`n[[mods]]`nid = `"SmokeDriver`"`nenabled = true`n"
[IO.File]::WriteAllText((Join-Path $Profile 'manifest.toml'),$taskManifest,[Text.UTF8Encoding]::new($false))
$taskOld=$env:KSA_SMOKE_FALLBACK
$taskPreviousSmokeRoot=$env:KSA_SMOKE_PROJECT_ROOT
try {
    $env:KSA_SMOKE_FALLBACK=if($Fallback){'1'}else{'0'}
    $env:KSA_SMOKE_PROJECT_ROOT=$taskRoot
    $taskProcess=Start-Process -FilePath (Join-Path $RuntimeDir 'StarMap.exe') -WorkingDirectory $RuntimeDir -ArgumentList @('-InstancePath',('"'+$Profile+'"')) -RedirectStandardOutput (Join-Path $taskRoot "work\runtime\$Tag.stdout.log") -RedirectStandardError (Join-Path $taskRoot "work\runtime\$Tag.stderr.log") -WindowStyle Hidden -PassThru
    $taskProcess.Id | Set-Content -LiteralPath (Join-Path $taskRoot "work\runtime\$Tag.pid")
    Write-Output "Smoke process $($taskProcess.Id), profile $Profile"
    if($WaitForExit) {
        $null=$taskProcess.Handle
        $taskProcess.WaitForExit()
        $taskExitCode=$taskProcess.ExitCode
        if($null -eq $taskExitCode) { throw 'Fixture exit code unavailable; do not record success.' }
        [IO.File]::WriteAllText((Join-Path $taskRoot "work\runtime\$Tag.exit-code.txt"),[string]$taskExitCode)
        Write-Output "Fixture exit code: $taskExitCode"
        if($taskExitCode -ne 0) { throw "Fixture failed with exit code $taskExitCode" }
        if($env:KSA_SMOKE_MULTILINGUAL -eq '1') {
            $taskLog=Get-Content -Raw -LiteralPath (Join-Path $Profile 'smoke.log')
            if($taskLog.Contains('ERROR=') -or !$taskLog.Contains('multilingual-completed; locales=9')) {
                throw 'Multilingual fixture did not complete; process exit code alone is insufficient.'
            }
            $taskLocaleResults=Get-Content -Raw -LiteralPath (Join-Path $Profile 'multilingual-runtime.json') | ConvertFrom-Json
            if(@($taskLocaleResults.results).Count -ne 9) { throw 'Missing locale runtime evidence.' }
        }
    }
} finally { $env:KSA_SMOKE_FALLBACK=$taskOld; $env:KSA_SMOKE_PROJECT_ROOT=$taskPreviousSmokeRoot }
