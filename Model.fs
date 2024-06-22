namespace blog

module Model = 

    open System

    type Category = 
        | Draft
        | Note
        | Article 

    type Post = {
        FileName : string
        Title : string
        Content : string 
        Tags : string array
        Category : Category
        Updated : DateTime
        Created : DateTime
    }




