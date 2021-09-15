using AutoMapper;
using Berger.Common;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Controllers.DealerFocus;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Common.Constants;
using System;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.Hirearchy;
using Microsoft.EntityFrameworkCore;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Models.Common;
using System.Linq.Expressions;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class DealerOpeningService : IDealerOpeningService
    {
        private IRepository<DealerOpening> _dealerOpeningSvc;
        private IRepository<DealerOpeningAttachment> _dealerOpeningAttachmentSvc;
        private readonly IFileUploadService _fileUploadSvc;
        private readonly IRepository<Attachment> _attachmentSvc;
        private readonly IMapper _mapper;
        private readonly IRepository<UserInfo> _userInfoSvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IRepository<SaleOffice> _saleOfficeSvc;
        private readonly IRepository<SaleGroup> _saleGroupSvc;
        private readonly IRepository<Territory> _territorySvc;
        private readonly IRepository<Zone> _zoneSvc;
        private readonly IRepository<EmailConfigForDealerOppening> _emailconfig;
        private readonly IRepository<DealerOpeningLog> _dealerOpeningLog;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public DealerOpeningService(
              IRepository<DealerOpening> dealerOpeningSvc,
              IFileUploadService fileUploadSvc,
              IRepository<Attachment> attachmentSvc,
              IRepository<DealerOpeningAttachment> dealerOpeningAttachmentSvc,
              IMapper mapper,
              IRepository<UserInfo> userInfoSvc,
              IRepository<Depot> depotSvc,
              IRepository<SaleOffice> saleOfficeSvc,
              IRepository<SaleGroup> saleGroupSvc,
              IRepository<Territory> territorySvc,
              IRepository<Zone> zoneSvc,
              IRepository<EmailConfigForDealerOppening> emailconfig,
              IRepository<DealerOpeningLog> dealerOpeningLog,
              IEmailSender emailSender,
              IWebHostEnvironment env


            )
        {
            _fileUploadSvc = fileUploadSvc;
            _dealerOpeningSvc = dealerOpeningSvc;
            _dealerOpeningAttachmentSvc = dealerOpeningAttachmentSvc;
            _attachmentSvc = attachmentSvc;
            _mapper = mapper;
            _userInfoSvc = userInfoSvc;
            this._depotSvc = depotSvc;
            this._saleOfficeSvc = saleOfficeSvc;
            this._saleGroupSvc = saleGroupSvc;
            this._territorySvc = territorySvc;
            this._zoneSvc = zoneSvc;
            _emailconfig = emailconfig;
            _dealerOpeningLog = dealerOpeningLog;
            _emailSender = emailSender;
            _env = env;


        }
        //public async Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> attachments)
        //{
        //    var user = _userInfoSvc.Where(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId).FirstOrDefault();

        //    var _dealerOpening = model.ToMap<DealerOpeningModel, DealerOpening>();
        //    _dealerOpening.CurrentApprovarId = user.Id;
        //    _dealerOpening.DealerOpeningStatus = (int)DealerOpeningStatus.Pending;
        //    var result = await _dealerOpeningSvc.CreateAsync(_dealerOpening);
        //    var _dealerOpeningModel = result.ToMap<DealerOpening, DealerOpeningModel>();
        //    foreach (var attach in attachments)
        //    {
        //        var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.DealerOpening);
        //        var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(DealerOpening) });
        //        //   _dealerOpeningModel.Attachments.Add(attachment.ToMap<Attachment,AttachmentModel>());
        //    }

        //    return _dealerOpeningModel;
        //}

        public async Task<int> DeleteDealerOpeningAsync(int DealerId)
        {
            return await _dealerOpeningSvc.DeleteAsync(f => f.Id == DealerId);
        }

        public async Task<IPagedList<DealerOpeningModel>> GetDealerOpeningListAsync(int index, int pageSize, string search)
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();

            var result = _mapper.Map<List<DealerOpeningModel>>((await _dealerOpeningSvc.GetAllAsync()).ToList());
            if (!string.IsNullOrEmpty(search))
                result = result.Search(search);


            //    var result = _dealerOpeningSvc.GetAllInclude(f => f.DealerOpeningAttachments);
            return result.ToPagedList(index, pageSize);
            // return mapper.Map<List<DealerOpeningModel>>(result);
            //var result = await _dealerOpeningSvc.GetAllAsync();
            //var dealerOpeningModel = result.ToMap<DealerOpening, DealerOpeningModel>();
            //foreach (var item in dealerOpeningModel.ToList())
            //{
            //    var attachment = await _attachmentSvc.FindAsync(f => f.ParentId == item.Id && f.TableName == nameof(DealerOpening));
            //    if (attachment != null)
            //    {
            //        var attachmentModel = attachment.ToMap<Attachment, AttachmentModel>();
            //       // item.Attachments.Add(attachmentModel);
            //    }

            //}
            //return dealerOpeningModel;

        }


        //public async Task<IPagedList<DealerOpeningModel>> GetDealerOpeningPendingListAsync(int index, int pageSize, string search)
        //{
        //    //var user = _userInfoSvc.Where(p => p.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
        //    var currentUser = AppIdentity.AppUser;
        //    var resultDb = new List<DealerOpening>();

        //    var emailConfig = _emailconfig.Where(p => currentUser.PlantIdList.Contains(p.BusinessArea)).FirstOrDefault();
            
        //    if (currentUser.EmployeeRole == (int)EnumEmployeeRole.Admin || currentUser.EmployeeRole == (int)EnumEmployeeRole.GM)
        //    {
        //        resultDb = (List<DealerOpening>)await _dealerOpeningSvc.GetAllAsync();
        //    }
        //    else if (emailConfig != null)
        //    {
        //        resultDb = (List<DealerOpening>)await _dealerOpeningSvc.FindAllAsync(p => currentUser.PlantIdList.Contains(p.BusinessArea));
        //    }
        //    else
        //    {
        //        resultDb = (List<DealerOpening>)await _dealerOpeningSvc.FindAllAsync(p => p.NextApprovarId == currentUser.UserId && p.DealerOpeningStatus == (int)DealerOpeningStatus.Pending);
        //    }

        //    var result = _mapper.Map<List<DealerOpeningModel>>(resultDb.OrderByDescending(o => o.CreatedTime));

        //    if (!string.IsNullOrEmpty(search))
        //        result = result.Search(search);

        //    #region get area mapping data
        //    var depotIds = result.Select(x => x.BusinessArea).Distinct().ToList();
        //    var saleGroupIds = result.Select(x => x.SaleGroup).Distinct().ToList();
        //    var saleOfficeIds = result.Select(x => x.SaleOffice).Distinct().ToList();
        //    //var territoryIds = result.Select(x => x.Territory).Distinct().ToList();
        //    //var zoneIds = result.Select(x => x.Zone).Distinct().ToList();

        //    var depots = (await _depotSvc.FindAllAsync(x => depotIds.Contains(x.Werks)));
        //    var saleGroups = (await _saleGroupSvc.FindAllAsync(x => saleGroupIds.Contains(x.Code)));
        //    var saleOffices = (await _saleOfficeSvc.FindAllAsync(x => saleOfficeIds.Contains(x.Code)));
        //    //var territories = (await _territorySvc.FindAllAsync(x => territoryIds.Contains(x.Code)));
        //    //var zones = (await _zoneSvc.FindAllAsync(x => zoneIds.Contains(x.Code)));

        //    foreach (var item in result)
        //    {
        //        item.BusinessAreaName = depots.FirstOrDefault(x => x.Werks == item.BusinessArea)?.Name1 ?? string.Empty;
        //        item.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == item.SaleGroup)?.Name ?? string.Empty;
        //        item.SaleOfficeName = saleOffices.FirstOrDefault(x => x.Code == item.SaleOffice)?.Name ?? string.Empty;
        //        //item.TerritoryName = territories.FirstOrDefault(x => x.Code == item.Territory)?.Code ?? string.Empty;
        //        //item.ZoneName = zones.FirstOrDefault(x => x.Code == item.Zone)?.Code ?? string.Empty;
        //        item.TerritoryName = item.Territory;
        //        item.ZoneName = item.Zone;
        //        item.DealerOpeningStatusText = ((DealerOpeningStatus)item.DealerOpeningStatus).ToString();
        //    }
        //    #endregion

        //    return result.ToPagedList(index, pageSize);
        //}




        //public async Task<bool> ChangeDealerStatus(DealerOpeningStatusChangeModel model)
        //{
        //    var user = _userInfoSvc.Where(p => p.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
        //    var dealer = _dealerOpeningSvc.Where(p => p.Id == model.DealerOpeningId).FirstOrDefault();
        //    var emailConfig = _emailconfig.Where(p => p.Designation == user.EmployeeRole.ToString() && p.BusinessArea == dealer.BusinessArea).FirstOrDefault();
        //    if (model.Status == (int)DealerOpeningStatus.Approved)
        //    {
        //        if (emailConfig != null)
        //        {
        //            dealer.NextApprovarId = null;
        //            dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Approved;

        //        }
        //        else
        //        {
        //            dealer.NextApprovarId = _userInfoSvc.Where(p => p.EmployeeId == user.ManagerId).FirstOrDefault().Id;

        //        }
        //        //dealer.NextApprovarId = _userInfoSvc.Where(p => p.EmployeeId == user.ManagerId).FirstOrDefault().Id;

        //    }
        //    else if (model.Status == (int)DealerOpeningStatus.Rejected)
        //    {
        //        dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Rejected;
        //    }
        //    dealer.CurrentApprovarId = AppIdentity.AppUser.UserId;
        //    await _dealerOpeningSvc.UpdateAsync(dealer);
        //    await DealerStatusLog(dealer, "DealerStatus", model.Status.ToString());
        //    if (emailConfig != null)
        //    {
        //        await sendEmail(emailConfig.Email, dealer.Id);
        //    }
        //    return false;
        //}


        public async Task<bool> ChangeStatusDealer(DealerOpeningStatusChangeModel model)
        {
            var isApproved = false;
            var dealer = _dealerOpeningSvc.Where(p => p.Id == model.DealerOpeningId).FirstOrDefault();
            var emailConfig = _emailconfig.Where(p => p.BusinessArea == dealer.BusinessArea).FirstOrDefault();
            if (model.Status == (int)DealerOpeningStatus.Approved)
            {

                dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Approved;
                isApproved = true;


            }
            else if (model.Status == (int)DealerOpeningStatus.Rejected)
            {
                dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Rejected;
            }
            await _dealerOpeningSvc.UpdateAsync(dealer);
            await DealerStatusLog(dealer, "DealerStatus", model.Status.ToString());
            if (emailConfig != null && isApproved)
            {
                await sendEmail(emailConfig.Email, dealer.Id);
            }
            return isApproved;
        }

        public async Task<DealerOpeningModel> UpdateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> attachments)
        {

            var dealerOpenig = model.ToMap<DealerOpeningModel, DealerOpening>();
            var result = await _dealerOpeningSvc.UpdateAsync(dealerOpenig);
            var dealerOpenigModel = result.ToMap<DealerOpening, DealerOpeningModel>();

            var existing = await _attachmentSvc.FindAllAsync(f => f.TableName == nameof(DealerOpening) && f.ParentId == model.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);

                await _attachmentSvc.DeleteAsync(f => f.Id == item.Id);

            }
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.FileName, FileUploadCode.DealerOpening);
                var _newAttachment = new Attachment { Path = path, Name = attach.FileName, TableName = nameof(DealerOpening), Format = Path.GetExtension(attach.FileName), Size = 1, ParentId = model.Id };
                var attachment = await _attachmentSvc.CreateAsync(_newAttachment);
                //  dealerOpenigModel.Attachments.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }


            return dealerOpenigModel;
        }

        public async Task<bool> IsExistAsync(int Id)
        {
            return await _dealerOpeningSvc.IsExistAsync(f => f.Id == Id);
        }

        public async Task<DealerOpeningModel> AppCreateDealerOpeningAsync(DealerOpeningModel model)
        {
            if (model.DealerOpeningAttachments.Any() && _fileUploadSvc.IsMaxSizeExceded(model.DealerOpeningAttachments.Select(s => s.Path).ToList(), 20))
                throw new Exception("Maximum file size 20 MB allowed.");

            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();

            var user = _userInfoSvc.Where(f => f.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
            var managerUser = _userInfoSvc.Where(f => f.EmployeeId == user.ManagerId).FirstOrDefault();

            //var _dealerOpening = model.ToMap<DealerOpeningModel, DealerOpening>();
            //_dealerOpening.NextApprovarId = user.Id;
            //_dealerOpening.DealerOpeningStatus = (int)DealerOpeningStatus.Pending;

            var dealerOpening = _mapper.Map<DealerOpening>(model);
            dealerOpening.CurrentApprovarId = managerUser.Id;
            dealerOpening.DealerOpeningStatus = (int)DealerOpeningStatus.Pending;

            //TODO: need to generate code
            dealerOpening.Code = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();


            foreach (var attach in dealerOpening.DealerOpeningAttachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                attach.Name = attach.Name.Replace("/", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                {
                    try
                    {
                        attach.Path = await _fileUploadSvc.SaveImageAsync(
                            attach.Path,
                            fileName, FileUploadCode.DealerOpening);
                    }
                    catch (System.Exception e)
                    {
                        var err = e;
                    }
                }

            }
            var result = await _dealerOpeningSvc.CreateAsync(dealerOpening);

            return _mapper.Map<DealerOpeningModel>(result);

        }

        public async Task<DealerOpeningModel> AppUpdateDealerOpeningAsync(DealerOpeningModel model)
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();

            var dealerOpening = _mapper.Map<DealerOpening>(model);

            var findDealerOpening = await _dealerOpeningSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.DealerOpeningAttachments);

            foreach (var item in findDealerOpening.DealerOpeningAttachments)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);
                await _dealerOpeningAttachmentSvc.DeleteAsync(f => f.Id == item.Id);
            }
            foreach (var attach in dealerOpening.DealerOpeningAttachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                attach.Name = attach.Name.Replace("/", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                    attach.Path = await _fileUploadSvc.SaveImageAsync(
                        attach.Path,
                        fileName, FileUploadCode.DealerOpening,
                        300, 300);
            }
            var result = await _dealerOpeningSvc.UpdateAsync(dealerOpening);


            return _mapper.Map<DealerOpeningModel>(result);
        }

        public async Task<IEnumerable<AppDealerOpeningModel>> AppGetDealerOpeningListByCurrentUserAsync()
        {
            var result = await (from d in _dealerOpeningSvc.FindAll(x => x.EmployeeId == AppIdentity.AppUser.EmployeeId)
                                join dep in _depotSvc.GetAll() on d.BusinessArea equals dep.Werks into depleftjoin
                                from depinfo in depleftjoin.DefaultIfEmpty()
                                join so in _saleOfficeSvc.GetAll() on d.SaleOffice equals so.Code into soleftjoin
                                from soinfo in soleftjoin.DefaultIfEmpty()
                                join sg in _saleGroupSvc.GetAll() on d.SaleGroup equals sg.Code into sgleftjoin
                                from sginfo in sgleftjoin.DefaultIfEmpty()
                                join t in _territorySvc.GetAll() on d.Territory equals t.Code into tleftjoin
                                from tinfo in tleftjoin.DefaultIfEmpty()
                                join z in _zoneSvc.GetAll() on d.Zone equals z.Code into zleftjoin
                                from zinfo in zleftjoin.DefaultIfEmpty()
                                orderby d.CreatedTime descending
                                select new AppDealerOpeningModel
                                {
                                    Id = d.Id,
                                    BusinessArea = $"{depinfo.Name1} ({depinfo.Werks})",
                                    SaleOffice = $"{soinfo.Name} ({soinfo.Code})",
                                    SaleGroup = $"{sginfo.Name} ({sginfo.Code})",
                                    Territory = $"{tinfo.Code}",
                                    Zone = $"{zinfo.Code}",
                                    Code = d.Code,
                                    OpeningDate = d.CreatedTime.ToString("yyyy-MM-dd"),
                                    DealerOpeningStatus = d.DealerOpeningStatus,
                                    DealerOpeningStatusText = ((DealerOpeningStatus)d.DealerOpeningStatus).ToString()
                                }).ToListAsync();

            return result;
        }

        public async Task<DealerOpeningModel> GetDealerOpeningDetailById(int id)
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();

            var result = await _dealerOpeningSvc.FindIncludeAsync(f => f.Id == id, f => f.DealerOpeningAttachments, f => f.dealerOpeningLogs);
            foreach (var item in result.dealerOpeningLogs)
            {
                var user = _userInfoSvc.Find(p => p.Id == item.UserId);
                item.User = user;
            }
            //var data = result.dealerOpeningLogs.OrderByDescending(p => p.CreatedTime);
            //return _mapper.Map<DealerOpeningModel>(result.dealerOpeningLogs.OrderByDescending(p=>p.CreatedTime)); 

            //var result = await _dealerOpeningSvc.GetAllIncludeAsync(x => x,
            //                    x => x.Id == id,
            //                    x => x.OrderByDescending(o => o.CreatedTime),
            //                    x => x.Include(i => i.DealerOpeningAttachments)
            //                        .Include(i => i.dealerOpeningLogs).ThenInclude(i => i.User),
            //                    true);

            var returnResult = _mapper.Map<DealerOpeningModel>(result);

            #region get area mapping data
            var depots = (await _depotSvc.FindAllAsync(x => returnResult.BusinessArea == x.Werks));
            var saleGroups = (await _saleGroupSvc.FindAllAsync(x => returnResult.SaleGroup == x.Code));
            var saleOffices = (await _saleOfficeSvc.FindAllAsync(x => returnResult.SaleOffice == x.Code));
            //var territories = (await _territorySvc.FindAllAsync(x => returnResult.Territory == x.Code));
            //var zones = (await _zoneSvc.FindAllAsync(x => returnResult.Zone == x.Code));

            returnResult.BusinessAreaName = depots.FirstOrDefault(x => x.Werks == returnResult.BusinessArea)?.Name1 ?? string.Empty;
            returnResult.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == returnResult.SaleGroup)?.Name ?? string.Empty;
            returnResult.SaleOfficeName = saleOffices.FirstOrDefault(x => x.Code == returnResult.SaleOffice)?.Name ?? string.Empty;
            //painterModel.TerritoryName = territories.FirstOrDefault(x => x.Code == returnResult.Territory)?.Code ?? string.Empty;
            //painterModel.ZoneName = zones.FirstOrDefault(x => x.Code == returnResult.Zone)?.Code ?? string.Empty;
            returnResult.TerritoryName = returnResult.Territory;
            returnResult.ZoneName = returnResult.Zone;
            returnResult.DealerOpeningStatusText = ((DealerOpeningStatus)returnResult.DealerOpeningStatus).ToString();
            #endregion

            return returnResult;

        }


        private async Task DealerStatusLog(DealerOpening dealerInfoId, string propertyName, string propertyValue)
        {

            var DealerStatusLog = new DealerOpeningLog()
            {
                DealerOpening = dealerInfoId,
                UserId = AppIdentity.AppUser.UserId,
                PropertyName = propertyName,
                PropertyValue = propertyValue,
                CreatedTime = DateTime.UtcNow
            };
            var res = await _dealerOpeningLog.CreateAsync(DealerStatusLog);


        }


        private async Task sendEmail(string email, int dealeropeningId)
        {
            try
            {
                var dealer = _dealerOpeningSvc.Find(p => p.Id == dealeropeningId);
                var attachment = await _dealerOpeningAttachmentSvc.FindAllAsync(p => p.DealerOpeningId == dealeropeningId);
                List<System.Net.Mail.Attachment> lstAttachment = new List<System.Net.Mail.Attachment>();
                if (attachment.Count > 0)
                {
                    foreach (var item in attachment)
                    {
                        if (!string.IsNullOrEmpty(item.Path))
                        {
                            string path = Path.Combine(_env.WebRootPath, item.Path);
                            if (File.Exists(path))
                            {
                                var url = new System.Net.Mail.Attachment(path);
                                lstAttachment.Add(url);
                            }

                        }

                    }

                }
                string[] lstemail = email.Split(',');


                foreach (var item in lstemail)
                {
                    var createdBy = _userInfoSvc.Find(p => p.Id == dealer.CreatedBy);
                    var LastApprovar = _userInfoSvc.Find(p => p.Id == dealer.CurrentApprovarId);
                    //string messageBody = string.Format(ConstantsLeadValue.DealerOpeningMailBody, createdBy?.FullName??string.Empty, LastApprovar?.FullName??string.Empty);
                    //string subject = string.Format(ConstantsLeadValue.DealerOpeningMailSubject, dealer.Code??string.Empty);

                    string subject = string.Empty;
                    string body = string.Empty;

                    subject = string.Format("Berger MSFA - New Dealer Opening Request. REQUEST ID: {0}.", dealer.Code);

                    body += $"Dear Concern,{Environment.NewLine}";

                    body += string.Format("A new dealer open request has been generated from " +
                        "“{0} & {1}” and got approved by “{2} & {3}”. " +
                        "You are requested to open the new dealer in SAP by using the attached information.",
                        createdBy.UserName,
                        createdBy.Designation,
                        LastApprovar.UserName,
                        LastApprovar.Designation);

                    body += $"{Environment.NewLine}{Environment.NewLine}";
                    body += $"Thank You,{Environment.NewLine}";
                    body += $"Berger Paints Bangladesh Limited";

                    await _emailSender.SendEmailWithAttachmentAsync(item, subject, body, lstAttachment);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DealerOpening>> GetDealerOpeningPendingListForNotificationAsync()
        {
            var result = await _dealerOpeningSvc.GetAllInclude(p => p.CurrentApprovar).Where(p => p.CurrentApprovarId == AppIdentity.AppUser.UserId && p.DealerOpeningStatus == (int)DealerOpeningStatus.Pending).ToListAsync();
            return result;
        }

        public async Task<QueryResultModel<DealerOpeningModel>> GetAllDealersAsync(DealerOpeningQueryObjectModel query)
        {

            var currentUser = AppIdentity.AppUser;

            var resultDb = await _dealerOpeningSvc.FindAllAsync(p => p.CurrentApprovarId == currentUser.UserId);

            var result = _mapper.Map<IEnumerable<DealerOpeningModel>>(resultDb);


            var depotIds = result.Select(x => x.BusinessArea).Distinct().ToList();
            var saleGroupIds = result.Select(x => x.SaleGroup).Distinct().ToList();
            var saleOfficeIds = result.Select(x => x.SaleOffice).Distinct().ToList();

            var depots = (await _depotSvc.FindAllAsync(x => depotIds.Contains(x.Werks)));
            var saleGroups = (await _saleGroupSvc.FindAllAsync(x => saleGroupIds.Contains(x.Code)));
            var saleOffices = (await _saleOfficeSvc.FindAllAsync(x => saleOfficeIds.Contains(x.Code)));

            foreach (var item in result)
            {
                item.BusinessAreaName = depots.FirstOrDefault(x => x.Werks == item.BusinessArea)?.Name1 ?? string.Empty;
                item.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == item.SaleGroup)?.Name ?? string.Empty;
                item.SaleOfficeName = saleOffices.FirstOrDefault(x => x.Code == item.SaleOffice)?.Name ?? string.Empty;
                item.TerritoryName = item.Territory;
                item.ZoneName = item.Zone;
                item.DealerOpeningStatusText = ((DealerOpeningStatus)item.DealerOpeningStatus).ToString();
            }


            var total = result.Count();

            result = result.Where(x =>
                     (string.IsNullOrEmpty(query.Depot) || query.Depot == x.BusinessArea) &&
                     (!query.Territories.Any() || query.Territories.Contains(x.Territory))).ToList();

            var filterCount = result.Count();



            result = await result.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();


            var items = await result.ToListAsync();

            var queryResult = new QueryResultModel<DealerOpeningModel>
            {
                Items = items,
                TotalFilter = filterCount,
                Total = total
            };

            return queryResult;
        }
    }
}
