using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uCondo.Domain.Entities
{
    public class Bill : Base
    {
        public Bill()
        {

        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        public Int32 TypeId { get; private set; }

        public virtual BillType Type { get; private set; }

        public Int32? ParentBillId { get; private set; }

        public bool AcceptReleases { get; private set; }

        public Bill(string name, string code, BillType type, Int32? parentBillId, bool acceptReleases)
        {
            this.SetName(name);
            this.SetCode(code);
            this.SetType(type);
            this.SetParentBill(parentBillId);
            this.SetAcceptReleases(acceptReleases);
        }

        public void ValidateRelationship(Bill firstBill, Bill secondBill)
        {
            if (firstBill.AcceptReleases || secondBill.AcceptReleases)
            {
                if (firstBill.ParentBillId is not null)
                {
                    if (firstBill.ParentBillId == secondBill.Id)
                        throw new Exception("A conta que aceita lançamentos não pode conter filhas.");
                }
                else if(secondBill.ParentBillId is not null)
                {
                    if(secondBill.ParentBillId == firstBill.Id)
                        throw new Exception("A conta que aceita lançamentos não pode conter filhas.");
                }
            }
        }

        private void SetName(string name)
        {
            this.Name = name;
        }

        private void SetCode(string code)
        {
            this.Code = code;
        }

        private void SetType(BillType type)
        {
            this.TypeId = type.Id;
        }

        private void SetParentBill(Int32? parentBillId)
        {
            this.ParentBillId = parentBillId;
        }

        private void SetAcceptReleases(bool acceptReleases)
        {
            this.AcceptReleases = acceptReleases;
        }
    }
}
