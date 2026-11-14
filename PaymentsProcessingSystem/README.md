# Payments Processing System

## Description

Payments Processing System is a learning project for designing and
building a realistic, cloud-based payment processing solution using .NET
and AWS.

The project is intended to explore professional backend engineering
practices, clean architectural boundaries, asynchronous messaging,
persistence, integrations, simulation, dynamic configuration, testing,
and deployment considerations.

The solution is being built incrementally. Architectural decisions are
discussed and agreed before implementation, and technologies are
introduced when there is a practical learning objective or requirement.

## Architecture Overview

The solution uses a layered architecture with separate executable
applications and class libraries.

``` text
PaymentsProcessingSystem/
├── src/
│   ├── Payments.API/
│   ├── Payments.Worker/
│   ├── Payments.UseCase/
│   ├── Payments.Domain/
│   ├── Payments.Abstractions/
│   ├── Payments.Persistence/
│   ├── Payments.Integrations/
│   └── Payments.Simulation/
│
├── tests/
│   └── Payments.UnitTests/
│
├── PaymentsProcessingSystem.sln
└── README.md
```

### Project Responsibilities

| Project | Responsibility |
|---|---|
| `Payments.API` | HTTP endpoints, controllers, HTTP request/response models, API-level validation, application startup and dependency registration. |
| `Payments.Worker` | Independently hosted SQS message consumer. Receives messages, deserializes and performs message-level validation, invokes UseCase services, and handles acknowledgement and message-processing failures. |
| `Payments.UseCase` | Application services and workflows. Coordinates domain behaviour, persistence and integrations through abstractions. |
| `Payments.Domain` | Domain entities, value objects, enums, invariants and business rules. Has no infrastructure dependencies. |
| `Payments.Abstractions` | Contracts/interfaces required by the application, including repository, messaging, storage and configuration abstractions. |
| `Payments.Persistence` | Database-specific implementations, including DynamoDB repositories and persistence-model mapping. |
| `Payments.Integrations` | Real external-system integrations, including AWS SQS, SNS and S3 implementations. |
| `Payments.Simulation` | Simulated implementations of application-facing contracts, allowing local development and selected environments to run without real external dependencies. |
| `Payments.UnitTests` | Dedicated unit-testing project for domain, UseCase and other testable application components. |

### Project References

| Project | References |
|---|---|
| `Payments.API` | `Payments.UseCase`, `Payments.Persistence`, `Payments.Integrations`, `Payments.Simulation` |
| `Payments.Worker` | `Payments.UseCase`, `Payments.Persistence`, `Payments.Integrations`, `Payments.Simulation` |
| `Payments.UseCase` | `Payments.Domain`, `Payments.Abstractions` |
| `Payments.Domain` | None |
| `Payments.Abstractions` | `Payments.Domain` |
| `Payments.Persistence` | `Payments.Domain`, `Payments.Abstractions` |
| `Payments.Integrations` | `Payments.Domain`, `Payments.Abstractions` |
| `Payments.Simulation` | `Payments.Domain`, `Payments.Abstractions` |
| `Payments.UnitTests` | References the production projects whose behaviour is being unit tested. It is not a production dependency. |

The API and Worker are separate executable applications and can be
deployed, scaled, restarted and operated independently. Both act as
composition roots and decide how their dependencies are registered.

## Architectural and Design Decisions

### Application and domain boundaries

-   `Payments.Domain` contains business concepts and rules. It must not
    depend on AWS SDKs, databases, HTTP, or other infrastructure
    technologies.
-   `Payments.UseCase` coordinates application workflows and calls
    dependencies directly.
-   No MediatR or mediator pattern is planned. Application services are
    invoked directly.
-   Services are grouped by responsibility (for example,
    `PaymentService`). A separate service is introduced when a
    responsibility or area becomes sufficiently large or distinct; a
    separate controller does not automatically require a separate
    service.
-   UseCase methods may accept individual parameters for simple
    operations and dedicated command/query models when an operation has
    a more complex input.
-   API request and response models belong in `Payments.API`, using
    names such as `CreatePaymentRequest` and `CreatePaymentResponse`.
    UseCase must not depend on HTTP models.
-   API-level validation checks request shape, required fields and
    formats. UseCase and Domain enforce application and business rules.
    Avoid duplicating the same validation without a reason.
-   API maps application/domain results to HTTP response models.

