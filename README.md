# 📍 LOCALS : Riyadh Through Local Eyes

``LOCALS`` is an interactive platform designed to enrich the digital tourism content of the **Riyadh Region**. Built on the principle of **Crowdsourced Tourism**, the platform empowers local residents to document hidden gems, heritage sites, and modern attractions that represent the authentic spirit of the Saudi capital.

---

## 🚀 Key Features

* **Interactive Mapping:** A live map engine powered by **Leaflet.js** that renders landmarks dynamically using real-time geographic coordinates.
* **Contributor Workflow:** A dedicated lifecycle allowing users to apply for "Local Expert" status to contribute verified locations.
* **Centralized Admin Dashboard:** A high-contrast management hub for reviewing pending landmarks and approving user role upgrades.
* **Advanced Media Handling:** Multi-image upload support with physical file processing on the server and dynamic gallery rendering.
* **Zero-Latency Filtering:** Instant client-side search and filtering by District or Category for a seamless user experience.

---

## 🛠 Technical Stack

| Category | Technology |
| :--- | :--- |
| **Backend** | ASP.NET Core 10.0 (MVC Architecture) |
| **Database** | SQL Server (Relational) |
| **ORM** | Entity Framework Core |
| **Frontend** | Razor Pages, JavaScript (ES6+), Bootstrap 5 |
| **Maps** | Leaflet.js API |
| **Security** | BCrypt.Net (Hashing) & Session-based RBAC |
| **Design** | CSS3 Grid/Flexbox, Animate.css, Tajawal Fonts |

---

## 🏗 System Architecture

The project follows the **MVC (Model-View-Controller)** pattern to ensure a clean **Separation of Concerns**:

* **Models:** Strongly typed data structures representing Riyadh's landmarks, users, and media galleries.
* **Views:** Responsive Razor templates with a localized RTL (Right-to-Left) layout and a "National Green" visual identity.
* **Controllers:** Logical engines handling data processing, geographic coordinate mapping, and secure authentication.

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
