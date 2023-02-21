# REST Course
_(**RE**)presentational (**S**)tate (**T**)ransfer_

[TOC]

Used to build communication system for distristributed services

## 6 contraints
### Uniform Interface: Clear defined interface between client and server
- Identification of resources
- Manipulation of resources through respresentations
- Self-descriptive messages
- Hypermedia as the engine of application state <--Heavily debated

### Stateless
The server in a single call should have everything it needs to process the call and not require a previous call

### Cacheable
Should explicitly state to the client if and how long the server will cache, its up to the client to bypass the cache

### Client-Server
Client and Server can change independly so long as the contract is the same

### Layered System
The client should not know where the request goes and how it processes

### Cod on demand (Optional): The server can send literal code to the client to run

## HTTP Information
### Resource naming and routing
_Naming should be plural_
- GET /movies -- To get all movies
- GET /movies/1 -- To get one movie
- GET /movies/1/ratings -- To get the ratings for the movie
- POST /movies/1/ratings -- To add a new rating
- PUT /movies/1/ratings -- To update a rating
- DELETE  /movies/1/ratings -- To delete
- GET /ratings/me -- Would be to get the resource for you

### HTTP Verbs are meaningful
- POST - Create
- GET - Retrieve
- PUT - Complete update
- PATCH - Partial update (_commonly not used due to complexity_)
- DELETE - Remove resource

### HTTP Status codes

**POST**
- Single resource (/items/1): N/A not used for a single resource
- Collection resource (/items): 201 (Location header), 202 Accepted

**GET**
- Single resource (/items/1): 200 - Success and returns a object, 404 - Not found
- Collection resource (/items): 200 - Returns if collection is full or empty

**PUT**
- Single resource (/items/1): 200, 204 (no content), 404
- Collection resource (/items): 405 Method not allowed

**DELETE**
- Single resource (/items/1): 200, 404
- Collection resource (/items): 405

## Idempotency
POST - Not Idempotent - When you create a user repeatedly as either a new user will be created or it will be rejected as a duplicate/bad request 
GET - Idempotent - Data will never change on the server
PUT - Idempotent - If I submit the same value repeatedly it will just override the data
DELETE - Idempotent - If I send the delete more than once the process will stay the same
HEAD - Idempotent - Will return the same value each time it is subsequently called
OPTIONS - Idempotent - Will return the same value each time it is subsequently called
TRACE - Idempotent - Will return the same value each time it is subsequently called

## HATEOAS (_Hypermedia as the Engine of Application State_)
_This in general is not used anymore especially with the documentation available with newer apis, swagger and OPENapi standards_
### Old method
_This is not commonly used anymore because it can bloat the contract_

```json
{
    "departmentId": 42,
    "departmentName": "Administration"
    "locationId": 1977,
    "managerId": 200,
    "links": [ // This provides links to additional resources for the api
        {
            "href": "42/employees", // This is the endpoint of department 42, that will return the list of employees
            "rel": "employees",  // Gets Employess
            "type": "GET" //Type of request
        }
    ]
}
```

### "New" method

```json
{
  "account":{
    "account_number": 12345,
    "balance":{
      "currency":"usd",
      "value": 100.00
    },
    "links":{ //This keeps the contract lighter
      "deposits": "/accounts/12345/deposits",
      "withdrawals": "/accounts/12345/withdrawals",
      "transfers": "/accounts/12345/transfers",
      "close-request": "/accounts/12345/close-requests"
    }
  }
}
```
## Faults

| Error                            | Fault                                          |
|----------------------------------|------------------------------------------------|
| 400 - Invalid but server is fine | 500 - Valid'ish request but server has faulted |
