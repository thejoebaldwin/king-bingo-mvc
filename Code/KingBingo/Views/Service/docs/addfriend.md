##Add Friend

**POST Request to http://itweb.fvtc.edu/service/v0/addfriend**

Use this command to send a friend request

* * *

##Sample request body: 
	{
    "user_id": "1",
    "authentication_token": "5bb757b7-2c1d-45e9-ad17-caee422fc264",
    "friend_user_id": "42"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "addfriend",
    "message": "successfully added friend"
	}
* * *