[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $stdio
)

$PSES_BUNDLE_PATH = "./PowerShellEditorServices"
$SESSION_TEMP_PATH = "./workdirectory"

$startCommand = "$PSES_BUNDLE_PATH/PowerShellEditorServices/Start-EditorServices.ps1 -BundledModulesPath $PSES_BUNDLE_PATH -LogPath $SESSION_TEMP_PATH/logs.log -SessionDetailsPath $SESSION_TEMP_PATH/session.json -FeatureFlags @() -AdditionalModules @() -HostName 'My Client' -HostProfileId 'myclient' -HostVersion 1.0.0 -LogLevel Normal"

$startStdioCommand = "$PSES_BUNDLE_PATH/PowerShellEditorServices/Start-EditorServices.ps1 -BundledModulesPath $PSES_BUNDLE_PATH -LogPath $SESSION_TEMP_PATH/logs.log -SessionDetailsPath $SESSION_TEMP_PATH/session.json -FeatureFlags @() -AdditionalModules @() -HostName 'My Client' -HostProfileId 'myclient' -HostVersion 1.0.0 -Stdio -LogLevel Normal"

$command = $startCommand

Write-Output $command

if ($stdio) {
    $command = $startStdioCommand
}

Invoke-Expression -Command $command