# New Project Discussion Points

This section is a reusable guide for discussing a new greenfield project
with a coding agent before implementation begins.

The goal is to make architectural and technical decisions deliberately,
without allowing the agent to generate a large amount of code based on
assumptions. Use the prompts below as a structured conversation. Discuss
one topic at a time, challenge recommendations, record decisions, and
only then move to implementation.

## How to use this guide

1.  Start with the **Opening Prompt**.
2.  Work through the discussion areas that are relevant to the project.
    Skip topics that do not apply.
3.  Ask the agent to explain trade-offs and recommend a pragmatic
    default, not simply agree with every suggestion.
4.  Confirm each decision before moving to the next topic.
5.  Maintain a decision log containing the decision, rationale,
    alternatives considered and consequences.
6.  Ask the agent to produce the proposed solution structure and
    project-reference matrix before creating projects.
7.  Do not start implementation until the architecture and initial scope
    are agreed.

## Opening Prompt

Copy and adapt this prompt when starting a new project discussion:

> I am joining a new greenfield software project and want to work with
> you as an architectural discussion partner before we write code.
>
> Help me establish the project setup, architecture, technologies,
> conventions, testing strategy, configuration, deployment approach and
> initial solution structure.
>
> Do not immediately generate code or assume a particular architecture.
> First understand the business problem, requirements, constraints,
> expected scale, team experience and operational requirements.
>
> Work through the topics one at a time. For each topic:
>
> -   Explain the relevant options in plain language.
> -   Recommend a pragmatic approach and explain why.
> -   Identify trade-offs, risks and implications.
> -   Challenge my assumptions when appropriate; do not simply agree
>     with me.
> -   Avoid unnecessary abstractions, patterns and projects.
> -   Ask only the most useful next question.
> -   Record the decision once we agree.
>
> Keep a running architecture decision log. Distinguish confirmed
> decisions from suggestions and unresolved questions. Do not treat an
> idea as agreed until I explicitly confirm it.
>
> At the end, produce the proposed solution/project structure, project
> responsibilities, project-reference matrix, technology list, coding
> conventions, configuration strategy, testing strategy and
> implementation plan.
>
> Only start creating projects or writing implementation code when I
> explicitly ask you to.

## Discussion 1: Understand the project

**Prompt:**

> Start by helping me define the project. Ask about its purpose, users,
> core business capabilities, main workflows, external systems, data,
> expected scale, availability requirements, security requirements and
> constraints. Identify assumptions and unresolved requirements before
> recommending architecture.

Topics to decide:

-   What problem does the system solve?
-   Who or what uses it?
-   What are the main workflows and business capabilities?
-   Is it a new system, replacement or integration?
-   What are the expected traffic, throughput and data volumes?
-   Does it need to operate 24/7?
-   What are the availability, recovery and performance expectations?
-   What are the security, privacy, regulatory and audit requirements?
-   Which external systems and teams does it depend on?

## Discussion 2: Technology selection

**Prompt:**

> Based on the requirements and team context, help me choose the
> technology stack. Compare realistic options and explain the
> operational, development, licensing and cost implications. Separate
> technologies required for the first release from technologies that can
> be introduced later.

Topics to decide:

-   Language and runtime/framework.
-   API framework and communication protocols.
-   Database and data access technology.
-   Messaging and event streaming.
-   Object/file storage.
-   Cloud provider and hosting model.
-   Authentication and authorisation.
-   Logging, metrics, tracing and monitoring.
-   Testing frameworks and supporting libraries.
-   Build, source control and CI/CD tooling.
-   Containerisation and orchestration, if required.

## Discussion 3: Solution architecture and project boundaries

**Prompt:**

> Propose an architecture and solution structure based on the
> requirements. Explain the responsibility of each project, the
> direction of dependencies and where interfaces and implementations
> belong. Compare the design with a simpler alternative and avoid
> splitting projects without a clear reason.

Topics to decide:

-   Modular monolith, distributed services or another architecture.
-   API and Worker hosting boundaries.
-   Domain, application/UseCase and infrastructure responsibilities.
-   Shared abstractions and contracts.
-   Persistence and integration boundaries.
-   Project naming and folder conventions.
-   Project-reference rules.
-   Whether separate projects are justified for workers, schedulers,
    integrations or testing.
