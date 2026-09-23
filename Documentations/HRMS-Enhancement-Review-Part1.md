# HRMS Enhancement Review — PART 1

This document records the first set of enhancement notes reviewed for the HRMS project.

> Scope: Review only. No implementation has been applied yet.

---

## Enhancement Note #1 — Move employee filtering to Repository

Keep the existing architecture:

```text
Controller
  ↓
Service
  ↓
Repository
  ↓
DbContext
```

Move database filtering from `EmployeeService` into `EmployeeRepository`.

Recommended flow:

```text
Service
  ↓
Pass filter request to Repository
  ↓
Repository builds EF Core IQueryable
  ↓
Apply Where() conditions
  ↓
SQL Server filters records
  ↓
Return filtered entities
  ↓
Service maps entities to models
```

Important rule:

```text
Where()
  ↓
OrderBy()
  ↓
Skip()
  ↓
Take()
  ↓
ToListAsync()
```

Avoid loading all employees first and filtering them in memory.

---

## Enhancement Note #2 — Use AsNoTracking() for read-only queries

For queries that only read data, add:

```csharp
.AsNoTracking()
```

Examples:

- `GetsAsync()`
- `GetByIdAsync()` when data is only displayed
- `GetFilteredAsync()`

Use normal tracking when the entity will be modified and saved.

Rule:

```text
Read only
  → AsNoTracking()

Read + modify + SaveChanges
  → Tracking
```

---

## Enhancement Note #3 — Centralize exception handling and use ILogger

Current repeated pattern:

```csharp
try
{
    // logic
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return ... ex.Message;
}
```

Recommended:

- Use ASP.NET Core structured logging with `ILogger<T>`.
- Use a global exception handler / middleware for unexpected errors.
- Do not return internal exception details to the client.

Recommended flow:

```text
Request
  ↓
Controller
  ↓
Service
  ↓
Repository
  ↓
Unexpected exception
  ↓
Global Exception Handler
  ├──→ ILogger → Server log
  └──→ Safe HTTP 500 response → Client
```

---

## Enhancement Note #4 — Validate route ID against request-body ID

Current update route:

```text
PUT /api/employee/{id}
```

The route `id` is currently received but not used.

Recommended:

```csharp
if (id != employeeDto.Id)
{
    return BadRequest("Employee ID mismatch.");
}
```

Flow:

```text
URL ID ───┐
          ├── Compare
Body ID ──┘
     ↓
Same?
 ├─ No  → 400 Bad Request
 └─ Yes → Service → Repository → Update
```

---

## Enhancement Note #5 — Add authentication and object-level authorization

Passing an ID in a URL is not itself a security problem.

Example:

```text
PUT /api/employee/5
```

The real security requirement is checking whether the authenticated user is allowed to access or modify employee 5.

Recommended flow:

```text
Request
  ↓
Authentication
  ↓
Authorization
  ↓
Allowed?
 ├─ No  → 403 Forbidden
 └─ Yes → Controller → Service → Repository
```

Do not rely on hiding IDs or switching from integer IDs to GUIDs as the main security control.

---

## Enhancement Note #6 — Check employee existence before Remove()

Current delete logic can call `Remove()` with a null employee.

Recommended:

```csharp
var employee = await _context.Employees.FindAsync(id);

if (employee == null)
    return null;

_context.Employees.Remove(employee);
await _context.SaveChangesAsync();
```

Expected flow:

```text
Employee exists?
  ├─ No  → return null → Service returns 404
  └─ Yes → Delete
```

---

## Enhancement Note #7 — Preserve HR history when deleting a manager

Current code deletes JobHistory records where:

```csharp
jh.ManagerId == employeeId
```

This can remove another employee's valid HR history.

Preferred HRMS approach:

```text
Employee leaves company
  ↓
Do not physically delete
  ↓
Status = Inactive / Terminated
  ↓
Preserve Employee record
  ↓
Preserve JobHistory
  ↓
Preserve manager history
```

If physical deletion is required, prefer clearing the nullable `ManagerId` instead of deleting the JobHistory record.

---

## Enhancement Note #8 — Standardize the mapping strategy

The project currently mixes:

- AutoMapper
- Custom static mapper classes such as `MapperEmployee` and `MapperJobHistory`

Both approaches are valid, but the project should use one strategy consistently unless there is a clear reason to mix them.

AutoMapper can handle:

- Nested object mapping
- Collection mapping
- Flattening, e.g. `JobHistory.JobRole.Title → JobRoleTitle`

Important: AutoMapper maps already-loaded objects; EF Core must still load required relationships.

---

## Enhancement Note #9 — Expand tests beyond happy paths

Keep the existing xUnit + Moq tests, but add more:

