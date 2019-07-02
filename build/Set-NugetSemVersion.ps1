$ver = "";
$prereleaseTag = "beta";
if($env:Build_Reason -eq "PullRequest" )
{
    $ver = "$env:GITVERSION_MAJORMINORPATCH-$prereleaseTag-$env:GITVERSION_PRERELEASENUMBER";
} 
elseif($env:BUILD_SOURCEBRANCHNAME -eq "master")
{
    $ver = "$env:GitVersion_NuGetVersionV2-$prereleaseTag"
}
elseif ($env:BUILD_SOURCEBRANCHNAME -like "azure-pipelines-test*") {
    $ver = "$env:GitVersion_NuGetVersionV2-$prereleaseTag-ReleasePipelineTest"
}
else 
{
    $ver = "$env:GitVersion_NuGetVersionV2-$prereleaseTag-$env:GITVERSION_PRERELEASENUMBER";
}
echo "##vso[task.setvariable variable=nugetVersion]$ver"
[Environment]::SetEnvironmentVariable("LC_NUGETVERSION", "$ver", "User")
Write-Host "Var is $ver";
Write-Host "Env is $env:LC_NUGETVERSION";
Write-Host "vso is $(nugetVersion)";