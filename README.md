# Realty API

A private web service that can index publicly listed realties.  

The consumer is responsible for all scheduling i.e. calling the  
appropriate routes in a regular fashion.  

The API uses bearer authentication and requires the consumer to  
have a valid user in the users collection.  

## Instructions
A sample of the hidden settings can be found in **secrets.fake.json**.  

```
dotnet publish -c Release                 # production build
dotnet watch run environment=Development  # start development server
```

## Usage
Connect to the API with a JavaScript client.

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
  // use this token to access secure routes
});
```

Use the access token to make requests to the API.
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
  // do something with the response..
});
```

## Copyright

Copyright © 2018 Adrian Solumsmo