
function Get-IsDuplicateNugetPackage {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $PackageId,

        [Parameter(Mandatory = $true)]
        [string]
        $Version,

        [string]
        $ApiKey
    )
    
    if((!([string]::IsNullOrWhiteSpace($ApiKey))))
    {
        $json = Get-PackageRootJson $PackageId -ApiKey $ApiKey;
    }
    else 
    {
        $json = Get-PackageRootJson $PackageId;
    }

    $releases = Get-Releases -Json $json;
    $versions = Get-Versions -Releases $releases;

    $isDuplicate = $versions -Contains $Version;

    $isDuplicate;
}

function Get-Versions {
    param (
        $Releases
    )
    
    $versions = $Releases | ForEach-Object {
        (Get-ReleaseInfo -Release $_).version;
    }

    $versions;
}

function Get-Version {
    param (
        $ReleaseInfo
    )
    $ReleaseInfo.version;
}

function Get-ReleaseInfo {
    param (
        $Release
    )
    
    $release.catalogEntry;
}

function Get-Releases {
    param (
        $Json
    )
    $releases = $json.items[0].items;

    $releases;
}

function Get-PackageRootJson {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $PackageId,

        [string]
        $ApiKey
    )

    $packageId = $PackageId.ToLower();

    if (!([string]::IsNullOrWhiteSpace($ApiKey))) 
    {
        $json = (Invoke-WebRequest https://api.nuget.org/v3/registration3/$packageId/index.json -UseBasicParsing -Headers -Headers @{"X-NuGet-ApiKey"="$ApiKey"}) | ConvertFrom-Json;
    }
    else
    {
        $json = (Invoke-WebRequest https://api.nuget.org/v3/registration3/$packageId/index.json -UseBasicParsing) | ConvertFrom-Json;
    }
    
    $json;
}