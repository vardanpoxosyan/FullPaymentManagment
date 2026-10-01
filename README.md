## 💳 Payment & Order Management

The project includes a complete backend flow for managing shopping cart orders and payments using **ASP.NET Core Web API, Entity Framework Core, JWT Authentication, and SQL Server**.

### 🔄 Order Flow

```text
User
 ↓
Shopping Cart
 ↓
Create Order
 ↓
Order Status: Pending
 ↓
Create Payment
 ↓
Payment Status: Pending
```

### 📦 Order Features

* Create an order from the user's shopping cart
* Automatically calculate the order total
* Store products and quantities as `OrderItems`
* Preserve the product price at the time of ordering
* Retrieve the authenticated user's orders
* Retrieve a specific user's order data
* Cancel pending orders
* Admin can retrieve all orders
* Admin can update order status

### 💳 Payment Features

* Create a payment for an existing order
* Automatically use the order's total amount
* Associate each payment with its order
* Track payment status
* Retrieve authenticated user's payments
* Admin can retrieve all payments
* One-to-one relationship between `Order` and `Payment`

### 🔐 Authorization

JWT authentication is used to identify the current user.

Regular users can access only their own:

* Orders
* Payments

Administrators can:

* View all orders
* View all payments
* Update order statuses

### 🧱 Technologies

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* AutoMapper
* REST API
* Fluent API
* Swagger

### 🗂️ Main Entities

```text
User
 ├── Cart
 ├── Orders
 │    └── OrderItems
 │         └── Product
 │
 └── Payments
      └── Order
```

### 📌 Payment Status

```text
Pending
Completed
Failed
Refunded
```

### 📌 Order Status

```text
Pending
Paid
Shipped
Completed
Cancelled
```

The project is structured to support integration with a real payment gateway in a later stage.
