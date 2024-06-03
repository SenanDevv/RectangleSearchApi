# Project description
The application is a web-based API designed to perform coordinate-based searches within a set of rectangles. The core functionality involves accepting two pairs of doubles, representing a segment as input and returning a list of rectangles that interset the input segment by any of the edges. Each rectangle in the database is defined by 4 points. The main search is done via an sql procedure SP_GetAllIntersectedRectangles.

# Used Patterns and Technologies:

#### Onion Architecture
The project follows the Onion Architecture, organizing code in layers for enhanced modularity and maintainability.

#### Repository Pattern
The application employs the Repository pattern, providing a structured way to manage data access and storage.

#### Unit Tests
The inclusion of unit tests ensures the reliability and correctness of the application, contributing to overall code quality.

#### Docker
The entire application is containerized using Docker, facilitating seamless deployment and portability across environments.

#### Database Connectivity
SQL Server in Container: The application establishes a connection to a SQL Server, which is containerized for efficient management and deployment.

#### Indexing

The project employs both a clustered index (default primary key ID) and a composite unclustered index for the X and Y coordinates combined. This indexing strategy enhances database query performance.

#### Authentication

Basic authentication is implemented for secure API access. Include the following Authorization header for testing:

Authorization: Basic dGVzdFVzZXJuYW1lOnRlc3RQYXNzd29yZA==

Note that all the settings (basic auth credentials) can be found in Project.Core/Settings/settings.json file.

## Request-Response Information:

Endpoint:
POST http://localhost:5000/api/Rectangles/Search

Request Body:
```
{
  "firstPair": {
    "x": 1,
    "y": 4
  },
  "secondPair": {
    "x": 5,
    "y": -3
  }
}
```

Response:

```
{
    {
    "success": true,
    "error": null,
    "data": [
        {
            "rectangleId": 2,
            "topRightPointId": 8,
            "topLeftPointId": 7,
            "bottomRightPointId": 6,
            "bottomLeftPointId": 5
        },
        {
            "rectangleId": 4,
            "topRightPointId": 16,
            "topLeftPointId": 15,
            "bottomRightPointId": 14,
            "bottomLeftPointId": 13
        },
        {
            "rectangleId": 5,
            "topRightPointId": 20,
            "topLeftPointId": 19,
            "bottomRightPointId": 18,
            "bottomLeftPointId": 17
        }
    ],
    "statusCode": 200
}
}
```

GET https://localhost:7057/api/Rectangles/GetRectangle/id

Response:

```
{
    "success": true,
    "error": null,
    "data": {
        "id": 3,
        "topRightPoint": {
            "id": 12,
            "x": -7,
            "y": 4
        },
        "topLeftPoint": {
            "id": 11,
            "x": -7,
            "y": 4
        },
        "bottomRightPoint": {
            "id": 10,
            "x": -7,
            "y": -5
        },
        "bottomLeftPoint": {
            "id": 9,
            "x": -7,
            "y": -5
        }
    },
    "statusCode": 200
}
```