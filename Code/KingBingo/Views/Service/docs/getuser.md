##Get Users

####POST Request to http://itweb.fvtc.edu/service/v0/allusers

Returns all currently joined users in a given game

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **status**: "ok" if successful, "error" if problem with request
- **users**: Array of user elements, each containing it's own user\_id and login attributes.
- **message**: Message associated with the response

* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "4e121e1a-d5ee-4789-8620-aeb094e7ea71"
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
            "bio": "This is the Bio for test user 1"
        },
        {
            "user_id": "2",
            "username": "test2",
            "name": "Test User 2",
            "bio": "This is the Bio for test user 2"
        }
			]
	}
* * *
