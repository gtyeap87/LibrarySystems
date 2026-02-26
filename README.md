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
-- ~Repositor~ (Already replaced by specification)
-- Future (Clean architecture/Vertical slice?)

Model
- Model for database design
- Dto for display purpose

ORM used
- EF Core

Database
- SQL Server

Log
- Build-in Logging
- Might use Structure log like serilog when need to move up to production.

Repository design
  1. ~Traditional Repository~
  2. Specification and generic query and command
  3. Mediatr
 
API Perfomarnce implentation
- Rate Limiting
- AsTracking for display only
- SplitQuery
- Pagination

Unit Test
- XUnit
- Moq
- Autofixture
  
Future implemention
V1
- ~Bulk CUD~
- ~API Versioning~
- ~Mediatr~
- ~Authorization and Authentication~
- React (Web)
- ~Pagination~
- ~Unit Test~
- Redis Cache

V2
- Container
- Microservice

V3
- Clean Architecture

V4
- Desktop version
- Minimal API

V5
- Angular (Web)
- Vue.js (Web)
- Mobile (Maui?)
- Blazor (Web)

