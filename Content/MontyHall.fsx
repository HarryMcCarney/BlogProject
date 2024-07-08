(**
---
category: Note
tags: Bayes, H&C, F#, Data Science
updated: 20230929 
created: 20230929
title: Bayesian F# Series - 1. The Monty Hall Paradox.
summary: This is the first in a series of posts exploring how Bayesian Techniques can be implemented in F#. It will also provide simple examples of how to started using Plotly.NET and FSharp.stats. 
---
*)
(**
# Introduction
The posts will closely follow Allen Downey's excellent book Think Bayes. Each post will cover one or two chapters from his book summarising the key ideas and porting the examples to F#. The first few posts will hopefully be quite accessible and the complexity will increase as we progress though the book.

The goal is that any one with some knowledge of F# will be able to implement the Bayesian techniques covered in the real world.

# Why F#
F# is an excellent language for Data Science. The rich static type system guides your thinking while you structure solutions to problems. It avoids the 'guess/refresh' approach sometimes found with loosely typed languages. Moreover, the extra precision of types that won't let you multiply strings, concatenate a boolean, or add an int to a float ensures your code is correct.

Lastly, functional immutable programming is simply a better conceptual fit for most data science work. Object oriented languages are primarily about managing state, not transforming data. F#’s functional first approach encourages us to write simple deterministic functions which are easy to understand and compose into larger and larger operations with no loss of readability.

# Data Science at Hack and Craft
At Hack and Craft we build simulations for logistics and manufacturing companies. Typically they are interested in measuring the effects of proposed chances to their processes. We model these initiatives and run simulations to produce synthetic data. This data is then analysed to measure impact of the proposed changes.

We find Bayesian approaches very applicable to this work and have gradually moved away from a more traditional 'frequentist' approach.

# Bayesian thinking
Frequentist techniques use ratios between the frequencies of possible cases of an observed sequence of data to derive their probability.

So the probability of a coin landing on heads is established by looking at a sequence of flips. Such as T, H, T, T, T, T, H, T, T, H, H, T. In this sequence there are 4 Heads and 8 Tails. SO the probability of Heads is 4/12 or 0.33. This conclusion feels wrong as we know that coins are normally evenly weighted and our conclusion that this is an uneven coin is based on a short sequence of data.

In the real world H&C's clients often have partial and noisy data. And they nearly alway have some pre-existing beliefs which should guide our analysis.

This is where Bayesian techniques can help. Instead of trying to calculate the probability of heads from the data, the Bayesian asks, given this data what the probability that this is a 50/50 coin. They can also ask what the probability that this is 33$ coin or a 66% coin.

These three coins represent three different hypotheses which are equally likely before we see the sequence of flips but not equally likely afterwards. Given that only 4/12 flips return heads we can infer that its now more likely that we have 33% coins than a 66% coin.

In this way our probability of having each coin can be updated using the observed data.

The formula for calculating this is known as Bayes Theorem. Its a complex theorem but the core idea is quite intuitive. Data allows us to update the probabilities of our competing beliefs. These beliefs are held with varying levels of certainty before we make observations and afterwards we change those convictions. This series of article will explore a variety of increasingly and sometimes complex but always practical applications of this approach.

# A Bayesian approach to the Monty Hall paradox
The Monty Hall Paradox is a well known probability puzzle which many people find intuitively challenging. It has divided opinion even among experts. 
Bayesian thinking makes this puzzle much more tractable than traditional frequentist so its a an excellent place to start.

The puzzle is as follows: Imagine a game show. The host is called Monty. There are three doors. Behind one of the doors is a car which the contestant is trying to win. 
A contestant is asked to to choose a door. Then Monty opens one of the other two doors which doesn't have the car behind it. 
This leaves two doors left and the car is behind one of them. Monty then asks the contestant if they would like to stick with their original choice or switch to the other door. 
Most people will assume there is a 50/50 chance for both doors so there is nothing to be gain from switching. However, switching doors is a the better choice and raises the odds of winning to 2/3

Its hard to explain this with frequentist theory but a Bayesian phrasing of the puzzle makes it intuitively clear that the contestant should switch.

But first for anyone sceptical about why the contestant should switch, which I was, here's empirical proof. The code below simulates the game 10k times. 
5k with a switch strategy and 5k with a non switching strategy. THe graph shows the switcher wins almost exactly 66.6% of the time, while the non-switcher's odds remain on 33.3%. Later we will show how we can derive the same results analytically using Bayes rule.
*)

#r "nuget: Plotly.NET.Interactive, 4.0.0"

open System
open Plotly.NET

//Helpers
let rnd from until = Random().Next(from, until)

let rec rndExclude from until (excl: int list) =
    let r = rnd from until

    if excl |> List.contains r then
        rndExclude from until excl
    else
        r

//Domain
type Player =
    | Switcher
    | NonSwitcher

type Door =
    | Door1
    | Door2
    | Door3

type Game =
    { Player: Player
      CarLocation: Door
      PlayersFirstChoice: Door option
      MontyRevealed: Door option
      PlayersFinalChoice: Door option
      Winner: bool option }

//Actions
let getDoorNumber door =
    match door with
    | Door1 -> 0
    | Door2 -> 1
    | Door3 -> 2

