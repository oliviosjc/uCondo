using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondo.Application.DTOs;

namespace uCondo.Application.Commands
{
    public class CreateBillCommand : IRequest<ResponseViewModel>
    {
        public CreateBillCommand()
        {

        }

        public string Name { get; set; }

        public string Code { get; set; }

        public Int32 TypeId { get; set; }

        public Int32? FatherBillId { get; set; }

        public bool AcceptReleases { get; set; }
    }
}
