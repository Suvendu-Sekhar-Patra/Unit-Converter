# Unit Conversion API

This project is an ASP.NET Core Web API for converting numerical values between different units of measurement (Length, Temperature, and Mass). It is designed with a scalable architecture, using a "Base Unit Conversion Pattern" to easily accommodate new units and categories in the future.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed.
- An IDE such as Visual Studio, VS Code, or JetBrains Rider.

## Project Structure

The project is structured into three main layers to separate concerns cleanly:

- **`UnitConversionApi.Core`**: Contains domain models (`UnitDefinition`, `UnitCategory`), interfaces (`IUnitRegistry`, `IConversionService`), and custom exceptions.
- **`UnitConversionApi.Services`**: Implements the business logic. It contains the `UnitRegistry` (which dynamically loads supported units from a `units.json` file) and the `ConversionService`.
- **`UnitConversionApi`**: The ASP.NET Core Web API that contains the `ConversionController` and a global exception handler.

## Design Decisions & Trade-offs

- **Data-Driven Architecture (JSON vs Hardcoded)**: Instead of hardcoding conversion formulas in C# using functions, the API calculates conversions using an `Offset` and `Multiplier` formula loaded from a JSON file. This allows non-developers to configure new units without recompiling the API, ensuring massive scalability.
- **Base Unit Conversion Pattern**: Rather than defining $N \times (N-1)$ conversion pairs, the API uses a "Base Unit" for each category (e.g., Meters for Length). It converts the source unit into the base unit, and then from the base unit to the target unit. This turns an $O(N^2)$ maintenance problem into $O(N)$.
- **GET vs POST**: The `/convert` endpoint uses a `GET` request. Since conversions are safe, idempotent, and don't modify server state, `GET` enables aggressive caching at the browser and CDN levels.
- **Flat vs Wrapped Responses**: Success responses use flat, clean JSON objects matching the DTOs, while errors are securely caught by a global middleware to return standard `400 Bad Request` models without leaking stack traces.

## How to Run Locally

1. Open a terminal or command prompt and navigate to the `UnitConversionApi` project folder:
   ```bash
   cd UnitConversionApi
   ```

2. Run & watch the application:
   ```bash
   dotnet watch run
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

Due to the new data-driven architectural design, adding a new unit is simple and requires **no C# code changes**. You only need to add its definition to the `src/UnitConversionApi.Services/units.json` file.

Each unit defines its conversion to and from the **Base Unit** of its category using a mathematical Offset and Multiplier approach:
- `ToBase = (Value + BaseOffset) * BaseMultiplier`
- `FromBase = (Value / BaseMultiplier) - BaseOffset`

For example, to add `Nautical Miles` (Category: Length, Base Unit: Meter), you only need to append this JSON object to the file:

```json
{
  "name": "Nautical Mile",
  "symbol": "nmi",
  "category": "Length",
  "baseOffset": 0.0,
  "baseMultiplier": 1852.0
}
```

The `ConversionService` will automatically parse this on startup and be able to convert Nautical Miles to and from any other length unit!
