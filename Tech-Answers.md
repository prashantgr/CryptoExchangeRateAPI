# Answers to Technical Questions

## 1. Time Spent on the Coding Assignment
I spent approximately 6 hours on the coding assignment. 

## 2. Most Useful Feature in the Latest Version of C#
### C# 9: 
- Introduces `record` types for concise, immutable data models with built-in value equality.
```csharp
public record Person(string Name, int Age);

- Allows properties to be set only during object initialization, improving immutability.
```csharp
public class Person {
    public string Name { get; init; }
}

- Simplifies program entry points by removing the need for the Main method.
```csharp
Console.WriteLine("Hello, World!");

- Adds relational (<, >, etc.) and logical (and, or, not) patterns.
```csharp
if (x is > 0 and < 10)

- Simplifies object instantiation by allowing type inference from the context.
```csharp
Person person = new("John", 30);  // Type inferred from context

- Allows overriding methods to return more specific types than the original method signature.






Records, init-only setters, top-level statements, and enhanced pattern matching.
C# 10: Global usings, file-scoped namespaces, record structs, and constant interpolated strings.
C# 11: Raw string literals, required members, list patterns, and UTF-8 string literals.
C# 12: Primary constructors for non-records, collection expressions, and enhanced switch expressions.

