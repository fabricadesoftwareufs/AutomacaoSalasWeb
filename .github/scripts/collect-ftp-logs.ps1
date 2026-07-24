param(
    [Parameter(Mandatory = $true)]
    [string]$FtpServer,

    [Parameter(Mandatory = $true)]
    [string]$FtpUsername,

    [Parameter(Mandatory = $true)]
    [string]$FtpPassword,

    [Parameter(Mandatory = $true)]
    [string[]]$RemoteDirectories,

    [int]$MaxFiles = 5,

    [int]$TailLines = 200
)

$ErrorActionPreference = "Continue"
$credentials = New-Object System.Net.NetworkCredential($FtpUsername, $FtpPassword)

function New-FtpRequest {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RemotePath,

        [Parameter(Mandatory = $true)]
        [string]$Method
    )

    $normalizedPath = $RemotePath.TrimStart("/")
    $request = [System.Net.FtpWebRequest]::Create("ftp://$FtpServer/$normalizedPath")
    $request.Method = $Method
    $request.Credentials = $credentials
    $request.UseBinary = $true
    $request.KeepAlive = $false

    return $request
}

function Get-FtpList {
    param([Parameter(Mandatory = $true)][string]$RemoteDirectory)

    try {
        $request = New-FtpRequest -RemotePath $RemoteDirectory -Method ([System.Net.WebRequestMethods+Ftp]::ListDirectory)
        $response = $request.GetResponse()
        $reader = New-Object System.IO.StreamReader($response.GetResponseStream())
        $content = $reader.ReadToEnd()
        $reader.Close()
        $response.Close()

        return $content -split "\r?\n" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    }
    catch {
        Write-Host "Nao foi possivel listar ${RemoteDirectory}: $($_.Exception.Message)"
        return @()
    }
}

function Get-FtpTextFile {
    param([Parameter(Mandatory = $true)][string]$RemoteFile)

    try {
        $request = New-FtpRequest -RemotePath $RemoteFile -Method ([System.Net.WebRequestMethods+Ftp]::DownloadFile)
        $response = $request.GetResponse()
        $reader = New-Object System.IO.StreamReader($response.GetResponseStream())
        $content = $reader.ReadToEnd()
        $reader.Close()
        $response.Close()

        return $content
    }
    catch {
        Write-Host "Nao foi possivel baixar ${RemoteFile}: $($_.Exception.Message)"
        return $null
    }
}

$foundAnyLog = $false

foreach ($remoteDirectory in $RemoteDirectories) {
    $directory = $remoteDirectory.TrimEnd("/")
    Write-Host "Procurando logs em $directory..."

    $files = Get-FtpList -RemoteDirectory $directory |
        Where-Object { $_ -match "(\.txt|\.log)$" -or $_ -match "^stdout" } |
        Sort-Object -Descending |
        Select-Object -First $MaxFiles

    foreach ($file in $files) {
        $remoteFile = if ($file.StartsWith("/")) { $file } else { "$directory/$file" }
        $foundAnyLog = $true

        Write-Host "::group::$remoteFile"
        $content = Get-FtpTextFile -RemoteFile $remoteFile

        if ([string]::IsNullOrWhiteSpace($content)) {
            Write-Host "Arquivo vazio ou indisponivel."
        }
        else {
            $lines = $content -split "\r?\n"
            $start = [Math]::Max(0, $lines.Length - $TailLines)

            for ($i = $start; $i -lt $lines.Length; $i++) {
                Write-Host $lines[$i]
            }
        }

        Write-Host "::endgroup::"
    }
}

if (-not $foundAnyLog) {
    Write-Host "Nenhum arquivo de log foi encontrado nos diretorios informados."
}
