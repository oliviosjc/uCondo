using System;
using System.Collections.Generic;
using System.Text;

namespace uCondo.Domain.Entities
{
    public class BillType : Base
    {
        public BillType()
        {

        }

        public string Name { get; private set; }

        public string PrefixDefault { get; private set; }

        public virtual ICollection<Bill> Bills { get; set; }

        public BillType(string name, string code)
        {
            this.SetName(name);
        }

        private void SetName(string name)
        {
            this.Name = name;
        }
    }
}
