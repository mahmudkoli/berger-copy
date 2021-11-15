using AutoMapper;
using Berger.Common;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using Berger.Odata.Services;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerSalesCall.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Services.DealerSalesCall.Implementation
{
    public class DealerSalesCallService : IDealerSalesCallService
    {
        private readonly IRepository<DSC.DealerSalesCall> _dealerSalesCallRepository;
        private readonly IRepository<DSC.DealerCompetitionSales> _dealerCompetitionSalesRepository;
        private readonly IRepository<DSC.DealerSalesIssue> _dealerSalesIssueRepository;

        private readonly IRepository<EmailConfigForDealerSalesCall> _repository;
        private readonly IDropdownService _dropdownService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IFinancialDataService _financialDataService;
        private readonly IRepository<UserInfo> _userInfo;
        private readonly IRepository<DealerInfo> dealerInfo;
        private readonly IRepository<Depot> _plantSvc;

        public DealerSalesCallService(
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IDropdownService dropdownService,
                IFileUploadService fileUploadService,
                IMapper mapper,
                IRepository<EmailConfigForDealerSalesCall> repository,
                IRepository<UserInfo> userInfo,
                IRepository<DealerInfo> dealerInfo,
                IRepository<Depot> plantSvc,
                IEmailSender emailSender,
                IFinancialDataService financialDataService,
                IRepository<DSC.DealerCompetitionSales> dealerCompetitionSalesRepository,
                IRepository<DSC.DealerSalesIssue> dealerSalesIssueRepository
            )
        {
            this._dealerSalesCallRepository = dealerSalesCallRepository;
            this._dropdownService = dropdownService;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
            _repository = repository;
            _emailSender = emailSender;
            this._financialDataService = financialDataService;
            _userInfo = userInfo;
            this.dealerInfo = dealerInfo;
            this._plantSvc = plantSvc;
            _dealerCompetitionSalesRepository = dealerCompetitionSalesRepository;
            _dealerSalesIssueRepository = dealerSalesIssueRepository;
        }

        public async Task<int> AddAsync(SaveDealerSalesCallModel model)
        {
            var dealerSalesCall = _mapper.Map<DSC.DealerSalesCall>(model);

            if (!string.IsNullOrWhiteSpace(model.CompetitionProductDisplayImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall);
            }

            if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall);
            }

            var result = await _dealerSalesCallRepository.CreateAsync(dealerSalesCall);

            await SendIssueEmail(result.Id);

            return result.Id;
        }

        public async Task DeleteImage(DealerImageModel dealerImageModel)
        {
            var item = await _dealerSalesCallRepository.FirstOrDefaultAsync(x => x.Id == dealerImageModel.Id);

            //string fileDirectory = Path.Combine(
            //    Directory.GetCurrentDirectory(), @"wwwroot\");
            //var fullPath = fileDirectory + dealerImageModel.URL;

            var fullPath = dealerImageModel.URL;

            if (item != null)
            {
                switch (dealerImageModel.Type)
                {
                    case "competitionProductDisplayImageUrl":
                        item.CompetitionProductDisplayImageUrl = null;
                        break;
                    case "competitionSchemeModalityImageUrl":
                        item.CompetitionSchemeModalityImageUrl = null;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(fullPath))
                {
                    await _fileUploadService.DeleteImageAsync(fullPath);
                    await _dealerSalesCallRepository.UpdateAsync(item);

                }

                //File.Delete();


            }
        }


        public async Task<int> UpdateAsync(AppDealerSalesCallModel model)
        {



            var dealerSalesCall = _mapper.Map<DSC.DealerSalesCall>(model);


            var dealerSalesIssues = dealerSalesCall.DealerSalesIssues.ToList();
            var dealerCompetitionSales = dealerSalesCall.DealerCompetitionSales.ToList();

            dealerSalesCall.DealerSalesIssues = null;
            dealerSalesCall.DealerCompetitionSales = null;



            if (!string.IsNullOrWhiteSpace(model.CompetitionProductDisplayImageBase64))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                model.CompetitionProductDisplayImageUrl = model.CompetitionProductDisplayImageBase64.Substring(model.CompetitionProductDisplayImageBase64.LastIndexOf(',') + 1);
                dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall);
            }

            if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageBase64))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                model.CompetitionSchemeModalityImageUrl = model.CompetitionSchemeModalityImageBase64.Substring(model.CompetitionSchemeModalityImageBase64.LastIndexOf(',') + 1);

                dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall);
            }

            var result = await _dealerSalesCallRepository.UpdateAsync(dealerSalesCall);


            var issue = await _dealerSalesIssueRepository.DeleteAsync(p => p.DealerSalesCallId == dealerSalesCall.Id);


            var issuecategoryAdd = await _dealerSalesIssueRepository.CreateListAsync(dealerSalesIssues);


            var dealerCompetitionSalesEdit = await _dealerCompetitionSalesRepository.UpdateListAsync(dealerCompetitionSales);




            //await SendIssueEmail(result.Id);

            return result.Id;
        }

        public async Task<bool> AddRangeAsync(List<SaveDealerSalesCallModel> models)
        {
            var dealerSalesCalls = new List<DSC.DealerSalesCall>();

            foreach (var model in models)
            {
                var dealerSalesCall = _mapper.Map<DSC.DealerSalesCall>(model);

                if (!string.IsNullOrWhiteSpace(model.CompetitionProductDisplayImageUrl))
                {
                    var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                    dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall);
                }

                if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageUrl))
                {
                    var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                    dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall);
                }

                dealerSalesCalls.Add(dealerSalesCall);
            }

            var result = await _dealerSalesCallRepository.CreateListAsync(dealerSalesCalls);

            foreach (var item in result)
            {
                await SendIssueEmail(item.Id);
            }

            return true;
        }

        public async Task<QueryResultModel<DealerSalesCallModel>> GetAllAsync(DealerSalesCallQueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DSC.DealerSalesCall, object>>>()
            {
                ["createdTime"] = v => v.CreatedTime,
                ["userFullName"] = v => v.User.FullName,
                ["userFullName"] = v => v.User.FullName,
            };

            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => (
                                      (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue) || x.Dealer.CustomerName.Contains(query.GlobalSearchValue)) &&
                                      (!query.Territories.Any() || query.Territories.Contains(x.Dealer.Territory)) &&
                                      (!query.SalesGroup.Any() || query.SalesGroup.Contains(x.Dealer.SalesGroup)) &&
                                      (!query.CustZones.Any() || query.SalesGroup.Contains(x.Dealer.CustZone)) &&
                                      (string.IsNullOrWhiteSpace(query.DepoId) || x.Dealer.BusinessArea == query.DepoId) &&
                                      (!query.DealerId.HasValue || x.DealerId == query.DealerId) &&

                                      (query.DealerType.Equals(DealerType.All)?true:(query.DealerType.Equals(DealerType.SubDealer)?x.IsSubDealerCall==true: x.IsSubDealerCall==false)

                                      )),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User).Include(i => i.Dealer),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<DealerSalesCallModel>>(result.Items);

            var queryResult = new QueryResultModel<DealerSalesCallModel>
            {
                Items = modelResult,
                TotalFilter = result.TotalFilter,
                Total = result.Total
            };

            return queryResult;
        }

        public async Task<IList<AppDealerSalesCallModel>> GetAllByUserIdAsync(int userId)
        {
            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserId == userId,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<AppDealerSalesCallModel>>(result);

            return modelResult;
        }

        public async Task<SaveDealerSalesCallModel> GetDealerSalesCallByDealerIdAsync(int id)
        {
            var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.DealerId == id,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.DealerCompetitionSales),
                                true
                            );

            var modelResult = new SaveDealerSalesCallModel();
            modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
            modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();
            modelResult.DealerId = id;

            var dealer = await dealerInfo.FindAsync(x => x.Id == id);
            var odata = await _financialDataService.CheckCustomerOSSlippage(dealer?.CustomerNo ?? string.Empty);
            modelResult.HasOS = odata.HasOS;
            modelResult.HasSlippage = odata.HasSlippage;

            var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetitionCompany);

            foreach (var item in companyList)
            {
                modelResult.DealerCompetitionSales.Add(
                        new SaveDealerCompetitionSalesModel
                        {
                            CompanyId = item.Id,
                            CompanyName = item.DropdownName
                        }
                    );
            }

            if (result != null)
            {
                modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                modelResult.HasPainterInfluence = result.HasPainterInfluence;
                modelResult.PainterInfluenceId = result.PainterInfluenceId;

                modelResult.HasBPBLSales = result.HasBPBLSales;
                modelResult.BPBLAverageMonthlySales = result.BPBLAverageMonthlySales;
                modelResult.BPBLActualMTDSales = result.BPBLActualMTDSales;

                modelResult.IsCBInstalled = result.IsCBInstalled;
                modelResult.HasCompetitionPresence = result.HasCompetitionPresence;

                if (result.SubDealerInfluenceId.HasValue)
                {
                    var subDealerInfluence = await _dropdownService.GetDropdownById(result.SubDealerInfluenceId.Value);
                    if (subDealerInfluence != null) modelResult.SubDealerInfluenceDropDownName = subDealerInfluence.DropdownName;
                }
                if (result.PainterInfluenceId.HasValue)
                {
                    var painterInfluence = await _dropdownService.GetDropdownById(result.PainterInfluenceId.Value);
                    if (painterInfluence != null) modelResult.PainterInfluenceDropDownName = painterInfluence.DropdownName;
                }

                if (result.DealerCompetitionSales != null)
                {
                    foreach (var item in result.DealerCompetitionSales)
                    {
                        var dcs = modelResult.DealerCompetitionSales.FirstOrDefault(X => X.CompanyId == item.CompanyId);
                        if (dcs != null)
                        {
                            dcs.AverageMonthlySales = item.AverageMonthlySales;
                            dcs.ActualMTDSales = item.ActualMTDSales;
                        }
                    }
                }
            }

            return modelResult;
        }

        public async Task<IList<SaveDealerSalesCallModel>> GetDealerSalesCallListByDealerIdsAsync(IList<int> ids)
        {
            var modelResults = new List<SaveDealerSalesCallModel>();

            var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetitionCompany);

            foreach (var id in ids)
            {

                var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                    x => x,
                                    x => x.DealerId == id,
                                    x => x.OrderByDescending(o => o.CreatedTime),
                                    x => x.Include(i => i.DealerCompetitionSales),
                                    true
                                );

                var modelResult = new SaveDealerSalesCallModel();
                modelResult.DealerCompetitionSales = new List<SaveDealerCompetitionSalesModel>();
                modelResult.DealerSalesIssues = new List<SaveDealerSalesIssueModel>();
                modelResult.DealerId = id;

                var dealer = await dealerInfo.FindAsync(x => x.Id == id);
                var odata = await _financialDataService.CheckCustomerOSSlippage(dealer?.CustomerNo ?? string.Empty);
                modelResult.HasOS = odata.HasOS;
                modelResult.HasSlippage = odata.HasSlippage;

                foreach (var item in companyList)
                {
                    modelResult.DealerCompetitionSales.Add(
                            new SaveDealerCompetitionSalesModel
                            {
                                CompanyId = item.Id,
                                CompanyName = item.DropdownName
                            }
                        );
                }

                if (result != null)
                {
                    modelResult.HasSubDealerInfluence = result.HasSubDealerInfluence;
                    modelResult.SubDealerInfluenceId = result.SubDealerInfluenceId;
                    modelResult.HasPainterInfluence = result.HasPainterInfluence;
                    modelResult.PainterInfluenceId = result.PainterInfluenceId;

                    modelResult.HasBPBLSales = result.HasBPBLSales;
                    modelResult.BPBLAverageMonthlySales = result.BPBLAverageMonthlySales;
                    modelResult.BPBLActualMTDSales = result.BPBLActualMTDSales;

                    modelResult.IsCBInstalled = result.IsCBInstalled;
                    modelResult.HasCompetitionPresence = result.HasCompetitionPresence;

                    if (result.SubDealerInfluenceId.HasValue)
                    {
                        var subDealerInfluence = await _dropdownService.GetDropdownById(result.SubDealerInfluenceId.Value);
                        if (subDealerInfluence != null) modelResult.SubDealerInfluenceDropDownName = subDealerInfluence.DropdownName;
                    }
                    if (result.PainterInfluenceId.HasValue)
                    {
                        var painterInfluence = await _dropdownService.GetDropdownById(result.PainterInfluenceId.Value);
                        if (painterInfluence != null) modelResult.PainterInfluenceDropDownName = painterInfluence.DropdownName;
                    }

                    if (result.DealerCompetitionSales != null)
                    {
                        foreach (var item in result.DealerCompetitionSales)
                        {
                            var dcs = modelResult.DealerCompetitionSales.FirstOrDefault(X => X.CompanyId == item.CompanyId);
                            if (dcs != null)
                            {
                                dcs.AverageMonthlySales = item.AverageMonthlySales;
                                dcs.ActualMTDSales = item.ActualMTDSales;
                            }
                        }
                    }
                }

                modelResults.Add(modelResult);
            }

            return modelResults;
        }

        public async Task<DealerSalesCallModel> GetByIdAsync(int id)
        {
            var result = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.User).Include(i => i.Dealer)
                                        .Include(i => i.SecondarySalesRatings).Include(i => i.PremiumProductLifting)
                                        .Include(i => i.Merchendising).Include(i => i.SubDealerInfluence)
                                        .Include(i => i.PainterInfluence).Include(i => i.DealerSatisfaction)
                                        .Include(i => i.DealerCompetitionSales).ThenInclude(i => i.Company)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.DealerSalesIssueCategory)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.Priority)
                                        .Include(i => i.DealerSalesIssues).ThenInclude(i => i.CBMachineMantainance),
                                true
                            );

            var modelResult = _mapper.Map<DealerSalesCallModel>(result);



            return modelResult;
        }

        private async Task SendIssueEmail(int dealerSalesCallId)
        {
            try
            {
                var salesCall = await _dealerSalesCallRepository.GetFirstOrDefaultIncludeAsync(x => x,
                                x => x.Id == dealerSalesCallId,
                                null,
                                x => x.Include(y => y.User).Include(y => y.Dealer)
                                        .Include(y => y.DealerSalesIssues).ThenInclude(y => y.DealerSalesIssueCategory)
                                        .Include(y => y.DealerSalesIssues).ThenInclude(y => y.Priority),
                                true);

                var plant = salesCall.Dealer.BusinessArea;
                var plantName = (await _plantSvc.FindAsync(x => x.Werks == plant)).Name1 ?? string.Empty;

                foreach (var issue in salesCall.DealerSalesIssues)
                {
                    string subject = string.Empty;
                    string body = string.Empty;
                    string issueName = issue.DealerSalesIssueCategory.DropdownName;
                    issueName = issueName.Contains("Complain") ? issueName : $"{issueName} Complain";

                    subject = string.Format("Berger MSFA - Sales Call Issue “{0}” has been Arrived.", issueName);

                    body += $"Dear Concern,{Environment.NewLine}";

                    body += string.Format("A {0} has been generated by “{1} - {2}” while visiting the {3} " +
                        "“{4}, {5}, {6}, {7} & {8}”. " +
                        "Complain details are attached below. " +
                        "Please check the issue and give your feedback to the concern person.",
                        issueName,
                        salesCall.User.UserName,
                        salesCall.User.Designation,
                        salesCall.IsSubDealerCall ? "Sub-Dealer" : "Dealer",
                        salesCall.Dealer.CustomerNo,
                        salesCall.Dealer.CustomerName,
                        plantName,
                        salesCall.Dealer.Territory,
                        salesCall.Dealer.CustZone);

                    body += $"{Environment.NewLine}{Environment.NewLine}";

                    if (issue.DealerSalesIssueCategory.DropdownCode == ConstantIssuesValue.ProductComplaintDropdownCode)
                    {
                        body += $"Material: {issue.MaterialName}{Environment.NewLine}" +
                            $"Material Group: {issue.MaterialGroup}{Environment.NewLine}" +
                            $"Quantity: {issue.Quantity}{Environment.NewLine}" +
                            $"Batch Number: {issue.BatchNumber}{Environment.NewLine}" +
                            $"Comments: {issue.Comments}{Environment.NewLine}" +
                            $"Priority: {issue.Priority.DropdownName}";
                    }
                    else if (issue.DealerSalesIssueCategory.DropdownCode == ConstantIssuesValue.ShopSignComplainDropdownCode)
                    {
                        body += $"Comments: {issue.Comments}{Environment.NewLine}" +
                            $"Priority: {issue.Priority.DropdownName}";
                    }
                    else if (issue.DealerSalesIssueCategory.DropdownCode == ConstantIssuesValue.DeliveryIssueDropdownCode)
                    {
                        body += $"Comments: {issue.Comments}{Environment.NewLine}" +
                            $"Priority: {issue.Priority.DropdownName}";
                    }
                    else if (issue.DealerSalesIssueCategory.DropdownCode == ConstantIssuesValue.DamageProductDropdownCode)
                    {
                        body += $"Material: {issue.MaterialName}{Environment.NewLine}" +
                            $"Material Group: {issue.MaterialGroup}{Environment.NewLine}" +
                            $"Quantity: {issue.Quantity}{Environment.NewLine}" +
                            $"Comments: {issue.Comments}{Environment.NewLine}" +
                            $"Priority: {issue.Priority.DropdownName}";
                    }
                    else
                    {
                        body +=$"Comments: {issue.Comments}";
                    }

                    body += $"{Environment.NewLine}{Environment.NewLine}";
                    body += $"Thank You,{Environment.NewLine}";
                    body += $"Berger Paints Bangladesh Limited";

                    var email = _repository.Where(p => p.DealerSalesIssueCategoryId == issue.DealerSalesIssueCategoryId && p.BusinessArea == plant)?.FirstOrDefault()?.Email;
                    if (!string.IsNullOrEmpty(email))
                    {
                        await SendEmail(email, subject, body);
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private async Task SendEmail(string email, string messagesubject, string messageBody)
        {
            try
            {
                string[] lstemail = email.Split(',');

                foreach (var item in lstemail)
                {
                    await _emailSender.SendEmailAsync(item, messagesubject, messageBody);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


    }
}
