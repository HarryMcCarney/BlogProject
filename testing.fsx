#r "nuget: FSharp.Formatting, 20.0.1"
open FSharp.Formatting.Literate 

let codeSnippet = """
let square x = x * x
square 5
"""

Literate.ParseScriptString codeSnippet
