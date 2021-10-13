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
            if (count == 0)
            {
                var result = await _repository.CreateListAsync(model.ToList());
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
            var res = await _repository.UpdateListAsync(lstNewDealerConversion);

            return result;
        }

        public async Task<IList<NewDealerDevelopment>> GetNewDealerDevelopmentByIdAsync(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopment>();

            var monthNumber = 0;

            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                              p.Territory == query.Territory &&
                                              p.FiscalYear == query.Year).ToListAsync();

            newDealerDevelopementList.AddRange(currentYear);

            if (newDealerDevelopementList.Count == 0)
            {
                for (int i = 1; i <= 12; i++)
                {
                    monthNumber = i + 3;
                    var res = new NewDealerDevelopment()
                    {
                        BusinessArea = query.Depot,
                        Territory = query.Territory,
                        Month = i,
                        Year = query.Year,
                        FiscalYear = query.Year,
                        ConversionTarget = 0,
                        Target = 0
                    };
                    if (i > 9)
                    {
                        monthNumber = i - 9;
                        res.Year = query.Year + 1;
                    }
                    res.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber) + "-" + GetYear(res.Year);
                    newDealerDevelopementList.Add(res);
                }
            }

            else
            {
                foreach (var item in newDealerDevelopementList)
                {
                    monthNumber = item.Month + 3;
                    if (item.Month > 9)
                    {
                        monthNumber = item.Month - 9;

                    }

                    item.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber) + "-" + GetYear(item.Year);
                }
            }

            return newDealerDevelopementList;
        }


        public async Task<IList<NewDealerDevelopmentModel>> GetNewDealerDevelopmentReport(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<NewDealerDevelopmentModel>();

            var monthNumber = 0;
            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                             p.Territory == query.Territory &&
                                             p.FiscalYear == query.Year).ToListAsync();

            if (!currentYear.Any())
            {
                currentYear = new List<NewDealerDevelopment>();
                var monthCount = 12;
                for (int i = 1; i <= monthCount; i++)
                {
                    currentYear.Add(new NewDealerDevelopment { Month=i,Year=i>9?query.Year+1:query.Year });
                }
            }


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

                    var actual = await GetActualDealerAsync(monthNumber, item.Year,query.Depot, query.Territory);

                    var result = new NewDealerDevelopmentModel()
                    {
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber) + "-" + GetYear(item.Year),
                        Target = item.Target,
                        Actual = actual,
                        TargetAch = GetAchivement(item.Target, actual)
                    };
                    newDealerDevelopementList.Add(result);
                }

                var totalSet = new NewDealerDevelopmentModel()
                {
                    MonthName = "Total",
                    Target = newDealerDevelopementList.Sum(p=>p.Target),
                    Actual = newDealerDevelopementList.Sum(p => p.Actual),
                    TargetAch = GetAchivement(newDealerDevelopementList.Sum(p => p.Target), newDealerDevelopementList.Sum(p => p.Actual))
                };
                newDealerDevelopementList.Add(totalSet);
            }

            return newDealerDevelopementList;
        }


        public async Task<IList<DealerConversionModel>> GetDealerConversionReport(SearchNewDealerDevelopment query)
        {
            var newDealerDevelopementList = new List<DealerConversionModel>();

            var monthNumber = 0;
            var currentYear = await _repository.Where(p => p.BusinessArea == query.Depot &&

                                             p.Territory == query.Territory &&
                                             p.FiscalYear == query.Year).ToListAsync();

            if (!currentYear.Any())
            {
                currentYear = new List<NewDealerDevelopment>();
                var monthCount = 12;
                for (int i = 1; i <= monthCount; i++)
                {
                    currentYear.Add(new NewDealerDevelopment { Month = i, Year = i > 9 ? query.Year + 1 : query.Year });
                }
            }


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
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber) + "-" + GetYear(item.Year),
                        ConversionTarget = item.ConversionTarget,
                        NumberOfConvertedFromCompetition = item.NumberofConvertedfromCompetition,
                    };
                    newDealerDevelopementList.Add(result);
                }


                //var totalSet = new DealerConversionModel()
                //{
                //    MonthName = "Total",
                //    ConversionTarget = newDealerDevelopementList.Sum(p=>p.ConversionTarget),
                //    NumberofConvertedfromCompetition = newDealerDevelopementList.Sum(p => p.NumberofConvertedfromCompetition),
                //};
                //newDealerDevelopementList.Add(totalSet);
            }

            return newDealerDevelopementList;
        }



        private async Task<int> GetActualDealerAsync(int monthnumber, int year, string depot, string territory)
        {




            var monthName = CultureInfo.CurrentCulture.
            DateTimeFormat.GetMonthName
            (monthnumber);

            //var count = _dealerInfo.Where(p => CustomConvertExtension.ObjectToDateTime(p.CreatedOn).Month == monthnumber && CustomConvertExtension.ObjectToDateTime(p.CreatedOn).Year == year &&
            //                                   p.Channel == ConstantsODataValue.DistrbutionChannelDealer &&
            //   p.Division == ConstantsODataValue.DivisionDecorative).Select(x => x.CustomerNo).Distinct().Count();

            var result = (from dealer in (await _dealerInfo.FindAllAsync(x => 
                                                (x.Channel == ConstantsODataValue.DistrbutionChannelDealer
                                                     && x.Division == ConstantsODataValue.DivisionDecorative) 
                                                && (x.BusinessArea == depot && x.Territory == territory)))
                         //join custGrp in (await _customerGroupSvc.FindAllAsync(x => true))
                         //on dealer.AccountGroup equals custGrp.CustomerAccountGroup
                         //into cust
                         //from cu in cust.DefaultIfEmpty()
                         where (
                             (
                                 CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Month == monthnumber
                                 && CustomConvertExtension.ObjectToDateTime(dealer.CreatedOn).Year == year
                             )
                         )
                         select new
                         {
                             Id = dealer.Id,
                             CustomerNo = dealer.CustomerNo,
                             //Territory = dealer.Territory,
                             //IsSubdealer = cu != null && !string.IsNullOrEmpty(cu.Description) && cu.Description.StartsWith("Subdealer")
                         }).ToList();

            var count = result.Select(x => x.CustomerNo).Distinct().Count();

            return count;

        }


        public decimal GetAchivement(decimal target, decimal actual)
        {
            var res= Convert.ToDecimal((target > 0 ? ((actual / target)) * 100 : decimal.Zero).ToString("0.00"));
            return res;
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
                                            p.FiscalYear == query.Year).ToListAsync();

            if (currentYear == null || !currentYear.Any()) throw new Exception("Dealer Conversion of this area and this fiscal year is not set yet.");

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

                    var actual = await GetActualDealerAsync (monthNumber, item.Year, query.Depot, query.Territory);

                    var result = new NewDealerDevelopmentSaveModel()
                    {
                        Id = item.Id,
                        Actual = actual,
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
