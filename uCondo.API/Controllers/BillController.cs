using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uCondo.Application.Commands;

namespace uCondo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : BaseController
    {
        private readonly IMediator _mediator;

        public BillController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBillCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return await BaseResponse(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("suggest-code")]
        public async Task<IActionResult> SuggestCode([FromBody] SuggestNextBillCodeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return await BaseResponse(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
