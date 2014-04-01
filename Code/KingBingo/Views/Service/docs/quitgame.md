##Quit Game

####*POST Request to http://itweb.fvtc.edu/service/v0/quitgame*

Use this command to quit a login to a previously joined game. 

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **game_id**: Id of the game attempting to quit. 
- **user_id**: Id of user to quit game
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response


* * *

##Sample request body: 
	{
    "user_id": "1",
    "authentication_token": "069d4a43-b5dd-4ece-a7e1-d866b3090dc0",
    "game_id": "1"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "quitgame",
    "message": "Successfully quit game"
	}
If user has already quit game or not currently joined, the following will be returned instead.
	

	{
    "status": "error",
    "operation": "quitgame",
    "message": "User was not joined to the game"
    }

* * *