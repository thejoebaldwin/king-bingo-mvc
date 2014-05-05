##Accept Friend

**POST Request to http://itweb.fvtc.edu/service/v0/acceptfriend**

Use this command to accept a friend request. 


* * *

##Sample request body: 
	{
    "user_id": "10",
    "authentication_token": "f051daaf-de02-4217-8d6a-97b0e9f73402",
    "friend_user_id": "1"
	}
* * *

##Sample response data:
	{
    "status": "ok",
    "operation": "acceptfriend",
    "message": "successfully accepted friend"
	}
* * *