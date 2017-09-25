# RestApiCodeChallenge

Web API Implementation of simple chat app

This applicaiton wascreated using VisualStudio 2017 Community Edition. Building the application is simple. Clone the repository and build the release branch. You can use any RESTtest tool such as Postman for your testing.

## Running the App

1. Send a POST to the /api/v1/sessions URI. Be sure to set the Accept header to "application/vnd.api+json". Copy the JWT token returned in the authorization header. Typical response data looks like this,

{
    "data": {
        "type": "sessions",
        "id": 0,
        "attributes": {
            "created_at": "2017-09-25T16:08:35.9389261Z"
        },
        "relationships": {
            "creator": {
                "links": {
                    "self": "http://localhost:60994/api/v1/sessions/0/relationships/creator",
                    "related": "http://localhost:60994/api/v1/sessions/0/creator"
                }
            },
            "data": {
                "type": "users",
                "id": 0
            }
        },
        "links": {
            "self": "http://localhost:60994/api/v1/sessions/0"
        }
    },
    "included": [
        {
            "type": "users",
            "id": 0,
            "attributes": {
                "username": "user210421"
            },
            "links": {
                "self": "http://localhost:60994/api/v1/users/0"
            }
        }
    ],
    "meta": {}
}

2. Once you have the token, you can create messages by sending a POST to the /api/v1/messages url. Set the Authorization header with the token you copied from step 1. Remember to set the Accept and Content-Type headers to "application/vnd.api+json". You must set the body of this post to a JSONAPI entity representing a message such as this,

{
  "data": {
    "type": "messages",
    "attributes": {
      "message": "This is a test of the emergency broadcast system 3"
    }
  }
}

3. After posting one or more messages you can read back the messages with pagination by sending a GET to the messages url with pagination query parameters. Remember the Authorization and Accept headers.
