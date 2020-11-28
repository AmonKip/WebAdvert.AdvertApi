using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdertApi.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertStorageService _advertStorageService;

        public AdvertController(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type=typeof(CreateAdvertResponse))]
        public async Task<IActionResult> CreateAdvert(AdvertModel model)
        {
            string recordId;

            try
            {
                recordId = await _advertStorageService.Add(model);
            }
            catch (KeyNotFoundException)
            {
                
                return new NotFoundResult();
            }
            catch(Exception exp)
            {
                return StatusCode(500, exp.Message);
            }

            return StatusCode(201, new CreateAdvertResponse { Id = recordId });
        }
        
        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
        {
            try
            {
                await _advertStorageService.ConfirmModel(model);
            }
            catch(KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }

            return new OkResult();
        }
    }
}