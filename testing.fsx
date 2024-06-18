#r "nuget: FSharp.Formatting, 20.0.1"

open FSharp.Formatting.Markdown
open FSharp.Formatting.Common

open System.IO

let doc =  File.ReadAllText "testing.md" 

let parsed = Markdown.Parse(doc)

printfn "%A" parsed.Paragraphs 

MarkdownParagraph.

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