# 🛒 E-Commerce API

**Scalable, Secure, and Cloud-Ready REST API for E-Commerce Platforms**  
Developed with **ASP.NET Core 7**, powered by **Entity Framework Core**, containerized via **Docker**, and deployable to **Azure**.

---

## 📌 Overview

This project is a modern **RESTful Web API** designed to support the core features of an e-commerce platform, including authentication, product management, order processing, and customer handling.

It provides full **CRUD** support for:

- 🛍️ Products  
- 📁 Categories  
- 🧑 Customers  
- 🧾 Orders  
- 💳 Payments  
- 🔐 JWT Authentication

---

## 🚀 Features

- ✅ Built with ASP.NET Core 7.0
- 🔐 JWT Authentication & Authorization
- 🗃️ Entity Framework Core with SQL Server / SQLite
- 🖼️ Image Upload Support
- 📘 Swagger UI for testing
- 🐳 Docker containerization (CI/CD-ready)
- ☁️ Azure Cloud Deployment support

---

## 🔐 Authentication Endpoints

| Method | Endpoint             | Description           |
|--------|----------------------|-----------------------|
| POST   | `/api/user/register` | User Registration     |
| POST   | `/api/user/login`    | Login with JWT Token  |

---

## 📁 API Endpoints Overview

| Resource    | Methods                              |
|-------------|--------------------------------------|
| Products    | GET, POST, PUT, DELETE               |
| Categories  | GET, POST, PUT, DELETE               |
| Customers   | GET, POST, PUT, DELETE               |
| Orders      | GET, POST, PUT, DELETE               |
| Payments    | POST (simulate payment, extendable)  |

Detailed API documentation is available at:  
🔗 `http://localhost:5000/swagger`

---

## 🛠 Tech Stack

| Layer     | Technology               |
|-----------|--------------------------|
| Backend   | ASP.NET Core 7.0         |
| Database  | SQL Server / SQLite      |
| Auth      | JWT (Bearer Token)       |
| Docs      | Swagger/OpenAPI          |
| Hosting   | Azure App Service        |
| DevOps    | Docker                   |

---

## ⚙ Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- (Optional) Azure account for cloud deployment
- Azure Data Studio or SSMS for DB management (optional)

---

## 🧪 Run Locally

```bash
# Clone the repo
git clone https://github.com/theaxmedovv/E-Commerce-API.git
cd E-Commerce-API

# Restore packages
dotnet restore

# Run the project
dotnet run
