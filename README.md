
---
# 🎟️ Event Ticketing System

A full-stack ticketing and event analytics platform built with **ASP.NET Core (.NET 8)**, **NHibernate**, **SQLite**, and **React**. It provides API endpoints for event listings and ticket sales analytics, along with a modern frontend UI for displaying and interacting with the data.

---


## ✅ Features

### 🔙 Backend (.NET 8, NHibernate)
- `GET /api/events/{days}`  
  Returns events starting within the next 30, 60, or 180 days.

- `GET /api/events/GetTop5EventsByRevenue`  
  Returns top 5 events based on revenue (total dollar amount).

- `GET /api/events/GetTop5EventsBySalesCount`  
  Returns top 5 events based on ticket quantity sold.

- `GET /api/events/GetTickets/{eventID}`  
  Returns tickets sold for a specific event ID.

- Uses **Dependency Injection** with interfaces:
  - `IEventService`
  - `ITicketSaleService`

- **NHibernate** ORM maps C# models to the SQLite database.
- Unit tests included using **xUnit**.

### 🖥️ Frontend (React)
- Displays event list in a sortable table (by name or start date).
- Sales summary page:
  - Top 5 events by ticket sales
- Responsive layout
- Error handling for API failures
## 🔧 API URL Configuration
The server project should be selected as the startup project
To ensure the React frontend communicates correctly with the backend, make sure the API base URL defined in `src/config.jsx` matches the `applicationUrl` in the backend's `launchSettings.json`:

### 📄 Frontend – `src/config.jsx`
```javascript
// This file contains the configuration for the API base URL used in the application.   
const API_BASE_URL = "https://localhost:7248/api";

const config = {
  apiBaseUrl: API_BASE_URL
};

export default config;
