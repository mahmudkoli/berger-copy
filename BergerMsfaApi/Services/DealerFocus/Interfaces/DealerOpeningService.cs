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
            _emailconfig = emailconfig;
            _dealerOpeningLog = dealerOpeningLog;
            _emailSender = emailSender;
            _env = env;


        }
        public async Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> attachments)
        {
            var user = _userInfoSvc.Where(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId).FirstOrDefault();

            var _dealerOpening = model.ToMap<DealerOpeningModel, DealerOpening>();
            _dealerOpening.NextApprovarId = user.Id;
            _dealerOpening.DealerOpeningStatus = (int)DealerOpeningStatus.Pending;
            var result = await _dealerOpeningSvc.CreateAsync(_dealerOpening);
            var _dealerOpeningModel = result.ToMap<DealerOpening, DealerOpeningModel>();
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.DealerOpening);
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(DealerOpening) });
                //   _dealerOpeningModel.Attachments.Add(attachment.ToMap<Attachment,AttachmentModel>());
            }

            return _dealerOpeningModel;
        }

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


        public async Task<IPagedList<DealerOpeningModel>> GetDealerOpeningPendingListAsync(int index, int pageSize, string search)
        {
            var result = _mapper.Map<List<DealerOpeningModel>>(await _dealerOpeningSvc.Where(p => p.NextApprovarId == AppIdentity.AppUser.UserId && p.DealerOpeningStatus == (int)DealerOpeningStatus.Pending).ToListAsync());


            return await result.ToPagedListAsync(index, pageSize);


        }


        public async Task<bool> ChangeDealerStatus(DealerOpeningStatusChangeModel model)
        {
            var user = _userInfoSvc.Where(p => p.Id == AppIdentity.AppUser.UserId).FirstOrDefault();
            var dealer = _dealerOpeningSvc.Where(p => p.Id == model.DealerOpeningId).FirstOrDefault();
            var emailConfig = _emailconfig.Where(p => p.Designation == user.EmployeeRole.ToString()).FirstOrDefault();
            if (model.Status == (int)DealerOpeningStatus.Approved)
            {
                if (emailConfig != null)
                {
                    dealer.NextApprovarId = null;
                    dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Approved;

                }
                else
                {
                    dealer.NextApprovarId = _userInfoSvc.Where(p => p.EmployeeId == user.ManagerId).FirstOrDefault().Id;

                }
                //dealer.NextApprovarId = _userInfoSvc.Where(p => p.EmployeeId == user.ManagerId).FirstOrDefault().Id;

            }
            else if (model.Status == (int)DealerOpeningStatus.Rejected)
            {
                dealer.DealerOpeningStatus = (int)DealerOpeningStatus.Rejected;
            }
            dealer.CurrentApprovarId = AppIdentity.AppUser.UserId;
            await _dealerOpeningSvc.UpdateAsync(dealer);
            await DealerStatusLog(dealer, "DealerStatus", model.Status.ToString());
            if (emailConfig != null)
            {
                await sendEmail(emailConfig.Email, dealer.Id);
            }
            return false;
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
            dealerOpening.NextApprovarId = managerUser.Id;
            dealerOpening.DealerOpeningStatus = (int)DealerOpeningStatus.Pending;

            foreach (var attach in dealerOpening.DealerOpeningAttachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                {
                    try
                    {
                        attach.Path = await _fileUploadSvc.SaveImageAsync(
                            attach.Path,
                            fileName, FileUploadCode.DealerOpening,
                            300, 300);
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

        public async Task<IEnumerable<DealerOpeningModel>> AppGetDealerOpeningListAsync()
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();
            var result = await Task.Run(() => _dealerOpeningSvc.GetAllInclude(f => f.DealerOpeningAttachments));
            return _mapper.Map<List<DealerOpeningModel>>(result);
        }
        public async Task<DealerOpeningModel> GetDealerOpeningDetailById(int id)
        {
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DealerOpeningAttachmentModel, DealerOpeningAttachment>().ReverseMap();
            //    cfg.CreateMap<DealerOpeningModel, DealerOpening>().ReverseMap();
            //}).CreateMapper();

            var result = await _dealerOpeningSvc.FindIncludeAsync(f => f.Id == id, f => f.DealerOpeningAttachments, f => f.dealerOpeningLogs);
            //foreach (var item in result.dealerOpeningLogs)
            //{
            //    var user = _userInfoSvc.Find(p => p.Id == item.UserId);
            //    item.User = user;
            //}

            //return _mapper.Map<DealerOpeningModel>(result.dealerOpeningLogs.OrderByDescending(p=>p.CreatedTime)); ;
            return _mapper.Map<DealerOpeningModel>(result);

        }


        private async Task DealerStatusLog(DealerOpening dealerInfoId, string propertyName, string propertyValue)
        {

            var DealerStatusLog = new DealerOpeningLog()
            {
                DealerOpening = dealerInfoId,
                UserId = AppIdentity.AppUser.UserId,
                PropertyName = propertyName,
                PropertyValue = propertyValue
            };
            var res = await _dealerOpeningLog.CreateAsync(DealerStatusLog);


        }


        private async Task sendEmail(string email, int dealeropeningId)
        {
            try
            {
                var dealer = _dealerOpeningSvc.Find(p => p.Id == dealeropeningId);
                var attachment = await _dealerOpeningAttachmentSvc.FindAllAsync(p => p.Id == dealeropeningId);
                List<System.Net.Mail.Attachment> lstAttachment = new List<System.Net.Mail.Attachment>();
                if (lstAttachment.Count > 0)
                {
                    foreach (var item in attachment)
                    {
                        string path = Path.Combine(_env.ContentRootPath, item.Path);
                        var url = new System.Net.Mail.Attachment(path);
                        lstAttachment.Add(url);
                    }

                }
                string[] lstemail = email.Split(',');


                foreach (var item in lstemail)
                {
                    var createdBy = _userInfoSvc.Find(p => p.Id == dealer.CreatedBy);
                    var LastApprovar = _userInfoSvc.Find(p => p.Id == dealer.CurrentApprovarId); ;
                    //string messageBody = string.Format(ConstantsLeadValue.OpeningMailBody, createdBy.UserName, LastApprovar.UserName);
                    //string subject = string.Format(ConstantsLeadValue.OpeningMailSubject, dealeropeningId);

                    await _emailSender.SendEmailWithAttachmentAsync(item, ConstantsLeadValue.OpeningMailSubject, ConstantsLeadValue.OpeningMailBody, lstAttachment);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<List<DealerOpening>> GetDealerOpeningPendingListForNotificationAsync()
        {
            var result = await _dealerOpeningSvc.GetAllInclude(p => p.CurrentApprovar, p => p.NextApprovar).Where(p => p.NextApprovarId == AppIdentity.AppUser.UserId && p.Status == (int)DealerOpeningStatus.Pending).ToListAsync();
            return result;
        }
    }
}
