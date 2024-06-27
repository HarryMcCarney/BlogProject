#r "nuget: FAKE, 5.16.0"
#r "nuget: Fake.Core.Target"
#r "nuget: Fake.DotNet.Cli, 6.0.0"
#r "nuget: Suave, 2.6.2"

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
        Shell.cleanDir "./Render/public" |> ignore
)


Target.create "BuildModel" (fun _ ->
        Shell.cd "Model" |> ignore
        DotNet.exec id "build" "./Model.fsproj" |> ignore 
        Shell.cd ".."
)

Target.create "Render" (fun _ ->
        Shell.cd "Render" |> ignore
        Shell.copyDir "public" "images" (fun _ -> true) |> ignore
        Shell.copyFile "public" "styles.css" |> ignore
        DotNet.exec id "run" "--project ./BlogProject.fsproj" |> ignore 
        Shell.cd ".."
)

Target.create "CompileJS" (fun _ ->
        Shell.cd "Javascript"
        DotNet.exec id "fable" "--outDir ../Render/public" |> ignore 
        Shell.cd ".."
)

Target.create "Run" (fun _ ->
        
        Shell.cd "Render"

        let app = 
                choose [
                GET >=> path "/" >=> Files.file "public/index.html"
                GET >=> Files.browseHome            
                RequestErrors.NOT_FOUND "Page not found." 
                ]
       
        let config =
            {defaultConfig with homeFolder = Some (Path.GetFullPath "public") }
            
        startWebServer config app 
       
)

"Clean" ==> "BuildModel" ==> "Render" ==> "CompileJS" ==> "Run"

Target.runOrDefaultWithArguments (args.[1])