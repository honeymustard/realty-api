# Realty API [![Build Status](https://travis-ci.org/honeymustard/realty-api.svg?branch=master)](https://travis-ci.org/honeymustard/realty-api)

A private web service that can index publicly listed realties.  

## Instructions
A sample of the hidden settings can be found in [secrets.fake.json](secrets.fake.json).  

```
dotnet publish -c Release                 # production build
dotnet watch run environment=Development  # start development server
```

## Usage
The API uses bearer authentication with Json web tokens and requires the  
consumer to have a valid user in the users collection.  

Connecting to the API with a JavaScript client.

```javascript
fetch('http://localhost:5000/api/auth/token', {
  method: 'POST',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    username: 'the-username',
    password: 'the-password',
  }),
})
.then(response => response.json())
.then(response => response.token)
.then(token => {
  // Use this token to make requests to the API.
});
```

The consumer is responsible for the scheduling by calling the parse route.  
```javascript
fetch('http://localhost:5000/api/realty/parse/today', {
  method: 'GET',
  headers: {
    'Accept': 'application/json',
    'Authorization' : `Bearer ${token}`,
  },
})
.then(response => response.json())
.then(response => {
  // The server will save new realties and report a status
});
```

## Output
The result is a list of new realties per day.

```javascript
// http://localhost:5000/api/realty/datum
[
  { "date": "2018-10-01T00:00:00", "value": 177 },
  { "date": "2018-10-02T00:00:00", "value": 211 },
  { "date": "2018-10-01T00:00:00", "value": 164 },
  { "date": "2018-10-03T00:00:00", "value": 102 },
]
```

## Copyright

Copyright © 2018 Adrian Solumsmo