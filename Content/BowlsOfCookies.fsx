(**
---
category: Note
tags: Bayes, F#, Data Science
updated: 20230930 
created: 20230930
title: Bayesian F# Series - 2. Probability distributions with Bayes.
summary:This is the second article in the series on Bayes and will examine representing probability as a distribution rather than a single metric.
---
*)
(**
# Introduction
In the first post of this series we saw how Bayesian thinking gives us a powerful tool to approach uncertainty. In particular we looked at examples which calculate the comparative likelihoods of competing hypotheses given some observed sequence of data. It was shown that this is sometimes more insightful than deriving a single probability from the ratios in that sequence of data.

In those examples we had a very small number of hypotheses. However for many real world problems we will have a very large number and Bayes allows us to work on these as a distribution.

The Think Bayes book, which this series is following, introduces this idea in chapter 3 with chocolate and vanilla cookies.

# 2 bowls of Cookies
Imagine we have two bowls of cookies. In the first bowl we have 30 vanilla cookies and 10 chocolate cookies In the second bowl we have 20 of each. Then imagine we have taken a cookie at random and it's a vanilla cookie. 
What is the probability that the cookie was from bowl 1.

Like the Monty Hall paradox this has a relatively trivial solution once we understand how to update prior probabilities after we observe data.

Our prior probability is 50/50 for both bowls. However the chance of a vanilla cookie if we select from bowl 1 is 50/50. Or to put it another way, 
the probability that we select a vanilla cookie in Hypothesis 1 - ie we are choosing from bowl 1' is 0.5. In the case of bowl two it's 0.75 as there are 30 vanilla cookies and only 10 chocolate ones.

So to get the probability that we are selecting from bowl 1 we use Bayes rule and multiply the prior probability by the likelihood. Then we normalise the relative probability and this gives us the posterior.

in F# this can be calculated as follows
*)
#r "nuget: SeqPrinter, 0.2.1"
open SeqPrinter

type Prior =
    { Hypothesis: string
      Prior: float
      Likelihood: float }

type Posterior =
    { Hypothesis: string
      Prior: float
      Likelihood: float
      Posterior: float }

let calcPosteriors (priors: Prior list) : Posterior list =
    let totalProbability = priors |> List.sumBy (fun r -> r.Prior * r.Likelihood)

    priors
    |> List.map (fun h ->
        { Hypothesis = h.Hypothesis
          Prior = h.Prior
          Likelihood = h.Likelihood
          Posterior = ((h.Prior * h.Likelihood) / totalProbability) })

let priors =
    [ { Hypothesis = "Bowl 1"
        Prior = 0.5
        Likelihood = 0.75 }
      { Hypothesis = "Bowl 2"
        Prior = 0.5
        Likelihood = 0.50 } ]

priors
|> calcPosteriors
|> Printer
|> Printer.withColumns [ "Hypothesis"; "Prior"; "Likelihood"; "Posterior" ]
|> Printer.print
(***include-it ***)
(**   
So we can see after one vanilla cookie the probability that we selected from bowl 1 increases to 0.6. BUt what if we have more than two possible hypotheses?

# 101 bowls of cookies
Like many real world examples we don't just have 2 competing hypotheses. Actually there is a range of possibilities and we need to understand how the observed data affects the chance of each of the alternatives.

In order to model these scenarios we need to start representing our hypotheses as a distribution rather than a list of cases. 
For illustration we can rewrite the simple example above with the support of the fsharp.stats library.
*)
#r "nuget: FSharp.Stats, 0.4.12-preview.1"

open FSharp.Stats
open FSharp.Stats.Distributions

let die = EmpiricalDistribution.createNominal () [ 1; 2; 3; 4; 5; 6 ]
die
(***include-it ***)

#r "nuget: FSharp.Stats, 0.4.12-preview.1"

open FSharp.Stats
open FSharp.Stats.Distributions

let priorDist = EmpiricalDistribution.createNominal () [ "Bowl 1"; "Bowl 2" ]

let likelihoodVanilla = [ "Bowl 1", 0.75; "Bowl 2", 0.5 ] |> Map.ofSeq

let likelihoodChocolate = [ "Bowl 1", 0.25; "Bowl 2", 0.5 ] |> Map.ofSeq

