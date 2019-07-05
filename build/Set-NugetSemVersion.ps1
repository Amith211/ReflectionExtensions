function Get-NugetVersion {
    [cmdletbinding(
        DefaultParameterSetName='Default'
    )]
    param (
        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [switch]
        $IsPreview,

        [Parameter(ParameterSetName = "Preview")]
        [string]
        $PreReleaseTag = $null,

        [string]
        $BranchName,

        [string]
        $Version
    )
    $branchName = if (!([string]::IsNullOrWhiteSpace($BranchName))) { $BranchName; } else { $env:BUILD_SOURCEBRANCHNAME; };
    $version = if (!([string]::IsNullOrWhiteSpace($Version))) { $Version; };

    Check-Params -ParamName "BranchName" -EnvVarName "BUILD_SOURCEBRANCHNAME"  -ParamOpt1 $branchName -ParamOpt2 $env:BUILD_SOURCEBRANCHNAME;
    

    $output = "";

    $preReleaseTag = Get-PrereleaseTag -PreReleaseTag $PreReleaseTag;
    $baseVer = Get-BaseVersion -BranchName $branchName -Version $version;

    $output = if ($IsPreview) { Get-FullVersion -BaseVersion $baseVer -BranchName $branchName -IsPreview -PreReleaseTag $preReleaseTag }
    else            { Get-FullVersion -BaseVersion $baseVer -BranchName $branchName }
    #$params = @{ BaseVersion = $baseVer; BranchName = $branchName; IsPreview = $IsPreview; PreReleaseTag = $preReleaseTag }
    #$output = Get-FullVersion @params;
    #-BaseVersion $baseVer -BranchName $branchName -IsPreview $IsPreview -PreReleaseTag $preReleaseTag;

    echo "##vso[task.setvariable variable=nugetVersion;isOutput=true]$output"
    [Environment]::SetEnvironmentVariable("LC_NUGETVERSION", "$output", "User")
    Write-Host "Var is $output";
    Write-Host "Env is $env:LC_NUGETVERSION";
    Write-Host "vso is $env:nugetVersion";
}

function Get-FullVersion 
{
    param (
        [Parameter(ParameterSetName = "Default", Mandatory = $true)]
        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]
        $BaseVersion,

        [Parameter(ParameterSetName = "Default", Mandatory = $true)]
        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]
        $BranchName,

        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [switch]
        $IsPreview,

        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]$PreReleaseTag
    )
    $output = $BaseVersion;

    if ($IsPreview) 
    {
        if ($BranchName -eq "master") 
        {
            $output += "-$PreReleaseTag";
        }

        if ($BranchName -like "azure-pipelines-test*") 
        {
            $output += "-$PreReleaseTag-ReleasePipelineTest";
        }
    }
    else 
    {
      if ($BranchName -like "azure-pipelines-test*") 
      {
          $output += "-ReleasePipelineTest" 
      }  
    }

    $output;
}

function Get-AzurePipeliesTestVersion {
    [cmdletbinding(
        DefaultParameterSetName = "Default"
    )]
    param (
        # Parameter help description
        [Parameter(ParameterSetName = "Default", Mandatory = $true)]
        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]
        $BaseVersion,

        [Parameter(ParameterSetName = "Default", Mandatory = $true)]
        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]
        $BranchName,

        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [switch]
        $IsPreview,

        [Parameter(ParameterSetName = "Preview", Mandatory = $true)]
        [string]$PreReleaseTag
    )
    $output = $BaseVersion;

    if ($IsPreview) 
    {
        if ($BranchName -like "azure-pipelines-test*" -and $IsPreview) 
        {
            $output += "-$PreReleaseTag-ReleasePipelineTest";
        }
    }
    elseif ($BranchName -like "azure-pipelines-test*") 
    {
        $output += "-ReleasePipelineTest" 
    }

    Write-Output $output;
}

function Get-BaseVersion {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $BranchName,

        [string]$Version = $null
    )
    $output = $null;
    [string]$output = if (!([string]::IsNullOrWhiteSpace($Version))) { $Version; };

    if ($BranchName -like "azure-pipelines-test*") 
    {
        Check-Params -ParamName "Version" -ParamOpt1 $Version -ParamOpt2 $env:GITVERSION_MAJORMINORPATCH;
        $output = "$env:GITVERSION_MAJORMINORPATCH"
        #-$PreReleaseTag-ReleasePipelineTest
    } 
    elseif ([string]::IsNullOrWhiteSpace($Version)) 
    {
        Check-Params -ParamName "Version" -ParamOpt1 $Version -ParamOpt2 $env:GITVERSION_NUGETVERSIONV2;
        $output = $env:GITVERSION_NUGETVERSIONV2;
    }

    Check-Params -ParamName "Version" -ParamOpt1 $Version -ParamOpt2 $output;

    Write-Output $output;
}


function Get-PrereleaseTag {
    param (
        [string]$PreReleaseTag = $null
    )

    if (!([string]::IsNullOrEmpty($PreReleaseTag))) { Write-Output $PreReleaseTag; break; }


    if ($null -eq $output -and ([string]::IsNullOrWhiteSpace($env:GITVERSION_NUGETPRERELEASETAGV2) `
                                -or [string]::IsNullOrWhiteSpace($env:GITVERSION_NUGETPRERELEASETAGV2)))
        {
            $output = $env:GITVERSION_NUGETPRERELEASETAGV2;
        }

    if ([string]::IsNullOrEmpty($output)) { $output = "beta" }

    Write-Output $output;
}

function Check-Params {
    [cmdletbinding(
        DefaultParameterSetName = "Single"
    )]
    param (
        [Parameter(ParameterSetName = "Single", Mandatory = $true)]
        [Parameter(ParameterSetName = "Double", Mandatory = $true)]
        [string]
        $ParamName,

        [string]
        $EnvVarName,

        [Parameter(ParameterSetName = "Single", Mandatory = $true)]
        [Parameter(ParameterSetName = "Double", Mandatory = $true)]
        [AllowNull()]
        $ParamOpt1,

        [Parameter(ParameterSetName = "Double", Mandatory = $true)]
        [AllowNull()]
        $ParamOpt2
    )

    if ($PSCmdlet.ParameterSetName -eq "Single") 
    {
        if ($null -eq $ParamOpt1 -or [string]::IsNullOrEmpty($ParamOpt1)) 
        {
            throw "Value for ""$ParamName"" must be supplied";
        }    
    }

    if ($PSCmdlet.ParameterSetName -eq "Double") 
    { 
        if (($null -eq $ParamOpt1 -or [string]::IsNullOrEmpty($ParamOpt1)) -and ($null -eq $ParamOpt2 -or [string]::IsNullOrEmpty($ParamOpt2)) ) 
        {
            if (![string]::IsNullOrWhiteSpace($EnvVarName))
            {
                throw "Value for ""$ParamName"" required either set it as a parameter or by the environment variable, ""$EnvVarName"".";
            } else 
            {
                throw "Value for ""$ParamName"" required either set it as a parameter or by using appropriate environment variable(s).";
            }
        }
    }
}

#Get-NugetVersion -BranchName 'master'  -Version "0.0.1"