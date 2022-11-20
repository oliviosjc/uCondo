using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondo.Domain.Entities;

namespace uCondo.Application.DTOs
{
    public class BillViewModel
    {
        public BillViewModel()
        {

        }

        public BillViewModel(Bill bill)
        {
            this.Id = bill.Id;
            this.Name = bill.Name;
            this.Code = bill.Code;
            this.AcceptReleases = bill.AcceptReleases;
        }

        public Int32 Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool AcceptReleases { get; set; }

        public List<BillViewModel> Releases { get; set; }
    }
}
