##Create Game

####POST Request to http://itweb.fvtc.edu/service/v0/creategame

Use this command to create a new game. 

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **user_id**: Id of user to quit game
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response


* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "e7984637-066a-49f0-b80e-148ffaee3eb5",
    "win_limit": "3",
    "user_limit": "3",
    "game_speed": "3",
    "name": "Game Test A",
    "description": "This is a description",
    "player_ids": "2",
    "private": "False"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "creategame",
    "message": "Successfully created game",
    "game_card": "3,13,14,10,8,20,30,21,25,26,45,36,32,33,44,54,52,49,47,56,63,62,67,69,70",
    "games": [
        {
            "game_id": "2",
            "name": "Game Test A",
            "description": "This is a description",
            "win_limit": "3",
            "win_count": "0",
			"game_speed": "3",
            "user_limit": "3",
            "user_count": "1",
            "created": "1397101677",
            "players": "1"
        }
    ]
	}
* * *