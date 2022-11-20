using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using uCondo.Application.Commands;
using uCondo.Application.DTOs;
using uCondo.Domain.Entities;
using uCondo.Domain.Repositories;

namespace uCondo.Application.Handlers
{
    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, ResponseViewModel>
    {
        private readonly IGenericRepository<Bill> _billRepository;
        private readonly IGenericRepository<BillType> _billTypeRepository;

        public CreateBillCommandHandler(IGenericRepository<Bill> billRepository,
                                        IGenericRepository<BillType> billTypeRepository)
        {
            this._billRepository = billRepository;
            this._billTypeRepository = billTypeRepository;
        }

        public async Task<ResponseViewModel> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseViewModel();
            try
            {
                var billType = await _billTypeRepository.Get(wh => wh.Id == request.TypeId);
                if (billType is null)
                {
                    response.Message = "O tipo de conta não existe no sistema.";
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var bill = await _billRepository.Get(wh => wh.Code == request.Code);
                if(bill is not null)
                {
                    response.Message = "Já existe uma conta com este código.";
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                if(request.FatherBillId.HasValue)
                {
                    var fatherBill = await _billRepository.Get(wh => wh.Id == request.FatherBillId);
                    if (fatherBill.AcceptReleases)
                    {
                        response.Message = "A conta pai ao qual está tentando vincular não aceita filhas.";
                        response.HttpStatusCode = HttpStatusCode.UnprocessableEntity;
                        return response;
                    }


                    if (fatherBill.TypeId != request.TypeId)
                    {
                        response.Message = "Não é possível vincular contas filhas em contas pais com tipos diferentes.";
                        response.HttpStatusCode = HttpStatusCode.UnprocessableEntity;
                        return response;
                    }

                    var fatherBillCodes = fatherBill.Code.Split('.');
                    var childrenBillCodes = request.Code.Split('.');

                    bill = new Bill(request.Name, request.Code, billType, fatherBill.Id, request.AcceptReleases);

                    await _billRepository.Create(bill);
                    await _billRepository.Save();
                    response.Message = "A conta foi criada com sucesso.";
                    response.HttpStatusCode = HttpStatusCode.OK;
                    response.Data = bill;

                    return response;
                }
                else
                {
                    var fatherBillCodes = request.Code.Split('.');
                    if(fatherBillCodes.Length > 1)
                    {
                        response.Message = "Contas pai devem iniciar com somente um prefixo.";
                        response.HttpStatusCode = HttpStatusCode.UnprocessableEntity;
                        return response;
                    }

                    bill = new Bill(request.Name, request.Code, billType, null, request.AcceptReleases);

                    await _billRepository.Create(bill);
                    await _billRepository.Save();
                    response.Message = "A conta foi criada com sucesso.";
                    response.HttpStatusCode = HttpStatusCode.OK;
                    response.Data = bill;

                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Ocorreu um erro interno no servidor.";
                response.Data = ex;
                response.HttpStatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }
    }
}
