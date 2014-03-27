##Auth

**POST Request to http://itweb.fvtc.edu/service/v0/auth**

Use this command to join a login to an existing game. A successful response will return the Game ID and a comma delimited list of numbers, in order, that make up the user's bingo card. The numbers should be placed in the card from top to bottom in a 5 x 5 grid. For example: using the below "board" data should produce a card with the following numbers in the "B" column, from top to bottom: 12, 1, 15, 2, and 9.

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
    "username": "test1",
    "hash": "DMK3DB2yBmJaBi7zV4TiSyJGc5E="
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "auth",
    "message": "Successfully authenticated",
    "hash": "DMK3DB2yBmJaBi7zV4TiSyJGc5E=",
    "user": {
        "user_id": "1",
        "username": "test1",
        "name": "Test User 1",
        "bio": "This is the Bio for test user 1",
        "Email": "test@test.com",
        "created": "3/26/2014 5:40:42 PM",
        "device_token": "0123456789ABCDEF",
        "zip": "54915",
        "birthdate": "10/25/1977 12:00:00 AM",
        "receive_emails": "True",
        "authentication_token": "3cc4b84d-5cfb-4a87-ac5c-27ec0ba51fd4",
        "authentication_token_expires": "4/2/2014 5:49:43 PM",
        "active": "True",
        "sex": "Male",
        "confirmed": "True"
		}
	}
* * *