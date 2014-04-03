##Get All Games  

####*POST Request to http://itweb.fvtc.edu/service/v0/allgames*

Returns all currently in progress games

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **games**: Array of game elements, each containing it's own game\_id, win\_limit, win\_count, user\_limit, user\_count, and created\_date.
- **game_id**: id of the game
- **win_limit**: number of wins allowed before game is closed
- **win_count**: number of wins so far in current game
- **user_limit**: limit of how many users are allowed to participate in current game
- **user_count**: number of users currently joined to the game
- **created**: timestamp of when game was created
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response
- **page**: Optional. The set of results to return. A page value of 0 would return the first 25 games, 1 would return 26-50, etc. When excluded from the post body a value of 0 is default.

* * *

##Sample request body: 
	{
    "user_id": "1",
    "authentication_token": "178f240f-aaae-4d91-b4a1-7a9b07d04e93",
    "page": "0"
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
            "user_count": "1",
            "created": "1396329703",
            "players": "1"
        }
    ]
	}
* * *