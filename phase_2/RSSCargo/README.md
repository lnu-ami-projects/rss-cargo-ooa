# RSSCargo

## Continuous Integration & Testing Guide

### Overview

This project uses GitHub Actions for continuous integration (CI) and automated testing. The CI pipeline will:

1. Build the .NET solution
2. Run all unit and integration tests
3. Generate code coverage reports
4. Upload test results as artifacts

### Test Infrastructure

The test project (`RSSCargo.Tests`) contains various test classes that validate the functionality of different components:

- Unit tests for services (CargoService, UserService, etc.)
- API tests
- UI tests

### Running Tests Locally

To run tests locally:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests with settings file
dotnet test --settings test.runsettings
```

### Test Configuration

The `test.runsettings` file contains configuration for test execution, including:

- Code coverage settings
- Excluded files and folders
- Output formats

### CI Environment

Tests running in the CI environment use:

- PostgreSQL database provided as a Docker container
- Special connection strings handled by `CIHelper` class
- Automated report generation

### Test Reports

After the CI pipeline runs, you can access:

1. Test results in the GitHub Actions summary
2. Code coverage reports as downloadable artifacts
3. Codecov integration for coverage tracking over time

### Adding New Tests

When adding new tests:

1. Follow the existing patterns in the test project
2. Use appropriate attributes (`[Fact]`, `[Theory]`, etc.)
3. Consider whether database setup is needed
4. Use the `CIHelper` class for CI-specific configuration

### Troubleshooting Failed Tests in CI

If tests fail in CI but work locally:

1. Check for environment-specific issues
2. Look at the full test output in GitHub Actions
3. Consider if database initialization might be different
4. Verify any timing or async issues that might appear only in CI