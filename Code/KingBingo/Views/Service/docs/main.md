##Bingo API v0 3/25/14#

Supported Operations

- [Get All Games](/service/v0/allgames "Get All Games")
- [Get All Users](/service/v0/allusers "Get All Users")
- [Get User](/service/v0/getuser "Get User")
- [Authenticate](/service/v0/auth "Authenticate")
- [Create User](/service/v0/createuser "Create User")
- [Join Game](/service/v0/joingame "Join Game")
- [Get Number](/service/v0/getnumber "Get Number")
- [Quit Game](/service/v0/quitgame "Quit Game")

Usage  


- All json field names in the response will be lowercase only. All field names in request need to be lowercase as well.
- All responses will return a "command" parameter. This will allow you to standardize your json parsing depending on which request you are making.
- Get a list of all currently in progress games. Within each block of "game" data, there is a game\_id.
- Create a user. This will give you a user\_id and authentication\_token that you can then join a game with. You can use "test1" for login and password of "test1" if you'd like to skip this step.
- Join a game using the user\_id, authentication\_token and game\_id. This will return a "gamecard" of numbers that you will use to construct your bingo card.
- Once you have your board, call use the get number operation to play the game!
- Be sure to check the response messages when getting errors to help troubleshoot your requests, and don't forget to validate your json.
- Error reports (specifically, what you were doing when it crashed) should be sent to [joe@humboldttechgroup.com](mailto:joe@humboldttechgroup.com "email")