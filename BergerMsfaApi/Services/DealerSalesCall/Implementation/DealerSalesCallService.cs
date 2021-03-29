using AutoMapper;
using Berger.Common;
using Berger.Common.Constants;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.Users;
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
        private readonly IRepository<EmailConfigForDealerSalesCall> _repository;
        private readonly IDropdownService _dropdownService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<UserInfo> _userInfo;


        public DealerSalesCallService(
                IRepository<DSC.DealerSalesCall> dealerSalesCallRepository,
                IDropdownService dropdownService,
                IFileUploadService fileUploadService,
                IMapper mapper,
                IRepository<EmailConfigForDealerSalesCall> repository,
                IRepository<UserInfo> userInfo,
                IEmailSender emailSender
            )
        {
            this._dealerSalesCallRepository = dealerSalesCallRepository;
            this._dropdownService = dropdownService;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
            _repository = repository;
            _emailSender = emailSender;
           _userInfo= userInfo;
        }

        public async Task<int> AddAsync(SaveDealerSalesCallModel model)
        {
            var dealerSalesCall = _mapper.Map<DSC.DealerSalesCall>(model);

            if (!string.IsNullOrWhiteSpace(model.CompetitionProductDisplayImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
            }

            if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageUrl))
            {
                var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
            }

            dealerSalesCall.CreatedTime = DateTime.Now;

            var result = await _dealerSalesCallRepository.CreateAsync(dealerSalesCall);
            var res = model.DealerSalesIssues.ToList().Select(p => p.DealerSalesIssueCategoryId).ToArray();
            var user = _userInfo.Where(p => p.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
            string body = string.Format("Customer No: ", dealerSalesCall.Dealer.CustomerNo, Environment.NewLine, "Customer Name: ", dealerSalesCall.Dealer.CustomerName, "Zone: ", dealerSalesCall.Dealer.CustZone);
            for (int i = 0; i < res.Length; i++)
            {
                var email = _repository.Where(p => p.DealerSalesIssueCategoryId == Convert.ToInt32(res[i])).FirstOrDefault().Email;
                if (!string.IsNullOrEmpty(email))
                {
                    var issue = await _dropdownService.GetDropdownById(res[i]);
                    await sendEmail(email, issue.DropdownName, user?.UserName ?? string.Empty, body);

                }
            }
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
                    dealerSalesCall.CompetitionProductDisplayImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionProductDisplayImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
                }

                if (!string.IsNullOrWhiteSpace(model.CompetitionSchemeModalityImageUrl))
                {
                    var fileName = dealerSalesCall.DealerId + "_" + Guid.NewGuid().ToString();
                    dealerSalesCall.CompetitionSchemeModalityImageUrl = await _fileUploadService.SaveImageAsync(model.CompetitionSchemeModalityImageUrl, fileName, FileUploadCode.DealerSalesCall, 1200, 800);
                }

                dealerSalesCall.CreatedTime = DateTime.Now;

                dealerSalesCalls.Add(dealerSalesCall);
            }

            var result = await _dealerSalesCallRepository.CreateListAsync(dealerSalesCalls);
            foreach (var item in dealerSalesCalls)
            {
                string body = string.Format("Customer No: ", item.Dealer.CustomerNo, Environment.NewLine, "Customer Name: ", item.Dealer.CustomerName, "Zone: ", item.Dealer.CustZone);
                var res = item.DealerSalesIssues.Select(c => c.DealerSalesIssueCategoryId).Distinct().ToArray();

                var user = _userInfo.Where(p => p.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
                for (int i = 0; i < res.Length; i++)
                {
                    var email = _repository.Where(p => p.DealerSalesIssueCategoryId == Convert.ToInt32(res[i])).FirstOrDefault().Email;
                    if (!string.IsNullOrEmpty(email))
                    {
                        var issue = await _dropdownService.GetDropdownById(Convert.ToInt32(res[i]));
                        await sendEmail(email, issue.DropdownName, user?.UserName ?? string.Empty, body);

                    }
                }
            }

            

            return true;
        }

        public async Task<QueryResultModel<DealerSalesCallModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<DSC.DealerSalesCall, object>>>()
            {
                ["dealerName"] = v => v.Dealer.CustomerName,
                ["userFullName"] = v => v.User.FullName,
            };

            var result = await _dealerSalesCallRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.User.FullName.Contains(query.GlobalSearchValue) || x.Dealer.CustomerName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.User).Include(i => i.Dealer),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<DealerSalesCallModel>>(result.Items);

            var queryResult = new QueryResultModel<DealerSalesCallModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

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

            var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetition);

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

            var companyList = await _dropdownService.GetDropdownByTypeCd(DynamicTypeCode.SwappingCompetition);

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



        private async Task sendEmail(string email, string issue,string createdby,string body)
        {
            try
            {
                string[] lstemail = email.Split(',');


                foreach (var item in lstemail)
                {
                    string messageBody =string.Format(ConstantsLeadValue.IssueCategoryMailBody,createdby,Environment.NewLine)+ body;
                    string messagesubject = string.Format(ConstantsLeadValue.IssueCategoryMailSubject,issue);

                    await _emailSender.SendEmailAsync(item, messagesubject, messageBody);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
