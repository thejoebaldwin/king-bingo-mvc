##All Friends

**POST Request to http://itweb.fvtc.edu/service/v0/allfriends**

Use this command to retrieve a list of friends for a given user. 

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
    "operation": "allfriends",
    "message": "successfully retrieved list of friends",
    "friends": [
        {
            "friend_id": "6",
            "friend_user_id": "1",
            "username": "test1",
            "name": "Test User 1",
            "status": "Rejected",
            "bio": "This is the Bio for test user 1"
        },
        {
            "friend_id": "20",
            "friend_user_id": "18",
            "username": "aaronanderson",
            "name": "aaron anderson",
            "status": "Pending",
            "bio": "precocious engineer"
        },
        {
            "friend_id": "30",
            "friend_user_id": "10",
            "username": "atomicsmith",
            "name": "atomic smith",
            "status": "Pending",
            "bio": "lifelong protagonist"
        }
    ]
	}
* * *