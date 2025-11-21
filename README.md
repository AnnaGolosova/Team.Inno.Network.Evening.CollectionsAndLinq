# README: LINQ Fundamentals & Inventory Management

This project serves as a comprehensive test of your understanding of **LINQ (Language Integrated Query)**, **C# collections**, and **algorithmic complexity (O notation)** as applied in a typical domain context.

---

## Prerequisites

Before starting, you must be comfortable with:
1.  **C# Collections:** `List<T>`, `Dictionary<K,V>`, `HashSet<T>`.
2.  **LINQ Syntax:** Lambda expressions (`=>`), `Where`, `Select`, `OrderBy`.
3.  **Complexity:** When an operation is O(1) versus O(n) (especially for `List` vs. `Dictionary`).

---

## Project Setup

### 1. Domain Model
This project contains two domain models, **`PlayerItem`** class, representing items in a game inventory and  **`ServiceRequest`** class, represents a network request tracking with ID, Route, Severity, and Timestamp.

```csharp
public class PlayerItem
{
    public string Name { get; set; }
    public int Durability { get; set; }
    public string Type { get; set; } // e.g., "Weapon", "Armor"
}
```

```csharp
public class ServiceRequest
{
    public int RequestId { get; set; }
    public string RouteName { get; set; }
    public int Severity { get; set; }
    public DateTime Timestamp { get; set; }
}
```

### 2. Implementation Target

Your task is to implement the methods in the InventoryManager static class AND to implement the bodies of the eight methods in the TrafficController class.

```csharp
public static class InventoryManager { /* Your code goes here */ }
```
```csharp
public static class TrafficController { /* Your code goes here */ }
```
Strict Rule: You must solve filtration and queries problems using LINQ expressions (no explicit `foreach` or `for` loops allowed in the solution body).
Problems that require collection changes can be solved with `foreach` or `for` loops


## Validation

The solution is validated using xUnit tests that follow the **AAA (Arrange-Act-Assert)** pattern and use FluentAssertions for readable assertions.

To pass the assignment, all tests in `InventoryManagerTests` and `TrafficControllerTests` must succeed.