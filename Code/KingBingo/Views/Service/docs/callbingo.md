##Call Bingo

####*POST Request to http://itweb.fvtc.edu/service/v0/callbingo*

Use this command verify a winning gamecard



* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "6f81e4cb-57cf-4737-b511-2671cb605016",
    "game_id": "1",
    "winning_numbers": "8,25,41,55,62",
    "game_card": "7,10,11,5,8,29,20,16,25,27,42,31,41,40,32,54,55,52,48,59,62,65,67,70,66"
	}
* * * 

##Sample response data:

	{
    "status": "ok",
    "operation": "callbingo",
    "message": "you won a game!"
	}