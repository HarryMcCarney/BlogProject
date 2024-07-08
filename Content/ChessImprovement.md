---
category: Note
tags: Chess, Data Science
updated: 20240707
created: 20230625
title: An empirical approach to chess improvement
summary: A "work in progress" to try and discover if players of online chess improve, and what the improvers are doing. 
---

Having played a lot of online chess myself, and found improving my rating to be pretty tricky, I wondered if most people are actually getting any better. 

Chess improvement is a huge field and focuses on things like opening theory. memorising book moves and understanding positional strategies.
But there's little consensus on what works best - particularly for players of different standards. 

However, Lichess has a fairly open API with game and rating histories for all players, so it's now possible to take a purely empirical approach to chess improvement. 

Instead of asking, "How do I improve?", we can ask "What are improving players doing?".

This is a rough proof of concept and would need more work before any reliable conclusions could be drawn. Code can be found [here](https://github.com/HarryMcCarney/ChessAnalytics)

# Approach 
First I downloaded a backup of all the games played in October 2015. This is available [here](https://database.lichess.org/)
Then I extracted all the games from the Lichess_db_standard_rated_2015-02.pgn file in the backup using [ParsePGN.fsx.](https://github.com/HarryMcCarney/ChessAnalytics/blob/master/ImportGames/ParsePGN.fs) I am only analysing "Rapid" games, but the code could easily include other time controls. Results of parsing the pgn are saved to csv format. Only the game headers, including results and opening classification codes, are saved, the moves are not needed as I don't plan to analyse these. 

The players' usernames are extracted from this csv and filtered to exclude players with less than 100 rapid games and who haven't played a game in the last 30 days. The resulting list is saved into the playercohort.csv.

Using the SavePlayerRatings.fsx we then query the Lichess API and get the rapid rating history for each player in the cohort file. 

These are averaged monthly for each player and stored in the playerRating.csv file.

# Rating change
We are then able to calculate the rating change for each player from 2015 until now. 

That gives the following histogram.

![](chess.png)

Viewing it as a probability distribution makes clear that most players have little chance of substantial improvement. Over 7 years of pretty active play, you have a 60 per cent chance of a 100-point improvement. 
A 350-point improvement is about half a per cent chance. So my meagre gains are not so bad after all. 

The picture is muddied by the rating deflation/inflation happening with Lichess as a whole. The chart below shows how the average (median and mean) rating has fluctuated since 2018. 
This is likely due to a large influx of beginners during the covid lockdowns who provided fresh points for those higher up the food chain. 

![Alt text](chess1.png)

Code to generate these graphs, as well as density functions and a cumulative probability chart, are in [Analytics\ChartRatingGains.fsx](https://github.com/HarryMcCarney/ChessAnalytics/blob/master/Analytics/ChartRatingGains.fsx)

# Drivers of rating increase

## Play more?
The first and obvious thing to check was whether simply playing more games would improve rating.
The chart below shows least squares regression for rating change over number of games.
Each dot is a player, x axis is number of games played since 2015, y axis is rating change.
The regression line does show some likely benefit to playing lots of games but not much.

Code to generate this is in [Analytics\AnalysePlayersGames.fsx](https://github.com/HarryMcCarney/ChessAnalytics/blob/master/Analytics/AnalysePlayersGames.fsx)

![Alt text](chess2.png)

## Switch openings often?
Finally, I wondered if I could find a correlation between variation in opening repertoire and rating gains. This would potentially answer the often debated question of whether focusing on a small number of openings is better than learning many different openings to a shallower depth. 

This is calculated in [Analytics\RatingChangePrediction.fsx.](https://github.com/HarryMcCarney/ChessAnalytics/blob/master/Analytics/RatingChangePrediction.fsx) It works in the following way. 
1. Get players games
2. Divide them into chunks of 100 
3. Calculate the number of different openings they played in each chunk of 100 games and express it as a ratio
4. Get their average ratio over all chunks. 

This is very crude and probably wrong/misleading in various ways, but for what its worth, it gives the following result

![Alt text](chess3.png)

As we can see there doesnt appear to be any correlation. This may not be true for all rating ranges but nothing is jumping out here.

## Limitations
There are many other factors behind the data, aka confounding variables, which Lichess doesn't give us. Players age is also certainly the most correlated with rating gain but Lichess doesn't record this. 
And of course how hard people are working on their chess behind the scenes. People changing openings frequently, and those specialising, may be equally likely to be reading books etc. Perhaps this effect is therefore balanced out in the large sample. Much more thorough analysis is needed before an empirical approach could be of practical use. But perhaps this sketch will be interesting for someone.