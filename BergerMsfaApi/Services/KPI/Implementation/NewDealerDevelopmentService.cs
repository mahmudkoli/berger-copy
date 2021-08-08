using AutoMapper;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.KPI;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Scheme;
using Berger.Odata.Services;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Scheme;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.KPI.interfaces;
using BergerMsfaApi.Services.Scheme.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.Implementation
{
    public class NewDealerDevelopmentService : INewDealerDevelopmentService
    {
        public readonly IRepository<NewDealerDevelopment> _repository;
      

        public NewDealerDevelopmentService(
            IRepository<NewDealerDevelopment> repository
          
            )
        {
            _repository = repository;


        }

        public async Task<int> AddNewDealerDevelopmentAsync(IList<NewDealerDevelopment> model)
        {
            
            var count = model.Where(p => p.Id > 0).Count();
            if (count > 0)
            {
                var result =await _repository.CreateListAsync(model.ToList());
            }
            else
            {
                var result = await _repository.UpdateListAsync(model.ToList());

            }

            return count;
        }

        public async Task<IList<NewDealerDevelopment>> GetNewDealerDevelopmentByIdAsync(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopment>();

           
          var  currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                              p.Territory == query.Territory
                                              
                                              &&

                                              //p.Month==(int)MonthEnum.Apr && 
                                              //p.Month==(int)MonthEnum.May && 
                                              //p.Month==(int)MonthEnum.Jun && 
                                              //p.Month==(int)MonthEnum.Jul && 
                                              //p.Month==(int)MonthEnum.Aug && 
                                              //p.Month==(int)MonthEnum.Sep && 
                                              //p.Month==(int)MonthEnum.Oct && 
                                              //p.Month==(int)MonthEnum.Nov && 
                                              //p.Month==(int)MonthEnum.Dec && 
                                             
                                              p.Year == query.Year )
                                              
                                                                            .ToListAsync();

          var  nextYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                              p.Territory == query.Territory 
                                              
                                              //&&
                                             
                                              //p.Month == (int)MonthEnum.Jan &&
                                              //p.Month == (int)MonthEnum.Feb &&
                                              //p.Month == (int)MonthEnum.Mar 
                                              && p.Year == (query.Year + 1))
                                                                            .ToListAsync();


            newDealerDevelopementList.AddRange(currentYear);
            newDealerDevelopementList.AddRange(nextYear);

            if (newDealerDevelopementList.Count ==0)
            {
                for (int i = 1; i <=12; i++)
                {
                    var res = new NewDealerDevelopment()
                    {
                        BusinessArea=query.Depot,
                        Territory=query.Territory,
                        Month=i,
                        Year=query.Year,
                        ConversionTarget=0,
                        Target=0
                    };
                    if (i > 9)
                    {
                        res.Year = query.Year+1;
                    }
                    newDealerDevelopementList.Add(res);
                }
            }

            return newDealerDevelopementList;
        }
    }
}
