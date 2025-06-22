# PharmaRep

**PharmaRep** is a modular-monolith ASP.NET web API for medical representatives. It helps streamline:

- **Appointment booking**: Medical reps can propose visits; doctors & midwives can view nearby reps and confirm appointments.
- **Onboarding hub**: New reps get key documents and links to get started quickly.

---

## Architecture & Modules

PharmaRep is designed as a **modular monolith** using Domain-Driven Design (DDD) principles:

- **Identity** – handles registration, login, and user CRUD.
- **Appointments** – CRUD operations for scheduling visits.
- *(Future modules: e.g. Documents, ...)*

Modules encapsulate their own models, commands, queries, handlers, database mappings, and API routes—all within the same ASP.NET process.

---

## Tech Stack

- **ASP.NET**
- **Entity Framework Core** with SQL Server
- **Custom dispatcher** for CQRS via commands & queries
- **Modular monolith**: Isolated module boundaries

---

## Quick Start

1. Clone:
   ```bash
   git clone https://github.com/sppat/PharmaRep.git
   cd PharmaRep
