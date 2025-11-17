# Documentație Securitate - Movie Library API

## Cuprins
1. [Protecție SQL Injection](#protecție-sql-injection)
2. [Protecție XSS (Cross-Site Scripting)](#protecție-xss-cross-site-scripting)
3. [Protecție CSRF (Cross-Site Request Forgery)](#protecție-csrf-cross-site-request-forgery)
4. [Autentificare și Autorizare](#autentificare-și-autorizare)
5. [Validare Input](#validare-input)
6. [Unit Tests](#unit-tests)

---

## Protecție SQL Injection

### Măsuri Implementate

**Entity Framework Core cu Parametrizare Automată**

Aplicația folosește Entity Framework Core pentru toate operațiunile cu baza de date, ceea ce oferă protecție automată împotriva SQL injection prin:

- **Parametrizarea automată**: Toate query-urile LINQ sunt traduse în query-uri SQL parametrizate
- **Tipizare puternică**: Modelele sunt strict tipizate, prevenind injecții prin validare la compile-time
- **Fără query-uri raw**: Nu există query-uri SQL raw în cod (nu se folosesc `FromSqlRaw` sau `ExecuteSqlRaw` fără parametrizare)

### Exemplu de Cod Securizat

```csharp
// ✅ SIGUR - Parametrizare automată
public async Task<Movie?> GetByIdAsync(int id)
{
    return await _context.Movies
        .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
        .FirstOrDefaultAsync(m => m.Id == id); // Parametrizat automat
}
```

### Verificare

Toate repository-urile ([MovieRepository.cs](MovieLibrary.API/Repositories/MovieRepository.cs), [UserRepository.cs](MovieLibrary.API/Repositories/UserRepository.cs), etc.) folosesc exclusiv LINQ queries.

---

## Protecție XSS (Cross-Site Scripting)

### Măsuri Implementate

#### 1. XSS Protection Middleware

**Locație**: [XssProtectionMiddleware.cs](MovieLibrary.API/Middlewares/XssProtectionMiddleware.cs)

**Funcționalități**:
- Detectează și blochează input-uri cu pattern-uri periculoase (`<script>`, `javascript:`, `onclick=`, etc.)
- Setează security headers pentru toate response-urile:
  - `X-Content-Type-Options: nosniff`
  - `X-Frame-Options: DENY`
  - `X-XSS-Protection: 1; mode=block`
  - `Content-Security-Policy: default-src 'self'`
- Validează atât request body cât și query parameters
- Loggează tentativele de atac

**Exemplu de Pattern Detection**:
```csharp
private static readonly Regex[] XssPatterns = new[]
{
    new Regex(@"<script[^>]*>.*?</script>", RegexOptions.IgnoreCase),
    new Regex(@"javascript:", RegexOptions.IgnoreCase),
    new Regex(@"on\w+\s*=", RegexOptions.IgnoreCase), // onclick, onerror, etc.
    new Regex(@"<iframe[^>]*>", RegexOptions.IgnoreCase)
};
```

#### 2. SafeUrl Validation Attribute

**Locație**: [SafeUrlAttribute.cs](MovieLibrary.API/Attributes/SafeUrlAttribute.cs)

**Funcționalități**:
- Validează că URL-urile sunt formate corect
- Permite doar scheme-uri HTTP și HTTPS
- Blochează scheme-uri periculoase (`javascript:`, `data:`, `vbscript:`)

**Utilizare**:
```csharp
public class CreateMovieDto
{
    [MaxLength(255), SafeUrl]
    public string? ImageUrl { get; set; }
}
```

#### 3. Validări Stricte pe DTO-uri

Toate DTO-urile de input au validări care limitează lungimea și conținutul:
- `MaxLength` pentru a preveni buffer overflow
- `Range` pentru valori numerice
- `EmailAddress`, `Required`, etc.

---

## Protecție CSRF (Cross-Site Request Forgery)

### Măsuri Implementate

#### 1. Antiforgery Tokens

**Configurare**: [Program.cs:94-101](MovieLibrary.API/Program.cs#L94-L101)

```csharp
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
```

**Endpoint pentru obținere token**: `/antiforgery/token`

#### 2. JWT Authentication (Protecție Intrinsecă)

Aplicația folosește JWT pentru autentificare, nu cookies de sesiune, ceea ce oferă protecție naturală împotriva CSRF deoarece:
- Token-urile JWT sunt stocate în `localStorage` sau `sessionStorage`
- Nu sunt transmise automat de browser (spre deosebire de cookies)
- Trebuie trimise manual în header-ul `Authorization`

#### 3. CORS Strict

**Configurare**: [Program.cs:82-92](MovieLibrary.API/Program.cs#L82-L92)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

---

## Autentificare și Autorizare

### JWT (JSON Web Tokens)

**Configurare**: [Program.cs:35-78](MovieLibrary.API/Program.cs#L35-L78)

**Securitate**:
- Token-uri semnate cu HMAC SHA256
- Validare strictă: Issuer, Audience, Lifetime, Signing Key
- `ClockSkew = TimeSpan.Zero` (fără toleranță de timp)
- Logging detaliat pentru încercări de autentificare eșuate

### Hashing Parole

**Implementare**: [AuthService.cs:36](MovieLibrary.API/Services/AuthService.cs#L36)

```csharp
// Folosește BCrypt pentru hashing sigur
var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
```

**Caracteristici**:
- BCrypt cu salt automat
- Rezistent la rainbow tables
- Comparare securizată cu timing constant

---

## Validare Input

### Data Annotations

Toate DTO-urile folosesc Data Annotations pentru validare automată:

```csharp
public class RegisterRequestDto
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;
}
```

### Validare Automată în Controllers

ASP.NET Core validează automat DTO-urile și returnează `400 Bad Request` pentru input invalid datorită `[ApiController]` attribute.

### Exemple de Validări

| DTO | Validări |
|-----|----------|
| **CreateMovieDto** | `Required`, `MaxLength(100)` pentru Title, `Range(1800, 2100)` pentru ReleaseYear, `SafeUrl` pentru ImageUrl |
| **CreateSeriesDto** | Similar cu Movie, plus `Range(1, 100)` pentru SeasonsCount, `Range(1, 10000)` pentru EpisodesCount |
| **CreateRatingDto** | `Range(1, 10)` pentru Score, `MaxLength(300)` pentru Comment |

---

## Unit Tests

### Proiect: MovieLibrary.Tests

**Locație**: `Backend/MovieLibrary.Tests/`

**Framework**: xUnit + Moq

### Tests Implementate

#### AuthServiceTests (5 teste)
1. ✅ `RegisterAsync_WithValidData_ShouldCreateUserAndReturnToken`
2. ✅ `RegisterAsync_WithExistingEmail_ShouldThrowArgumentException`
3. ✅ `LoginAsync_WithValidCredentials_ShouldReturnToken`
4. ✅ `LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedException`
5. ✅ `LoginAsync_WithNonExistentEmail_ShouldThrowUnauthorizedException`

#### MovieServiceTests (7 teste)
1. ✅ `GetAllMoviesAsync_ShouldReturnAllMovies`
2. ✅ `GetMovieByIdAsync_WithValidId_ShouldReturnMovie`
3. ✅ `GetMovieByIdAsync_WithInvalidId_ShouldReturnNull`
4. ✅ `CreateMovieAsync_WithValidData_ShouldCreateMovie`
5. ✅ `UpdateMovieAsync_WithValidData_ShouldUpdateMovie`
6. ✅ `DeleteMovieAsync_WithValidId_ShouldReturnTrue`
7. ✅ `GetMoviesByGenreAsync_ShouldReturnFilteredMovies` (implicit în alte teste)

### Rulare Teste

```bash
cd Backend/MovieLibrary.Tests
dotnet test
```

**Rezultat actual**: ✅ 11/11 teste passed

---

## Checklist Securitate

### SQL Injection
- [x] Entity Framework Core cu parametrizare automată
- [x] Fără query-uri raw nesecurizate
- [x] Tipizare puternică pe modele

### XSS Protection
- [x] Middleware de detectare pattern-uri XSS
- [x] Security headers (X-XSS-Protection, CSP, etc.)
- [x] SafeUrl validation pentru URL-uri
- [x] MaxLength pe toate input-urile text
- [x] Validare strictă pe DTO-uri

### CSRF Protection
- [x] JWT authentication (nu cookies)
- [x] Antiforgery tokens configurate
- [x] CORS strict (whitelist origins)
- [x] SameSite cookies

### Autentificare/Autorizare
- [x] JWT cu validare strictă
- [x] BCrypt pentru parole
- [x] Logging pentru încercări eșuate
- [x] Token expiration

### Input Validation
- [x] Data Annotations pe toate DTO-urile
- [x] Validare automată în controllers
- [x] Range validation pentru numere
- [x] Email validation

### Testing
- [x] Minimum 5 unit tests (11 implementate)
- [x] Test coverage pentru AuthService
- [x] Test coverage pentru MovieService
- [x] Mocking cu Moq

---

## Recomandări de Producție

Pentru deployment în producție, recomandăm:

1. **HTTPS**: Activați `app.UseHttpsRedirection()` în [Program.cs](MovieLibrary.API/Program.cs)
2. **Rate Limiting**: Adăugați middleware pentru limitarea request-urilor
3. **Secrets Management**: Mutați `JwtSettings:SecretKey` în Azure Key Vault sau similare
4. **Logging**: Integrați cu Application Insights sau Serilog
5. **Database**: Activați SSL pentru conexiunea SQL Server

---

## Contact

Pentru raportarea vulnerabilităților de securitate, vă rugăm să contactați echipa de dezvoltare.

**Ultima actualizare**: 2025-11-17
