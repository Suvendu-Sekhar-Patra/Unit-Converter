using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Core.Interfaces;
using UnitConversionApi.Core.Models;

namespace UnitConversionApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ConversionController : ControllerBase
{
    private readonly IConversionService _conversionService;
    private readonly IUnitRegistry _unitRegistry;

    public ConversionController(IConversionService conversionService, IUnitRegistry unitRegistry)
    {
        _conversionService = conversionService;
        _unitRegistry = unitRegistry;
    }

    /// <summary>
    /// Converts a value from one unit to another.
    /// </summary>
    /// <param name="value">The numerical value to convert.</param>
    /// <param name="from">The symbol of the source unit (e.g., m, c, kg).</param>
    /// <param name="to">The symbol of the target unit (e.g., ft, f, lb).</param>
    /// <returns>The converted value.</returns>
    [HttpGet("convert")]
    public IActionResult Convert([FromQuery] double value, [FromQuery] string from, [FromQuery] string to)
    {
        if (string.IsNullOrWhiteSpace(from))
            return BadRequest("The 'from' parameter is required.");
        
        if (string.IsNullOrWhiteSpace(to))
            return BadRequest("The 'to' parameter is required.");

        var result = _conversionService.Convert(value, from, to);
        
        return Ok(new 
        { 
            Value = value, 
            From = from, 
            To = to, 
            Result = result 
        });
    }

    /// <summary>
    /// Retrieves all supported units for conversion.
    /// </summary>
    /// <returns>A list of available units.</returns>
    [HttpGet("units")]
    public IActionResult GetUnits()
    {
        var units = _unitRegistry.GetAllUnits()
            .Select(u => new 
            { 
                u.Name, 
                u.Symbol, 
                Category = u.Category.ToString() 
            })
            .ToList();
            
        return Ok(units);
    }
}
