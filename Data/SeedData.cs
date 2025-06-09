using Microsoft.EntityFrameworkCore;
using TNAI_Proj.Models;

namespace TNAI_Proj.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Check if data already exists
                // if (context.Categories.Any() || context.Cars.Any() || context.Dealerships.Any() || context.Users.Any())
                // {
                //     return; // Database has been seeded
                // }

                // Clear existing data to prevent duplicates
                context.Reviews.RemoveRange(context.Reviews);
                context.Orders.RemoveRange(context.Orders);
                context.MaintenanceRecords.RemoveRange(context.MaintenanceRecords);
                context.Cars.RemoveRange(context.Cars);
                context.Users.RemoveRange(context.Users);
                context.Dealerships.RemoveRange(context.Dealerships);
                context.Categories.RemoveRange(context.Categories);
                context.SaveChanges();

                // Seed Categories
                var categories = new List<Category>
                {
                    new Category { Name = "Sedan", Description = "Comfortable family cars", DisplayOrder = 1, CreatedAt = DateTime.UtcNow },
                    new Category { Name = "SUV", Description = "Sport Utility Vehicles", DisplayOrder = 2, CreatedAt = DateTime.UtcNow },
                    new Category { Name = "Sports Car", Description = "High-performance vehicles", DisplayOrder = 3, CreatedAt = DateTime.UtcNow }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();

                // Seed Dealerships
                var dealerships = new List<Dealership>
                {
                    new Dealership { Name = "AutoMax", Address = "123 Main St", PhoneNumber = "555-1234", Email = "info@automax.com", ManagerName = "John Doe", OperatingHours = "9 AM - 6 PM", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Dealership { Name = "CarWorld", Address = "456 Oak St", PhoneNumber = "555-5678", Email = "info@carworld.com", ManagerName = "Jane Smith", OperatingHours = "8 AM - 7 PM", IsActive = true, CreatedAt = DateTime.UtcNow }
                };
                context.Dealerships.AddRange(dealerships);
                context.SaveChanges();

                // Seed Users
                var users = new List<User>
                {
                    new User { Name = "Admin", Email = "admin@example.com", PasswordHash = "admin123", PhoneNumber = "555-9999", Address = "789 Pine St", Role = "Admin", RefreshToken = "default-refresh-token", CreatedAt = DateTime.UtcNow },
                    new User { Name = "User", Email = "user@example.com", PasswordHash = "user123", PhoneNumber = "555-8888", Address = "101 Elm St", Role = "User", RefreshToken = "default-refresh-token", CreatedAt = DateTime.UtcNow }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // Seed Cars
                var cars = new List<Car>
                {
                    new Car
                    {
                        Name = "Toyota Camry",
                        Description = "Reliable and fuel-efficient sedan",
                        Price = 25000.00M,
                        Horsepower = 203,
                        Torque = 184,
                        TopSpeed = 135,
                        Acceleration = 7.5M,
                        FuelEfficiency = 28,
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        ImagePath = "toyota-camry.jpg",
                        IsAvailable = true,
                        Category = categories[0],
                        Dealership = dealerships[0],
                        CreatedAt = DateTime.UtcNow
                    },
                    new Car
                    {
                        Name = "Honda CR-V",
                        Description = "Spacious and practical SUV",
                        Price = 30000.00M,
                        Horsepower = 190,
                        Torque = 179,
                        TopSpeed = 125,
                        Acceleration = 8.2M,
                        FuelEfficiency = 28,
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        ImagePath = "honda-cr-v.jpg",
                        IsAvailable = true,
                        Category = categories[1],
                        Dealership = dealerships[0],
                        CreatedAt = DateTime.UtcNow
                    },
                    new Car
                    {
                        Name = "Porsche 911",
                        Description = "High-performance sports car",
                        Price = 100000.00M,
                        Horsepower = 379,
                        Torque = 331,
                        TopSpeed = 182,
                        Acceleration = 4.2M,
                        FuelEfficiency = 20,
                        FuelType = "Gasoline",
                        Transmission = "Automatic",
                        ImagePath = "porsche-911.jpg",
                        IsAvailable = true,
                        Category = categories[2],
                        Dealership = dealerships[1],
                        CreatedAt = DateTime.UtcNow
                    }
                };
                context.Cars.AddRange(cars);
                context.SaveChanges();

                // Seed MaintenanceRecords
                var maintenanceRecords = new List<MaintenanceRecord>
                {
                    new MaintenanceRecord
                    {
                        Car = cars[0],
                        ServiceType = "Oil Change",
                        Description = "Regular oil change and filter replacement",
                        Cost = 50.00M,
                        Mileage = 5000,
                        ServiceProvider = "AutoMax Service Center",
                        MaintenanceDate = DateTime.UtcNow.AddDays(-30),
                        Notes = "Regular maintenance completed",
                        CreatedAt = DateTime.UtcNow
                    },
                    new MaintenanceRecord
                    {
                        Car = cars[1],
                        ServiceType = "Brake Service",
                        Description = "Brake pad replacement and rotor resurfacing",
                        Cost = 300.00M,
                        Mileage = 15000,
                        ServiceProvider = "CarWorld Service Center",
                        MaintenanceDate = DateTime.UtcNow.AddDays(-15),
                        Notes = "Brake system maintenance completed",
                        CreatedAt = DateTime.UtcNow
                    }
                };
                context.MaintenanceRecords.AddRange(maintenanceRecords);
                context.SaveChanges();

                // Seed Orders
                var orders = new List<Order>
                {
                    new Order
                    {
                        User = users[1],
                        Car = cars[0],
                        TotalAmount = 25000.00M,
                        Status = "Completed",
                        PaymentMethod = "Credit Card",
                        TransactionId = "TRX123456",
                        OrderDate = DateTime.UtcNow.AddDays(-10),
                        CompletionDate = DateTime.UtcNow.AddDays(-5),
                        Notes = "Order completed successfully"
                    },
                    new Order
                    {
                        User = users[1],
                        Car = cars[1],
                        TotalAmount = 30000.00M,
                        Status = "Pending",
                        PaymentMethod = "Bank Transfer",
                        TransactionId = "TRX789012",
                        OrderDate = DateTime.UtcNow.AddDays(-2),
                        Notes = "Awaiting payment confirmation"
                    }
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();

                // Seed Reviews
                var reviews = new List<Review>
                {
                    new Review { UserId = users[1].Id, CarId = cars[0].Id, Rating = 5, Comment = "Great car, very reliable!", CreatedAt = DateTime.UtcNow, IsVerified = true },
                    new Review { UserId = users[1].Id, CarId = cars[1].Id, Rating = 4, Comment = "Good SUV, comfortable ride.", CreatedAt = DateTime.UtcNow, IsVerified = true }
                };
                context.Reviews.AddRange(reviews);
                context.SaveChanges();
            }
        }
    }
} 