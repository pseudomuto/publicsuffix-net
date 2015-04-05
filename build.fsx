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

"Clean"
  ==> "Build"
  ==> "BuildTest"

"BuildTest"
  ==> "Test"

RunTargetOrDefault "Build"
