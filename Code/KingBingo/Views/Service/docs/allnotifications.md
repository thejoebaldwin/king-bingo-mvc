##Get Notifications

####*POST Request to http://itweb.fvtc.edu/service/v0/allnotifications*

Use this command retrieve all notifications for the given user



* * *

##Sample request body: 
	{
    "user_id": "1",
    "authentication_token": "fc68f3c7-62f5-4f7e-ab4d-220b90a546ff"
	}


* * * 

##Sample response data:
	{
    "status": "ok",
    "operation": "allnotifications",
    "message": "successfully retrieved list of all notifications",
    "notifications": [
        {
            "notification_id": "1",
            "created": "1399270423",
            "message": "test1 Sent you a friend request",
            "type": "Friend",
            "user_id": "1",
            "game_id": "0",
            "result_id": "0"
        }
    ]
	}
* * * 