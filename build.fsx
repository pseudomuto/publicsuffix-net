#I @"./packages/FAKE.Core/tools"
#r @"./packages/FAKE.Core/tools/FakeLib.dll"

open Fake

// paths
let buildDir     = "./build/"
let testDir      = "./test/"
let deployDir    = "./deploy/"
let testToolsDir = "./packages/NUnit.Runners.2.6.4/tools"

// sources
let appReferences  = !! "PublicSuffix/**/*.csproj"
let testReferences = !! "PublicSuffix.Test/**/*.csproj"

// nuget package details
let packageAuthors = ["PseudoMuto"]
let packageName = "PublicSuffixList"
let packageDescription = "A Domain Name parser based on the Public Suffix List"
let packageSummary = packageDescription
let packageRoot = deployDir
let packageDir = packageRoot @@ "PublicSuffix"

// Restore NuGet Packages
RestorePackages()

Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; deployDir]
)

Target "Build" (fun _ ->
  MSBuildRelease buildDir "Build" appReferences
    |> Log "Build-Output: "
)

Target "BuildTest" (fun _ ->
  MSBuildDebug testDir "Build" testReferences
    |> Log "Test-Build-Output: "
)

Target "Test" (fun _ ->
  !! (testDir + "PublicSuffix.Test.dll")
    |> NUnit (fun p ->
      {p with
        ToolPath = testToolsDir;
        DisableShadowCopy = true;
        OutputFile = testDir + "TestResults.xml" })
)

Target "Package" (fun _ ->
  let targetDir = packageDir @@ "lib/net45"
  CleanDirs [packageRoot; packageDir; targetDir]

  CopyFile targetDir (buildDir @@ "PublicSuffix.dll")
  CopyFile targetDir (buildDir @@ "PublicSuffix.dll.mdb")
  CopyFiles packageDir ["README.md"; "LICENSE"]

  NuGet (fun p ->
    {p with
      Files = [("lib/net45/**.*", None, None)]
      Authors = packageAuthors
      Project = packageName
      Description = packageDescription
      Summary = packageSummary
      OutputPath = packageRoot
      WorkingDir = packageDir
      AccessKey = getBuildParamOrDefault "nugetkey" ""
      Publish = hasBuildParam "nugetkey" }
  ) "PublicSuffix.nuspec"
)

Target "Default" DoNothing

"Clean"
  ==> "Build"
  ==> "BuildTest"

"BuildTest"
  ==> "Test"

"Test"
  ==> "Default"

"Build"
  ==> "Package"

RunTargetOrDefault "Default"
