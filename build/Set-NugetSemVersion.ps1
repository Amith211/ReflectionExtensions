function Get-NugetVersion {
    param (
        [switch]$IsPreview,
        [string]$PreReleaseTag = $null
    )
    $branchName = $env:BUILD_SOURCEBRANCHNAME;
    $ver = "";

    $preReleaseTag = Get-PrereleaseTag -PreReleaseTag $PreReleaseTag;
    $baseVer = Get-BaseVersion -BranchName $branchName;
    $ver = Get-FullVersion -BaseVersion $baseVer -BranchName $branchName -IsPreview $IsPreview -PreReleaseTag $preReleaseTag;

    echo "##vso[task.setvariable variable=nugetVersion]$ver"
    [Environment]::SetEnvironmentVariable("LC_NUGETVERSION", "$ver", "User")
    Write-Host "Var is $ver";
    Write-Host "Env is $env:LC_NUGETVERSION";
    Write-Host "vso is $env:nugetVersion";
}

function Get-FullVersion 
{
    param (
        [string]$BaseVersion,
        [string]$BranchName,
        [switch]$IsPreview,
        [string]$PreReleaseTag = 'beta'
    )
    $ver = $BaseVersion;

    if ($IsPreview) 
    {
        if ($BranchName -eq "master") 
        {
            $ver += "-$PreReleaseTag";
        }

        if ($BranchName -like "azure-pipelines-test*") 
        {
            $ver += "-$PreReleaseTag-ReleasePipelineTest";
        }
    }
    else 
    {
      if ($BranchName -like "azure-pipelines-test*") 
      {
          $ver += "-ReleasePipelineTest" 
      }  
    }

    $ver;
}

function Get-AzurePipeliesTestVersion {
    param (
        [string]$BaseVersion,
        [string]$BranchName,
        [switch]$IsPreview,
        [string]$PreReleaseTag
    )
    if ($IsPreview) 
    {
        if ($BranchName -like "azure-pipelines-test*" -and $IsPreview) 
        {
            $ver += "-$PreReleaseTag-ReleasePipelineTest";
        }
    }
    elseif ($BranchName -like "azure-pipelines-test*") 
    {
        $ver += "-ReleasePipelineTest" 
    }
}

function Get-BaseVersion {
    param (
        [string]$BranchName
    )
    $ver = "";

    if ($BranchName -like "azure-pipelines-test*") 
    {
        $ver = "$env:GITVERSION_MAJORMINORPATCH"
        #-$PreReleaseTag-ReleasePipelineTest
    } 
    else 
    {
        $ver = $env:GITVERSION_NUGETVERSIONV2;
    }     
}


function Get-PrereleaseTag {
    param (
        [string]$PreReleaseTag = $null
    )

    if ($null -ne $env:GITVERSION_NUGETPRERELEASETAGV2 -or $env:GITVERSION_NUGETPRERELEASETAGV2 -ne "" ) 
        {
            $PreReleaseTag = $env:GITVERSION_NUGETPRERELEASETAGV2
        }
        if ($null -eq $PreReleaseTag) { $PreReleaseTag = "beta" }

    $PreReleaseTag;
}