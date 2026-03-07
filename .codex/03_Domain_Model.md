# 03. Domain Model

## Entity map

### User
Base class for all users.

**Properties**
- `Id`
- `FullName`
- `Email`
- `PasswordHash` or plain password for console demo if simplicity is required
- `Role`

**Responsibilities**
- identity
- role awareness

### Customer : User
Adds customer-specific state.

**Properties**
- `WalletBalance`
- `Cart`
- `Orders`
- `Reviews`

### Administrator : User
May simply inherit `User` with role `Administrator`.

### Product
**Properties**
- `Id`
- `Name`
- `Description`
- `Category`
- `Price`
- `StockQuantity`
- `IsActive`
- `Reviews`

**Rules**
- price >= 0
- stock >= 0

### Cart
**Properties**
- `CustomerId`
- `Items : List<CartItem>`

**Behaviors**
- add item
- update quantity
- remove item
- calculate total
- clear

### CartItem
**Properties**
- `ProductId`
- `ProductName`
- `UnitPrice`
- `Quantity`
- `LineTotal`

### Order
**Properties**
- `Id`
- `CustomerId`
- `Items : List<OrderItem>`
- `TotalAmount`
- `Status`
- `CreatedAt`
- `Payment`

### OrderItem
Snapshot of product data at purchase time.

**Properties**
- `ProductId`
- `ProductName`
- `UnitPrice`
- `Quantity`
- `LineTotal`

### Payment
**Properties**
- `Id`
- `OrderId`
- `Amount`
- `Method` (wallet at baseline, strategy-capable on Monday)
- `Status`
- `PaidAt`

### Review
**Properties**
- `Id`
- `ProductId`
- `CustomerId`
- `Rating`
- `Comment`
- `CreatedAt`

## Key enums

```csharp
public enum UserRole
{
    Customer = 1,
    Administrator = 2
}

public enum OrderStatus
{
    Pending = 1,
    Paid = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3
}
```

## Important invariants

- user emails must be unique
- cart quantity must be greater than zero
- cannot add more product units than available stock
- cannot checkout empty cart
- cannot checkout if wallet funds are insufficient
- successful checkout reduces product stock
- order items should preserve purchase snapshot even if product later changes
- ratings should be within valid range such as 1 to 5

## Suggested repository interfaces

```csharp
public interface IRepository<T>
{
    List<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Remove(Guid id);
}
```

Specialized interfaces can extend this:

```csharp
public interface IUserRepository : IRepository<User>
{
    User? GetByEmail(string email);
}

public interface IProductRepository : IRepository<Product>
{
    List<Product> Search(string term);
    List<Product> GetLowStockProducts(int threshold);
}
```

## LINQ opportunities

- search products by name/category
- sort products by price or rating
- get low-stock products
- group orders by status
- total revenue from completed payments
- best-selling products
- customer order history descending by date
- average product rating

## Testing implications

Prioritize tests for domain invariants from the beginning:
- `Product` validation rules
- `Cart` quantity behavior
- checkout preconditions (stock and funds)
- `Review` rating boundaries
