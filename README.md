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
The core data structure is the **`PlayerItem`** class, representing items in a game inventory.

```csharp
public class PlayerItem
{
    public string Name { get; set; }
    public int Durability { get; set; }
    public string Type { get; set; } // e.g., "Weapon", "Armor"
}
```

### 2. Implementation Target

Your task is to implement the methods in the InventoryManager static class.

```csharp
public static class InventoryManager { /* Your code goes here */ }
```
Strict Rule: You must solve these problems using LINQ expressions (no explicit `foreach` or `for` loops allowed in the solution body).


## Validation

The solution is validated using xUnit tests that follow the **AAA (Arrange-Act-Assert)** pattern and use FluentAssertions for readable assertions.

To pass the assignment, all tests in `InventoryManagerTests` must succeed.