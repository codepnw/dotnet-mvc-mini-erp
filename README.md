# Mini ERP

A personal ERP system built with ASP.NET Core MVC and Entity Framework Core, focusing on backend development, business workflows, authentication, and maintainable application architecture.

---

## Features

### Authentication & Authorization
* Cookie-based Authentication
* Role-based Authorization (Admin, Employee)
* Claims-based User Identity
* Protected Controller Actions using Authorize Attributes

### Employee Management
* Employee CRUD
* Department Assignment
* Soft Delete Support
* Search and Pagination

### Department Management
* Department CRUD
* Soft Delete Support
* Search and Pagination

### Product Management
* Product CRUD
* Product Category Management
* Search and Pagination

### Inventory Management
* Product Stock Tracking
* Stock Adjustment Operations

### Leave Request Workflow
* Create Leave Requests
* Leave Type Management
* Leave Status Management (Pending, Approved, Rejected)
* Date Validation and Overlap Validation

### Order Management
* Order CRUD
* Product Selection
* Business Validation

---

## Technical Highlights

* ASP.NET Core MVC (.NET 10)
* Entity Framework Core
* SQL Server
* Service Layer Architecture
* Result Pattern for Error Handling
* DTO Pattern
* Soft Delete Implementation
* Pagination and Search Support
* Claims-based Authentication
* Role-based Authorization

---

## Architecture

```text
Controllers
    ↓
Services
    ↓
Entity Framework Core
    ↓
SQL Server
```
---

## Project Status

Active Development

Core ERP modules, authentication, authorization, search, and pagination have been implemented.

Future improvements include UI enhancements, automated testing, and additional business workflows.