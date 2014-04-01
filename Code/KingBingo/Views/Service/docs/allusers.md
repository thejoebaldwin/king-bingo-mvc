##Get All Users

####*POST Request to http://itweb.fvtc.edu/service/v0/allusers*

Returns all users in the system in batches of 25

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **status**: "ok" if successful, "error" if problem with request
- **users**: Array of user elements, each containing it's own user\_id and attributes.
- **message**: Message associated with the response
- **page**: Optional. The set of results to return. A page value of 0 would return the first 25 users, 1 would return 26-50, etc. When excluded from the post body a value of 0 is default.

* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "178f240f-aaae-4d91-b4a1-7a9b07d04e93",
    "page": "0"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "allusers",
    "message": "Successfully retrieved list of all users",
    "users": [
        {
            "user_id": "1",
            "username": "test1",
            "name": "Test User 1",
            "bio": "This is the Bio for test user 1",
            "wins": "0",
            "rank": "0",
            "friends": "0",
            "games": "0"
        },
        {
            "user_id": "2",
            "username": "test2",
            "name": "Test User 2",
            "bio": "This is the Bio for test user 2",
            "wins": "0",
            "rank": "0",
            "friends": "0",
            "games": "0"
        }
			]
	}
* * *
