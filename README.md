# 🏧 ATM Management System – Desktop Application  
**Course:** CSC 834 — Software Engineering 
**Developed using:** C#, Windows Forms (WinForms), SQL Server

---

## 📌 Project Overview

This project is a full-featured **ATM Management System** developed as part of a Software Engineering course. The system simulates real-world ATM operations for a fictional bank (ZZZ Bank), allowing users to perform essential banking functions such as **depositing money**, **withdrawing cash**, **checking account balance**, and **transferring funds between accounts**.

The project was built as a **desktop application** using **C# in Visual Studio**, with the backend powered by **SQL Server**. The user interface was designed using **Windows Forms**, and the system is backed by **a secure and normalized relational database**.

---

## 🛠️ Project Development Approach

I followed a structured Software Development Life Cycle (SDLC) to build the ATM system. Below is a step-by-step overview of the development process:

---

### 1️⃣ Requirements Specification  
Before development, I focused on clearly defining both **functional** and **non-functional** requirements:

#### ✅ Functional Requirements:
- Allow users to log in to their bank accounts securely
- Enable users to perform operations like:
  - Withdrawing cash
  - Depositing funds
  - Checking account balance
  - Transferring money between accounts
- Record each transaction securely in the database
- Enforce a maximum daily transaction limit of $3,000 per account

#### ✅ Non-Functional Requirements:
- User-friendly and intuitive interface for all age groups
- System must be secure with proper authentication
- Data consistency and transactional integrity
- Real-time database updates for critical operations
- Efficient error handling and feedback mechanisms

---

### 2️⃣ User Interface Design (GUI)

Using **Visual Studio’s Windows Forms Designer**, I created a responsive and functional GUI for all major user interactions.

#### Key Interfaces Implemented:
- **Login Screen:** Authenticates user credentials
- **Main Menu:** Provides navigation options
- **Withdraw Screen:** Allows cash withdrawal with balance/limit validation
- **Deposit Screen:** Records deposits and generates confirmation
- **Check Balance Screen:** Displays real-time balance fetched from database
- **Transfer Screen:** Transfers funds between accounts with validation

> All forms are backed by event-driven programming logic implemented in `form1.cs`, `form1.Designer.cs`, and other module classes like `customer.cs`, `account.cs`, and `transaction.cs`.

---

### 3️⃣ UML Diagrams

To visualize and plan the system architecture, I created detailed UML diagrams:

- **Use Case Diagram:** Illustrates interactions between users and system features
- **Class Diagram:** Defines object structure and relationships
- **Sequence Diagrams:** Captures dynamic interactions for each operation (withdraw, deposit, login, etc.)
- **Activity Diagrams:** Shows the step-by-step workflow of all processes
- **State Diagrams:** Demonstrates the transitions between various system states based on user actions

> These diagrams ensured clarity in design and acted as blueprints throughout development.

---

### 4️⃣ Database Design

I designed a robust **relational database** in SQL Server to handle user data, account details, and transaction history. The database was normalized to reduce redundancy and improve integrity.

#### 💾 Tables Implemented:
- **Customer:** Stores user credentials (username, password)
- **Account:** Tracks account balances, daily limits, and associated customer IDs
- **Transaction:** Logs all user transactions including deposits, withdrawals, and transfers with timestamps

#### 🔗 ER Design:
- **One-to-many relationship** between Customer and Account
- **One-to-many relationship** between Account and Transaction

> Schema was defined with clear data types, primary/foreign keys, and constraints to maintain consistency.

---

### 5️⃣ Backend Connectivity & Functionality

All GUI components were connected to the database using ADO.NET in C#. The backend logic enables real-time interaction between the UI and the database.

#### Features Implemented:
- Dynamic account lookups and balance retrieval
- Daily transaction limit enforcement logic
- Secure data validation for user inputs
- Efficient database CRUD operations (Insert, Update, Select)
- Custom error messages for invalid operations

> Example: If a user tries to withdraw more than the allowed daily limit or current balance, the system prevents the operation and displays a relevant message.

---

### 6️⃣ Transaction Management & Logging

The system ensures that all **monetary actions** (deposit, withdrawal, transfer) are:
- Validated in real-time
- Logged in the `Transaction` table
- Linked to specific accounts
- Time-stamped for audit purposes

Receipts and success messages confirm actions to the user. All logs are permanently stored in the database.

---

### 7️⃣ User Authentication & Security

Security is a key feature of this ATM system. I implemented:
- **Credential-based login system**
- **Password masking** during login input
- **Authentication checks** before allowing access to core features
- **Session control** to reset/clear state on logout

This ensures that only verified users can perform sensitive banking operations.

---


