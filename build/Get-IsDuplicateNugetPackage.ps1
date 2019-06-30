
function Get-IsDuplicateNugetPackage {
    param (
        [string]$PackageId,
        [string]$Version
    )
    
    $json = Get-PackageRootJson $PackageId.ToLower();
    $releases = Get-Releases -Json $json;
    $versions = Get-Versions -Releases $releases;

    $isDuplicate = $versions.Contains($Version.ToLower());

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
        [string]$PackageId
    )
    $json = (Invoke-WebRequest https://api.nuget.org/v3/registration3/lcattell.reflectionextensions/index.json) | ConvertFrom-Json;
    
    $json;
}