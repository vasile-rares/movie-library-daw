# 🎬 Movie Library - DAW Project

![React](https://img.shields.io/badge/React-18.0-61DAFB?style=flat&logo=react&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)

A modern full-stack web application for managing and rating movies and TV shows. Built with React and ASP.NET Core, featuring a Netflix-inspired UI with comprehensive user management, personalized watchlists, and an admin panel.

## ✨ Features

### User Features

-   🔐 **Authentication & Authorization** - JWT-based authentication with HttpOnly cookies
-   🎯 **Browse Content** - Separate pages for movies and TV shows with genre filtering
-   ⭐ **Rating System** - Rate content from 1-10 with optional comments
-   📝 **Personal Watchlist** - Add titles to your list with custom watch status:
    -   Plan to Watch
    -   Watching
    -   Completed
    -   On Hold
    -   Dropped
-   🔍 **Detailed Views** - View comprehensive information including ratings, genres, and user reviews
-   👤 **User Profile** - Upload profile pictures and manage account settings
-   📱 **Responsive Design** - Fully optimized for mobile, tablet, and desktop

### Admin Features

-   ➕ **Content Management** - Add, edit, and delete movies and TV shows
-   🏷️ **Genre Management** - Create and manage genre categories
-   👥 **User Management** - View and manage user accounts

## 🛠️ Tech Stack

### Frontend

-   **React 18** - Modern UI library with hooks
-   **React Router v6** - Client-side routing
-   **Axios** - HTTP client for API calls
-   **Vite** - Fast build tool and dev server
-   **CSS3** - Custom styling with CSS variables

### Backend

-   **ASP.NET Core 8.0** - RESTful API
-   **Entity Framework Core** - ORM for database operations
-   **SQL Server** - Relational database
-   **BCrypt.NET** - Password hashing
-   **JWT Bearer** - Token-based authentication
-   **AutoMapper** - Object mapping
-   **Swagger/OpenAPI** - API documentation

### Security Features

-   HTTPS support
-   XSS protection middleware
-   CORS configuration
-   Rate limiting
-   HttpOnly cookies for token storage
-   Password hashing with BCrypt
-   Input validation and sanitization

## 📁 Project Structure

```
movie-library-daw/
├── Backend/
│   └── MovieLibrary.API/
│       ├── Controllers/        # API endpoints
│       ├── Services/          # Business logic
│       ├── Repositories/      # Data access layer
│       ├── Models/            # Entity models
│       ├── DTOs/              # Data transfer objects
│       ├── Data/              # DbContext & seeder
│       ├── Middlewares/       # Custom middleware
│       ├── Mappings/          # AutoMapper profiles
│       └── Interfaces/        # Service & repository interfaces
│
└── Frontend/
    └── src/
        ├── components/        # Reusable UI components
        ├── guards/           # Route guards (ProtectedRoute, AdminRoute)
        ├── pages/            # Page components
        ├── services/         # API service layer
        ├── context/          # React context (Auth)
        └── assets/           # Static assets

```

## 🚀 Getting Started

### Prerequisites

-   Node.js 18+ and npm
-   .NET 8.0 SDK
-   SQL Server (LocalDB or Express)

### Backend Setup

1. Navigate to the Backend directory:

```bash
cd Backend/MovieLibrary.API
```

2. Update the connection string in `appsettings.json`:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MovieLibraryDb;Trusted_Connection=true;TrustServerCertificate=true"
    }
}
```

3. Apply database migrations:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run
```

The API will be available at:

-   HTTPS: `https://localhost:7002`
-   HTTP: `http://localhost:5002`
-   Swagger UI: `https://localhost:7002/swagger`

### Frontend Setup

1. Navigate to the Frontend directory:

```bash
cd Frontend
```

2. Install dependencies:

```bash
npm install
```

3. Start the development server:

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## 🔑 Default Credentials

The database is automatically seeded with sample data:

**Admin Account:**

-   Email: `admin@movielibrary.com`
-   Password: `password123`

**User Accounts:**

-   Email: `john.doe@example.com` | Password: `password123`
-   Email: `jane@example.com` | Password: `password123`

## 📡 API Endpoints

### Authentication

-   `POST /api/auth/register` - Register new user
-   `POST /api/auth/login` - Login user
-   `POST /api/auth/logout` - Logout user

### Titles (Movies/Shows)

-   `GET /api/titles` - Get all titles
-   `GET /api/titles/{id}` - Get title by ID
-   `GET /api/titles/type/{type}` - Get titles by type (0=Movie, 1=Series)
-   `GET /api/titles/genre/{genreId}` - Get titles by genre
-   `POST /api/titles` - Create title (Admin only)
-   `PUT /api/titles/{id}` - Update title (Admin only)
-   `DELETE /api/titles/{id}` - Delete title (Admin only)

### Ratings

-   `GET /api/ratings` - Get all ratings
-   `GET /api/ratings/title/{titleId}` - Get ratings for a title
-   `POST /api/ratings` - Create rating
-   `PUT /api/ratings/{id}` - Update rating
-   `DELETE /api/ratings/{id}` - Delete rating

### My List

-   `GET /api/mylist` - Get user's list
-   `POST /api/mylist` - Add to list
-   `PUT /api/mylist/{id}` - Update status
-   `DELETE /api/mylist/{id}` - Remove from list

### Genres

-   `GET /api/genres` - Get all genres
-   `POST /api/genres` - Create genre (Admin only)

### Users

-   `GET /api/users/me` - Get current user
-   `PUT /api/users/me` - Update current user
-   `POST /api/users/me/profile-picture` - Upload profile picture

## 🔒 Security

-   **HTTPS** - Secure communication
-   **JWT Tokens** - Stored in HttpOnly cookies
-   **Password Hashing** - BCrypt with salt
-   **XSS Protection** - Custom middleware
-   **Rate Limiting** - API throttling
-   **CORS** - Configured for frontend origin
-   **Input Validation** - Server-side validation

## 📝 License

MIT License - see the [LICENSE](LICENSE) file for details.

This software is provided "as is", without warranty of any kind, express or implied. Feel free to use, modify, and distribute this project for personal or commercial purposes.