-   How the architecture can evolve without building speculative
    infrastructure.

Ask the agent to provide a project-reference matrix and explain any
dependency that points inward or creates coupling.

## Discussion 4: Application and domain design

**Prompt:**

> Help define how business logic, application workflows, domain models,
> validation and API contracts should be organised. Discuss the
> trade-offs before recommending patterns. Keep the design
> straightforward and do not introduce a mediator or CQRS framework
> unless the requirements justify it.

Topics to decide:

-   Domain entities, value objects and enums.
-   Business invariants and state transitions.
-   Application services and service grouping.
-   Repository interfaces and implementations.
-   Commands, queries and input models.
-   API Request and Response models.
-   Mapping responsibilities and boundaries.
-   API validation versus business validation.
-   Error handling and exception translation.
-   Direct calls versus mediator/message dispatch patterns.

## Discussion 5: Persistence and data modelling

**Prompt:**

> Help design the persistence boundary. Explain how domain models relate
> to technology-specific records, where mapping happens, and how
> repositories should be structured. Consider consistency, transactions,
> concurrency, indexing, migrations and data lifecycle.

Topics to decide:

-   Repository interfaces and ownership.
-   Domain entities versus persistence records.
-   Mapping location and complexity.
-   SQL, NoSQL or polyglot persistence.
-   Transactions and consistency requirements.
-   Optimistic concurrency and idempotency.
-   Query patterns, indexes and performance.
-   Migration and data seeding strategy.
-   Data retention, backup and recovery.

## Discussion 6: Integrations, messaging and simulation

**Prompt:**

> Help design external integrations and messaging. Keep vendor-specific
> SDKs behind infrastructure implementations and application-facing
> interfaces. Discuss publishing and consuming messages, reliability,
> retries, idempotency, dead-letter handling and how real dependencies
> can be simulated.

Topics to decide:

-   Which project owns external integrations?
-   Which project owns message consumption?
-   Publisher and consumer contracts.
-   Message schemas and versioning.
-   Serialization and deserialization.
-   Retry, timeout and dead-letter policies.
-   Idempotency and duplicate delivery.
-   Real versus simulated implementations.
-   How simulation is selected and configured.
-   Whether a Worker should be independently hosted and deployed.

## Discussion 7: Configuration and feature management

**Prompt:**

> Design the configuration strategy for local development and deployed
> environments. Separate startup/infrastructure configuration from
> operational settings and feature toggles. Identify which settings need
> dynamic refresh and how updates reach all running instances without
> downtime.

Topics to decide:

-   Local Development, SIT, UAT, staging and Production environments.
-   Base and environment-specific configuration.
-   Secret storage and credential management.
-   Startup-only versus dynamic configuration.
-   Central configuration provider.
-   Feature flags for optional capabilities.
-   Simulation settings per dependency.
-   Safe rollout, rollback, auditing and access control.
-   Production safeguards.
-   Multi-instance configuration consistency.

## Discussion 8: Dependency injection and application startup

**Prompt:**

> Propose how dependencies should be registered and how executable
> applications should be composed. Keep startup code readable, make API
> and Worker registration independent, and explain whether registration
> belongs in project extension methods or application-level installer
> classes.

Topics to decide:

-   Composition roots.
-   Installer or registration class naming.
-   Which executable owns each registration.
-   Shared versus application-specific registrations.
-   Lifetime choices: singleton, scoped and transient.
-   Selecting real, simulated or configurable implementations.
-   Configuration binding and validation.
-   Startup validation and failure behaviour.

## Discussion 9: API, Worker and scheduled processing

**Prompt:**

> Define the responsibilities and deployment boundaries of the API,
> message Worker and any scheduled/background processing. Do not assume
> that all background work belongs in one Worker. Explain how each host
> invokes shared application services and how they operate
> independently.

Topics to decide:

-   API responsibilities and endpoint conventions.
-   Worker responsibilities and message acknowledgement.
-   Independent deployment and scaling.
-   Long-running operations.
-   Scheduling requirements.
-   Hangfire, EventBridge Scheduler, Step Functions or other options.
-   Whether a separate scheduler/host is justified.
-   Health checks, graceful shutdown and cancellation.

