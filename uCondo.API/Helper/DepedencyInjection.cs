using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uCondo.Application.Handlers;
using uCondo.Data.Repositories;
using uCondo.Domain.Entities;
using uCondo.Domain.Repositories;

namespace uCondo.API.Helper
{
    public static class DepedencyInjection
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region REPOSITORIES
            services.AddTransient(typeof(IGenericRepository<Bill>), typeof(GenericRepository<Bill>));
            services.AddTransient(typeof(IGenericRepository<BillType>), typeof(GenericRepository<BillType>));
            #endregion

            #region HANDLERS
            services.AddMediatR(typeof(Startup));
            services.AddMediatR(typeof(CreateBillCommandHandler));
            services.AddMediatR(typeof(SuggestNextBillCodeCommandHandler));
            #endregion
        }
    }
}
