# 💸 Personal Expense Management Web App

A full-stack expense management application built with **ASP.NET Core 8** and **React 18** featuring comprehensive financial tracking, budgeting, subscriptions, and reporting capabilities.

## 🌟 Features

### Core Functionality
- ✅ **Authentication & Authorization** - JWT-based secure authentication with refresh tokens
- 📊 **Dashboard** - Visual overview with charts showing income, expenses, and monthly trends
- 💰 **Expense Management** - CRUD operations with categories, tags, and optional image attachments
- 📈 **Income Tracking** - Record and categorize income from multiple sources
- 🔄 **Subscription Management** - Track recurring subscriptions with automated reminders
- 🎯 **Budget Planning** - Set monthly/quarterly/yearly budgets with real-time progress tracking
- 📑 **Reports & Analytics** - Export data to PDF/Excel with detailed insights
- 🔁 **Recurring Transactions** - Automated creation of repeating expenses/incomes
- 🔔 **Notifications** - Email and in-app notifications for budget alerts and subscriptions
- 🌍 **Multi-Currency Support** - Support for VND, USD, EUR, GBP with conversion
- 🌙 **Dark Mode** - Beautiful dark/light theme toggle

## 🏗️ Architecture

### Backend
- **Clean Architecture** (Domain, Application, Infrastructure, WebAPI)
- **ASP.NET Core 8** with Entity Framework Core
- **MSSQL Database** with code-first migrations
- **ASP.NET Core Identity** for user management
- **JWT Authentication** with secure token refresh
- **AutoMapper** for DTO mapping
- **FluentValidation** for input validation
- **Hangfire** for background jobs (reminders, recurring transactions)
- **QuestPDF** for PDF generation
- **ClosedXML** for Excel export
- **SendGrid** for email notifications (with SMTP fallback)
- **Serilog** for structured logging

### Frontend
- **React 18** with **TypeScript**
- **Vite** for fast development and building
- **Redux Toolkit** for state management
- **RTK Query** for API calls with caching
- **TailwindCSS** for styling
- **Recharts** for data visualization
- **React Router v6** for navigation
- **Axios** for HTTP requests

## 📁 Project Structure

```
expense-management/
├── src/
│   ├── ExpenseManagement.Domain/          # Core entities and domain logic
│   ├── ExpenseManagement.Application/      # Business logic, DTOs, interfaces
│   ├── ExpenseManagement.Infrastructure/   # Data access, services, identity
│   └── ExpenseManagement.WebAPI/           # Controllers, middleware
├── expense-management-ui/                   # React frontend
│   ├── src/
│   │   ├── features/                       # Feature-based modules
│   │   ├── components/                     # Reusable components
│   │   ├── app/                            # Redux store
│   │   └── utils/                          # Utilities
└── docker-compose.yml                       # Docker orchestration
```

## 🚀 Getting Started

