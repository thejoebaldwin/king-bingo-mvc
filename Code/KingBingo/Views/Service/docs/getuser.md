##Get User

####*POST Request to http://itweb.fvtc.edu/service/v0/getuser*

Returns profile information about a single user

- **user_id**: id of the user making the request
- **authentication_token**: token retrieved by authenticating via the auth operation
- **query\_user\_id**: id of the user to retrieve information about 
- **status**: "ok" if successful, "error" if problem with request
- **users**: Array of a single user element, consisting of public user attributes.
- **message**: Message associated with the response

* * *

##Sample request body: 

	{
    "user_id": "1",
    "authentication_token": "b7e10511-eb23-4cb2-bd46-fb65ca6cf749",
    "query_user_id": "1"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "getuser",
    "message": "Successfully retrieved user",
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
        }
    ]
	}
* * *

##If a user is not found with the given query\_user\_id the following is returned:
	
	{
    "status": "error",
    "operation": "getuser",
    "message": "User not found"
	}

* * *