### Abstractions and repositories

-   `Payments.Abstractions` is the shared home for contracts needed by
    the application. It contains interfaces, not infrastructure
    implementations or general-purpose helper classes.
-   Interfaces describe what the application needs, rather than exposing
    how a technology works.
-   Repository interfaces use Domain models and must not expose
    DynamoDB, SQL or AWS SDK types.
-   Repository implementations retrieve and persist data and translate
    between technology-specific persistence representations and Domain
    entities.
-   Persistence-specific records and mapping stay inside
    `Payments.Persistence`.
-   Repositories do not contain business rules or application workflow
    logic. Mapping can start inside the repository implementation and be
    extracted into a mapper if it becomes complex or widely reused.
-   AWS-specific exceptions and response types remain inside
    infrastructure projects. Translate errors at the boundary when the
    application needs a technology-independent outcome.

### Integrations and simulation

-   `Payments.Integrations` contains real external-system
    implementations, such as SQS publishers, SNS publishers and S3
    storage.
-   `Payments.Simulation` contains fake/simulated implementations of the
    same application-facing interfaces.
-   A simulated implementation should fulfil the same contract as its
    real counterpart.
-   A configurable/delegating implementation (for example,
    `ConfigurablePaymentPublisher`) can implement an interface and
    delegate to either the real or simulated implementation based on the
    current configuration.
-   The caller injects and uses the interface directly; it does not need
    to know which implementation is selected.
-   A factory is appropriate when the caller genuinely needs to
    request/select an implementation and then decide which operation to
    invoke. A configurable implementation is appropriate when selection
    should be transparent to the caller.
-   Simulation is intended for local development and selected
    non-production environments. Simulation must never be enabled in
    Production; Production startup/configuration should reject simulated
    registrations.
-   Simulation and feature toggles are related but distinct. Simulation
    replaces a dependency implementation. Feature toggles enable or
    disable optional capabilities, such as sending a payment-success
    email. Essential business rules and mandatory payment validation are
    not feature toggles.

### Configuration

Configuration is considered in three broad categories:

1.  **Infrastructure/startup configuration:** AWS region, credentials,
    connection details and dependency wiring. These are generally
    established at application startup.
2.  **Operational configuration:** Values such as payment limits,
    timeouts and retry settings.
3.  **Feature switches and simulation settings:** Optional capabilities
    and the selection of real or simulated dependencies.

-   Local Development, SIT, UAT and Production can have different
    configuration values.
-   Operational settings and feature switches should support dynamic
    refresh where practical, so a configuration change does not require
    an application restart or redeployment.
-   Dynamic configuration must be provided by a source/provider that
    supports refresh. A normal startup-only configuration value does not
    become dynamic merely because it is wrapped in an interface.
-   Application services should depend on application-facing
    configuration abstractions where appropriate, rather than reading
    `IConfiguration` throughout the codebase.
-   A future learning objective is to investigate AWS AppConfig for
    centrally managed configuration and feature flags.
-   Runtime configuration changes in multi-instance environments must
    reach all relevant API and Worker instances. Configuration changes
    should be controlled and, where appropriate, audited.
-   Production must not select simulated implementations.

### API, Worker and background processing

-   `Payments.API` handles HTTP traffic and invokes UseCase services.
-   `Payments.Worker` is an independent SQS consumer. It receives and
    deserializes messages, performs message-level validation, invokes
    UseCase services, and handles acknowledgement and message-processing
    failures.
-   The Worker does not host the API and does not depend on the API
    being available.
-   The Worker is not a general-purpose scheduler or host for every
    background task.
-   Long-running or scheduled jobs are a separate concern. Hangfire,
    EventBridge Scheduler or another suitable technology may be explored
    later. If introduced, scheduling/background processing should have
    an appropriate hosting component rather than being added to the SQS
    Worker by default.

### Dependency registration

-   `Program.cs` should remain focused on application startup and host
    configuration.
-   Each executable project has its own `Installer.cs` containing its
    dependency registration methods.
-   `Payments.API` and `Payments.Worker` own their respective installers
    and independently decide which services they register.
-   Class libraries do not expose DI extension methods as the initial
    convention. If registration duplication becomes substantial, shared
    registration can be reconsidered.
-   Both applications can reuse the same implementation projects while
    registering only the dependencies they actually need.

### Error handling and coding conventions

