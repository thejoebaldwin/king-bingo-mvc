##Bingo API v0 3/25/14#

Supported Operations

- [Authenticate](v0/auth "Authenticate")
- [Create User](v0/createuser "Create User")
- [Get User](v0/getuser "Get User")
- [Get All Users](v0/allusers "Get All Users")
- [Get All Games](v0/allgames "Get All Games")
- [Create Game](v0/creategame "Create Game")
- [Join Game](v0/joingame "Join Game")
- [Quit Game](v0/quitgame "Quit Game")
- [Get Number](v0/getnumber "Get Number")
- [Call Bingo](v0/callbingo "Call Bingo")
- [Invite Users](v0/inviteusers "Invite Users")
- [Get Notifications](v0/allnotifications "Get Notifications")
- [Update Profile Image](v0/updateprofileimage "Update Profile Image")
- [Update User](v0/updateuser "Update User")

Usage  


- All json field names in the response will be lowercase only. All field names in request need to be lowercase as well. Generally, any attribute consisting of one or more words will use the "\_" to separate the words instead of a space or camel-casing.
- All responses will return a "operation" parameter. This will allow you to standardize your json parsing depending on which request you are making.
- Get a list of all currently in progress games. Within each block of "game" data, there is a game\_id.
- You must authenticate against the "auth" operation to use the api, excepting the "createuser" operation. This will give you a user\_id and authentication\_token that you can then join a game with. You can use "test1" for login and password of "test1" if you'd like to skip this step.
- Gist examples of the password hashing algorithm to use in the auth operation, in objective-c and c# can be found [here](https://gist.github.com/thejoebaldwin/10443564 "here") and [here](https://gist.github.com/thejoebaldwin/10443397 "here").
- Join a game using the user\_id, authentication\_token and game\_id. This will return a "gamecard" of numbers that you will use to construct your bingo card.
- Once you have your board, call use the getnumber operation to play the game!
- Be sure to check the response messages when getting errors to help troubleshoot your requests, and don't forget to validate your json.
- Error reports (specifically, what you were doing when it crashed) should be sent to [joe@humboldttechgroup.com](mailto:joe@humboldttechgroup.com "email")