# 📍 LOCALS : Riyadh Through Local Eyes
[![Framework](https://img.shields.io/badge/Framework-ASP.NET%20Core%2010.0%20MVC-512bd4)](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-red)](https://www.microsoft.com/en-us/sql-server/)
[![ORM](https://img.shields.io/badge/ORM-Entity%20Framework%20Core-blueviolet)](https://learn.microsoft.com/en-us/ef/core/)

``LOCALS`` is an interactive platform designed to enrich the digital tourism content of the **Riyadh Region**. Built on the principle of **Crowdsourced Tourism**, the platform empowers local residents to document hidden gems, heritage sites, and modern attractions that represent the authentic spirit of the Saudi capital.

---

## 🚀 Key Features

* **Interactive Mapping:** A live map engine powered by **Leaflet.js** that renders landmarks dynamically using real-time geographic coordinates.
* **Contributor Workflow:** A dedicated lifecycle allowing users to apply for "Local Expert" status to contribute verified locations.
* **Centralized Admin Dashboard:** A high-contrast management hub for reviewing pending landmarks and approving user role upgrades.
* **Advanced Media Handling:** Multi-image upload support with physical file processing on the server and dynamic gallery rendering.
* **Zero-Latency Filtering:** Instant client-side search and filtering by District or Category for a seamless user experience.

---

## 🏗 System Architecture

The project follows the **MVC (Model-View-Controller)** pattern to ensure a clean **Separation of Concerns**:

* **Models:** Strongly typed data structures representing Riyadh's landmarks, users, and media galleries.
* **Views:** Responsive Razor templates with a localized RTL (Right-to-Left) layout and a "National Green" visual identity.
* **Controllers:** Logical engines handling data processing, geographic coordinate mapping, and secure authentication.

---

## 🛠 Technical Specifications

| Layer | Implementation Details |
| :--- | :--- |
| **Authentication** | Secure custom identity management via **HttpContext Sessions**. |
| **Security** | One-way cryptographic password hashing using **BCrypt.Net**. |
| **Storage Strategy** | Physical server file-system storage with dynamic Database path-mapping. |
| **Front-end Performance** | **Client-side DOM Manipulation** for zero-latency data filtering (JavaScript ES6). |
| **UI/UX Identity** | RTL-optimized layout featuring the **Tajawal** typography and a "National Green" design system. |

---

## 🔐 Database Engineering (ERD)
The relational schema is optimized for **Referential Integrity** and normalized to 3NF:
- **Places Table:** The central entity node, linked via Foreign Keys to `Districts` and `Categories`.
- **Media Gallery:** Implements a **1:N (One-to-Many)** relationship via the `PlaceImages` table to handle polymorphic media assets.
- **Workflow State Management:** A dedicated `UpgradeRequests` table manages user role transitions, storing proof of expertise and proposed local landmarks.

---

## 🔐 Security & Data Integrity

* **Secure Hashing:** Industry-standard **BCrypt** hashing is used to protect user passwords (no plain-text storage).
* **Authorization:** Role-Based Access Control (RBAC) implemented via Sessions to protect administrative actions.
* **Integrity:** Strict Foreign Key constraints between Places, Districts, and Categories to prevent orphan data records.

---

## ⚙️ Installation & Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/YourUsername/iLOCALS.git](https://github.com/YourUsername/iLOCALS.git)
    ```
2.  **Configure Database:**
    Update the `DefaultConnection` string in `appsettings.json` with your SQL Server credentials.
3.  **Apply Migrations:**
    ```bash
    dotnet ef database update
    ```
4.  **Run the Application:**
    ```bash
    dotnet run
    ```

---

## SA Vision 2030 Alignment
This project leverages modern software engineering to support the digital transformation of the Saudi tourism sector, showcasing Riyadh as a global destination in alignment with **Saudi Vision 2030**.

---

**Developer:** [Nusaybah Altrbaolsi]  
**Project:** Graduation Project from Tuwaiq Academy / Portfolio  
