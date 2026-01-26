# Library Systems
Description
- Basically prototype project to explore new idea or improving existing implementation.

Note
- For model, endpoints and architecture design, no AI assitance is used. The AI usage is mainly use in realizing the code design.
- Utilized SonarQube for code improvement suggestion.

Architecure
- N-tier
-- Controller
-- Service
-- Repository

Model
- Model for database design
- Dto for display purpose

ORM used
- EF Core

Database
- SQL Server

Log
- Build-in Logging

Repository design
  1. Traditional Repository
  2. Specification and generic query and command
  3. Mediatr
 
API Perfomarnce implentation
- Rate Limiting
- AsTracking for display only
- SplitQuery
- Pagination
  
Future implemention
V1
- ~Bulk CUD~
- ~API Versioning~
- ~Mediatr~
- ~Authorization and Authentication~
- React (Web)
- ~Pagination~
- Unit Test
- Redis Cache

V2
- Container
- Microservice
- Blazor (Web)

V3
- Desktop version
- Minimal API

V4
- Angular (Web)
- Vue.js (Web)
- Mobile (Maui?)

V5
- Clean Architecture