- Not-found cases
- Duplicate email / phone
- Invalid IDs
- Invalid dates
- Update non-existing employee
- Delete non-existing employee
- Repository exceptions
- Boundary cases

Also add a small number of integration tests.

Difference:

```text
Unit Test
  → Does service logic work?

Integration Test
  → Do API + Service + Repository + EF Core + Database work together?
```

---

## Enhancement Note #10 — Simplify generic ApiService response handling

`ApiService` is generic but contains Employee-specific checks such as:

```csharp
typeof(T) == typeof(EmployeeModel)
```

Recommended:

```csharp
var result =
    JsonConvert.DeserializeObject<ResponseModel<T>>(content);
```

Goal:

```text
ApiService<T>
  ↓
Deserialize ResponseModel<T>
  ↓
Return
```

Avoid adding special cases for each model type.

---

## Enhancement Note #11 — Avoid unconditional Database.Migrate() in production startup

Current startup behavior:

```text
Start API
  ↓
Database.Migrate()
  ↓
Apply pending schema changes
  ↓
Start accepting requests
```

Recommended production flow:

```text
Deployment
  ↓
Run migration once
  ↓
Migration successful?
 ├─ No  → Stop deployment
 └─ Yes → Start API
```

Automatic migration can still be kept for Development if useful.

---

## Enhancement Note #12 — Return 200 + empty collection for empty list/search results

For collection endpoints:

```text
GET /api/employee
  ↓
No records
  ↓
200 OK + []
```

For a specific missing resource:

```text
GET /api/employee/999
  ↓
Employee does not exist
  ↓
404 Not Found
```

Use `404` for missing specific resources, not for valid collection queries that simply return zero rows.

---

## Enhancement Note #13 — Use nullable return types where null is possible

If a repository method uses `FirstOrDefaultAsync()`, it can return null.

Current:

```csharp
Task<Employee>
```

Recommended:

```csharp
Task<Employee?>
```

Apply the same idea to methods such as:

- `GetByIdAsync()`
- `GetByEmailAsync()`
- `GetByPhoneAsync()`

This matches the project's existing `<Nullable>enable</Nullable>` setting.

---

## Enhancement Note #14 — Add server-side pagination

Even after filtering is moved into the Repository, large result sets should be paged.

Recommended EF Core order:

```text
Where()
  ↓
OrderBy()
  ↓
Skip()
  ↓
Take()
  ↓
ToListAsync()
```

Example:

```csharp
query
    .OrderBy(e => e.Id)
    .Skip((page - 1) * pageSize)
    .Take(pageSize);
```

The API should also return metadata such as:

- Page
- PageSize
- TotalRecords
- TotalPages

---

## Enhancement Note #15 — Keep database secrets out of source control

Do not store real production credentials in committed `appsettings.json`.

Recommended:

```text
Source-controlled appsettings.json
  → Non-secret configuration

User Secrets / CI-CD secrets / Environment variables / Key Vault
  → Passwords and connection secrets
```

Existing code can continue using:

```csharp
builder.Configuration.GetConnectionString("HRMSDb")
```

because ASP.NET Core merges configuration sources.

---

## Enhancement Note #16 — Fix CI coverage-format mismatch

Current pipeline generates:

```text
OpenCover XML
```

but GitLab declares:

```text
Cobertura
```

The generated format and declared format should match.

Possible design:

```text
Tests
  ↓
Coverage
  ├──→ Cobertura → GitLab
  └──→ OpenCover → SonarCloud
```

or configure one format consistently if both tools support it.

---

## Enhancement Note #17 — Replace fixed free-text status values with enums

Current status fields use strings such as:

```csharp
public string Status { get; set; }
```

This allows accidental values such as:

```text
Active
active
ACTIVE
Actve
```

Recommended:

```csharp
public enum EmployeeStatus
{
    Active,
    Inactive,
    Terminated
}
```

Then:

```csharp
public EmployeeStatus Status { get; set; }
```

If readable database values are preferred, configure EF Core to store the enum as a string:

```csharp
modelBuilder.Entity<Employee>()
    .Property(e => e.Status)
    .HasConversion<string>();
```

---

# PART 1 Summary

The main focus areas in this review are:

- Database query efficiency
- EF Core read performance
- Centralized error handling
- Safer update/delete behavior
- Authorization
- HR data-history preservation
- Consistent mapping
- Stronger automated testing
- Cleaner generic API-client code
- Controlled database migrations
- Better REST response semantics
- Nullable reference correctness
- Server-side pagination
- Secret management
- CI/CD correctness
- Stronger domain modeling with enums

No implementation changes are included in this document. These items are enhancement candidates for later review and implementation.
