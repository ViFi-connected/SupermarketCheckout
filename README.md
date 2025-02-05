# Supermarket Checkout

Supermarket Checkout is a .NET 9 web application that provides an API for managing stock keeping units (SKUs) and calculating the total price of items in a basket, including special offers.

## Features

- Fetch all stock keeping units
- Fetch a specific stock keeping unit by ID
- Calculate the total price of items in a basket, including special offers

## Technologies

- .NET 9
- ASP.NET Core
- Swagger for API documentation
- Docker for containerization

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker

### Building and Running the Application

1. Clone the repository:

`git clone https://github.com/yourusername/SupermarketCheckout.git`

`cd SupermarketCheckout`

2. Build the Docker image:

`docker build -t supermarket-checkout .`

3. Run the Docker container:

`docker run -d -p 8080:8080 supermarket-checkout`

4. Access the application:

    Open a web browser and navigate to `http://localhost:8080` to access the application.

### API Endpoints

- `GET /checkout`: Fetch all stock keeping units
- `GET /checkout/{id}`: Fetch a specific stock keeping unit by ID
- `POST /checkout/{basket}`: Calculate the total price of items in a basket

### Example Requests

#### Fetch All Stock Keeping Units

`curl -X GET http://localhost:8080/checkout`

#### Fetch a Specific Stock Keeping Unit

`curl -X GET http://localhost:8080/checkout/A`

#### Calculate Total Price for a Basket

`curl -X POST http://localhost:8080/checkout/AAA`

### Running Tests

1. Navigate to the test project directory:

`cd SupermarketCheckout.Tests`

2. Run the tests:

`dotnet test`

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Swagger](https://swagger.io/)
- [Docker](https://www.docker.com/)
