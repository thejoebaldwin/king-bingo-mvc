##Auth

####*POST Request to http://itweb.fvtc.edu/service/v0/auth*


Use this command to authenticate to the web service and gain and authentication token for subsequent requests.

- **username**: add explanation
- **hash**: add explanation
- **status**: "ok" if successful, "error" if problem with request
- **message**: information about response
- **user**: Array of user attributes
- **user_id**: Id of user authenticated
- **authentication_token**: add explanation
- **authentication_token_expires**: add explanation
- **game_id**: add explanation

* * *

##Sample request body: 

	{
    "username": "test1",
    "hash": "DMK3DB2yBmJaBi7zV4TiSyJGc5E="
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "auth",
    "message": "Successfully authenticated",
    "user": {
        "user_id": "1",
        "username": "test1",
        "name": "Test User 1",
        "bio": "This is the Bio for test user 1",
        "Email": "test@test.com",
        "created": "1396329702",
        "device_token": "0123456789ABCDEF",
        "zip": "54915",
        "birthdate": "246603600",
        "receive_emails": "True",
        "authentication_token": "0798d29a-eb34-4ef1-97a5-75d5b458458a",
        "authentication_token_expires": "1396934916",
        "game_id": "1",
        "active": "True",
        "sex": "Male",
        "confirmed": "True"
		}
	}
* * *