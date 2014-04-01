##Join Game

####POST Request to *http://itweb.fvtc.edu/service/v0/joingame*

Use this command to join a login to an existing game. A successful response will return the Game ID and a comma delimited list of numbers, in order, that make up the user's bingo card. The numbers should be placed in the card from top to bottom in a 5 x 5 grid. For example: using the below "board" data should produce a card with the following numbers in the "B" column, from top to bottom: 12, 1, 15, 2, and 9.

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **game_id**: Id of the game attempting to join. 
- **user_id**: Id of user to join to game
- **timestamp**: UTC Time of request/response
- **board_id**: Returned id of generated board (bingo card)
- **board**: Comma-delimited list of numbers making up the generated board
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
    "operation": "joingame",
    "message": "Successfully joined game",
    "game_card": "4,11,14,1,5,23,27,19,22,17,31,36,44,45,39,51,55,60,57,59,63,68,67,64,69"
	}
If user has already joined game, the following will be returned instead.
	
	{
    "status": "error",
    "operation": "joingame",
    "message": "User is already joined to game"
	}
* * *