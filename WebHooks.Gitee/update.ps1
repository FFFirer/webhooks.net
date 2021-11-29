[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $Update
)

$VerbosePreference = "Continue"
$DebugPreference = "Continue"

Write-Debug "$Update"