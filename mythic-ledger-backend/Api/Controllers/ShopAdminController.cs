using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mythic_ledger_backend.Application.DTOs;
using mythic_ledger_backend.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace mythic_ledger_backend.Api.Controllers
{
    [Route("api/shop-admins")]
    [ApiController]
    [Authorize(Roles = "SUPER_ADMIN")]
    public class ShopAdminsController : ControllerBase
    {
        private readonly ILogger<ShopAdminsController> _logger;
        private readonly IShopAdminService _shopAdminService;

        public ShopAdminsController(ILogger<ShopAdminsController> logger, IShopAdminService shopAdminService)
        {
            _logger = logger;
            _shopAdminService = shopAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var shopAdmins = await _shopAdminService.GetAllShopAdmins();
                return Ok(shopAdmins);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching shop admins. Our team has been notified."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShopAdminRequestDto requestModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.Email) || string.IsNullOrWhiteSpace(requestModel.Password))
                {
                    return BadRequest(new { message = "Email and Password are strictly required." });
                }

                var newShopAdmin = await _shopAdminService.CreateShopAdmin(requestModel);

                return StatusCode(StatusCodes.Status201Created, newShopAdmin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while creating the shop admin. Our team has been notified."
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateShopAdminRequestDto requestModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.Email))
                {
                    return BadRequest(new { message = "Email is strictly required." });
                }
                var updatedShopAdmin = await _shopAdminService.UpdateShopAdmin(id, requestModel);

                return Ok(updatedShopAdmin);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while updating the shop admin. Our team has been notified."
                });
            }
        }
    }
}