## Discussion 10: Testing strategy

**Prompt:**

> Propose a testing strategy with a dedicated unit-testing project.
> Explain what should be unit tested, what should be integration tested,
> how dependencies should be substituted, and how tests should be
> organised. Avoid making tests depend on live cloud resources unless
> the test is explicitly an integration test.

Topics to decide:

-   Unit-testing framework.
-   Mocking/substitution library.
-   Dedicated unit-test project structure.
-   Domain tests.
-   UseCase tests.
-   Repository and mapper tests.
-   API tests.
-   Worker/message-consumer tests.
-   Integration and end-to-end tests.
-   Test data builders and fixtures.
-   Local emulators versus cloud test environments.
-   CI test execution and coverage expectations.

## Discussion 11: Coding standards and repository setup

**Prompt:**

> Help establish practical coding standards and repository conventions.
> Focus on consistency, readability and maintainability. Explain the
> benefit of each proposed rule and avoid enforcing stylistic
> preferences that do not add value.

Topics to decide:

-   Target framework and language version.
-   Nullable reference types.
-   Implicit usings.
-   Top-level statements versus explicit Program/Main.
-   Naming conventions.
-   Async method naming.
-   CancellationToken usage.
-   Exception handling and logging.
-   Formatting and `.editorconfig`.
-   Analyzers and warnings.
-   XML documentation requirements.
-   Git branching, commits and pull requests.
-   README and architecture decision records.

## Discussion 12: Deployment, operations and cost

**Prompt:**

> Help plan how the system will be built, deployed, operated and
> monitored. Consider independent application deployment, environment
> promotion, rollback, observability, resilience, security and cloud
> costs. Separate initial requirements from future improvements.

Topics to decide:

-   Hosting services and environments.
-   CI/CD pipeline.
-   Infrastructure as code.
-   Environment configuration and secrets.
-   Health checks and readiness.
-   Structured logs, metrics and distributed tracing.
-   Alerts and operational dashboards.
-   Retry and resilience policies.
-   Scaling and capacity.
-   Backup, recovery and disaster recovery.
-   IAM permissions and least privilege.
-   Cost budgets, alerts and resource cleanup.

## Final Architecture Review Prompt

Once the discussions are complete, use this prompt:

> Review all confirmed project decisions and identify contradictions,
> missing concerns, unnecessary complexity and risks.
>
> Produce:
>
> 1.  A concise architecture overview.
> 2.  The complete solution and folder structure.
> 3.  A responsibility description for every project.
> 4.  A project-reference matrix.
> 5.  The technology stack and package strategy.
> 6.  The configuration and environment strategy.
> 7.  The testing strategy and test-project structure.
> 8.  Coding conventions and repository standards.
> 9.  Security, reliability and operational considerations.
> 10. An implementation plan broken into small, verifiable steps.
> 11. A list of future improvements that are deliberately excluded from
>     the initial implementation.
> 12. An architecture decision log showing each decision, its rationale
>     and any alternatives considered.
>
> Do not silently change agreed decisions. Highlight any recommendation
> that differs from an agreed decision and explain why it should be
> reconsidered. Do not generate implementation code until I approve the
> final architecture.

## Architecture Decision Log Template

Use this table to record decisions during the discussion.

| Topic | Decision | Reason | Alternatives considered | Status |
|---|---|---|---|---|
| Architecture |  |  |  | Proposed / Agreed |
| Technology stack |  |  |  | Proposed / Agreed |
| Project structure |  |  |  | Proposed / Agreed |
| Persistence |  |  |  | Proposed / Agreed |
| Messaging |  |  |  | Proposed / Agreed |
| Configuration |  |  |  | Proposed / Agreed |
| Testing |  |  |  | Proposed / Agreed |
| Deployment |  |  |  | Proposed / Agreed |

------------------------------------------------------------------------

The discussion guide is intentionally reusable. Not every project needs
every topic, and the coding agent should help determine which decisions
are relevant rather than forcing every pattern or technology into the
design.