-   Do not use top-level statements. Use an explicit `Program` class and
    `Main` method.
-   Use `try/catch` where it adds value, such as handling, translating,
    recovering from or logging an exception at an appropriate boundary.
    Do not add catch blocks indiscriminately or swallow exceptions.
-   The `Async` suffix is not mandatory for every asynchronous method.
    Use it where it improves clarity and consistency with the
    surrounding codebase.
-   Use `CancellationToken` for asynchronous operations where
    cancellation is relevant, especially I/O and request/message
    processing.
-   Use `ILogger` for application logging.
-   Prefer straightforward, maintainable code over unnecessary
    abstractions and patterns.
-   Use direct service calls; no mediator.
-   Keep technology-specific dependencies out of Domain and UseCase.

## Technologies

The initial technology choices are:

| Area | Technology |
|---|---|
| Language and framework | C#, .NET 10 |
| HTTP API | ASP.NET Core Web API |
| Database | Amazon DynamoDB |
| Messaging | Amazon SQS |
| Notifications | Amazon SNS |
| Object/file storage | Amazon S3 |
| Dependency injection | Built-in .NET dependency injection |
| Logging | `Microsoft.Extensions.Logging` |
| Unit testing | NUnit and Moq |
| Mapping | Manual mapping initially |

Additional technologies may be introduced as the project develops. Avoid
adding packages before there is a concrete requirement.

## Prerequisites

Install or have access to:

-   Visual Studio 2026 or a compatible .NET IDE.
-   .NET 10 SDK.
-   AWS account.
-   AWS CLI v2.
-   An AWS IAM identity/profile with only the permissions required for
    the learning project.
-   Git.

AWS resources can incur charges. Check service pricing, configure
budgets/alerts where appropriate, and remove resources that are no
longer needed. Do not assume every AWS service or configuration is
covered by the Free Tier.

## Installing

### 1. Clone the repository

``` bash
git clone <repository-url>
cd PaymentsProcessingSystem
```

### 2. Open the solution

Open `PaymentsProcessingSystem.sln` in Visual Studio.

### 3. Restore dependencies

``` bash
dotnet restore
```

### 4. Configure AWS credentials for local development

Use AWS CLI authentication and a named local profile. The application
should use the AWS SDK credential provider chain rather than storing
access keys in source code or configuration files.

The current learning setup uses the profile:

``` text
aws-learning
```

The AWS region currently used for the learning environment is:

``` text
us-east-1
```

Do not commit credentials, access keys, secrets or local
machine-specific configuration.

### 5. Configure environment settings

The API and Worker load their configuration at startup. Use
environment-specific configuration for local Development, SIT, UAT and
Production as appropriate. Keep secrets out of committed `appsettings`
files.

### 6. Build and test

``` bash
dotnet build
dotnet test
```

## AWS Learning Environment

The initial AWS learning work established:

-   AWS CLI v2 installed and authenticated with the `aws-learning`
    profile.
-   AWS SDK for .NET configured to resolve local credentials.
-   DynamoDB table named `Payments` created in `us-east-1`.
-   Partition key: `PaymentId` (String).
-   Initial provisioned capacity: 1 read capacity unit and 1 write
    capacity unit, with autoscaling disabled.

The initial sample payment is:

``` json
{
  "PaymentId": "PAY-1001",
  "Amount": 125.50,
  "Currency": "ZAR",
  "Status": "PENDING",
  "CreatedAt": "2026-11-05T12:00:00Z"
}
```

Review AWS capacity, pricing and permissions before expanding the
environment. The existing `Payments.AwsDemo` console project is retained
as a reference while the new multi-project solution is built.

## Future Learning and Improvements

These are future learning topics, not requirements for the initial
solution:

-   AWS AppConfig for dynamic configuration and feature flags.
-   Hangfire or another scheduler for scheduled and long-running jobs,
    hosted separately from the SQS Worker where appropriate.
-   AWS EventBridge Scheduler and Step Functions, comparing their use
    cases with Hangfire and SQS.
-   More complete integration testing and end-to-end testing.
-   Resilience patterns such as retries, timeouts, circuit breakers and
    dead-letter queues.
-   Observability, metrics, tracing and structured logging.
-   CI/CD, containerisation and deployment to AWS.
-   Security hardening, IAM least privilege and production configuration
    safeguards.

## Author

Sashen Govender
