(**
---
category: Note
tags: F#, Data Science, Game Theory, Supply Chain
updated: 20230310
created: 20230310
title: Recreating Robert Axelrod's Prisoner's dilemma tournament
summary:This is a rough F# implementation of Robert Axelrod's classic Prisoner's dilemma tournament held in 1984. It successfully reproduces the same results..
---
*)
(**
This is an F# implementation of Robert Axelrod's classic Prisoner's dilemma tournament held in 1984.
It successfully reproduces the results described in his book  [The Evolution of Cooperation](https://en.wikipedia.org/wiki/The_Evolution_of_Cooperation)  
and includes the most interesting strategies including Tit For Tat, Grudger, Tit for Two Tats, and Tester.

The code is entirely immutable and uses recursion to spawn new generations of successful strategies. 
New strategies can be defined in the Strategies module.

An interesting adaptation of the game could include variable payoffs depending on depletion/regeneration of a shared environment. 
Perhaps this could explore optimal strategies in non zero sum games that must react to changing payoffs while anticipating the strategies of others. 
The maximum score achievable for a sustainable strategy, given a tournament with specific other strategies, may not be the leading strategy overall.
*)
#r "nuget: Plotly.NET"
#r "nuget: Plotly.NET.Interactive, 4.0.0"
open System
open Plotly.NET

module Util = 

    let rec combinations acc size set = seq {
        match size, set with 
        | n, x::xs -> 
            if n > 0 then yield! combinations (x::acc) (n - 1) xs
            if n >= 0 then yield! combinations acc n xs 
        | 0, [] -> yield acc 
        | _, [] -> () }

module GameLogic = 
    open Util

    type Move = | Cooperate | Defect 
    type Rounds = (Move * Move) array

    type Player = {
        Id : Guid 
        Name : string
        Strategy : (Move array -> Move array -> Move) 
        Recreate : unit -> Player
    }

    type Game = {
        Player1 : Player 
        Player1Moves : Move array
        Player1Score : int option
        Player2 : Player 
        Player2Moves : Move array
        Player2Score : int option
    }

    type Tournament = {
        Players : Player array 
        Games : Game array
    }

    
    let evaluateRound  (p1: Move) (p2: Move) = 
        match p1, p2 with
        | Cooperate , Cooperate -> 3,3
        | Defect , Defect  -> 1,1
        | Cooperate , Defect  -> 0,5
        | Defect , Cooperate  -> 5,0

    let rnd() = System.Random()

    let play (player1: Player) (player2 : Player) (rounds: int)  =
        let rec playRound (game: Game) (rounds:int) (counter:int) : Game = 
            if counter > rounds 
                then game 
                else 
                    let updatedGame : Game = 
                        let p1Moves = [|(game.Player1.Strategy game.Player1Moves game.Player2Moves)|] |> Array.append game.Player1Moves
                        let p2Moves = [|(game.Player2.Strategy game.Player2Moves game.Player1Moves)|] |> Array.append  game.Player2Moves
                        {game with Player1Moves = p1Moves; Player2Moves = p2Moves}

                    playRound updatedGame rounds (counter + 1)

        let g: Game = {Player1 = player1; Player1Moves = [||]; Player1Score = None; Player2 = player2 ; Player2Moves =  [||]; Player2Score = None }        

        playRound g rounds 1
        
    let evaluateGame (game: Game) = 
        game.Player2Moves
        |> Array.zip game.Player1Moves
        |> Array.map(fun (p1,p2) -> evaluateRound p1 p2)
        |> Array.unzip
        |> fun (p1, p2) -> 
            let evaluatedGame =  
                {
                    game with Player1Score = p1 |> Array.sum |> Some; Player2Score = p2 |> Array.sum |> Some
                }
            //printfn "%A" evaluatedGame 
            evaluatedGame

    let createPairings (players: Player array) = 
        let result = 
            combinations [] 2 (players |> Array.toList)
            |> Seq.append (players |> Array.map(fun s -> [s;s;] ))
        result

    let runTournament players = 
        createPairings players
        |> Seq.map(fun c -> 
            play c[0] c[1] 200
            |> evaluateGame)
        |> Seq.toArray
        |> fun games -> 

            players
            |> Array.map(fun p -> 
                p,
                games
                |> Array.filter(fun g -> g.Player1.Id = p.Id)
                |> Array.choose(fun g -> g.Player1Score)
                |> Array.append (
                    games
                    |> Array.filter(fun g -> g.Player2.Id = p.Id)
                    |> Array.choose(fun g -> g.Player2Score) 
                )
                |> Array.sum
            )

    type GenerationResult = {
        Generation : int
        StrategyPopulation : Map<string , int>
    }

    let getLine (results: GenerationResult array) strategyName = 
        let x, y =
            results
            |> Array.map(fun gr -> 
                gr.Generation,
                (
                gr.StrategyPopulation 
                |> Map.find strategyName
                )
            )
            |> Array.unzip
        
        Chart.Line(x = x,y = y, Name = strategyName)

    let runExperiment startingPlayers generations = 

        let initResults = 
            {
                Generation = 0;
                StrategyPopulation =
                    startingPlayers 
                    |> Array.groupBy(fun p -> p.Name)
                    |> Array.map(fun p -> p |> fst, p |> snd |> Array.length)
                    |> Map
            }


        let rec go players generations counter (results: GenerationResult array) =
            if counter >= generations 
                then results
                else 
                    let tournamentResults = runTournament players
                    let nextGen = 
                        tournamentResults
                        |> Array.groupBy(fun (p,s) -> p.Name)
                        |> Array.map(fun (_, scores) -> scores |> Array.head |> fst, scores |> Array.sumBy snd)
                        |> fun scores -> 
                            let totalScore = scores |> Array.sumBy snd |> float
                            scores
                            |> Array.map(fun (p, s) -> p, float s  / totalScore * 100. |> int)
                            |> Array.map(fun (p, s) -> 
                                [|0..s|]
                                |> Array.map(fun _ -> p.Recreate()) 
                            )
                        |> Array.concat
                    
                    let result = 
                        {
                        Generation = counter;
                        StrategyPopulation =
                            nextGen 
                            |> Array.groupBy(fun p -> p.Name)
                            |> Array.map(fun p -> p |> fst, p |> snd |> Array.length)
                            |> Map
                        }

                    go nextGen generations (counter + 1) ( [|result|] |> Array.append results)
        
        go startingPlayers generations 0 [|initResults|]

module Strategies = 
    open GameLogic 

    let rec tester() : Player = 
        let name = "Tester"

        let strategy = 
            fun (playerMoves: Move array) (opponentMoves : Move array) -> 
                if playerMoves.Length = 0 
                    then Defect
                elif  playerMoves.Length <= 3
                    then Cooperate 
                elif 
                    not 
                    (opponentMoves 
                    |> Array.exists(fun m -> m = Defect))
                    then 
                        if playerMoves.Length % 2 = 0 
                            then Defect
                        else Cooperate
                elif 
                    opponentMoves |> Array.last = Defect
                    &&  
                    opponentMoves
                    |> Array.filter(fun m -> m = Defect)
                    |> Array.length
                        = 1
                    then Cooperate
                else 
                    opponentMoves   
                    |> Array.last

        {Id = Guid.NewGuid(); Name = name; Strategy = strategy; Recreate = tester}

    let rec titForTwoTats() : Player = 
        let name = "TitForTwoTats"

        let strategy = 
            fun (playerMoves: Move array) (opponentMoves : Move array) -> 
                if playerMoves.Length < 2 
                    then Cooperate
                elif  
                    opponentMoves   
                    |> Array.rev
                    |> Array.take 2
                    |> Array.exists(fun m -> m = Cooperate)
                    then Cooperate
                else Defect

        {Id = Guid.NewGuid(); Name = name; Strategy = strategy; Recreate = titForTwoTats}

    let rec titForTat() : Player =
        let name = "TitForTat"
        
        let strategy = 
            fun (playerMoves: Move array) (opponentMoves : Move array) -> 
                if playerMoves.Length = 0 
                then Cooperate
                else 
                    opponentMoves
                    |> Array.last

        {Id = Guid.NewGuid(); Name = name; Strategy = strategy; Recreate = titForTat}

    let rec grudger() : Player = 
        let name = "Grudger"
        let strategy = 
            fun (playerMoves: Move array) (opponentMoves : Move array) -> 
                if playerMoves.Length = 0 
                then Cooperate
                else
                if 
                    opponentMoves
                    |> Array.exists(fun m -> m = Defect)
                    then Defect
                    else Cooperate
        {Id = Guid.NewGuid(); Name = name; Strategy = strategy; Recreate = grudger}

    let rec random() : Player = 
        let name = "Random"
        let strategy = 
            fun _  _-> 
                
                let randomBit = rnd().Next(0,2)
                if randomBit = 0 then Cooperate else Defect
        {Id = Guid.NewGuid();Name = name; Strategy = strategy; Recreate = random}

    let rec alwaaysDefect() : Player = 
        let name = "AlwaaysDefect"
        let strategy = 
            fun _  _ -> 
                Defect
        {Id = Guid.NewGuid();Name = name; Strategy = strategy; Recreate = alwaaysDefect}


    let rec alwaaysCooperate() : Player = 
        let name = "AlwaaysCooperate"
        let strategy = 
            fun _  _ -> 
                Cooperate
        {Id = Guid.NewGuid();Name = name; Strategy = strategy; Recreate = alwaaysCooperate}


open GameLogic
open Strategies

let players = 
    [|titForTat(); titForTwoTats();random(); alwaaysDefect(); alwaaysCooperate(); grudger(); tester()|] 

runExperiment players 40
|> fun results -> 
    players
    |> Array.map(fun p -> getLine results p.Name)
|> Chart.combine
|> Chart.withXAxisStyle ("Generations")
|> Chart.withYAxisStyle ("% of Population")
|> GenericChart.toEmbeddedHTML
(***include-it-raw***)
