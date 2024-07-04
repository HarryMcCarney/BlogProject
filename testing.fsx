
#r "nuget: FSharp.Formatting, 20.0.1"


open FSharp.Formatting.Literate 
open FSharp.Formatting.Markdown 
open System.IO

let file =  File.ReadAllText "Render/content/aEssay.md"

let doc = Literate.ParseMarkdownString(file)




let tags = 
    doc.Paragraphs
    |> Seq.map(fun p -> 
        match p with 
        | YamlFrontmatter (meta,_) -> 
            
            meta
            |> List.map(fun md ->  md.Split ':' |> fun x -> x[0].Trim(), x[1].Trim())
            |> Some
        | _ -> None)
    |> Seq.choose id
    |> Seq.concat
    |> Map


let tags2 = 
    md.Paragraphs
    |> Seq.map(fun p -> 
        match p with 
        | YamlFrontmatter (meta,_) -> 
            
            meta
            |> List.map(fun md ->  md.Split ':' |> fun x -> x[0].Trim(), x[1].Trim())
            |> Some
        | _ -> None)
    |> Seq.choose id
    |> Seq.concat
    |> Map



Markdown.Parse markdown
|> Markdown.ToHtml