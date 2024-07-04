#r "nuget: FAKE, 5.16.0"
#r "nuget: Fake.Core.Target"
#r "nuget: Fake.DotNet.Cli, 6.0.0"
#r "nuget: Suave, 2.6.2"
#r "nuget: Fake.BuildServer.GitHubActions, 6.0.0"

open Fake.Core
open Fake.IO
open Fake.DotNet
open Fake.Core.TargetOperators
open Suave
open Suave.Filters
open Suave.Operators
open System.IO


let args = fsi.CommandLineArgs

System.Environment.GetCommandLineArgs()
|> Array.skip 2 // 3 if run in interactive window.
|> Array.toList
|> Context.FakeExecutionContext.Create false "build.fsx"
|> Context.RuntimeContext.Fake
|> Context.setExecutionContext

Target.create "Clean" (fun _ ->
        Shell.cleanDir "./docs" |> ignore
)

Target.create "BuildModel" (fun _ ->
        Shell.cd "Model" |> ignore
        DotNet.exec id "build" "./Model.fsproj" |> ignore 
        Shell.cd ".."
)

Target.create "Render" (fun _ ->
        Shell.cd "Render" |> ignore
        Shell.copyDir "../docs" "images" (fun _ -> true) |> ignore
        Shell.copyFile "../docs" "styles.css" |> ignore
        DotNet.exec id "run" "--project ./BlogProject.fsproj" |> ignore 
        Shell.cd ".." 
)

Target.create "CompileJS" (fun _ ->
        Shell.cd "Javascript"
        DotNet.exec id "fable" "--outDir ../docs" |> ignore 
        Shell.rm "../docs/.gitignore"
        Shell.cd ".."
)

Target.create "Run" (fun _ ->
        
        let app = 
                choose [
                GET >=> path "/" >=> Files.file "docs/index.html"
                GET >=> Files.browseHome            
                RequestErrors.NOT_FOUND "Page not found."  
                ]
       
        let config =
            {defaultConfig with homeFolder = Some (Path.GetFullPath "docs") }
            
        startWebServer config app 
       
)

Target.create "Commit" (fun _ ->
        let filePath = "./docs/CNAME"
        let content = "harrymccarney.com"
        // Write the content to the file
        File.writeString false filePath content
        Shell.Exec("git", "add .") |> ignore
        Shell.Exec("git",  "commit -a -m \"deploying to github pages\"") |> ignore
)

Target.create "Deploy" (fun _ ->
        Shell.Exec("git", "push") |> ignore
)

"Clean" ==> "BuildModel" ==> "Render" ==> "CompileJS" ==> "Run" 

"Clean" ==> "BuildModel" ==> "Render" ==> "CompileJS" ==> "Commit" ==> "Deploy"

Target.runOrDefaultWithArguments (args.[1])