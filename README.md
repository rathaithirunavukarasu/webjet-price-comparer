
# 🎬 Webjet Price Comparer

Welcome to **Webjet Price Comparer**! This is a full-stack web application that lets you compare movie prices from multiple providers (Cinemaworld and Filmworld).

---

## 🛠️ Technologies Used

- **client**: React + TypeScript
- **api**: .NET 8 Web API
- **Cache**: Redis
- **Containerization**: Docker + Docker Compose

---

## 🗂 Project Structure

Here’s how the project is organized:

```plaintext
webjet-price-comparer/
├── api/                        # .NET API
│   └── WebjetPriceComparer/
├── client/                       # React UI
│   └── movie-price-comparer/
├── docker-compose.yml              # Multi-container orchestration
├── README.md                       # This file
```

---

## 🚀 Running the Project Locally

To run **Webjet Price Comparer** on your local machine, follow these instructions:

### 1. Set Up Configuration

In the **`appsettings.json`** file, update it with the **Webjet API token** and base URL. Replace the placeholders with your actual values:

```json
{
  "WebjetApi": {
    "BaseUrl": "https://webjetapi.example.com/",
    "ApiToken": "YOUR_ACTUAL_API_TOKEN_HERE"
  }
}
```

### 2. Install Prerequisites

Before you begin, make sure you have the following tools installed on your system:

- **Docker**: [Install Docker](https://www.docker.com/get-started)
- **Docker Compose**: [Install Docker Compose](https://docs.docker.com/compose/install/)

### 3. Running the Project

After setting up the configuration, follow these steps to run the project locally:

1. **Stop any existing containers** (if running):

   ```bash
   docker-compose down --volumes --remove-orphans
   ```

2. **Build the containers**:

   ```bash
   docker-compose build
   ```

3. **Start the containers**:

   ```bash
   docker-compose up
   ```

This will launch both the api (**.NET Web API**) and the client (**React app**) in separate containers. The application will be available on your local machine.

---

## 💡 Features

- **Movie Price Comparison**: Compare movie prices across different providers.
- **Dynamic Data**: Fetches real-time pricing and movie details from Cinemaworld and Filmworld.
- **Caching**: Optimized using Redis to store frequently accessed data.
- **Containerized**: The entire application is containerized for easy deployment using Docker and Docker Compose.

---

Now you’re all set! 🎬 Enjoy comparing movie prices with **Webjet Price Comparer**!

---

## 🔧 Prerequisites

Before running the project, ensure you have the following tools installed:

- **Docker** (for containerization)
- **Docker Compose** (for managing multi-container setups)
