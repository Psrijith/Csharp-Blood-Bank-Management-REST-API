# Blood Bank Management API

## For more clarity, please check out [Explanation.docs](https://github.com/Psrijith/Csharp-Blood-Bank-Management-REST-API/blob/main/Explanation.pdf)


## Overview

This project implements a **Blood Bank Management REST API** using **C# and ASP.NET Core**. The API provides basic **CRUD operations** (Create, Read, Update, Delete) along with additional features like **pagination**, **search functionality**, **sorting**, and **filtering** to manage blood bank entries.

The application stores data in an **in-memory list**, simulating a database. You can interact with the API using tools like **Swagger** and **Postman**.

## Model: Donor

The **Donor** model represents the structure of a blood donor's entry and includes:
- **Id**: Unique identifier (auto-generated).
- **DonorName**: Name of the donor.
- **Age**: Age of the donor.
- **BloodType**: Blood type of the donor (e.g., O+, A-, B+).
- **ContactInfo**: Contact details (e.g., phone number).
- **Quantity**: Amount of blood donated (in ml).
- **CollectionDate**: Date when the blood was collected.
- **ExpirationDate**: Expiration date for the blood unit (42 days from collection).
- **Status**: Current status of the blood (Available, Requested, Expired).


## Features

### 1. **CRUD Operations**:
   - **Create** (POST `/api/donors`): Add a new donor entry.
   - **Read** (GET `/api/donors`): Retrieve all donor entries.
   - **Read by ID** (GET `/api/donors/{id}`): Retrieve a donor entry by ID.
   - **Update** (PUT `/api/donors/{id}`): Update an existing donor entry.
   - **Delete** (DELETE `/api/donors/{id}`): Remove a donor entry by ID.
![image](https://github.com/user-attachments/assets/03503157-5826-4a0a-8c1f-82e7453e12bf)
 

### 2. **Pagination** (GET `/api/donors/page`):
   - Retrieve a paginated list of donors by specifying the page number and size.

### 3. **Search Functionality**:
   - **By Blood Type** (GET `/api/donors/blood`): Search donors by blood type.
   - **By Status** (GET `/api/donors/status`): Search donors by their status (e.g., "Available", "Requested", "Expired").
   - **By Donor Name** (GET `/api/donors/name`): Search donors by name.

### 4. **Sorting** (GET `/api/donors/sort`):
   - Sort donors by **BloodType** or **CollectionDate** with options for ascending or descending order.

### 5. **Filtering** (GET `/api/donors/blood,status`):
   - Filter donors by both **BloodType** and **Status** simultaneously.

**Endpoint:** `/api/donors`  
**Example Body:**
```json
{
  "DonorName": "Srijith",
  "Age": 20,
  "BloodType": "O+",
  "ContactInfo": "1231231234",
  "Quantity": 500,
  "CollectionDate": "2024-11-19T10:00:00",
  "Status": "Available"
}
```
