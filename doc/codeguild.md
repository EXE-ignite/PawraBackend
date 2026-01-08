# Code Guild - PawraBackend Project

> Hướng dẫn coding standards và best practices cho team phát triển PawraBackend

## 📋 Mục lục
- [Giới thiệu](#giới-thiệu)
- [Cấu trúc Project](#cấu-trúc-project)
- [Quy tắc đặt tên](#quy-tắc-đặt-tên)
- [Kiến trúc & Pattern](#kiến-trúc--pattern)
- [Database & Entities](#database--entities)
- [DTOs & Validation](#dtos--validation)
- [Services Layer](#services-layer)
- [Controllers & API](#controllers--api)
- [Authentication & Authorization](#authentication--authorization)
- [Error Handling](#error-handling)
- [AutoMapper](#automapper)
- [Git Workflow](#git-workflow)

---

## 🎯 Giới thiệu

**PawraBackend** là API backend cho hệ thống quản lý thú cưng, sử dụng **.NET 8**, **PostgreSQL**, và **JWT Authentication**.

**Tech Stack:**
- .NET 8 Web API
- Entity Framework Core
- PostgreSQL (Supabase)
- AutoMapper
- JWT Bearer Authentication
- BCrypt.Net (Password Hashing)

---

## 🏗️ Cấu trúc Project

```
PawraBackend/
├── Pawra.DAL/               # Data Access Layer
│   ├── Entities/            # Database models
│   ├── Repository/          # Generic repository
│   ├── UnitOfWork/          # Unit of Work pattern
│   ├── Data/                # Seed data & extensions
│   └── PawraDBContext.cs    # DbContext
├── Pawra.BLL/               # Business Logic Layer
│   ├── DTOs/                # Data Transfer Objects
│   ├── Interfaces/          # Service interfaces
│   ├── Service/             # Service implementations
│   ├── Exceptions/          # Custom exceptions
│   └── Mappings/            # AutoMapper profiles
└── PawraBackend/            # Presentation Layer
    ├── Controllers/         # API Controllers
    ├── Middlewares/         # Custom middlewares
    └── Program.cs           # App configuration
```

---

## 📝 Quy tắc đặt tên

### General Naming Conventions

| Loại | Convention | Ví dụ |
|------|-----------|--------|
| **Class** | PascalCase | `AccountService`, `AuthController` |
| **Interface** | IPascalCase | `IAccountRoleService`, `IRepository<T>` |
| **Method** | PascalCase | `GetAllAsync()`, `CreateAsync()` |
| **Variable** | camelCase | `var accountRole`, `passwordHash` |
| **Constant** | PascalCase | `const int MaxRetries = 3` |
| **Private field** | _camelCase | `private readonly IMapper _mapper` |
| **DTO** | PascalCaseDto | `LoginRequestDto`, `AccountRoleDto` |

### Naming Files

- **Entity**: `AccountRole.cs`, `Account.cs`
- **DTO**: `CreateAccountRoleDto.cs`, `UpdateAccountRoleDto.cs`
- **Service**: `AccountRoleService.cs`
- **Interface**: `IAccountRoleService.cs`
- **Controller**: `AccountRoleController.cs`

---

## 🏛️ Kiến trúc & Pattern

### 3-Layer Architecture

```
┌─────────────────────────────────┐
│  Presentation Layer (API)        │  Controllers, Middlewares
├─────────────────────────────────┤
│  Business Logic Layer (BLL)      │  Services, DTOs, Mappings
├─────────────────────────────────┤
│  Data Access Layer (DAL)         │  Entities, Repository, UnitOfWork
└─────────────────────────────────┘
```

### Dependency Flow

```
PawraBackend (API) 
    ↓ depends on
Pawra.BLL (Business Logic)
    ↓ depends on
Pawra.DAL (Data Access)
```

**❗ Quan trọng:** 
- API chỉ gọi Services, KHÔNG gọi trực tiếp DbContext hoặc Repository
- Services kế thừa BaseService và inject **UnitOfWork**
  - Dùng UnitOfWork để truy cập Repositories
  - Gọi `UnitOfWork.SaveChangesAsync()` sau mỗi thao tác write
  - Hỗ trợ transactions qua UnitOfWork
- Repository **KHÔNG** tự `SaveChanges()` - UnitOfWork quản lý
- Controllers KHÔNG chứa business logic

**Luồng dữ liệu:**
```
Controller → Service → UnitOfWork → Repository → DbContext → Database
              ↓                ↓
         Business Logic   SaveChanges()
```

---

## 🗄️ Repository & UnitOfWork Pattern

### Repository Pattern

Repository cung cấp abstraction layer giữa business logic và data access.

**Base Repository:**
```csharp
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    internal PawraDBContext dbContext;
    internal DbSet<T> dbSet;

    // CRUD operations KHÔNG tự SaveChanges
    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        // ❌ KHÔNG SaveChanges ở đây
    }
}
```

**Custom Repository:**
```csharp
// Interface
public interface IAccountRoleRepository : IRepository<AccountRole>
{
    Task<bool> HasAccountsUsingRoleAsync(Guid roleId);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}

// Implementation
public class AccountRoleRepository : BaseRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(PawraDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> HasAccountsUsingRoleAsync(Guid roleId)
    {
        // ✅ Repository có quyền truy cập dbContext
        return await dbContext.Accounts.AnyAsync(a => a.RoleId == roleId);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var query = dbContext.AccountRoles.AsNoTracking()
            .Where(r => r.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
```

### UnitOfWork Pattern

UnitOfWork quản lý transactions và SaveChanges tập trung.

**Interface:**
```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    IAccountRoleRepository AccountRoleRepository { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

**Implementation:**
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly PawraDBContext _dbContext;
    private IAccountRoleRepository? _accountRoleRepository;

    public UnitOfWork(PawraDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Lazy loading của custom repositories
    public IAccountRoleRepository AccountRoleRepository
    {
        get
        {
            _accountRoleRepository ??= new AccountRoleRepository(_dbContext);
            return _accountRoleRepository;
        }
    }

    // Generic repository access
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        return new BaseRepository<T>(_dbContext);
    }

    // Quản lý SaveChanges tập trung
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
```

**✅ Best Practices:**

1. **Repository KHÔNG tự SaveChanges**
   ```csharp
   // ❌ SAI
   public async Task AddAsync(T entity)
   {
       await dbSet.AddAsync(entity);
       await dbContext.SaveChangesAsync(); // ❌
   }

   // ✅ ĐÚNG
   public async Task AddAsync(T entity)
   {
       await dbSet.AddAsync(entity);
       // Để UnitOfWork gọi SaveChanges
   }
   ```

2. **Service luôn inject IUnitOfWork**
   ```csharp
   private readonly IUnitOfWork _unitOfWork;
   ```

3. **Gọi SaveChanges sau mỗi write operation**
   ```csharp
   await _unitOfWork.YourRepository.AddAsync(entity);
   await _unitOfWork.SaveChangesAsync(); // ✅ Bắt buộc
   ```

4. **Dùng Transactions cho multiple operations**
   ```csharp
   await _unitOfWork.BeginTransactionAsync();
   try
   {
       // Multiple operations
       await _unitOfWork.SaveChangesAsync();
       await _unitOfWork.CommitTransactionAsync();
   }
   catch
   {
       await _unitOfWork.RollbackTransactionAsync();
       throw;
   }
   ```

---

## 🗄️ Database & Entities

### BaseEntity Pattern

Tất cả entities phải kế thừa từ `BaseEntity`:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    public bool IsDeleted { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }

    public void SetUpdatedDate()
    {
        UpdatedDate = DateTime.UtcNow;
    }
}
```

### Entity Example

```csharp
public class AccountRole : BaseEntity
{
    public string Name { get; set; } = null!;
    
    // Navigation properties
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
```

**✅ Best Practices:**
- Sử dụng `Guid` làm Primary Key
- Luôn có `CreatedDate` và `UpdatedDate`
- Navigation properties phải khởi tạo empty collection
- Dùng `= null!;` cho required properties (C# 8+)

---

## 📦 DTOs & Validation

### DTO Types

1. **Request DTOs**: Nhận data từ client
   - `CreateXxxDto` - Tạo mới
   - `UpdateXxxDto` - Cập nhật
   - `LoginRequestDto` - Login
   
2. **Response DTOs**: Trả data về client
   - `XxxDto` - Chi tiết entity
   - `LoginResponseDto` - Response sau login

### Validation

Sử dụng Data Annotations:

```csharp
public class CreateAccountRoleDto
{
    [Required(ErrorMessage = "Tên role là bắt buộc")]
    [StringLength(50, ErrorMessage = "Tên role không được vượt quá 50 ký tự")]
    public string Name { get; set; } = null!;
}

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password là bắt buộc")]
    [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
    public string Password { get; set; } = null!;
}
```

**✅ Best Practices:**
- Validate tại DTO level (không validate trong service)
- Error messages viết bằng Tiếng Việt
- Kiểm tra `ModelState.IsValid` trong controller

---

## 🔧 Services Layer

### Service Structure

Tất cả services nên kế thừa từ `BaseService<TEntity, TDto>` và inject **IUnitOfWork** để tận dụng CRUD operations sẵn có:

```csharp
public class AccountRoleService : BaseService<AccountRole, AccountRoleDto>, IAccountRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountRoleService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.AccountRoleRepository, mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountRoleDto> CreateAsync(CreateAccountRoleDto dto)
    {
        // Business validation
        var exists = await _unitOfWork.AccountRoleRepository.ExistsByNameAsync(dto.Name);
        if (exists)
        {
            throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
        }

        // Create entity
        var role = _mapper.Map<AccountRole>(dto);
        await _unitOfWork.AccountRoleRepository.AddAsync(role);
        
        // ✅ SaveChanges qua UnitOfWork
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountRoleDto>(role);
    }
}
```

**✅ Best Practices:**

1. **Kế thừa BaseService**: Tận dụng CRUD operations có sẵn
   ```csharp
   public class YourService : BaseService<YourEntity, YourDto>, IYourService
   ```

2. **Inject IUnitOfWork**: CHỈ inject UnitOfWork, KHÔNG inject DbContext
   ```csharp
   private readonly IUnitOfWork _unitOfWork;
   public YourService(IUnitOfWork unitOfWork, IMapper mapper)
   ```

3. **Truy cập Repository qua UnitOfWork**:
   ```csharp
   await _unitOfWork.YourRepository.GetByIdAsync(id);
   await _unitOfWork.Repository<YourEntity>().GetAllAsync();
   ```

4. **Luôn gọi SaveChanges sau write operations**:
   ```csharp
   await _unitOfWork.YourRepository.AddAsync(entity);
   await _unitOfWork.SaveChangesAsync(); // ✅ Bắt buộc
   ```

5. **Sử dụng Transactions cho multiple operations**:
   ```csharp
   await _unitOfWork.BeginTransactionAsync();
   try
   {
       await _unitOfWork.YourRepository.AddAsync(entity1);
       await _unitOfWork.AnotherRepository.AddAsync(entity2);
       await _unitOfWork.SaveChangesAsync();
       await _unitOfWork.CommitTransactionAsync();
   }
   catch
   {
       await _unitOfWork.RollbackTransactionAsync();
       throw;
   }
   ```

6. **AutoMapper**: Dùng `_mapper` từ BaseService
   ```csharp
   return _mapper.Map<AccountRoleDto>(role);
   ```

7. **Exception Handling**: Throw custom exceptions
   ```csharp
   throw new NotFoundException($"Không tìm thấy...");
   ```

8. **Business Validation**: Validate logic trong service
   ```csharp
   var exists = await _unitOfWork.YourRepository.ExistsByNameAsync(name);
   if (exists)
   {
       throw new Exception("Entity đã tồn tại");
   }
   ```

### BaseService Methods

BaseService cung cấp các methods cơ bản:

```csharp
// CRUD operations có sẵn từ BaseService
Task<TDto> Create(TDto dto);
Task<List<TDto>> Read(int pageSize, int pageNumber);
Task<TDto> Read(Guid id);
Task Update(TDto dto);
Task Delete(Guid id);
```

Bạn có thể:
- **Override** để customize behavior
- **Thêm methods mới** cho business logic phức tạp
- **Sử dụng trực tiếp** các methods có sẵn

---

## 🎮 Controllers & API

### Controller Structure

Tất cả controllers nên kế thừa từ `BaseController<TService, TDto>` để tận dụng CRUD endpoints có sẵn:

```csharp
/// <summary>
/// Controller quản lý Account Roles - kế thừa BaseController
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]  // Authorization tại controller level
public class AccountRoleController : BaseController<IAccountRoleService, AccountRoleDto>
{
    private readonly IAccountRoleService _accountRoleService;

    public AccountRoleController(IAccountRoleService accountRoleService) : base(accountRoleService)
    {
        _accountRoleService = accountRoleService;
    }

    /// <summary>
    /// Lấy danh sách tất cả các role (Public endpoint)
    /// </summary>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var roles = await _accountRoleService.GetAllAsync();
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách role thành công",
                data = roles
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Override method từ BaseController để custom response
    /// </summary>
    [HttpGet("{id}")]
    public override async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var role = await _accountRoleService.GetByIdAsync(id);
            return Ok(new
            {
                success = true,
                message = "Lấy thông tin role thành công",
                data = role
            });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }
}
```

**✅ Best Practices:**

1. **Kế thừa BaseController**: Tận dụng CRUD endpoints có sẵn
   ```csharp
   public class YourController : BaseController<IYourService, YourDto>
   ```

2. **Constructor injection**: Call base constructor
   ```csharp
   public YourController(IYourService service) : base(service)
   ```

3. **Override khi cần**: Override methods từ BaseController để customize
   ```csharp
   public override async Task<IActionResult> Get(Guid id)
   ```

4. **Thêm custom endpoints**: Thêm routes mới với tên rõ ràng
   ```csharp
   [HttpGet("all")]  // /api/yourcontroller/all
   [HttpPost("create")]  // /api/yourcontroller/create
   ```

5. **XML Comments**: Luôn thêm XML documentation
   ```csharp
   /// <summary>
   /// Mô tả endpoint
   /// </summary>
   ```

6. **ModelState validation**: Kiểm tra dữ liệu đầu vào
   ```csharp
   if (!ModelState.IsValid)
   {
       return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ", errors = ModelState });
   }
   ```

### BaseController Endpoints

BaseController tự động cung cấp các endpoints:

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/[controller]` | Create mới (dùng dto từ BaseService) |
| `GET` | `/api/[controller]?pageSize=100&pageNumber=1` | Get list với pagination |
| `GET` | `/api/[controller]/{id}` | Get by Id |
| `PUT` | `/api/[controller]` | Update (dùng dto từ BaseService) |
| `DELETE` | `/api/[controller]/{id}` | Delete by Id |

**Lưu ý**: Endpoints từ BaseController dùng generic DTO. Để dùng Create/Update DTOs cụ thể, tạo custom endpoints:

```csharp
[HttpPost("create")]
public async Task<IActionResult> Create([FromBody] CreateAccountRoleDto dto)
{
    // Custom logic với CreateDto
}

[HttpPut("update/{id}")]
public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountRoleDto dto)
{
    // Custom logic với UpdateDto
}
```

### Response Format

**Tất cả API responses phải có format:**

✅ **Success Response:**
```json
{
  "success": true,
  "message": "Mô tả thành công",
  "data": { ... }
}
```

❌ **Error Response:**
```json
{
  "success": false,
  "message": "Mô tả lỗi"
}
```

### HTTP Methods & Status Codes

| Method | Action | Success Status |
|--------|--------|----------------|
| `GET` | Đọc | `200 OK` |
| `POST` | Tạo mới | `201 Created` |
| `PUT` | Cập nhật toàn bộ | `200 OK` |
| `PATCH` | Cập nhật một phần | `200 OK` |
| `DELETE` | Xóa | `200 OK` hoặc `204 No Content` |

### Route Naming

```csharp
[Route("api/[controller]")]           // api/accountrole
[HttpGet]                              // GET api/accountrole
[HttpGet("{id}")]                      // GET api/accountrole/{id}
[HttpPost]                             // POST api/accountrole
[HttpPut("{id}")]                      // PUT api/accountrole/{id}
[HttpDelete("{id}")]                   // DELETE api/accountrole/{id}
```

---

## 🔐 Authentication & Authorization

### JWT Configuration

File: `appsettings.json`
```json
{
  "JwtSettings": {
    "Key": "your-secret-key-here-minimum-32-characters",
    "Issuer": "PawraBackend",
    "Audience": "PawraFrontend"
  }
}
```

### Authorization Attributes

1. **Controller Level** - Tất cả endpoints cần auth:
```csharp
[Authorize(Roles = "admin")]
public class AccountRoleController : ControllerBase
```

2. **Action Level** - Override cho endpoint cụ thể:
```csharp
[AllowAnonymous]  // Public endpoint
public async Task<IActionResult> GetAll()
```

3. **Multiple Roles:**
```csharp
[Authorize(Roles = "admin,veterinarian")]
```

### Implementing Auth in Service

```csharp
// Generate JWT Token
private string GenerateJwtToken(Account account)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
        new Claim(ClaimTypes.Email, account.Email),
        new Claim(ClaimTypes.Name, account.FullName),
        new Claim(ClaimTypes.Role, account.Role.Name)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
        audience: _configuration["JwtSettings:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(24),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

### Password Hashing

**✅ LUÔN hash password với BCrypt:**

```csharp
// Hash password khi register/create
var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

// Verify password khi login
bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash);
```

**❌ KHÔNG BAO GIỜ:**
- Lưu plain password vào database
- Log password ra console
- Trả password trong response

---

## 🚨 Error Handling

### Custom Exceptions

File: `Pawra.BLL/Exceptions/NotFoundException.cs`
```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
```

### Usage in Services

```csharp
if (role == null)
{
    throw new NotFoundException($"Không tìm thấy role với ID: {id}");
}

if (existingRole != null)
{
    throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
}
```

### Usage in Controllers

```csharp
try
{
    var role = await _accountRoleService.GetByIdAsync(id);
    return Ok(new { success = true, data = role });
}
catch (NotFoundException ex)
{
    return NotFound(new { success = false, message = ex.Message });
}
catch (Exception ex)
{
    return BadRequest(new { success = false, message = ex.Message });
}
```

---

## 🗺️ AutoMapper

### ⚠️ QUY TẮC QUAN TRỌNG

**TUYỆT ĐỐI:** Mọi mapping configuration phải được định nghĩa trong file `Pawra.BLL/Mappings/MappingProfile.cs`. 

**❌ KHÔNG BAO GIỜ:**
- Tạo mapping trực tiếp trong Service
- Tạo MapperConfiguration trong controller
- Sử dụng `Mapper.CreateMap()` ngoài MappingProfile
- Tự tạo instance của Mapper

**✅ LUÔN LUÔN:**
- Thêm tất cả mapping vào `MappingProfile.cs`
- Inject `IMapper` qua constructor
- Sử dụng `_mapper.Map<>()` trong services

### Configuration trong MappingProfile.cs

File: `Pawra.BLL/Mappings/MappingProfile.cs`

```csharp
using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ===== AccountRole Mappings =====
            CreateMap<AccountRole, AccountRoleDto>();
            CreateMap<CreateAccountRoleDto, AccountRole>();
            CreateMap<UpdateAccountRoleDto, AccountRole>();

            // ===== Account Mappings =====
            // Basic mapping
            CreateMap<Account, LoginResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<RegisterRequestDto, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            // ===== Thêm mapping mới tại đây =====
            // CreateMap<YourEntity, YourDto>();
        }
    }
}
```

**📝 Lưu ý khi thêm mapping mới:**
1. Group theo entity (dùng comment để phân chia)
2. Mapping theo thứ tự: Entity → DTO, CreateDto → Entity, UpdateDto → Entity
3. Dùng `.ForMember()` khi cần custom logic
4. Dùng `.Ignore()` cho properties sẽ set riêng

### Register in Program.cs

File: `PawraBackend/Program.cs`

```csharp
// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(Pawra.BLL.Mappings.MappingProfile));
```

⚠️ **Chỉ cần register một lần** trong Program.cs, tất cả mappings trong MappingProfile sẽ tự động được load.

### Usage in Services

```csharp
public class AccountRoleService : IAccountRoleService
{
    private readonly PawraDBContext _context;
    private readonly IMapper _mapper;  // ✅ Inject IMapper

    public AccountRoleService(PawraDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;  // ✅ Lưu vào field
    }

    public async Task<AccountRoleDto> GetByIdAsync(Guid id)
    {
        var role = await _context.AccountRoles.FindAsync(id);
        
        // ✅ ĐÚNG: Sử dụng _mapper
        return _mapper.Map<AccountRoleDto>(role);
        
        // ❌ SAI: Manual mapping
        // return new AccountRoleDto 
        // { 
        //     Id = role.Id, 
        //     Name = role.Name 
        // };
    }
}
```

### Mapping Examples

**1. Entity → DTO (Read Operations)**
```csharp
// Single object
var roleDto = _mapper.Map<AccountRoleDto>(role);

// Collection
var rolesDto = _mapper.Map<IEnumerable<AccountRoleDto>>(roles);
var rolesList = _mapper.Map<List<AccountRoleDto>>(rolesList);
```

**2. DTO → Entity (Create Operations)**
```csharp
// Tạo entity mới từ DTO
var newRole = _mapper.Map<AccountRole>(createDto);

// Có thể set thêm properties sau khi map
newRole.PasswordHash = hashedPassword;
newRole.RoleId = defaultRoleId;
```

**3. DTO → Entity (Update Operations)**
```csharp
// Map và update existing entity
var existingRole = await _context.AccountRoles.FindAsync(id);
_mapper.Map(updateDto, existingRole);  // Update properties của existingRole

// Set UpdatedDate từ BaseEntity
existingRole.SetUpdatedDate();

await _context.SaveChangesAsync();
```

### Advanced Mapping Rules

**Ignore Properties:**
```csharp
CreateMap<RegisterRequestDto, Account>()
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.RoleId, opt => opt.Ignore());
```

**Custom Mapping:**
```csharp
CreateMap<Account, LoginResponseDto>()
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
```

**Conditional Mapping:**
```csharp
CreateMap<Account, AccountDto>()
    .ForMember(dest => dest.IsActive, 
        opt => opt.MapFrom(src => !src.IsDeleted && src.EmailVerified));
```

**Nested Objects:**
```csharp
CreateMap<Account, AccountDetailDto>()
    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
    .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.ClinicManager.Clinic.Name));
```

### Common Mistakes

**❌ SAI - Tạo mapping ngoài MappingProfile:**
```csharp
// KHÔNG làm thế này!
public class AccountRoleService
{
    public AccountRoleService()
    {
        Mapper.Initialize(cfg => {
            cfg.CreateMap<AccountRole, AccountRoleDto>();
        });
    }
}
```

**❌ SAI - Manual mapping khi đã có AutoMapper:**
```csharp
// KHÔNG làm thế này khi project đã có AutoMapper!
return new AccountRoleDto 
{
    Id = role.Id,
    Name = role.Name,
    CreatedDate = role.CreatedDate,
    UpdatedDate = role.UpdatedDate
};
```

**✅ ĐÚNG - Dùng AutoMapper:**
```csharp
// Thêm mapping vào MappingProfile.cs:
CreateMap<AccountRole, AccountRoleDto>();

// Sử dụng trong service:
return _mapper.Map<AccountRoleDto>(role);
```

**✅ Best Practices:**
- ✅ Luôn dùng AutoMapper thay vì manual mapping
- ✅ Tất cả mapping phải ở trong `MappingProfile.cs`
- ✅ Group mappings theo entity với comments
- ✅ Inject `IMapper` qua constructor, KHÔNG tạo instance mới
- ✅ Dùng `.ForMember()` để custom mapping rules
- ✅ Dùng `.Ignore()` cho properties sẽ set sau
- ✅ Test mapping sau khi thêm mới
- Dùng `.ForMember()` để custom mapping rules
- Dùng `.Ignore()` cho properties không cần map

---

## 🌿 Git Workflow

### Branch Strategy

```
master (production)
  ↓
develop (integration)
  ↓
feature/feature-name
bugfix/bug-name
hotfix/hotfix-name
```

### Commit Message Format

```
<type>: <subject>

[optional body]
```

**Types:**
- `feat`: Tính năng mới
- `fix`: Sửa bug
- `docs`: Thay đổi documentation
- `style`: Format code (không ảnh hưởng logic)
- `refactor`: Refactor code
- `test`: Thêm tests
- `chore`: Công việc maintain (update packages, etc.)

**Ví dụ:**
```
feat: thêm API CRUD cho AccountRole

- Tạo AccountRoleController với các endpoint CRUD
- Implement AccountRoleService với validation
- Thêm DTOs và AutoMapper configuration
- Chỉ admin được phép truy cập API này
```

### Before Commit Checklist

- [ ] Code đã build thành công (`dotnet build`)
- [ ] Đã test các API endpoints
- [ ] Services ONLY inject IUnitOfWork (NEVER DbContext directly)
- [ ] Repository methods do NOT call SaveChanges
- [ ] Service methods call `await _unitOfWork.SaveChangesAsync()` after repository operations
- [ ] Custom repositories are registered with their interfaces in Program.cs
- [ ] Repositories are added to IUnitOfWork and UnitOfWork implementation
- [ ] Đã update MappingProfile nếu thêm DTOs mới
- [ ] Đã thêm XML comments cho controller actions
- [ ] Đã xóa console.log/debug code
- [ ] Đã validate ModelState trong controller
- [ ] Follow proper layering: Controller → Service → UnitOfWork → Repository → DbContext

---

## 🎯 Development Workflow

### 1. Tạo Entity mới

```csharp
// Pawra.DAL/Entities/NewEntity.cs
public class NewEntity : BaseEntity
{
    public string Name { get; set; } = null!;
}
```

### 2. Update DbContext

```csharp
// Pawra.DAL/PawraDBContext.cs
public DbSet<NewEntity> NewEntities { get; set; }
```

### 3. Tạo Migration

```bash
dotnet ef migrations add AddNewEntity --project Pawra.DAL --startup-project PawraBackend
dotnet ef database update --project Pawra.DAL --startup-project PawraBackend
```

### 4. Tạo DTOs

```csharp
// Pawra.BLL/DTOs/NewEntityDto.cs
// Pawra.BLL/DTOs/CreateNewEntityDto.cs
// Pawra.BLL/DTOs/UpdateNewEntityDto.cs
```

### 5. Update AutoMapper

```csharp
// Pawra.BLL/Mappings/MappingProfile.cs
CreateMap<NewEntity, NewEntityDto>();
CreateMap<CreateNewEntityDto, NewEntity>();
CreateMap<UpdateNewEntityDto, NewEntity>();
```

### 6. Tạo Repository Interface (nếu cần custom methods)

```csharp
// Pawra.DAL/Interfaces/INewEntityRepository.cs
public interface INewEntityRepository : IRepository<NewEntity>
{
    // Thêm custom methods nếu cần
    Task<IEnumerable<NewEntity>> GetActiveEntitiesAsync();
    Task<bool> ExistsByNameAsync(string name);
}
```

### 7. Tạo Repository Implementation (nếu cần custom methods)

```csharp
// Pawra.DAL/Repository/NewEntityRepository.cs
public class NewEntityRepository : BaseRepository<NewEntity>, INewEntityRepository
{
    public NewEntityRepository(PawraDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<NewEntity>> GetActiveEntitiesAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(e => e.Name == name);
    }
}
```

### 8. Update IUnitOfWork Interface

```csharp
// Pawra.DAL/UnitOfWork/IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    IAccountRoleRepository AccountRoleRepository { get; }
    INewEntityRepository NewEntityRepository { get; } // ✅ Add this
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

### 9. Update UnitOfWork Implementation

```csharp
// Pawra.DAL/UnitOfWork/UnitOfWork.cs
public class UnitOfWork : IUnitOfWork
{
    private readonly PawraDBContext _context;
    private IAccountRoleRepository? _accountRoleRepository;
    private INewEntityRepository? _newEntityRepository; // ✅ Add this
    
    public IAccountRoleRepository AccountRoleRepository => 
        _accountRoleRepository ??= new AccountRoleRepository(_context);
    
    public INewEntityRepository NewEntityRepository => 
        _newEntityRepository ??= new NewEntityRepository(_context); // ✅ Add this
}
```

### 10. Tạo Service Interface

```csharp
// Pawra.BLL/Interfaces/INewEntityService.cs
public interface INewEntityService : IService<NewEntity, NewEntityDto>
{
    Task<IEnumerable<NewEntityDto>> GetAllAsync();
    Task<NewEntityDto> GetByIdAsync(Guid id);
    Task<NewEntityDto> CreateAsync(CreateNewEntityDto dto);
    Task<NewEntityDto> UpdateAsync(Guid id, UpdateNewEntityDto dto);
    Task<bool> DeleteAsync(Guid id);
}
```

### 11. Tạo Service Implementation (MUST use UnitOfWork)

```csharp
// Pawra.BLL/Service/NewEntityService.cs
public class NewEntityService : BaseService<NewEntity, NewEntityDto>, INewEntityService
{
    private readonly IUnitOfWork _unitOfWork;

    public NewEntityService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.NewEntityRepository, mapper)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<NewEntityDto> CreateAsync(CreateNewEntityDto dto)
    {
        var entity = _mapper.Map<NewEntity>(dto);
        
        // Validate uniqueness
        if (await _unitOfWork.NewEntityRepository.ExistsByNameAsync(entity.Name))
        {
            throw new ValidationException("Tên đã tồn tại");
        }
        
        await _unitOfWork.NewEntityRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(); // ✅ MUST call SaveChanges
        
        return _mapper.Map<NewEntityDto>(entity);
    }
}
```

### 12. Tạo Controller

```csharp
// PawraBackend/Controllers/NewEntityController.cs
[ApiController]
[Route("api/[controller]")]
public class NewEntityController : BaseController<INewEntityService, NewEntityDto>
{
    public NewEntityController(INewEntityService service) : base(service)
    {
    }
    
    // Custom endpoints nếu cần
}
```

### 13. Register Services trong Program.cs

```csharp
// PawraBackend/Program.cs
// ✅ Register Repository (nếu có custom repository)
builder.Services.AddScoped<INewEntityRepository, NewEntityRepository>();

// ✅ Register Service
builder.Services.AddScoped<INewEntityService, NewEntityService>();
```

---

## 🧪 Testing APIs

### Using VS Code REST Client

File: `doc/test/YourApi.http`

```http
### Variables
@baseUrl = https://localhost:7001/api
@token = your-jwt-token-here

### Login
POST {{baseUrl}}/auth/login
Content-Type: application/json

{
  "email": "admin@pawra.com",
  "password": "Admin@123"
}

### Get All Roles (with auth)
GET {{baseUrl}}/accountrole
Authorization: Bearer {{token}}

### Create Role
POST {{baseUrl}}/accountrole
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "name": "New Role"
}
```

---

## 📚 Resources

### Documentation
- [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://docs.automapper.org/)

### NuGet Packages Used
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.11" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.11" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.11" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.15.0" />
```

---

## ✨ Tips & Tricks

### 1. Async/Await Pattern
```csharp
// ✅ Good - Service uses UnitOfWork
public async Task<AccountRoleDto> GetByIdAsync(Guid id)
{
    var role = await _unitOfWork.AccountRoleRepository.GetByIdAsync(id);
    if (role == null)
    {
        throw new NotFoundException($"Không tìm thấy role với ID: {id}");
    }
    return _mapper.Map<AccountRoleDto>(role);
}

// ❌ Bad - Direct DbContext access
public async Task<AccountRoleDto> GetByIdAsync(Guid id)
{
    var role = await _context.AccountRoles.FindAsync(id);
    return _mapper.Map<AccountRoleDto>(role);
}
```
```

### 2. Null Checking
```csharp
// ✅ Good
if (role == null)
{
    throw new NotFoundException($"Không tìm thấy role với ID: {id}");
}

// ❌ Bad
// Không check null
```

### 3. Using Statements
```csharp
// ✅ Good - Clean code
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using AutoMapper;

// ❌ Bad - Full namespace
var context = new Pawra.DAL.PawraDBContext();
```

### 4. Transaction Management with UnitOfWork
```csharp
// ✅ Good - Explicit transaction for complex operations
public async Task<AccountRoleDto> CreateWithAccountsAsync(CreateAccountRoleDto dto)
{
    await _unitOfWork.BeginTransactionAsync();
    try
    {
        var role = _mapper.Map<AccountRole>(dto);
        await _unitOfWork.AccountRoleRepository.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();
        
        // More operations...
        
        await _unitOfWork.CommitTransactionAsync();
        return _mapper.Map<AccountRoleDto>(role);
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}

// ❌ Bad - SaveChanges in repository
public async Task AddAsync(AccountRole entity)
{
    await _dbSet.AddAsync(entity);
    await _context.SaveChangesAsync(); // ❌ NEVER do this in repository
}
```

### 5. Service Layer Separation
```csharp
// ✅ Good - Service uses UnitOfWork only
public class AccountRoleService : BaseService<AccountRole, AccountRoleDto>, IAccountRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AccountRoleService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.AccountRoleRepository, mapper)
    {
        _unitOfWork = unitOfWork;
    }
}

// ❌ Bad - Service directly injects DbContext
public class AccountRoleService : BaseService<AccountRole, AccountRoleDto>, IAccountRoleService
{
    private readonly PawraDBContext _context; // ❌ NEVER inject DbContext in Service
    
    public AccountRoleService(PawraDBContext context, IMapper mapper)
    {
        _context = context;
    }
}
```

---

## 🆘 Common Issues

### Issue 1: Forgot to call SaveChangesAsync
```
Error: Changes not persisted to database
```
**Solution:** MUST call `await _unitOfWork.SaveChangesAsync()` after repository operations:
```csharp
// ✅ Correct
await _unitOfWork.AccountRoleRepository.AddAsync(role);
await _unitOfWork.SaveChangesAsync(); // ✅ MUST call this

// ❌ Wrong - Changes won't be saved
await _unitOfWork.AccountRoleRepository.AddAsync(role);
// Missing SaveChangesAsync() ❌
```

### Issue 2: Repository calling SaveChanges
```
Error: SaveChanges called multiple times
```
**Solution:** Repository should NEVER call SaveChanges - only UnitOfWork should:
```csharp
// ✅ Good - Repository
public async Task AddAsync(AccountRole entity)
{
    await _dbSet.AddAsync(entity);
    // NO SaveChanges here ✅
}

// ❌ Bad - Repository calling SaveChanges
public async Task AddAsync(AccountRole entity)
{
    await _dbSet.AddAsync(entity);
    await _context.SaveChangesAsync(); // ❌ NEVER do this
}
```

### Issue 3: Service injecting DbContext directly
```
Error: Architecture violation - layering broken
```
**Solution:** Services should ONLY inject IUnitOfWork, never DbContext:
```csharp
// ✅ Good
public class AccountRoleService : BaseService<AccountRole, AccountRoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AccountRoleService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork.AccountRoleRepository, mapper)
    {
        _unitOfWork = unitOfWork;
    }
}

// ❌ Bad
public class AccountRoleService
{
    private readonly PawraDBContext _context; // ❌ NEVER inject DbContext
    
    public AccountRoleService(PawraDBContext context, IMapper mapper)
    {
        _context = context;
    }
}
```

### Issue 4: Invalid salt version (BCrypt)
```
Error: Invalid salt version
```
**Nguyên nhân:** Password trong database không phải BCrypt hash hợp lệ (plain text hoặc hash sai format)

**Solution:**
```csharp
// ✅ ĐÚNG - Hash password với BCrypt khi seed data
if (!context.Accounts.Any(a => a.Id == adminAccountId))
{
    var adminAccount = new Account
    {
        Email = "admin@pawra.com",
        FullName = "Admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // ✅ MUST hash
        RoleId = adminRoleId
    };
    context.Accounts.Add(adminAccount);
}

// ❌ SAI - Plain text password
PasswordHash = "hashedpassword123" // ❌ This is NOT a valid BCrypt hash
```

**Fix:** Drop database và tạo lại với password đã hash đúng:
```bash
dotnet ef database drop --force --project Pawra.DAL --startup-project PawraBackend
dotnet ef database update --project Pawra.DAL --startup-project PawraBackend
```

### Issue 5: JWT Token không được parse (401 Unauthorized)
```
Error: JWT Token: NULL/EMPTY
JWT Challenge - Error: '', ErrorDescription: '', AuthFailure:
```
**Nguyên nhân:** Authorization header có format sai (có dấu quotes hoặc middleware không extract được token)

**Giải pháp:** Thêm custom token extraction trong JWT configuration:
```csharp
// Program.cs - JWT Events
options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader))
        {
            var token = authHeader;
            // Remove 'Bearer ' prefix (case-insensitive)
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring(7);
            }
            // Remove quotes if they exist
            token = token.Trim('\'', '"', ' ');
            
            if (!string.IsNullOrEmpty(token) && token.Contains("."))
            {
                context.Token = token;
            }
        }
        return Task.CompletedTask;
    }
};
```

**Debug JWT Issues:**
```csharp
// Thêm logging để debug
options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        Console.WriteLine($"Auth Header: {context.Request.Headers["Authorization"]}");
        Console.WriteLine($"Token: {context.Token ?? "NULL"}");
        return Task.CompletedTask;
    },
    OnAuthenticationFailed = context =>
    {
        Console.WriteLine($"Auth Failed: {context.Exception.Message}");
        return Task.CompletedTask;
    },
    OnTokenValidated = context =>
    {
        var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
        Console.WriteLine($"Claims: {string.Join(", ", claims ?? Array.Empty<string>())}");
        return Task.CompletedTask;
    }
};
```

**Common Authorization header formats:**
```bash
# ✅ Đúng - Swagger tự động thêm "Bearer "
Authorization: Bearer eyJhbGci...

# ❌ Sai - Có quotes quanh Bearer
Authorization: 'Bearer' eyJhbGci...

# ❌ Sai - Chỉ có token không có Bearer
Authorization: eyJhbGci...
```

### Issue 6: JWT Role Authorization không hoạt động
```
403 Forbidden (có token nhưng vẫn bị từ chối)
```
**Nguyên nhân:** Role claim type không được map đúng

**Solution:** Thêm RoleClaimType vào TokenValidationParameters:
```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
    ValidAudience = builder.Configuration["JwtSettings:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key),
    RoleClaimType = System.Security.Claims.ClaimTypes.Role, // ✅ MUST add this
    NameClaimType = System.Security.Claims.ClaimTypes.Name
};
```

**Lưu ý:** Role name trong `[Authorize(Roles = "Admin")]` phải khớp chính xác (case-sensitive) với role trong database:
```csharp
// ✅ Đúng - Match với database
[Authorize(Roles = "Admin")]  // Database: "Admin"

// ❌ Sai - Case không khớp
[Authorize(Roles = "admin")]  // Database: "Admin" (will fail)
```

### Issue 7: Version Conflict with AutoMapper
```
Error: Version conflict detected for AutoMapper
```
**Solution:** Đảm bảo cả 2 packages dùng cùng version:
```xml
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

### Issue 8: JWT Token không hoạt động
```
401 Unauthorized
```
**Checklist:**
- [ ] JwtSettings trong appsettings.json đúng format
- [ ] Token được thêm vào header: `Authorization: Bearer {token}`
- [ ] Token chưa expired
- [ ] Claims trong token đúng với role required

### Issue 9: Migration lỗi
```
Unable to create migration
```
**Solution:**
```bash
# Xóa migration
dotnet ef migrations remove --project Pawra.DAL --startup-project PawraBackend

# Tạo lại
dotnet ef migrations add MigrationName --project Pawra.DAL --startup-project PawraBackend
```

---

## 📞 Support

Nếu có thắc mắc hoặc issue, liên hệ:
- Team Lead: [Your Name]
- Email: [your-email@example.com]
- Slack: #pawra-backend

---

**Happy Coding! 🚀**

*Last Updated: January 8, 2026*
