using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using uCondo.Application.DTOs;

namespace uCondo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<IActionResult> BaseResponse(ResponseViewModel model)
        {
            switch (model.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await Task.FromResult(Ok(model));
                    }

                case HttpStatusCode.BadRequest:
                    {
                        return await Task.FromResult(BadRequest(model));
                    }

                case HttpStatusCode.NoContent:
                    {
                        return await Task.FromResult(NoContent());
                    }

                case HttpStatusCode.UnprocessableEntity:
                    {
                        return await Task.FromResult(UnprocessableEntity(model));
                    }

                default:
                    {
                        return await Task.FromResult(StatusCode(500));
                    }
            }
        }
    }
}
