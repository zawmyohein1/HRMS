INSERT INTO Locations (Name, City, Country) VALUES
('Headquarters', 'New York', 'USA'),
('Regional Office', 'Los Angeles', 'USA'),
('Branch Office', 'London', 'UK'),
('Development Center', 'Berlin', 'Germany');


INSERT INTO Departments (Name, Description, LocationId) VALUES
('HR', 'Handles recruitment and payroll.', (SELECT Id FROM Locations WHERE Name = 'Headquarters')),
('IT', 'Manages IT infrastructure and software development.', (SELECT Id FROM Locations WHERE Name = 'Regional Office')),
('Sales', 'Handles customer sales and business growth.', (SELECT Id FROM Locations WHERE Name = 'Branch Office')),
('R&D', 'Research and development of new products.', (SELECT Id FROM Locations WHERE Name = 'Development Center'));


INSERT INTO JobRoles (Title, Description, DepartmentId) VALUES
('HR Manager', 'Manages HR operations.', (SELECT Id FROM Departments WHERE Name = 'HR')),
('Software Engineer', 'Develops software solutions.', (SELECT Id FROM Departments WHERE Name = 'IT')),
('Sales Executive', 'Handles sales and customer relations.', (SELECT Id FROM Departments WHERE Name = 'Sales')),
('Research Scientist', 'Conducts research and innovation.', (SELECT Id FROM Departments WHERE Name = 'R&D'));


INSERT INTO Employees (Name, BirthDate, Phone, Email, Gender, Address, Status) VALUES
('Alice Johnson', '1985-05-15', '123-456-7890', 'alice.johnson@example.com', 'Female', '123 Main St, NY', 'Active'),
('Bob Smith', '1990-08-22', '987-654-3210', 'bob.smith@example.com', 'Male', '456 Elm St, LA', 'Active');


INSERT INTO JobHistories (EmployeeId, JobRoleId, StartDate, EndDate, ManagerId) VALUES
((SELECT Id FROM Employees WHERE Name = 'Alice Johnson'),
 (SELECT Id FROM JobRoles WHERE Title = 'HR Manager'),
 '2020-01-10', NULL, NULL),

((SELECT Id FROM Employees WHERE Name = 'Bob Smith'),
 (SELECT Id FROM JobRoles WHERE Title = 'Software Engineer'),
 '2019-07-01', '2023-02-15',
 (SELECT Id FROM Employees WHERE Name = 'Alice Johnson'));


