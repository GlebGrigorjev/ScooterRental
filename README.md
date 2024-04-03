# Scooter Rental Application 🛴 🛴 🛴 

This is a simple application for managing scooter rentals. It allows users to rent scooters, calculate rental costs based on usage, and track rental profits.

## How It's Made:

The application is built using C# and follows object-oriented principles. It consists of several classes/interfaces:

- **Scooter**: Represents a scooter with an ID and price per minute.
- **ScooterService**: Manages the creation, retrieval, and removal of scooters.
- **RentedScooter**: Records details of scooter rentals, including start/end times and rental fees.
- **RentedScooterArchive**: Stores rental records and provides methods for managing rentals.
- **ProfitArchive**: Stores profits earned from scooter rentals.
- **RentalCalculatorService**: Calculates rental costs based on rental duration and price per minute.
- **RentalCompany**: Represents a rental company and provides methods for starting/ending rentals and calculating income.

Unit tests are implemented using the MSTest framework and Moq for mocking dependencies.

## Future Improvements:

1. **User Interface**: Develop a user-friendly interface for easier interaction with the application.
2. **Database Integration**: Integrate a database to persist scooter, rental, and profit data for scalability and data consistency.
3. **Authentication and Authorization**: Implement authentication and authorization mechanisms to secure access to rental operations.
4. **Advanced Pricing Models**: Introduce more complex pricing models, such as discounts for long-term rentals or dynamic pricing based on demand.

## Lessons Learned:

- **Test-Driven Development (TDD)**: Practice writing unit tests before implementing functionality to ensure code quality and maintainability.
- **Dependency Injection (DI)**: Utilize dependency injection to decouple components and improve testability and flexibility.
- **Exception Handling**: Implement error handling to handle unexpected scenarios and improve application reliability.
- **Mocking Frameworks**: Explore mocking frameworks like Moq to simplify unit testing by isolating dependencies and simulating behavior.
