# AwesomeGIC Bank

A banking console application using SOLID principles, Clean Architecture, Domain Driven Design and Test Driven Development in C#.

Project Dependencies Flow:
ConsoleApp → Infrastructure → Application → Domain

To run the application in command prompt:
```
dotnet run --project AwesomeGICBank.ConsoleApp
```

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Domain Model](#domain-model)
- [Application Layer](#application-layer)
- [Infrastructure Layer](#infrastructure-layer)
- [Console Application Layer](#console-application-layer)
- [Testing](#testing)
- [Key Features](#key-features)
- [Design Patterns Used](#design-patterns-used)
- [SOLID Principles Application](#solid-principles-application)

## Architecture Overview

The application follows Clean Architecture and Domain-Driven Design principles, with a clear separation of concerns across four main layers:

### 1. Domain Layer
* Core business logic and entities
* No dependencies on other layers
* Contains value objects, aggregates, and domain exceptions

### 2. Application Layer
* Use cases that orchestrate domain objects
* Defines repository interfaces
* Contains commands and results

### 3. Infrastructure Layer
* Implements repository interfaces
* Provides concrete implementations of system services
* Contains in-memory data persistence

### 4. Console Application Layer
* User interface implementation
* Dependency injection configuration
* Input parsing and command handling

## Domain Model

### Key Domain Objects

#### 1. Account (Aggregate Root)
* Properties: Id, Balance, Transactions
* Methods:
  * Deposit
  * Withdraw
  * GetBalance
  * GetTransactions
* Enforces invariants like preventing negative balances

#### 2. Money (Value Object)
* Immutable representation of monetary amounts
* Validates non-negative values
* Supports basic operations:
  * Add
  * Subtract
* Maintains precise decimal calculations

#### 3. Transaction
* Properties:
  * Id
  * Date
  * AccountId
  * Type
  * Amount
  * ResultingBalance
* Types:
  * Deposit
  * Withdrawal
  * Interest
* Factory methods for creating different transaction types

#### 4. InterestRule
* Properties:
  * EffectiveDate
  * RuleId
  * Rate
* Validates interest rates between 0-100%
* Calculates daily interest

### Domain Validation and Exceptions

The domain layer implements robust validation through custom exceptions:
* `InvalidAccountIdException`
* `InvalidMoneyException`
* `InsufficientFundsException`
* `InvalidInterestRateException`
* `InvalidTransactionIdException`

## Application Layer

### Use Cases

#### 1. ProcessTransactionUseCase
* Handles deposits and withdrawals
* Validates inputs
* Updates account balance
* Returns transaction results

#### 2. DefineInterestRuleUseCase
* Creates new interest rules
* Validates rule parameters
* Maintains rule history

#### 3. GenerateStatementUseCase
* Generates monthly account statements
* Calculates interest
* Combines transactions and interest calculations

### Interface Abstractions

#### 1. ITransactionRepository
```csharp
public interface ITransactionRepository
{
    Task<Account?> GetAccountAsync(AccountId id);
    Task SaveAccountAsync(Account account);
    Task<IEnumerable<Transaction>> GetTransactionsForPeriodAsync(
        AccountId accountId,
        DateTime startDate,
        DateTime endDate);
}
```

#### 2. IInterestRuleRepository
```csharp
public interface IInterestRuleRepository
{
    Task<List<InterestRule>> GetEffectiveRulesAsync(DateTime startDate, DateTime endDate);
    Task SaveRuleAsync(InterestRule rule);
    Task<InterestRule?> GetRuleByIdAsync(string ruleId);
    Task<List<InterestRule>> GetAllRulesAsync();
}
```

## Infrastructure Layer

### Repository Implementations

#### 1. InMemoryTransactionRepository
* Uses `Dictionary<string, Account>` for storage
* Implements transaction retrieval and storage
* Maintains account state

#### 2. InMemoryInterestRuleRepository
* Stores interest rules in memory
* Handles rule effective dates
* Manages rule versions

## Console Application Layer

### User Interface Components

#### 1. ConsoleUI
* Main application loop
* Menu display and navigation
* Command routing

#### 2. MenuHandler
* Processes user input
* Formats output
* Handles command execution

#### 3. Command Parsers
* `TransactionCommandParser`
* `InterestRuleCommandParser`
* `StatementCommandParser`

### Dependency Injection
* Uses Microsoft.Extensions.DependencyInjection
* Configures scoped and singleton services
* Separates infrastructure and application services

## Testing

The application includes unit tests covering:

### 1. Domain Models
* Account operations
* Money calculations
* Transaction creation
* Interest rule validation

### 2. Use Cases
* Transaction processing
* Interest calculation
* Statement generation

### Test Examples
```csharp
[Fact]
public void Withdraw_WithInsufficientFunds_ShouldThrowException()
{
    var account = new Account(AccountId.From("AC001"));
    var transactionDate = new DateTime(2023, 06, 26);

    account.Deposit(transactionDate, Money.FromDecimal(50.00m));

    var exception = Assert.Throws<InsufficientFundsException>(() => 
        account.Withdraw(transactionDate, Money.FromDecimal(60.00m)));
    Assert.Contains("Insufficient funds", exception.Message);
}
```

## Key Features

### 1. Transaction Management
* Deposits and withdrawals
* Balance tracking
* Transaction history

### 2. Interest Calculation
* Rule-based interest rates
* Daily balance calculation
* Monthly interest accrual

### 3. Statement Generation
* Monthly account statements
* Transaction listing
* Interest calculation

### 4. Data Validation
* Input validation
* Business rule enforcement
* Error handling

## Design Patterns Used

### 1. Repository Pattern
* Abstracts data access
* Enables testing
* Supports different storage implementations

### 2. Command Pattern
* Encapsulates operations
* Separates command data from execution
* Supports validation

### 3. Factory Pattern
* Creates domain objects
* Enforces invariants
* Centralizes object creation

### 4. Value Objects
* Immutable objects
* Encapsulated validation
* Business logic containment

## SOLID Principles Application

### 1. Single Responsibility Principle
* Each class has a single purpose
* Clear separation of concerns
* Focused component responsibilities

### 2. Open/Closed Principle
* Extensible design
* New features through extension
* Minimal modification risk

### 3. Liskov Substitution Principle
* Interface-based design
* Consistent behavior
* Polymorphic implementations

### 4. Interface Segregation Principle
* Focused interfaces
* Minimal dependencies
* Clear contracts

### 5. Dependency Inversion Principle
* Dependency injection
* Abstract dependencies
* Flexible configuration
