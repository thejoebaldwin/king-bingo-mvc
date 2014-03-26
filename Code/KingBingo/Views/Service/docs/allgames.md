##Get All Games  

**POST Request to http://itweb.fvtc.edu/service/v0/allgames**

Returns all currently in progress games

- **games**: Array of game elements, each containing it's own game\_id, win\_limit, win\_count, user\_limit, user\_count, and created\_date.
- **game_id**: id of the game. used in joingame command to join user to the game.
- **win_limit**: number of wins allowed before game is closed
- **win_count**: number of wins so far in current game
- **user_limit**: limit of how many users are allowed to participate in current game
- **user_count**: number of users currently joined to the game
- **created_date**: timestamp of when game was created
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response

* * *

##Sample request body: 

{
    "user_id": "1",
    "authentication_token": "cb501ead-ce59-4043-8a14-f6314b3c4970"
}
* * *

##Sample response data:

{
    "status": "ok",
    "operation": "allgames",
    "message": "Successfully retrieved list of all games",
    "games": [
        {
            "game_id": "1",
            "name": "New Game 1",
            "description": "New game description",
            "win_limit": "1",
            "win_count": "0",
            "user_limit": "3",
            "user_count": "0",
            "created_date": "2014-03-25 09:18:59"
        }
    ]
}
* * *