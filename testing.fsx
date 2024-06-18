#r "nuget: FSharp.Formatting, 20.0.1"
#r "nuget: FSharp.Compiler.Service, 43.8.300"
#r "nuget: FSharp.Formatting.Literate, 20.0.1"

open System
open System.IO
open FSharp.Formatting.Literate
open FSharp.Formatting.Markdown
open FSharp.Formatting.Templating
open FSharp.Formatting
open FSharp.Compiler


//https://github.com/fsprojects/FSharp.Formatting/blob/4140311cf170c3704c69f5d1dff0bb6d4d35b069/tests/FSharp.Literate.Tests/LiterateTests.fs#L184

open System.IO

let doc =  File.ReadAllText "testing.md" 

let md = Literate.ParseMarkdownString(doc)