#r "packages/FAKE/tools/FakeLib.dll"

open Fake
open System.Text.RegularExpressions
open Testing.NUnit3

let appName = (Fake.FileSystemHelper.directoryInfo currentDirectory).Name

let compile name target source =
  FscHelper.Compile [ FscHelper.FscParam.Out name ; FscHelper.FscParam.Target target ] source

let copyLibs source dest =
  for ref in ReadFile source |> Seq.filter (fun x -> x.Contains "#r") do
    let refPath = Regex.Match(ref, "\"(.*dll)\"").Groups.[1].Value
    let sourcePath = directory source @@ refPath
    let destPath = filename sourcePath |> sprintf "%s/%s" dest
    CopyFile destPath sourcePath

let build() =
  CleanDir "build"
  if fileExists "build.files" then ReadFile "build.files" |> CopyTo "build"
  compile (sprintf "build/%s.exe" appName) FscHelper.TargetType.Exe [ "src/Main.fsx" ]
  copyLibs "paket-files/include-scripts/net45/include.main.group.fsx" "build"

let retry max f =
  let rec inner i =
    try f()
    with ex ->
      logf "%O" ex
      match i = max with
      | true -> raise ex
      | _ -> i + 1 |> inner
  inner 1

let tests() =
  CleanDir "tests/build"
  let testFiles =
    !!"tests/**/*.fsx"
    |> Seq.map (fun file -> file, ReadFileAsString file)
    |> Seq.filter (fun (file, text) -> text.Contains "[<Test")
    |> Seq.map (fun (file, _) -> file)
    |> Seq.toList
  let source = "tests/References.fsx" :: testFiles
  compile "tests/build/Tests.dll" FscHelper.TargetType.Library source
  copyLibs "paket-files/include-scripts/net45/test/include.test.group.fsx" "tests/build"
  copyLibs "paket-files/include-scripts/net45/include.main.group.fsx" "tests/build"
  NUnit3 id ["tests/build/Tests.dll"]

Target "Tests" tests
Target "Build" build
Target "Default" DoNothing
"Build" ==> "Tests" ==> "Default"
RunTargetOrDefault "Default"