
#r "nuget: Markdig, 0.37.0"


open System 
open System.Globalization
let d = "20240620"

(("tag1, tag2, tag3").Split ",") |> Array.map(fun t -> t.Trim())



let vals = ["tag1"; "tag2"; "tag3"; "tag4"; "tag5"; "tag6"; "tag7"; "tag8";  "tag9"]


let getColumnPosts posts colNo totCols = 
    posts
    |> Seq.mapi(fun i p -> i,p)
    |> Seq.filter(fun (i, _ ) -> 
        [colNo..totCols..1000] |> Seq.contains  (i + 1)
        )
    |> Seq.map snd

getColumnPosts vals 1 3
getColumnPosts vals 2 3
getColumnPosts vals 3 3

vals
|> Seq.mapi(fun i p -> i,p)
|> Seq.filter(fun (i, _ ) -> (i + 1) % 2 = 0)
|> Seq.map snd







DateTime.ParseExact(d, "yyyyMMdd", CultureInfo.InvariantCulture)

open System.IO
open Markdig
open Markdig.Parsers

let doc =  File.ReadAllText "testing.md" 

let parsed = Markdig.Markdown.Parse doc

Markdig.Markdown.

Markdig.Markdown.ToHtml parsed

parsed.


Directory.EnumerateFiles "content"

(doc)




open FSharp.Formatting.Markdown
open FSharp.Formatting.Common



let doc =  File.ReadAllText "testing.md" 

let parsed = Markdown.Parse(doc)

MarkdownParagraph.YamlFrontmatter

parsed.Paragraphs
|> List.filter(fun p -> 
    match p with 
    | HorizontalRule (character, range)-> true
    | _ -> false  
    )
|> List.take 2

// get first and second 

printfn "%A" parsed.Paragraphs 

for par in parsed.Paragraphs do

    match par with
    | Heading (size = 2; body = [ Literal (text = text) ]) ->
        // Recognize heading that has a simple content
        // containing just a literal (no other formatting)
        printfn "%s" text
    | Paragraph (spans, _) -> 
        spans
        |> List.iter(fun s -> 
            match s with 
            | Paragraph (body = [ Literal (text = text) ]) -> 
                printfn "%s" text
            | DirectLink (body = [ Literal (link = link)]) -> 
                printfn "%s" link
        
        )
    | _ -> ()
    |> ignore


let rec collectSpanLinks span =
    seq {
        match span with
        | DirectLink (link = url) -> yield url
        | IndirectLink (key = key) -> yield fst (parsed.DefinedLinks.[key])
        | MarkdownPatterns.SpanLeaf _ -> ()
        | MarkdownPatterns.SpanNode (_, spans) ->
            for s in spans do
                yield! collectSpanLinks s
    }

/// Returns all links in the specified paragraph node
let rec collectParLinks par =
    seq {
        match par with
        | MarkdownPatterns.ParagraphLeaf _ -> ()
        | MarkdownPatterns.ParagraphNested (_, pars) ->
            for ps in pars do
                for p in ps do
                    yield! collectParLinks p
        | MarkdownPatterns.ParagraphSpans (_, spans) ->
            for s in spans do
                yield! collectSpanLinks s
    }

Seq.collect collectParLinks parsed.Paragraphs