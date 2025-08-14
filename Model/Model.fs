namespace blog

module Model =
    open System

    type Category =
        | Draft
        | Note
        | Essay
        | Talk


    type Post =
        { FileName: string
          Title: string
          Summary: string
          MainImage: string option
          Content: string
          Tags: string array
          Category: Category
          Updated: DateTime
          Created: DateTime }

    type JsonContainer = { Posts: Post seq }
