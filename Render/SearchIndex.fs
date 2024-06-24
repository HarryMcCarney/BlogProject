namespace blog

module SearchIndex = 
    open Model
    open Thoth.Json.Net

    type JsonContainer = {
        Posts : Post seq
    }


    let categoryEncoder (category: Category) =
        match category with
        | Draft -> Encode.string "Draft"
        | Note -> Encode.string "Note"
        | Article -> Encode.string "Article"
  
    let postEncoder (post: Post) =
        Encode.object [
            "FileName", Encode.string post.FileName
            "Title", Encode.string post.Title
            "Content", Encode.string post.Content
            "Tags", 
                post.Tags 
                |> Array.map Encode.string
                |> Array.toList
                |> Encode.list
            "Category", categoryEncoder post.Category
            "Updated", Encode.datetime post.Updated
            "Created", Encode.datetime post.Created
        ]

    let postsEncoder jsonContainer = 
        Encode.object [
            "Posts",
                Encode.list
                    (
                        jsonContainer.Posts
                        |> Seq.toList
                        |> List.map postEncoder
                    )
        ]

    let buildSearchIndex (posts: Post seq) = 
        let container = {Posts = posts}
        (postsEncoder container).ToString()
        |> fun j -> System.IO.File.WriteAllText("public/SearchIndex.json", j)
        



