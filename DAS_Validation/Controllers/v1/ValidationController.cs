using Azure;
using DAS_Validation.Models;
using DAS_Validation.Models.Dto;
using DAS_Validation.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace DAS_Validation.Controllers.v1
{
    [Route("api/v{version:apiVersion}/validation")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ValidationController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepo;
        protected APIResponse _response;

        public ValidationController(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
            _response = new();
        }

        [HttpPost("validate")]
        [Authorize(Roles = "user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TicketValidation([FromBody] ValidationRequestDTO model)
        {
            try
            {
                var response = await _ticketRepo.Validate(model);
                if (response.Error != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = response.Error;
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = response.TicketDTO;
                return Ok(_response);
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new { code = 0, message = "General error!" };
                return BadRequest(_response);
            }
        }

        [HttpPost("validate/batch")]
        [Authorize(Roles = "user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BatchTicketValidation([FromBody] BatchValidationRequestDTO model)
        {
            try
            {
                var response = await _ticketRepo.ValidateBatch(model);
                if (response.Error != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = response.Error;
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = response.TicketDTO;
                return Ok(_response);
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new { code = 0, message = "General error!" };
                return BadRequest(_response);
            }
        }
    }
}