### Prerequisites
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 20+** - [Download](https://nodejs.org/)
- **SQL Server** or **Docker** for database
- **Visual Studio 2022** or **VS Code** (optional)

### Option 1: Run with Docker (Recommended)

1. **Clone the repository**
```bash
git clone <repository-url>
cd expense-management
```

2. **Start all services with Docker Compose**
```bash
docker-compose up -d
```

3. **Access the application**
- Frontend: http://localhost:3000
- Backend API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- Hangfire Dashboard: http://localhost:5000/hangfire

4. **Demo credentials**
```
Email: demo@expensemanager.com
Password: Demo@123
```

### Option 2: Run Locally

#### Backend Setup

1. **Navigate to the backend directory**
```bash
cd expense-management
```

2. **Update connection string**
Edit `src/ExpenseManagement.WebAPI/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ExpenseManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

3. **Run database migrations**
```bash
dotnet ef database update --project src/ExpenseManagement.Infrastructure --startup-project src/ExpenseManagement.WebAPI
```

4. **Run the backend**
```bash
cd src/ExpenseManagement.WebAPI
dotnet run
```

Backend will start at: http://localhost:5000

#### Frontend Setup

1. **Navigate to frontend directory**
```bash
cd expense-management-ui
```

2. **Install dependencies**
```bash
npm install
```

3. **Create environment file**
```bash
cp .env.example .env
```

Edit `.env`:
```
VITE_API_URL=http://localhost:5000/api
```

4. **Run the frontend**
```bash
npm run dev
```

Frontend will start at: http://localhost:3000

## 🎯 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh-token` - Refresh access token
- `POST /api/auth/logout` - Logout

### Dashboard
- `GET /api/dashboard` - Get dashboard data with charts and statistics

### Expenses
- `GET /api/expenses` - Get paginated expenses with filters
- `POST /api/expenses` - Create expense
- `PUT /api/expenses/{id}` - Update expense
- `DELETE /api/expenses/{id}` - Delete expense

### Budgets
- `POST /api/budgets` - Create budget
- Similar CRUD endpoints available

### Subscriptions
- `POST /api/subscriptions` - Create subscription
- Similar CRUD endpoints available

## 🔧 Configuration

### Backend Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ExpenseManagementDb;..."
  },
  "Jwt": {
    "Secret": "YourSecretKey",
    "Issuer": "ExpenseManagementAPI",
    "Audience": "ExpenseManagementClient",
    "ExpiryMinutes": "60"
  },
  "SendGrid": {
    "ApiKey": "your-sendgrid-key",
    "FromEmail": "noreply@expensemanager.com"
  }
}
```

### Frontend Configuration (.env)

```
VITE_API_URL=http://localhost:5000/api
```

## 📊 Background Jobs

The application uses **Hangfire** for automated tasks:

- **Subscription Reminders** - Runs daily at 9 AM
- **Recurring Transactions** - Runs daily at midnight
- **Budget Alerts** - Triggered when budget thresholds are exceeded

Access Hangfire Dashboard: http://localhost:5000/hangfire

## 🎨 UI Features

- **Responsive Design** - Works on desktop, tablet, and mobile
- **Dark Mode** - Toggle between light and dark themes
- **Modern UI** - Clean, professional design with TailwindCSS
- **Interactive Charts** - Line charts, pie charts with Recharts
- **Real-time Updates** - RTK Query auto-refetching
- **Loading States** - Smooth loading indicators
- **Error Handling** - User-friendly error messages

## 🧪 Testing

### Run Backend Tests
```bash
dotnet test
```

### Run Frontend Tests
```bash
cd expense-management-ui
npm test
```

## 📦 Database Migrations

### Create a new migration
```bash
dotnet ef migrations add <MigrationName> --project src/ExpenseManagement.Infrastructure --startup-project src/ExpenseManagement.WebAPI
```

### Apply migrations
```bash
dotnet ef database update --project src/ExpenseManagement.Infrastructure --startup-project src/ExpenseManagement.WebAPI
```

### Remove last migration
```bash
dotnet ef migrations remove --project src/ExpenseManagement.Infrastructure --startup-project src/ExpenseManagement.WebAPI
```

## 🐳 Docker Commands

### Build and start all services
```bash
docker-compose up -d
```

### Stop all services
```bash
docker-compose down
```

### View logs
```bash
docker-compose logs -f
```

### Rebuild services
```bash
docker-compose up -d --build
```

## 📝 Default Data

The application seeds the following data on first run:

- **Demo User**: demo@expensemanager.com / Demo@123
- **Currencies**: VND (default), USD, EUR, GBP
- **Expense Categories**: Food & Dining, Transportation, Shopping, Entertainment, Bills, Healthcare, Education
- **Income Categories**: Salary, Freelance, Investment, Other Income
- **Sample Transactions**: A few sample expenses and income records

## 🔐 Security Features

- Password hashing with ASP.NET Core Identity
- JWT tokens with expiration
- Refresh token rotation
- HTTPS enforcement
- CORS configuration
- SQL injection prevention via EF Core
- Input validation with FluentValidation
- XSS protection

## 🌐 Deployment

### Deploy to Azure
1. Create Azure SQL Database
2. Deploy backend to Azure App Service
3. Deploy frontend to Azure Static Web Apps or App Service
4. Update connection strings and environment variables

### Deploy to AWS
1. Create RDS SQL Server instance
2. Deploy backend to Elastic Beanstalk or ECS
3. Deploy frontend to S3 + CloudFront or Amplify
4. Update configuration

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👨‍💻 Author

Created with ❤️ by [Your Name]

## 📧 Support

For issues and questions:
- Create an issue on GitHub
- Email: support@expensemanager.com

## 🙏 Acknowledgments

- ASP.NET Core team
- React team
- TailwindCSS team
- All open-source contributors

---

**Happy expense tracking! 💰📊**
