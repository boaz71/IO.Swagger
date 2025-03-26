using IO.Swagger.Controllers;
using IO.Swagger.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MathCalculator.Tests
{
    public class CalculatorControllerTests
    {
        private readonly CalculatorController _controller;

        public CalculatorControllerTests()
        {
            _controller = new CalculatorController();
        }

        [Fact]
        public void Add_TwoNumbers_ReturnsCorrectResult()
        {
            var body = new MathCalculateBody { Number1 = 5, Number2 = 3 };
            var result = _controller.Calculate(body, "add") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(8.0, GetResultValue(result));
        }

        [Fact]
        public void Subtract_TwoNumbers_ReturnsCorrectResult()
        {
            var body = new MathCalculateBody { Number1 = 10, Number2 = 4 };
            var result = _controller.Calculate(body, "subtract") as OkObjectResult;

            Assert.Equal(6.0, GetResultValue(result));
        }

        [Fact]
        public void Multiply_TwoNumbers_ReturnsCorrectResult()
        {
            var body = new MathCalculateBody { Number1 = 6, Number2 = 7 };
            var result = _controller.Calculate(body, "multiply") as OkObjectResult;

            Assert.Equal(42.0, GetResultValue(result));
        }

        [Fact]
        public void Divide_ValidInput_ReturnsCorrectResult()
        {
            var body = new MathCalculateBody { Number1 = 20, Number2 = 5 };
            var result = _controller.Calculate(body, "divide") as OkObjectResult;

            Assert.Equal(4.0, GetResultValue(result));
        }

        [Fact]
        public void Divide_ByZero_ReturnsBadRequest()
        {
            var body = new MathCalculateBody { Number1 = 10, Number2 = 0 };
            var result = _controller.Calculate(body, "divide") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Cannot divide by zero.", result.Value);
        }

        [Fact]
        public void InvalidOperation_ReturnsBadRequest()
        {
            var body = new MathCalculateBody { Number1 = 1, Number2 = 2 };
            var result = _controller.Calculate(body, "xor") as BadRequestObjectResult;

            Assert.Contains("Invalid operation type", result.Value.ToString());

        }

        [Fact]
        public void NullBody_ReturnsBadRequest()
        {
            var result = _controller.Calculate(null, "add") as BadRequestObjectResult;

            Assert.Equal("Missing body or operation type.", result.Value);
        }

        [Fact]
        public void EmptyHeader_ReturnsBadRequest()
        {
            var body = new MathCalculateBody { Number1 = 1, Number2 = 2 };
            var result = _controller.Calculate(body, "") as BadRequestObjectResult;

            Assert.Equal("Missing body or operation type.", result.Value);
        }

        [Fact]
        public void InvalidOperationType_ReturnsBadRequest()
        {
            var body = new MathCalculateBody { Number1 = 5, Number2 = 2 };
            var result = _controller.Calculate(body, "modulo") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Invalid operation type. Must be one of: add, subtract, multiply, divide.", result.Value);
        }

        [Fact]
        public void MissingOperationTypeHeader_ReturnsBadRequest()
        {
            var body = new MathCalculateBody { Number1 = 5, Number2 = 2 };
            var result = _controller.Calculate(body, "") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Missing body or operation type.", result.Value);
        }

    

        // Helper method to extract "result" value from anonymous object
        private double GetResultValue(OkObjectResult result)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(result.Value);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("result").GetDouble();
        }
    }
}
