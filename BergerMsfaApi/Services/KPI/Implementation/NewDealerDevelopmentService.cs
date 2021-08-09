﻿using AutoMapper;
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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.Implementation
{
    public class NewDealerDevelopmentService : INewDealerDevelopmentService
    {
        public readonly IRepository<NewDealerDevelopment> _repository;
        public readonly IRepository<DealerInfo> _dealerInfo;
      

        public NewDealerDevelopmentService(
            IRepository<NewDealerDevelopment> repository,
            IRepository<DealerInfo> dealerInfo

            )
        {
            _repository = repository;
            _dealerInfo = dealerInfo;


        }

        public async Task<int> AddNewDealerDevelopmentAsync(IList<NewDealerDevelopment> model)
        {
            
            var count = model.Where(p => p.Id > 0).Count();
            if (count < 0)
            {
                var result =await _repository.CreateListAsync(model.ToList());
            }
            else
            {
                var result = await _repository.UpdateListAsync(model.ToList());

            }

            return count;
        }


        public async Task<bool> AddDealerConversionAsync(IList<NewDealerDevelopmentSaveModel> model)
        {
            var result = false;
            var lstNewDealerConversion = new List<NewDealerDevelopment>();
            foreach (var item in model)
            {
                var data = _repository.Where(p => p.Id == item.Id).AsNoTracking().FirstOrDefault();
                if (data != null)
                {
                    data.NumberofConvertedfromCompetition = item.NumberofConvertedfromCompetition;
                    lstNewDealerConversion.Add(data);
                    //var res = _repository.UpdateAsync(data);
                    result = true;

                }

            }
            var res =await _repository.UpdateListAsync(lstNewDealerConversion);

            return result;
        }

        public async Task<IList<NewDealerDevelopment>> GetNewDealerDevelopmentByIdAsync(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopment>();

           
          var  currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                              p.Territory == query.Territory
                                              
                                              &&
                                             
                                              p.Year == query.Year )
                                              
                                                                            .ToListAsync();

          var  nextYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                              p.Territory == query.Territory 
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


        public async Task<IList<NewDealerDevelopmentModel>> GetNewDealerDevelopment(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopmentModel>();

            var monthNumber = 0;
            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory &&
                                               p.Year == query.Year).ToListAsync();

            var nextYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory
                                               && p.Year == (query.Year + 1)).ToListAsync();


            currentYear.AddRange(nextYear);
            currentYear.OrderBy(p=>p.Month);

            if (currentYear.Count == 12)
            {
                foreach (var item in currentYear)
                {
                    if (item.Month >= 1 && item.Month <= 9)
                    {
                        monthNumber = item.Month + 3;
                    }

                    else if (item.Month >= 10 && item.Month <= 12)
                    {
                        monthNumber = item.Month - 9;

                    }

                    var actual = GetActualDealer(monthNumber, item.Year);

                    var result = new NewDealerDevelopmentModel()
                    {
                        MonthName= CultureInfo.CurrentCulture. DateTimeFormat.GetMonthName(monthNumber)+"-"+ GetYear(item.Year),
                        Target=item.Target,
                        Actual=actual,
                        TargetAch= GetAchivement(item.Target, actual)
                    };
                    newDealerDevelopementList.Add(result);
                }
            }

            return newDealerDevelopementList;
        }


        public async Task<IList<DealerConversionModel>> GetDealerConversion(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<DealerConversionModel>();

            var monthNumber = 0;
            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory &&
                                               p.Year == query.Year).ToListAsync();

            var nextYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory
                                               && p.Year == (query.Year + 1)).ToListAsync();


            currentYear.AddRange(nextYear);
            currentYear.OrderBy(p => p.Month);

            if (currentYear.Count == 12)
            {
                foreach (var item in currentYear)
                {
                    if (item.Month >= 1 && item.Month <= 9)
                    {
                        monthNumber = item.Month + 3;
                    }

                    else if (item.Month >= 10 && item.Month <= 12)
                    {
                        monthNumber = item.Month - 9;

                    }


                    var result = new DealerConversionModel()
                    {
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber)+ "-"+ GetYear(item.Year),
                        ConversionTarget = item.ConversionTarget,
                        NumberofConvertedfromCompetition = item.NumberofConvertedfromCompetition,
                    };
                    newDealerDevelopementList.Add(result);
                }
            }

            return newDealerDevelopementList;
        }



        private int GetActualDealer(int monthnumber,int year)
        {

            


            var monthName = CultureInfo.CurrentCulture.
            DateTimeFormat.GetMonthName
            (monthnumber);

            var count = _dealerInfo.Where(p => p.CreatedTime.Month == monthnumber && p.CreatedTime.Year == year).Count();

            return count;

        }


        public decimal GetAchivement(decimal target, decimal actual)
        {
            return target > 0 ? ((actual / target)) * 100 : decimal.Zero;
        }

        private string GetYear(int year)
        {
            DateTime expirationDate = new DateTime(year, 1, 31); // random date
            string lastTwoDigitsOfYear = expirationDate.ToString("yy");

            return lastTwoDigitsOfYear;
        }

        public async Task<IList<NewDealerDevelopmentSaveModel>> GetDealerConversionByYearAsync(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopmentSaveModel>();

            var monthNumber = 0;
            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory &&
                                               p.Year == query.Year).ToListAsync();

            var nextYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                               p.Territory == query.Territory
                                               && p.Year == (query.Year + 1)).ToListAsync();


            currentYear.AddRange(nextYear);
            currentYear.OrderBy(p => p.Month);

            if (currentYear.Count == 12)
            {
                foreach (var item in currentYear)
                {
                    if (item.Month >= 1 && item.Month <= 9)
                    {
                        monthNumber = item.Month + 3;
                    }

                    else if (item.Month >= 10 && item.Month <= 12)
                    {
                        monthNumber = item.Month - 9;

                    }

                    var actual = GetActualDealer(monthNumber, item.Year);

                    var result = new NewDealerDevelopmentSaveModel()
                    {
                        Id=item.Id,
                        Actual= actual,
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber) + "-" + GetYear(item.Year),
                        ConversionTarget = item.ConversionTarget,
                        NumberofConvertedfromCompetition = item.NumberofConvertedfromCompetition,
                    };
                    newDealerDevelopementList.Add(result);
                }
            }

            return newDealerDevelopementList;
        }
    }
}
