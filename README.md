# GameOn
*Game On!* is a [competition ranking ladder](http://en.wikipedia.org/wiki/Ladder_tournament) mobile web-app for Table Tennis.
Players are ranked using [Elo ratings](http://en.wikipedia.org/wiki/Elo_rating_system), made famous by the FaceMash website on [The Social Network](http://www.imdb.com/title/tt1285016/).

Game On! is a mobile web-app which you can install on any server that runs IIS. Once installed you can connect to the app via a mobile (or desktop) browser. The ladder will rank players of any two-player game where matches cannot result in a draw. It can be used for team-sports as well. The jQuery mobile user interface allows match results to be updated from the field, court or table. Player's are instantly ranked as soon as the match is played.

The Elo Rating system is a very good algorithm for ranking players, and is used by international sports bodies to calculate world rankings. It works just as well for ranking players in your workplace or social club.

Game On! is lean and simple. Its only function is to rank players on a ladder. The current ladder can be exported as CSV for posting, or for seeding a tournament. History and stats of all matches and ratings are stored in a SQL Compact Edition database.

## NOTE
This code is heavly based on [GameOn](https://archive.codeplex.com/?p=gameon) which was made for asp.net mvc 3 and hosted on codeplex, but upgraded to ASP.NET Core 3 preview 5.

## Credits
I'm not sure who made the original GameOn, but if you know then give me shoutout and I will add the correct credits :)
