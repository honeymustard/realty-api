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

## Copyright

Copyright © 2018 Adrian Solumsmo