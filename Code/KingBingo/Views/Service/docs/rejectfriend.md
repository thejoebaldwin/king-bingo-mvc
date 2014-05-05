##Reject Friend

**POST Request to http://itweb.fvtc.edu/service/v0/rejectfriend**

Use this command to reject a friend request. 

* * *

##Sample request body: 
	{
    "user_id": "43",
    "authentication_token": "dd46bd87-f484-4c0e-bd43-d374dcc6245e",
    "friend_user_id": "1"
	}
* * *

##Sample response data:
	{
    "status": "ok",
    "operation": "rejectfriend",
    "message": "successfully rejected friend"
	}
* * *