let createGame player : Game =
    let r = rnd 0 3

    { Player = player
      CarLocation = ([ Door1; Door2; Door3 ][r])
      PlayersFirstChoice = None
      MontyRevealed = None
      PlayersFinalChoice = None
      Winner = None }

let makeFirstChoice (game: Game) : Game =
    let r = rnd 0 3

    { game with
        PlayersFirstChoice = Some([ Door1; Door2; Door3 ][r]) }

let montyRevealsDoor (game: Game) : Game =
    let chosenDoor = getDoorNumber game.PlayersFirstChoice.Value
    let carDoor = getDoorNumber game.CarLocation
    let r = rndExclude 0 3 [ chosenDoor; carDoor ]

    { game with
        MontyRevealed = Some([ Door1; Door2; Door3 ][r]) }

let decideToSwitch game : Game =
    match game.Player with
    | Switcher ->
        let montyRevealed = getDoorNumber game.MontyRevealed.Value
        let playersFirstChoice = getDoorNumber game.PlayersFirstChoice.Value
        let r = rndExclude 0 3 [ montyRevealed; playersFirstChoice ]

        { game with
            PlayersFinalChoice = Some([ Door1; Door2; Door3 ][r]) }
    | NonSwitcher ->
        { game with
            PlayersFinalChoice = game.PlayersFirstChoice }

let isWinner (game: Game) =
    { game with
        Winner = Some(game.CarLocation = game.PlayersFinalChoice.Value) }

let play player =
    createGame player
    |> makeFirstChoice
    |> montyRevealsDoor
    |> decideToSwitch
    |> isWinner

let games = 10000

let results =
    [ 0..games ]
    |> List.map (fun i -> if (i % 2 = 0) then Switcher else NonSwitcher)
    |> List.map play
    |> List.filter (fun g -> g.Winner.Value)
    |> List.countBy (fun g -> (g.Player = Switcher))

let switchers = results |> List.find fst |> snd
let nonSwitchers = results |> List.find (fun (s, w) -> s = false) |> snd

Chart.Column(
    values = [ float switchers / float (games / 2); float nonSwitchers / float (games / 2) ],
    Keys = [ "Switchers"; "Non Switchers" ]
)
|> GenericChart.toEmbeddedHTML
(***include-it-raw***)

(**
So how can we explain this using a Bayesian approach. As we saw with the coin example, we think of the car being behind each door as distinct hypotheses. Hypothesis 1 is 'car is behind door 1', Hypothesis 2 is the 'car is behind door 2', Hypothesis 3 the 'car is behind door 3'. The probability of the car being behind each door at the start of the game is 0.33. And lets say we chose door 1 as our first choice.

To summarise

Hypothesis	Probability	First Choice
| H1 | 0.33 | X | H2 | 0.33 | | H3 | 0.33 |

Monty has to open a door and he selects door 3. Now comes the key step in all applications of Bayes rule, we update the probabilities of the competing Hypotheses based on the data. In this case the data is that Monty selected door 3. The likelihood that he would have selected door 3 is actually different for each hypothesis.

For Hypothesis one, Monty wont open the door that we have chosen so he is left with a choice of doors 2 and 3. So, for H1, there is a 50/50 chance of him opening door 3. However, for H2 there is a 100% chance that Monty opens door 3. He wont open the door you have chosen, door 1, or the door that the car is behind, door 2 in this hypothesis, so he has to open door 3. Lastly there is 0% chance of the car being behind door 3 as he opened this one and he wont open the door that the car is behind it.

So, after Monty opens door three we have the following updated table.

Hypothesis	Probability	First Choice	Likelihood of opening door 3
H1	0.33	X	0.5
H2	0.33		1
H3	0.33		0
First application of Bayes rule in F#
Bayes rule tells us to simply multiply the initial probability known as the 'Prior' by the likelihood of the hypothesis given the new data. This gives a new probability known as the 'un-normalised Posterior'. We then normalise the Posteriors to give their relative chance. A rough F# implementation would be look as follows.
*)
open System

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

let montyPriors =
    [ 
      { Hypothesis = "H1"
        Prior = 0.3333333333
        Likelihood = 0.5 
      }
      { Hypothesis = "H2"
        Prior = 0.3333333333
        Likelihood = 1 }
      { Hypothesis = "H3"
        Prior = 0.3333333333
        Likelihood = 0 
      } ]

let montyPosteriors =  montyPriors |> calcPosteriors
  
let rds (n: float)=
  Math.Round (n,3) |> string

Chart.Table(
      headerValues = ["<b>Hypothesis</b>"; "<b>Probability</b>";"<b>Likelihood of opening door 3</b>"; "<b>Posterior</b>"],
      cellsValues = (montyPosteriors |> List.map( fun p -> [p.Hypothesis; rds p.Prior; rds p.Likelihood; rds p.Posterior]))
  )
|> GenericChart.toEmbeddedHTML
(***include-it-raw***)
(**
As you can see the posterior result matches the results from empirical simulation.

# Wrap up
Bayesian thinking gives us intuitive and powerful ways to approach probability problems. This article has followed the first couple of chapters of the Think Bayes book which is an excellent resource well worth reading in full.

The next post in this series will look at how Bayes rule can be applied to more complex examples. We will also show how the fsharp.stats library can help compose and solve these problems with remarkable efficiency.
*)