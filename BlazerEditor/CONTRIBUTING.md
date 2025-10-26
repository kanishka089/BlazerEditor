# Contributing to BlazerEditor

Thank you for your interest in contributing to BlazerEditor! This document provides guidelines and instructions for contributing.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or JetBrains Rider
- Git

### Setup Development Environment

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/yourusername/blazereditor.git
   cd blazereditor
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the demo:
   ```bash
   cd Demo
   dotnet run
   ```

## Development Workflow

### Branch Naming

- Feature: `feature/your-feature-name`
- Bug fix: `fix/bug-description`
- Documentation: `docs/what-you-document`

### Making Changes

1. Create a new branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. Make your changes

3. Test your changes:
   ```bash
   dotnet test
   ```

4. Commit your changes:
   ```bash
   git commit -m "Add: your feature description"
   ```

5. Push to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```

6. Create a Pull Request

## Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Write unit tests for new features

### Example:

```csharp
/// <summary>
/// Exports the email design to HTML format
/// </summary>
/// <param name="design">The email design to export</param>
/// <returns>HTML string representation</returns>
public string ExportToHtml(EmailDesign design)
{
    // Implementation
}
```

## Testing

- Write unit tests for new features
- Ensure all tests pass before submitting PR
- Test in multiple browsers (Chrome, Firefox, Edge, Safari)

## Documentation

- Update README.md if adding new features
- Add examples to docs/Examples.md
- Update API.md for new public APIs
- Include code comments for complex logic

## Pull Request Process

1. Update documentation
2. Add tests for new features
3. Ensure all tests pass
4. Update CHANGELOG.md
5. Request review from maintainers

## Reporting Issues

### Bug Reports

Include:
- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Browser and .NET version
- Screenshots if applicable

### Feature Requests

Include:
- Clear description of the feature
- Use cases
- Proposed implementation (optional)

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers
- Focus on constructive feedback
- Help others learn and grow

## Questions?

- Open an issue for questions
- Join our discussions
- Check existing documentation

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
