using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mythic_ledger_backend.Application.DTOs;
using mythic_ledger_backend.Application.Interfaces;
using System.Security.Claims;

namespace mythic_ledger_backend.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;

        public CustomersController(ILogger<CustomersController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string GetUserRole() => User.FindFirstValue(ClaimTypes.Role)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllCustomers(GetUserId(), GetUserRole());
                return Ok(customers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching customers."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id, GetUserId(), GetUserRole());

                if (customer == null)
                {
                    return NotFound(new { message = "Customer not found." });
                }

                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching the customer."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequestDto requestModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.IngameId) || string.IsNullOrWhiteSpace(requestModel.ZoneId))
                {
                    return BadRequest(new { message = "IngameId and ZoneId are strictly required." });
                }

                var newCustomer = await _customerService.CreateCustomer(GetUserId(), requestModel);

                return StatusCode(StatusCodes.Status201Created, newCustomer);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while creating the customer."
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCustomerRequestDto requestModel)
        {
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomer(id, GetUserId(), GetUserRole(), requestModel);
                return Ok(updatedCustomer);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while updating the customer."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _customerService.DeleteCustomer(id, GetUserId(), GetUserRole());
                return Ok(new { message = "Customer deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while deleting the customer."
                });
            }
        }
    }
}
