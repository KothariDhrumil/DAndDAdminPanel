# SonarQube Setup and Configuration

This document describes the SonarQube integration for the Dealers and Distributors project.

## Overview

SonarQube has been integrated into this project to ensure code quality, detect bugs, code smells, and security vulnerabilities. The project uses:

- **SonarScanner for .NET** (dotnet-sonarscanner) version 11.0.0
- **FluentValidation** for input validation across all commands and queries

## Files Added

1. **sonar-project.properties** - SonarQube project configuration
2. **.github/workflows/sonarqube.yml** - GitHub Actions workflow for automated analysis

## Local SonarQube Analysis

### Prerequisites

- .NET 9.0 SDK
- SonarScanner for .NET (installed globally)

### Installation

```bash
dotnet tool install --global dotnet-sonarscanner
```

### Running Analysis Locally

To run SonarQube analysis locally (requires a SonarQube server):

```bash
# Begin analysis
dotnet sonarscanner begin \
  /k:"DealersAndDistributors" \
  /d:sonar.host.url="http://localhost:9000" \
  /d:sonar.token="your-token-here"

# Build the project
dotnet build --no-incremental

# End analysis and upload results
dotnet sonarscanner end /d:sonar.token="your-token-here"
```

## CI/CD Integration

The project includes a GitHub Actions workflow (`.github/workflows/sonarqube.yml`) that automatically runs SonarQube analysis on:

- Push to `main` or `develop` branches
- Pull requests

### Required GitHub Secrets

To enable the workflow, configure these secrets in your repository settings:

1. **SONAR_TOKEN** - Your SonarQube authentication token
2. **SONAR_HOST_URL** - Your SonarQube server URL (e.g., `https://sonarcloud.io`)

## FluentValidation Implementation

FluentValidation has been added to all commands and queries that handle user input. This ensures:

- Input validation before business logic execution
- Consistent validation rules across the application
- Clear, maintainable validation logic

### New Validators Added

The following validators were added as part of this integration:

#### Plans Module
- `UpdatePlanCommandValidator` - Validates plan updates with business rules

#### Support Tickets Module
- `AddSupportTicketNoteCommandValidator` - Validates ticket note additions

#### Tenant Plans Module
- `UpdateTenantPlanCommandValidator` - Validates tenant plan updates with date validations

#### Customers Module
- `CreateCustomerOrderCommandValidator` - Validates customer order creation
- `CreateChildCustomerCommandValidator` - Validates franchise child creation
- `SetFranchiseParentCommandValidator` - Validates hierarchy parent changes
- `InitializeFranchiseProfileCommandValidator` - Validates franchise profile initialization

#### User Types Module
- `CreateUserTypeCommandValidator` - Validates user type creation
- `UpdateUserTypeCommandValidator` - Validates user type updates
- `DeleteUserTypeCommandValidator` - Validates user type deletion

#### Todos Module
- `UpdateTodoCommandValidator` - Validates todo item updates

### Validation Example

```csharp
public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(255);
        RuleFor(c => c.PlanValidityInDays).GreaterThan(0);
        RuleFor(c => c.PlanRate).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Description).MaximumLength(1000);
    }
}
```

## Code Quality Improvements

The following code quality improvements were made:

1. **Removed unused using statements** - Cleaned up unnecessary imports
2. **Fixed variable naming conventions** - Ensured camelCase for local variables
3. **Added null checks** - Protected against null reference exceptions
4. **Implemented duplicate name checking** - Added validation for unique plan names
5. **Enhanced validators** - Added comprehensive validation rules for all fields
6. **Removed dead code** - Cleaned up commented-out code and empty loops
7. **Initialized collections** - Ensured Lists have default empty initializers to prevent null issues

## Known Issues

### AuthPermissions Package Version

The project currently references `AuthPermissions.AspNetCore` version 9.0.9, which does not exist in the NuGet repository (only 9.0.0 is available). This appears to be a dependency on a future version. The code uses features (FirstName, LastName, PhoneNumber on AuthUser) that are not available in version 9.0.0.

**Resolution**: Keep the current version reference until AuthPermissions 9.0.9 is released, or coordinate with the package maintainers.

## SonarQube Configuration

The `sonar-project.properties` file includes:

- **Project Key**: DealersAndDistributors
- **Exclusions**: bin, obj, node_modules, minified JS, wwwroot libraries, client app
- **Code Coverage**: Configured to read OpenCover format reports
- **Generated Code**: Excluded from analysis

## Next Steps

1. Set up a SonarQube server or use SonarCloud
2. Configure the required GitHub secrets
3. Monitor quality gates and address issues
4. Set up quality profiles specific to your organization's standards
5. Configure branch analysis for feature branches

## Resources

- [SonarQube Documentation](https://docs.sonarqube.org/)
- [SonarScanner for .NET](https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [SonarCloud](https://sonarcloud.io/) - Free for open source projects
