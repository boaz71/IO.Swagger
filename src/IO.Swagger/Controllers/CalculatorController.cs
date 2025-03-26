using IO.Swagger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace IO.Swagger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase

    {

   
        /// <summary>
        /// Performs a mathematical operation on two numbers.
        /// </summary>
        /// <param name="body">The input numbers.</param>
        /// <param name="xOperationType">The operation type (add, subtract, multiply, divide).</param>
        /// <returns>The result of the operation.</returns>
        /// 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("calculate")]
        [Authorize]
        public IActionResult Calculate([FromBody] MathCalculateBody body, [FromHeader(Name = "X-Operation-Type")] string xOperationType)
        {
            if (body == null || string.IsNullOrWhiteSpace(xOperationType))
                return BadRequest("Missing body or operation type.");

            double result;

            try
            {
                switch (xOperationType.ToLower())
                {
                    case "add":
                        result = (double)(body.Number1 + body.Number2);
                        break;
                    case "subtract":
                        result = (double)(body.Number1 - body.Number2);
                        break;
                    case "multiply":
                        result = (double)(body.Number1 * body.Number2);
                        break;
                    case "divide":
                        if (body.Number2 == 0)
                            return BadRequest("Cannot divide by zero.");
                        result = (double)(body.Number1 / body.Number2);
                        break;
                    default:
                        return BadRequest("Invalid operation type.");
                }

                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
