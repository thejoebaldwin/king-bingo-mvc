##Create User

####POST Request to *http://itweb.fvtc.edu/service/v0/createuser*

Creates a user and returns a user\_id, or existing user\_id if given login name already exists.

- **username**: add explanation
- **password**: add explanation
- **email**: add explanation
- **status**: "ok" if successful, "error" if problem with request
- **message**: Message associated with the response
- **user**: Array of user attributes
- **user_id**: Id of user authenticated
- **authentication_token**: add explanation
- **authentication\_token\_expires**: add explanation

* * *

##Sample request body: 

	{
    "username": "test34",
    "password": "test34",
    "email": "test34@test.com"
	}
* * *

##Sample response data:

	{
    "status": "ok",
    "operation": "createuser",
    "message": "Successfully created user",
    "user": {
        "user_id": "4",
        "username": "test34",
        "name": "test34",
        "bio": "",
        "Email": "test34@test.com",
        "created": "1396331441",
        "device_token": "",
        "zip": "",
        "birthdate": "246603600",
        "receive_emails": "True",
        "authentication_token": "33b887b5-2520-440a-bdb4-4b254ae7d726",
        "authentication_token_expires": "1396936241",
        "active": "True",
        "sex": "Female",
        "confirmed": "True",
        "location": "88,-120"
		}
	}
If the username already exists, the following response will be returned instead:  

	{
    "status": "error",
    "operation": "createuser",
    "message": "user already exists"
	}
* * *

If email already exists, the following response will be returned instead:  

	{
    "status": "error",
    "operation": "createuser",
    "message": "email already exists"
	}
* * *
