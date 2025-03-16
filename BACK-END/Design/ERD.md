@startuml
title Improved E-Commerce Application - ERD (Without Multi-tenancy)

' Define entities with attributes
entity Users {
  + id : UUID [PK]
  + name : varchar
  + role : enum (Customer, Admin, Manager)
  + contactId : UUID [FK]
  + password : varchar "Hashed"
  + lastLogin : datetime
  + isActive : boolean
  + createdAt : datetime
  + updatedAt : datetime
  + deletedAt : datetime "For soft delete"
}

entity Contacts {
  + id : UUID [PK]
  + email : varchar [UQ]
  + mobile : varchar [UQ]
  + addressLine1 : varchar
  + addressLine2 : varchar
  + city : varchar
  + state : varchar
  + country : varchar
  + postalCode : varchar
  + isEmailVerified : boolean
  + isPhoneVerified : boolean
  + createdAt : datetime
  + updatedAt : datetime
}

entity Products {
  + id : UUID [PK]
  + name : varchar
  + description : text
  + basePrice : decimal
  + categoryId : UUID [FK]
  + isActive : boolean
  + isFeatured : boolean
  + createdAt : datetime
  + updatedAt : datetime
  + deletedAt : datetime
}

entity ProductVariants {
  + id : UUID [PK]
  + productId : UUID [FK]
  + sku : varchar [UQ]
  + size : varchar
  + color : varchar
  + price : decimal
  + barcode : varchar [UQ]
  + weight : decimal
  + createdAt : datetime
}

entity ProductImages {
  + id : UUID [PK]
  + productId : UUID [FK]
  + variantId : UUID [FK] "Optional"
  + url : varchar
  + displayOrder : integer
  + isPrimary : boolean
  + createdAt : datetime
}

entity Inventory {
  + id : UUID [PK]
  + variantId : UUID [FK]
  + warehouseId : UUID [FK]
  + quantity : integer
  + reorderLevel : integer
  + createdAt : datetime
  + updatedAt : datetime
}

entity Warehouses {
  + id : UUID [PK]
  + name : varchar
  + contactId : UUID [FK]
  + isActive : boolean
  + createdAt : datetime
}

entity Categories {
  + id : UUID [PK]
  + name : varchar
  + slug : varchar [UQ]
  + parentId : UUID [FK] "Self-reference for hierarchy"
  + createdAt : datetime
}

entity Orders {
  + id : UUID [PK]
  + userId : UUID [FK]
  + subtotal : decimal
  + tax : decimal
  + discount : decimal
  + shippingCost : decimal
  + totalAmount : decimal
  + status : enum (Pending, Processing, Shipped, Delivered, Cancelled)
  + notes : text
  + createdAt : datetime
  + updatedAt : datetime
}

entity OrderItems {
  + id : UUID [PK]
  + orderId : UUID [FK]
  + variantId : UUID [FK]
  + quantity : integer
  + unitPrice : decimal
  + totalPrice : decimal
  + createdAt : datetime
}

entity Cart {
  + id : UUID [PK]
  + userId : UUID [FK]
  + variantId : UUID [FK]
  + quantity : integer
  + createdAt : datetime
  + updatedAt : datetime
}

entity Wishlist {
  + id : UUID [PK]
  + userId : UUID [FK]
  + productId : UUID [FK]
  + createdAt : datetime
}

entity Payments {
  + id : UUID [PK]
  + orderId : UUID [FK]
  + paymentMethod : enum (Credit Card, PayPal, COD, Bank Transfer)
  + paymentStatus : enum (Pending, Completed, Failed, Refunded)
  + transactionId : varchar [UQ]
  + amount : decimal
  + createdAt : datetime
  + updatedAt : datetime
}

entity Shipping {
  + id : UUID [PK]
  + orderId : UUID [FK]
  + contactId : UUID [FK] "Shipping address"
  + trackingNumber : varchar [UQ]
  + status : enum (Processing, Shipped, Delivered, Failed)
  + carrier : varchar
  + estimatedDelivery : datetime
  + actualDelivery : datetime
  + createdAt : datetime
  + updatedAt : datetime
}

entity Reviews {
  + id : UUID [PK]
  + productId : UUID [FK]
  + userId : UUID [FK]
  + rating : integer "1-5 stars"
  + comment : text
  + isVerifiedPurchase : boolean
  + isApproved : boolean
  + createdAt : datetime
}

entity Discounts {
  + id : UUID [PK]
  + code : varchar [UQ]
  + name : varchar
  + type : enum (Percentage, Fixed Amount)
  + value : decimal
  + startDate : datetime
  + endDate : datetime
  + isActive : boolean
  + createdAt : datetime
}

entity DiscountRules {
  + id : UUID [PK]
  + discountId : UUID [FK]
  + type : enum (Product, Category, User, Order Value)
  + entityId : UUID "ProductId, CategoryId, etc."
  + minOrderValue : decimal
  + minQuantity : integer
  + isStackable : boolean
  + createdAt : datetime
}

entity CustomerSegments {
  + id : UUID [PK]
  + name : varchar
  + criteria : text
  + createdAt : datetime
}

' Define relationships
Users ||--|| Contacts : "has"
Users ||--o{ Orders : "places"
Users ||--o{ Cart : "has"
Users ||--o{ Wishlist : "has"
Users ||--o{ Reviews : "writes"

Products ||--|| Categories : "belongs to"
Products ||--o{ ProductVariants : "has"
Products ||--o{ ProductImages : "has"
Products ||--o{ Reviews : "receives"

OrderItems ||--|| ProductVariants : "refers to"
Cart ||--|| ProductVariants : "contains"
Wishlist ||--|| Products : "contains"
Inventory ||--|| ProductVariants : "tracks"
Inventory ||--|| Warehouses : "located in"

Orders ||--o{ OrderItems : "contains"
Orders ||--|| Payments : "processed by"
Orders ||--|| Shipping : "delivered by"
Orders }|--o{ Discounts : "applies"

Discounts ||--o{ DiscountRules : "defined by"
CustomerSegments }o--o{ Users : "groups"

' Self-reference for category hierarchy
Categories }|--o| Categories : "parent"
@enduml