namespace blog

open Feliz.ViewEngine 

module RulesForDistributedAgile =
    let title = "10 rules for Distributed Agile — How H&C gets stuff done."
    let mainImage = "images/berlin.png"

    let content = 
        [
            Html.div [
                prop.classes ["title" ; "is-1"]
                prop.text  "10 rules for Distributed Agile -- How H&C gets stuff done."
            ]

            Html.img [
                prop.href "Images\\berlin.png"
            ]

            Html.div [
                prop.classes ["is-family-primary" ; "is-size-5"]
                prop.text "H&C has been working from home since before it became synonymous with flattening the curve. Many companies have previously allowed employees to work from home but the need for complex systems to be built by teams of people who don’t ever physically meet is a new challenge. Hopefully this account of how H&C does this will demonstrate that Distributed Agile is not only possible, but actually more efficient than co-located teams."
            ]

            Html.div [
                prop.classes ["title" ; "is-3"]
                prop.text "How we got here"
            ]

            Html.div [
                prop.classes ["is-family-primary" ; "is-size-5"]
                prop.text "Initially, this way of working was driven by H&C’s need to hire top notch engineers. As our Berlin office filled, we realised we needed more space. At this time in Berlin there was also a large influx of venture capital into new startups which created huge competition for the best developers. In response to this we opened an office in Barcelona and began hiring developers, effectively splitting the product teams across two locations.
                            Unfortunately, we quickly realised that crucial communication gets lost in split location product and engineering teams. Subtle but critical decisions were made in the physical coffee meetings and subsequently lost in the sometimes chaotic video conferences between two packed and echoey meeting rooms. We began thinking there must be a better way.
                            We were also greatly influenced by this Joel Spolsky article. He argues that all developers and knowledge workers doing ‘System 2’ thinking, as defined by Nobel prize winning psychologist Daniel Kahneman, should have their own single person office. We realised we could address this, improve channels of communication, and access a wider pool of talent around the world by transferring everyone to home working.
                            "
            ]
        ]

  
