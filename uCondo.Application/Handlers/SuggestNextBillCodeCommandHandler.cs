using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uCondo.Application.Commands;
using uCondo.Application.DTOs;
using uCondo.Domain.Entities;
using uCondo.Domain.Repositories;

namespace uCondo.Application.Handlers
{
    public class SuggestNextBillCodeCommandHandler : IRequestHandler<SuggestNextBillCodeCommand, ResponseViewModel>
    {
        private readonly IGenericRepository<Bill> _billRepository;

        public SuggestNextBillCodeCommandHandler(IGenericRepository<Bill> billRepository)
        {
            this._billRepository = billRepository;
        }

        public async Task<ResponseViewModel> Handle(SuggestNextBillCodeCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseViewModel();
            try
            {
                if (request.FatherBill is null)
                {
                    var fatherBills = await _billRepository.GetAll(wh => wh.ParentBillId == null
                                                                && wh.AcceptReleases == false, null);

                    if (!fatherBills.Any())
                    {

                        response.Message = "Está é sua primeira conta PAI.";
                        response.Data = "1.";
                        response.HttpStatusCode = HttpStatusCode.OK;
                        return response;
                    }
                    else
                    {
                        response.Message = "Como o usuário não selecionou nenhuma conta Pai, o código sugerido é o valor do último criado + 1";
                        response.Data = this.SuggestNextCode(new BillViewModel(fatherBills.Last()), null ,true, fatherBills.Select(sl => new BillViewModel(sl)).ToList());

                        response.HttpStatusCode = HttpStatusCode.OK;
                        return response;
                    }
                }
                else
                {
                    var billFather = await _billRepository.Get(wh => wh.Id == request.FatherBill.Value
                                                               && wh.AcceptReleases == false);

                    if (billFather is null)
                    {
                        response.Message = "A conta pai enviada não existe ou não pode conter contas filhas.";
                        response.HttpStatusCode = HttpStatusCode.UnprocessableEntity;
                        return response;
                    }
                    else
                    {
                        var fatherBill = await _billRepository.Get(wh => wh.Id == request.FatherBill.Value);
                        var childrenBills = await _billRepository.GetAll(wh => wh.ParentBillId == fatherBill.Id, null);

                        var bill = new BillViewModel(fatherBill);
                        bill.Releases = await GenerateReleases(childrenBills.ToList());

                        if (bill.Releases.Any())
                            response.Data = this.SuggestNextCode(bill.Releases.Last(), bill.Releases ,false, null);
                        else
                            response.Data = this.SuggestNextCode(new BillViewModel(fatherBill), null ,false, null);


                        response.HttpStatusCode = HttpStatusCode.OK;
                        response.Message = "Foi identificado a última conta filha da conta selecionada.";

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Ocorreu um erro interno no servidor.";
                response.HttpStatusCode = HttpStatusCode.InternalServerError;
                response.Data = ex;
                return response;
            }
        }

        private async Task<List<BillViewModel>> GenerateReleases(List<Bill> childrens)
        {
            List<BillViewModel> response = new List<BillViewModel>();

            foreach (var children in childrens)
            {
                var subChildrens = await _billRepository.GetAll(wh => wh.ParentBillId == children.Id, null);
                BillViewModel bill = new BillViewModel
                {
                    Id = children.Id,
                    Name = children.Name,
                    AcceptReleases = children.AcceptReleases,
                    Code = children.Code,
                    Releases = await GenerateReleases(subChildrens.ToList())
                };

                response.Add(bill);
            }

            return response;
        }

        private string SuggestNextCode(BillViewModel lastBill, List<BillViewModel> childrens ,bool isFather, List<BillViewModel> fathers)
        {
            string result = "";
            var codes = lastBill.Code.Split('.').ToList();
            var i = 0;

            if (childrens != null)
                if (childrens.Any())
                    if (childrens.Count > 1)
                        if (Convert.ToInt32(childrens.Last().Code.Split('.').Last()) != 1)
                            childrens = childrens.Where(wh => wh.Id != lastBill.Id).ToList();

            if (fathers != null)
                if (fathers.Any())
                    if(fathers.Count > 1)
                    fathers = fathers.Where(wh => wh.Id != lastBill.Id).ToList();

            foreach (var code in codes)
            {
                i += 1;
                if (i == codes.Count())
                {
                    if (isFather is false)
                    {
                        if (childrens != null)
                        {
                            if (childrens.Any())
                            {
                                var childrenCode = Convert.ToInt32(childrens.Last().Code.Split('.').Last());

                                if (Convert.ToInt32(code) - childrenCode == 1)
                                {
                                    var nextNumber = Convert.ToInt32(code) + 1;
                                    if (nextNumber < 999)
                                        result += nextNumber;
                                    else
                                        result = lastBill.Code + ".1";
                                }
                                else
                                {
                                    var nextNumber = Convert.ToInt32(childrens.Last().Code.Split('.').Last()) + 1;
                                    if (nextNumber < 999)
                                        result += nextNumber;
                                }
                            }
                        }

                        if(childrens is null)
                        {
                            result += Convert.ToInt32(lastBill.Code) + "." + 1;
                        }
                    }
                    else
                    {
                        var fatherCode = Convert.ToInt32(fathers.Last().Code);
                        if(Convert.ToInt32(code) - fatherCode == 1)
                        {
                            var nextNumber = Convert.ToInt32(code) + 1;
                            if (nextNumber < 999)
                                result += nextNumber;
                            else
                                result = "Você atingiu o limite máximo de 999 contas pai.";
                        }
                        else
                        {
                            var nextNumber = fatherCode + 1;
                            if (nextNumber < 999)
                                result += nextNumber;
                        }
                    }
                }
                else
                    result += code + ".";
            }

            return result;
        }
    }
}
