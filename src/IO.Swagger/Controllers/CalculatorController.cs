using IO.Swagger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;

namespace IO.Swagger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase

    {


        /// <summary>
        /// פעולה מתמטית בין שני מספרים לפי סוג פעולה שמגיע ב-Header
        /// </summary>
        /// <param name="body">שני מספרים: number1 ו-number2</param>
        /// <param name="xOperationType">סוג פעולה: add / subtract / multiply / divide</param>
        /// <returns>תוצאה מספרית או הודעת שגיאה</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("calculate")]
        [Authorize]
        public IActionResult Calculate([FromBody] MathCalculateBody body, [FromHeader(Name = "X-Operation-Type")] string xOperationType)
        {
            //בדיקת תקינות הקלט
            if (body == null || string.IsNullOrWhiteSpace(xOperationType))
                return BadRequest("Missing body or operation type.");



            // ניסיון להמיר את המחרוזת ל־enum באופן בטוח
            if (!Enum.TryParse<OperationType>(xOperationType, ignoreCase: true, out var operation))
                return BadRequest("Invalid operation type. Must be one of: add, subtract, multiply, divide.");



            double result;

            try
            {
                // פעולה לפי ה־enum שהתקבל
                switch (operation)
                {
                    case  OperationType.Add:
                        result = (double)(body.Number1 + body.Number2);
                        break;
                    case OperationType.Subtract :
                        result = (double)(body.Number1 - body.Number2);
                        break;
                    case OperationType.Multiply:
                        result = (double)(body.Number1 * body.Number2);
                        break;
                    case OperationType.Divide:
                        if (body.Number2 == 0)
                            return BadRequest("Cannot divide by zero.");
                        result = (double)(body.Number1 / body.Number2);
                        break;
                    default:
                        return BadRequest("Invalid operation type.");
                }

                // מחזיר את התוצאה בפורמט JSON
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                // במקרה של חריגה מסוג Exception מחזיר תשובת שגיאה
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