let normalise (dist: Map<'a, float>) =
    let totalProbability = dist |> Map.toSeq |> Seq.sumBy snd
    dist |> Map.map (fun k v -> v / totalProbability)

let updatePosteriorDist (likelihoods: Map<'a, float>) (priorDist: Map<'a, float>) =
    priorDist
    |> Map.map (fun k v ->
        match (likelihoods.TryFind k) with
        | Some l -> v * l
        | None -> v)
    |> normalise

updatePosteriorDist likelihoodVanilla priorDist
|> updatePosteriorDist likelihoodVanilla
(***include-it ***)
(**
Interestingly we can easily see what the probability would look like if we had drawn 10 vanilla cookies in a row.
*)
[ 1..10 ]
|> List.fold (fun dist _ -> updatePosteriorDist likelihoodVanilla dist) priorDist
(***include-it ***)
(**
The key difference in the example above is the use of the EmpiricalDistribution module to create a Probability mass function (PMF). 
This is created from a sequence of data and generates a probability function based on the ratios of the values.

The following example creates a PMF for a 6 sided dice and then reveals the probability of each outcome in a fair roll of the dice. 
This uniform distribution of probability is often called an uninformed prior because it reveals that we have no knowledge of the likelihood of each side before we start the experiment.
*)
die
(***include-it ***)
(**
Representing our priors like this enables us to model more complex problems. 
For example Think Bayes then presents the scenario of 101 bowls of cookies in which bowl 0 has 0% vanilla cookies, 
bowl 1 has 1 % and bowl 2 has 2% etc right up to bowl 100 which has 100% vanilla cookies.

This can be modelled with PMF as follows
*)
let prior101Dist = EmpiricalDistribution.createNominal () { 0..100 }
prior101Dist
(***include-it ***)
(**
Now we calculate the likelihood of the bowl given that we have chosen a vanilla cookie. This is just the fraction of vanilla cookies in the bowl. 
Conversely the likelihood for chocolate is the reverse. We create these sequences as follows.
*)
let likelihoodVanillaDistribution =
    [ 0..100 ] |> List.map (fun i -> i, (float i / 100.)) |> Map.ofList

let likelihoodChocolateDistribution =
    [ 0..100 ] |> List.map (fun i -> i, 1. - (float i / 100.)) |> Map.ofList
(**
We can then calculate the probability of each bowl given a vanilla cookie using the same update function as before
*)
let hundredBowlsPosterior =
    updatePosteriorDist likelihoodVanillaDistribution prior101Dist

hundredBowlsPosterior
(***include-it ***)
(**
We can visualise the resulting distribution as follows
*)
#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.Interactive, 4.0.0"

open Plotly.NET

let drawChart priorDist posteriorDist vanillas chocolates =
    let posteriorAfterOneVanillaLine =
        Chart.Line((posteriorDist |> Map.toSeq), Name = "Posterior")

    let prior101DistLine = Chart.Line((priorDist |> Map.toSeq), Name = "Prior")

    let title =
        (sprintf "Posterior after %i vanilla cookies and %i chocolate cookies" vanillas chocolates)


    [ posteriorAfterOneVanillaLine; prior101DistLine ]
    |> Chart.combine
    |> Chart.withXAxisStyle ("Bowl")
    |> Chart.withYAxisStyle ("PMF")
    |> Chart.withTitle (title)
    |> GenericChart.toEmbeddedHTML


drawChart prior101Dist hundredBowlsPosterior 1 0
(***include-it-raw***)
(**
We can also see the probability distribution after any number of observed cookies. For instance after 2 vanilla cookies and one chocolate it looks as follows:
*)
let posterior3 =
    updatePosteriorDist likelihoodVanillaDistribution prior101Dist
    |> updatePosteriorDist likelihoodVanillaDistribution
    |> updatePosteriorDist likelihoodChocolateDistribution


drawChart prior101Dist posterior3 2 1
(***include-it-raw***)
(**
This post has shown how we can use FSharp.Stats and Plotly.Net to implement and visualise bayesian techniques.
It is a excellent way to approach many real world data science problems, particularly when we want to overcome data limitations with existing (prior) domain knowledge.
*)
