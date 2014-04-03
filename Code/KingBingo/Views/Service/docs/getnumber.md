##Get Number

####*POST Request to http://itweb.fvtc.edu/service/v0/getnumber*

Returns the next drawn number in a given game. The user\_id must match that of a user who has been joined to the game of the requested game\_id.

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **game_id**: Id of the game user is joined to
- **user_id**: Id of user making request
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response
- **number**: the bingo number that has been drawn. B12, B6, I50, etc.
- **next\_number\_time**: Unix time of when next number will become available


* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "b7e10511-eb23-4cb2-bd46-fb65ca6cf749",
    "game_id": "1"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "getnumber",
    "message": "Successfully retrieved number",
    "number": "I18",
    "next_number_time": "1396414625"
	}
* * *

##If the user is not joined to the game, the following will be returned:
	{
    "status": "error",
    "operation": "getnumber",
    "message": "User not joined to game."
	}
***
