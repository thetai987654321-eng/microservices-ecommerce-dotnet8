🚀 Microservices E-Commerce Core
This repository features a modern, distributed backend system designed to simulate a real-world e-commerce environment. By leveraging a microservices architecture, the project ensures high availability, service independence, and a streamlined deployment process.

🏗️ System Architecture
The project is built on the principle of decoupling business logic into specialized services:

Identity Service: A dedicated security layer using ASP.NET Core Identity. It handles user registration and issues JWT Bearer tokens for secure access. Data is persisted via a lightweight SQLite database.

Product Service: Manages the product catalog and inventory. It uses MongoDB to provide a flexible schema and high-speed data processing for modern e-commerce requirements.

API Gateway (Ocelot): Acts as the entry point for the entire system. It simplifies client interaction by routing requests to the appropriate internal services while maintaining security and abstraction.

Containerization: The entire ecosystem is orchestrated using Docker Compose, allowing for consistent environments from development to production.

🛠️ Tech Stack
Core Framework

.NET 8.0 (LTS) for all backend services.

API Management

Ocelot Gateway for centralized routing and request management.

Data Storage

MongoDB for the Product catalog (NoSQL).

SQLite for Identity management (Relational).

Security & DevOps

JWT (JSON Web Tokens) for cross-service authentication.

Docker & Docker Compose for orchestration.

🚀 Getting Started
Prerequisites
Make sure you have Docker Desktop installed and running on your machine.

Installation
Clone this repository to your local drive.

Open your terminal in the project root directory.

Run the following command to build and start the containers:
docker compose up --build

📡 API Endpoints (Gateway Port: 8000)
All interactions should go through the Gateway URL: http://localhost:8000

Authentication Flow

Register: POST /auth/register (Public)

Login: POST /auth/login (Public)

Product Management

Get All Products: GET /products (Public)

Create Product: POST /products (Requires Valid JWT Token)

Delete Product: DELETE /products/{id} (Requires Valid JWT Token)

✨ Key Technical Highlights
Database-per-Service Pattern: Strict isolation of data ensures that a failure in one service does not affect the data integrity of another.

Unified Entry Point: Frontend applications only need to communicate with one port (8000), reducing CORS issues and simplifying client-side logic.

Infrastructure as Code: The docker-compose file defines the entire infrastructure, making the project highly portable.

JWT Pass-through: The Gateway intelligently forwards authentication headers to downstream services for verification.

👤 Author
Duong Van The Tai
Information Technology Student
Focus: Backend Architecture & Distributed Systems

This project is a demonstration of modern backend engineering practices. If you find this architecture helpful, please consider giving it a ⭐!