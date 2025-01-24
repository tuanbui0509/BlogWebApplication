# ![CodeStudyMind](./images/logo.png)
*CodeStudyMind* is a blog to sharing technical skill for C#, React, NextJS, Azure and speacially for <sup>&dagger;</sup> ASP.NET<sup>&Dagger;</sup>, ...
### Table of Contents
1. **[Installation Instructions](#installation-instructions)**<br>
2. **[Solution Technologies](#Solution-technologies)**<be>
3. **[Solution Architecture](#solution-architecture)**<br>
4. **[Compatibility](#compatibility)**<br>
5. **[Deployment](#deployment)**<br>
6. **[Extension Bundles](#extension-bundles)**<br>

## Solution Technologies
* Create a Database schema for blog management *Code first, Lazy Eager Explicit Loading*.
* Use NoSQL MongoDB.
* Apply design pattern **Unit of Work** for web.
* *Authentication and Authorization* apply **(2FA)** login Google, Facebook, ...
* Apply Unit Test **NUnit** for web.
* Apply Caching **Memory Cache, Redis cache** for web.
* Apply Log **Serilog, Datadog** for web.
* Apply Mapping **AutoMapper** for web.
* Apply API Client and Communication **AutoMapper** for web.
* Apply CI/CD **GitHub Actions/ Azure Pipline**.
* Apply Handling messages  **MediatR**.
* Apply Validation **FluentValidation**.
* Apply **Polly Retry** to define a set of policies, such as retry, circuit breaker, and timeout.
* **Another Technical** *Microservices: Orchestration (Kubernetes), API-Gateway(Ocelot) Containerization (Docker) Reverse Proxy(Yarp) Message Queue(Kafka, RabbiMQ)*

## Installation Instructions

### Back-end: ASP.NET 8
Go to the [Nuget Web Store page for *.NET*](https://www.nuget.org/packages/) and install normally.

#### CleanArchitectureDemo.Domain project needs the following Nuget packages

- [MediatR (12.0.1)](https://www.nuget.org/packages/MediatR)

#### CleanArchitectureDemo.Application project needs the following Nuget packages

- [MediatR (12.0.1)](https://www.nuget.org/packages/MediatR)
- [Microsoft.EntityFrameworkCore (8.0.5)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
- [AutoMapper (12.0.1)](https://www.nuget.org/packages/automapper/)
- [AutoMapper.Extensions.Microsoft.DependencyInjection (12.0.1)](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection)
- [FluentValidation (11.5.2)]("https://www.nuget.org/packages/FluentValidation)
- [FluentValidation.AspNetCore (11.3.0)](https://www.nuget.org/packages/FluentValidation.AspNetCore)

#### CleanArchitectureDemo.Persistence project needs the following Nuget packages

- [MediatR (12.0.1)](https://www.nuget.org/packages/MediatR)
- [Microsoft.EntityFrameworkCore (8.0.5)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
- [Microsoft.EntityFrameworkCore.SqlServer (8.0.5)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/)
- [Microsoft.Extensions.Configuration (8.0.0)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/)

#### CleanArchitectureDemo.WebAPI project needs the following Nuget packages

- [MediatR (12.0.1)](https://www.nuget.org/packages/MediatR)
<!-- Note Security for sentensenive -->

### Front-end User page: NextJS => For SEO
### Front-end Admin page: ReactJS
## Solution Architecture

1. Clean Architecture
<img src="./images/Clean-Architecture.png" />
2. MediatR
<img src="./images/mediatR.jpeg" />
3. JWT-Authentication
<img src="./images/jwt-authentication-flows.webp" />


## Features
### On Backend project
- Apply the clean architecture for an ASP.NET application using Domain-Driven Design (DDD) principles for a Blog web application
- Authentication/Authorization
- Get OTP 
- Send Email 

### On Frontend project

-  Sending state while redirecting user
-  Nested Routing
-  useState
-  useEffect
-  Sweet Alert on Create, Edit and Delete
-  Confirmation on Delete
-  Elegant, Beatifull and fully Responsive Navbar
-  TypeScript Interface
-  TypeScript Partial
-  Axios
-  SASS
-  Mixin
-  Moment

## Setting SSH Key
- cat ~/.ssh/id_rsa.pub
# Finally

[**Markdownguide**](https://www.markdownguide.org/basic-syntax/)<br>
