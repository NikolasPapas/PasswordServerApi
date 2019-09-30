# Password Server Api
---
## Description
Project .Net Core Rest Api For Saving Passwords 



# Tech

Dillinger uses a number of open source projects to work properly:

* [c#] - AspNetCore 2.2 !
* [Visual Studio IDE] - awesome web-based text editor

---
##  Postman Requests
### Login
```sh
Method : Post => https://localhost:44390/api/Authentication/logIn
Body : Json =>
    For Admin:
    {
        "username":"username105",
        "password":"105"
    }
    For User:
    {
        "username":"username106",
        "password":"106"
    }
    For Viewer:
    {
        "username":"username107",
        "password":"107"
    }
Responce : Json =>
    {
      "payload":
      {
          "token":"...",
          "actions":
                    [
                        "id": "c25b9787-8751-4fbd-bc6c-c63a48026d30",
                        "name": "...",
                        "needsComment": true || false,             *1
                        "tooltip": "...",                          *1
                        "sendApplicationData": true || false,      *1
                        "validationMode": 1 || 2 || 3,             *1
                        "refreshAfterAction": true || false,       *1
                        "collapseApplication": thue || false,      *1
                        "icon": "...",                             *1
                        "needsConfirmation": true || false         *1
                    ],
      },
      "selectedAction": "00000000-0000-0000-0000-000000000000",
      "warnnings": []
    }
```

# Actions
```sh
Method : Post => https://localhost:44390/api/accounts/accountAction
Authorization : Admin : Bearer Token => Token from login responce
Body: json = >
    {
        "ActionId":" From login responce ",
        "AccountId":""          *2
        "UserName":"",          *1
        "Password":"",          *1
        "Email":"",             *1
        "CustomFilterID":"",    *1 
        "CustomFilterValue":"", *1
        "Account":
	      {
            "firstName": "...",
            "lastName": "...",
            "userName": "...",
            "email": "...",
            "sex": 0 || 1,
            "role": "Admin" || "User" || "Viewer",
            "password": "...",
            "passwords": []
        },
    }
Responce : Json =>
    {
      "payload": [
                    {
                        "firstName": "...",
                        "lastName": "...",
                        "userName": "...",
                        "email": "...",
                        "sex": 0 || 1,
                        "role": "Admin" || "User" || "Viewer",
                        "password": "...",
                        "passwords": []
                    },
                    ...
                 ],
      "SelectedAction": "00000000-0000-0000-0000-000000000000",
      "warnnings": []
    }
```
```sh
Method : Post => https://localhost:44390/api/passwords/passwordAction
Authorization : Admin + User : Bearer Token => Token from login responce
Body: json = >
    {
    	  "ActionId":" From login responce ",
	      "name":"...",
	      "LoginLink":"..."
        "password":
            {
              "passwordId":"..",
              "AccountId":""          *2
              "name":"",
              "userName":"",
              "password":"",
              "logInLink":"",
              "sensitivity": 0 || 1 || 2 || 3 || 4 ,
              "strength": 0 || 1 || 2 || 3 || 4 ,
            }
    }
Responce : Json =>
    {
      "payload": [
                    {
                        "passwordId":"..",
                        "AccountId":""          *2
                        "name":"",
                        "userName":"",
                        "password":"",
                        "logInLink":"",
                        "sensitivity": 0 || 1 || 2 || 3 || 4 ,
                        "strength": 0 || 1 || 2 || 3 || 4 ,
                    },
                    ...
                 ],
      "SelectedAction": "00000000-0000-0000-0000-000000000000",
      "warnnings": []
    }
```

```sh
*1 For Future Use
*2 NotUsed Overiting on Controller
```


