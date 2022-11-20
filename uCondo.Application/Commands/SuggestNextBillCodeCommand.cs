using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondo.Application.DTOs;

namespace uCondo.Application.Commands
{
    public class SuggestNextBillCodeCommand : IRequest<ResponseViewModel>
    {
        public SuggestNextBillCodeCommand()
        {

        }

        public Int32? FatherBill { get; set; }

        public Int32? BillType { get; set; }
    }
}
