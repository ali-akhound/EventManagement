# Caching.md

# 🎟️ Ticketing System Design & Caching Strategy

## System Design Overview

To handle thousands of concurrent users buying tickets on a high-traffic event platform, I propose a **Microservice Architecture** deployed in a cloud environment.

### Key Components:

- **Microservices:**
  - **Payment Service**
  - **Notification Service**
  - **Event Service**
  - **User & Authentication Service**

Each microservice is implemented as an independent application with its own database (SQL or NoSQL), enabling **database per service** design to improve modularity and scalability.

### Cloud Infrastructure:

- **Kubernetes** is used to orchestrate and manage microservice instances, providing:
  - Automated scaling based on load
  - Self-healing and fault tolerance
  - Rolling updates with zero downtime

- **API Gateway** layer acts as a single entry point that isolates microservices from direct external access, enhancing security and access control.

- **Message Queue (e.g., Azure Service Bus, AWS SQS, Kafka):**
  - Facilitates asynchronous communication between services
  - Ensures loose coupling and reliability
  - Supports event-driven workflows (e.g., ticket purchase triggers payment and notification)

- **Databases:**
  - Each service manages its own database instance (SQL or NoSQL), suited to its data model and access patterns.
  - Cloud-managed databases with automated scaling and backups.

---

## Fault Tolerance & Scalability

- Kubernetes orchestrates containerized microservices to **scale horizontally** based on demand.
- Each service can scale independently, e.g., Ticket Service scales during high traffic.
- Use of **circuit breakers**, **retry policies**, and **timeouts** prevents cascading failures.
- Services deployed across multiple availability zones to avoid single points of failure.
- Message queues ensure asynchronous processing and decouple service dependencies.

---

# 🎯 Caching Strategy: Redis with Cache-Aside (Lazy Loading)

## 🧠 Why Cache-Aside?
The **Cache-Aside (Lazy Loading)** strategy improves read performance while keeping cache memory lean and ideal for read-heavy workloads. It works well when:

- Reads are more frequent than writes (e.g., browsing event/ticket data)
- Cache doesn't need to hold everything at all times
- Data changes frequently enough to need a fresh fetch occasionally

---

## Architecture Diagram

```mermaid
graph TD
    Client -->|API Requests| APIGateway
    APIGateway --> PaymentService
    APIGateway --> NotificationService
    APIGateway --> EventService
    APIGateway --> UserAuthService

    PaymentService -->|Writes/Reads| PaymentDB[(SQL/NoSQL DB)]
    NotificationService --> NotificationDB[(SQL/NoSQL DB)]
    EventService --> EventDB[(SQL/NoSQL DB)]
    UserAuthService --> UserDB[(SQL/NoSQL DB)]

    PaymentService --> MessageQueue[(Message Queue)]
    NotificationService --> MessageQueue
    EventService --> MessageQueue

    AllServices -.-> RedisCache[(Redis Cache)]

    Kubernetes[("Kubernetes Cluster")] --> APIGateway
    Kubernetes --> PaymentService
    Kubernetes --> NotificationService
    Kubernetes --> EventService
    Kubernetes --> UserAuthService
