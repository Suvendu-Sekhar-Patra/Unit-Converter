# Unit Conversion API

This project is an ASP.NET Core Web API for converting numerical values between different units of measurement (Length, Temperature, and Mass). It is designed with a scalable architecture, using a "Base Unit Conversion Pattern" to easily accommodate new units and categories in the future.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed.
- An IDE such as Visual Studio, VS Code, or JetBrains Rider.

## Project Structure

The project is structured into three main layers to separate concerns cleanly:

- **`UnitConversionApi.Core`**: Contains domain models (`UnitDefinition`, `UnitCategory`), interfaces (`IUnitRegistry`, `IConversionService`), and custom exceptions.
- **`UnitConversionApi.Services`**: Implements the business logic. It contains the `UnitRegistry` (which hardcodes the currently supported units) and the `ConversionService`.
- **`UnitConversionApi`**: The ASP.NET Core Web API that contains the `ConversionController` and a global exception handler.

## How to Run Locally

1. Open a terminal or command prompt and navigate to the `UnitConversionApi` project folder:
   ```bash
   cd UnitConversionApi
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. The API will start on a local port (usually `http://localhost:5147` or `https://localhost:7147`).

4. Open a web browser and navigate to the Swagger UI to view and test the endpoints interactively:
   ```
   http://localhost:5147/swagger
   ```

## API Endpoints

### 1. Convert Units
**GET** `/api/v1/conversion/convert`

Converts a value from one unit to another.

**Query Parameters:**
- `value` (double): The numerical value to convert.
- `from` (string): The symbol of the source unit.
- `to` (string): The symbol of the target unit.

**Example Request:**
```
GET /api/v1/conversion/convert?value=100&from=c&to=f
```

**Example Response (200 OK):**
```json
{
  "value": 100,
  "from": "c",
  "to": "f",
  "result": 212
}
```

**Example Error Response (400 Bad Request):**
```json
{
  "StatusCode": 400,
  "Message": "Cannot convert between different categories (Length to Temperature)."
}
```

### 2. List Supported Units
**GET** `/api/v1/conversion/units`

Returns a list of all currently supported units and their symbols.

**Example Request:**
```
GET /api/v1/conversion/units
```

**Example Response (200 OK):**
```json
[
  {
    "name": "Meter",
    "symbol": "m",
    "category": "Length"
  },
  {
    "name": "Celsius",
    "symbol": "c",
    "category": "Temperature"
  }
]
```

## Adding New Units

Due to the architectural design, adding a new unit is simple. You only need to define its conversion to and from the **Base Unit** of its category.

For example, to add `Nautical Miles` (Category: Length, Base Unit: Meter), you only need to add one line to the `InitializeUnits` method in `UnitRegistry.cs`:

```csharp
AddUnit(new UnitDefinition { 
    Name = "Nautical Mile", 
    Symbol = "nmi", 
    Category = UnitCategory.Length, 
    ToBase = x => x * 1852, 
    FromBase = x => x / 1852 
});
```

The `ConversionService` will automatically be able to convert Nautical Miles to and from any other length unit!
