<#
.SYNOPSIS
    Set environment variables in the environment.
    Azure Pipeline and CMD environments are considered.
.PARAMETER Variables
    A hashtable of variables to be set.
.OUTPUTS
    A boolean indicating whether the environment variables can be expected to propagate to the caller's environment.
#>
[CmdletBinding(SupportsShouldProcess=$true)]
Param(
    [Parameter(Mandatory=$true, Position=1)]
    $Variables,
    [string[]]$PrependPath
)

if ($Variables.Count -eq 0) {
    return $true
}

$cmdInstructions = !$env:TF_BUILD -and !$env:GITHUB_ACTIONS -and $env:PS1UnderCmd -eq '1'
if ($cmdInstructions) {
    Write-Warning "Environment variables have been set that will be lost because you're running under cmd.exe"
    Write-Output "Environment variables that must be set manually:" -ForegroundColor Blue
} else {
    Write-Output "Environment variables set:" -ForegroundColor Blue
    Write-Output ($Variables | Out-String)
    if ($PrependPath) {
        Write-Output "Paths prepended to PATH: $PrependPath"
    }
}

if ($env:TF_BUILD) {
    Write-Output "Azure Pipelines detected. Logging commands will be used to propagate environment variables and prepend path."
}

if ($env:GITHUB_ACTIONS) {
    Write-Output "GitHub Actions detected. Logging commands will be used to propagate environment variables and prepend path."
}

$Variables.GetEnumerator() |% {
    Set-Item -Path env:$($_.Key) -Value $_.Value

    # If we're running in a cloud CI, set these environment variables so they propagate.
    if ($env:TF_BUILD) {
        Write-Host "##vso[task.setvariable variable=$($_.Key);]$($_.Value)"
    }
    if ($env:GITHUB_ACTIONS) {
        Write-Output "::set-env name=$($_.Key)::$($_.Value)"
    }

    if ($cmdInstructions) {
        Write-Output "SET $($_.Key)=$($_.Value)"
    }
}

$pathDelimiter = ';'
if ($IsMacOS -or $IsLinux) {
    $pathDelimiter = ':'
}

if ($PrependPath) {
    $PrependPath |% {
        $newPathValue = "$_$pathDelimiter$env:PATH"
        Set-Item -Path env:PATH -Value $newPathValue
        if ($cmdInstructions) {
            Write-Output "SET PATH=$newPathValue"
        }

        if ($env:TF_BUILD) {
            Write-Host "##vso[task.prependpath]$_"
        }
        if ($env:GITHUB_ACTIONS) {
            Write-Output "::add-path::$_"
        }
    }
}

return !$cmdInstructions
