using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Painter;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using PNTR = Berger.Data.MsfaEntity.PainterRegistration;
namespace BergerMsfaApi.Services.PainterRegistration.Implementation
{
    public class PainterRegistrationService : IPainterRegistrationService
    {
        private readonly IRepository<PainterStatusLog> _painterStatusLog;
        private readonly IRepository<Painter> _painterSvc;
        private readonly IRepository<Attachment> _attachmentSvc;
        private readonly IRepository<PainterAttachment> _painterAttachmentSvc;
        private readonly IFileUploadService _fileUploadSvc;
        private readonly IRepository<SaleGroup> _saleGroupSvc;
        private readonly IRepository<Zone> _zoneSvc;
        private readonly IRepository<Territory> _territorySvc;
        private readonly IRepository<Depot> _depotSvc;
        private readonly IRepository<AttachedDealerPainter> _attachedDealerSvc;
        private readonly IRepository<PainterCall> _painterCallSvc;
        private readonly IRepository<PainterCompanyMTDValue> _painterCallCompanyMTDValueSvc;
        private readonly IRepository<AttachedDealerPainterCall> _painterCallAttachedDealerValueSvc;
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PainterRegistrationService(
             IRepository<PainterStatusLog> painterStatusLog,
             IRepository<Painter> painterSvc,
             IRepository<PainterAttachment> painterAttachmentSvc,
             IFileUploadService fileUploadSvc,
             IRepository<Attachment> attachmentSvc,
             IRepository<SaleGroup> saleGroupSvc,
             IRepository<Zone> zoneSvc,
             IRepository<Territory> territorySvc,
             IRepository<Depot> depotSvc,
             IRepository<AttachedDealerPainter> attachedDealerSvc,
             IRepository<PNTR.PainterCall> painterCallSvc,
             IRepository<PainterCompanyMTDValue> painterCallCompanyMTDValueSvc,
             IRepository<AttachedDealerPainterCall> painterCallAttachedDealerValueSvc,
             IRepository<DealerInfo> dealerInfoSvc,
              IMapper mapper,
              ApplicationDbContext context

            )
        {
            _painterStatusLog = painterStatusLog;
            _painterSvc = painterSvc;
            _fileUploadSvc = fileUploadSvc;
            _attachmentSvc = attachmentSvc;
            _painterAttachmentSvc = painterAttachmentSvc;
            _territorySvc = territorySvc;
            _depotSvc = depotSvc;
            _zoneSvc = zoneSvc;
            _saleGroupSvc = saleGroupSvc;
            _attachedDealerSvc = attachedDealerSvc;
            _painterCallSvc = painterCallSvc;
            _painterCallCompanyMTDValueSvc = painterCallCompanyMTDValueSvc;
            _painterCallAttachedDealerValueSvc = painterCallAttachedDealerValueSvc;
            _dealerInfoSvc = dealerInfoSvc;
            _mapper = mapper;
            _context = context;

        }

        public async Task<PainterModel> CreatePainterAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.CreateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }
        public async Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments)
        {
            var _painter = model.ToMap<PainterModel, Painter>();

            var _fileName = $"{_painter.PainterImageUrl}_{_painter.Phone}";
            var _path = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.PainterRegistration);
            _painter.PainterImageUrl = _path;

            var result = await _painterSvc.CreateAsync(_painter);

            var painterModel = result.ToMap<Painter, PainterModel>();
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.PainterRegistration);
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(Painter) });
                //    painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }
            return painterModel;
        }

        public async Task<bool> PainterStatusUpdate(PainterStatusUpdateModel model)
        {
            var userId = AppIdentity.AppUser.UserId;

            var find = await _painterSvc.FindAsync(p => p.Id == model.Id);
            if (find == null) return false;

            find.Status = (Status)model.Status;
            await _painterSvc.UpdateAsync(find);

            PainterStatusLog painterStatusLog = new PainterStatusLog
            {
                CreatedTime = DateTime.Now,
                PainterId = model.Id,
                UserId = userId,
                Status = (Status)model.Status,
                Reason = model.Reason
            };

            await _painterStatusLog.CreateAsync(painterStatusLog);

            return true;
        }

        public async Task<PainterModel> GetPainterByIdAsync(int Id)
        {
            var result = await _painterSvc.GetFirstOrDefaultIncludeAsync(
                    painter => painter,
                    painter => painter.Id == Id,
                    null,
                    painter => painter
                                      .Include(i => i.Attachments)
                                      .Include(i => i.AttachedDealers)
                                      .Include(i => i.PainterCat)
                                      .Include(i => i.PainterCalls).ThenInclude(i => i.PainterCompanyMTDValue).ThenInclude(i=>i.Company)
                                      .Include(i => i.PainterCalls).ThenInclude(i => i.PainterCat)
                                      .Include(i => i.PainterCalls).ThenInclude(i => i.AttachedDealers),

                    true
                );

            
            var painterModel = _mapper.Map<PainterModel>(result);

            if (painterModel.PainterCalls.Any()) 
                painterModel.PainterCalls = painterModel.PainterCalls.OrderByDescending(o => o.CreatedTime).ToList();
            
            //calculate painter call mtd value
            foreach (var painterCall in painterModel.PainterCalls)
            {
                decimal sum = 0;
                foreach (var mtdvalue in painterCall.PainterCompanyMTDValue)
                {
                    sum += Convert.ToDecimal(mtdvalue.Value);
                }
                if (sum == 0)
                {
                    foreach (var item in painterCall.PainterCompanyMTDValue)
                    {
                        item.CountInPercent = 0;
                        item.CumelativeInPercent = 0;
                    }
                }
                else
                {
                    //for percent
                    foreach (var item in painterCall.PainterCompanyMTDValue)
                    {
                        float cal = (float)(item.Value / sum * 100);
                        item.CountInPercent = (float)Math.Round(cal, 2);
                    }
                    //for cumulative percent
                    for (int it = 0;it < painterCall.PainterCompanyMTDValue.Count; it++)
                    {
                        if (it < painterCall.PainterCompanyMTDValue.Count - 1)
                        {
                            var item = painterCall.PainterCompanyMTDValue[it];
                            var forwardItem = painterCall.PainterCompanyMTDValue[it + 1];
                            float cal = ((float)(item.CountInPercent * forwardItem.CountInPercent) / 100);
                            item.CumelativeInPercent = (float)Math.Round(cal, 2);

                            painterCall.PainterCompanyMTDValue[it].CumelativeInPercent = item.CumelativeInPercent;
                        }
                       
                    }

                }
            }

            //dummy data

            //painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = "Mr. qwds fsad", CustomerNo = 103 });
            //painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = "M. abc def", CustomerNo = 101 });
            //painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = "Mr. bds fsad", CustomerNo = 102 });

            var dealerIds = new List<int>();
            if (painterModel.AttachedDealers.Any()) dealerIds.AddRange(painterModel.AttachedDealers);
            if (painterModel.PainterCalls.Any() && painterModel.PainterCalls[0].AttachedDealers.Any()) dealerIds.AddRange(painterModel.PainterCalls.SelectMany(x => x.AttachedDealers.Select(y => y.DealerId)));

            if (dealerIds.Any())
            {
                var dealers = await _dealerInfoSvc.GetAllIncludeAsync(x => new { x.Id, x.CustomerNo, x.CustomerName },
                                        x => dealerIds.Contains(x.Id),
                                        null, null, true);

                if (painterModel.AttachedDealers.Any())
                {
                    painterModel.DealerDetails = new List<AttachedDealerDetails>();
                    foreach (var dId in painterModel.AttachedDealers)
                    {
                        var dealer = dealers.FirstOrDefault(d => d.Id == dId);
                        painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = dealer?.CustomerName ?? string.Empty, CustomerNo = dealer?.CustomerNo ?? string.Empty });
                    }
                }

                if (painterModel.PainterCalls.Any() && painterModel.PainterCalls[0].AttachedDealers.Any())
                {
                    foreach (var pc in painterModel.PainterCalls)
                    {
                        foreach (var pad in pc.AttachedDealers)
                        {
                            var dealer = dealers.FirstOrDefault(d => d.Id == pad.DealerId);
                            pad.CustomerName = dealer?.CustomerName ?? string.Empty; 
                            pad.CustomerNo = dealer?.CustomerNo ?? string.Empty;
                        }
                    }
                }
            }

            //foreach (var id in painterModel.AttachedDealers)
            //{
            //    var dealerDetails = await _dealerInfoSvc.FindAsync(dealerInfo => dealerInfo.Id == id);
            //    painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = dealerDetails?.CustomerName??string.Empty, CustomerNo = dealerDetails?.CustomerNo??string.Empty });
                
            //}

            // var painterAttachments = await _attachmentSvc.FindAllAsync(f => f.ParentId == painterModel.Id && f.TableName == nameof(Painter));
            // foreach (var item in painterCallList)
            //painterModel.PainterCallList.Add(item); //(PainterCall.ToMap<Attachment, AttachmentModel>()

            #region get area mapping data
            var depots = (await _depotSvc.FindAllAsync(x => painterModel.Depot == x.Werks));
            var saleGroups = (await _saleGroupSvc.FindAllAsync(x => painterModel.SaleGroup == x.Code));
            //var territories = (await _territorySvc.FindAllAsync(x => painterModel.Territory == x.Code));
            //var zones = (await _zoneSvc.FindAllAsync(x => painterModel.Zone == x.Code));

            painterModel.DepotName = depots.FirstOrDefault(x => x.Werks == painterModel.Depot)?.Name1 ?? string.Empty;
            painterModel.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == painterModel.SaleGroup)?.Name ?? string.Empty;
            //painterModel.TerritoryName = territories.FirstOrDefault(x => x.Code == painterModel.Territory)?.Code ?? string.Empty;
            //painterModel.ZoneName = zones.FirstOrDefault(x => x.Code == painterModel.Zone)?.Code ?? string.Empty;
            painterModel.TerritoryName = painterModel.Territory;
            painterModel.ZoneName = painterModel.Zone;
            #endregion

            return painterModel;
        }

        private int SkipCount(QueryObjectModel query) => (query.Page - 1) * query.PageSize;


        public async Task<QueryResultModel<PainterModel>> GetPainterListAsync(PainterRegistrationReportSearchModel query)
        {

            var painters = await (from p in _context.Painters
                                  join u in _context.UserInfos on p.EmployeeId equals u.EmployeeId into uleftjoin
                                  from userInfo in uleftjoin.DefaultIfEmpty()
                                  join d in _context.DropdownDetails on p.PainterCatId equals d.Id into dleftjoin
                                  from dropDownInfo in dleftjoin.DefaultIfEmpty()
                                  //join adp in _context.AttachedDealerPainters on p.AttachedDealerCd equals adp.Id.ToString() into adpleftjoin
                                  //from adpInfo in adpleftjoin.DefaultIfEmpty()
                                  //join di in _context.DealerInfos on adpInfo.DealerId equals di.Id into dileftjoin
                                  //from diInfo in dileftjoin.DefaultIfEmpty()
                                  join dep in _context.Depots on p.Depot equals dep.Werks into depleftjoin
                                  from depinfo in depleftjoin.DefaultIfEmpty()
                                  join sg in _context.SaleGroup on p.SaleGroup equals sg.Code into sgleftjoin
                                  from sginfo in sgleftjoin.DefaultIfEmpty()
                                  join t in _context.Territory on p.Territory equals t.Code into tleftjoin
                                  from tinfo in tleftjoin.DefaultIfEmpty()
                                  join z in _context.Zone on p.Zone equals z.Code into zleftjoin
                                  from zinfo in zleftjoin.DefaultIfEmpty()
                                  where (
                                     (!query.UserId.HasValue || userInfo.Id == query.UserId.Value)
                                     && (string.IsNullOrWhiteSpace(query.Depot) || p.Depot == query.Depot)
                                     && (!query.Territories.Any() || query.Territories.Contains(p.Territory))
                                     && (!query.Zones.Any() || query.Zones.Contains(p.Zone))
                                     && (!query.FromDate.HasValue || p.CreatedTime.Date >= query.FromDate.Value.Date)
                                     && (!query.ToDate.HasValue || p.CreatedTime.Date <= query.ToDate.Value.Date)
                                     && (!query.PainterId.HasValue || p.Id == query.PainterId.Value)
                                     && (!query.PainterType.HasValue || p.PainterCatId == query.PainterType.Value)
                                     && (string.IsNullOrWhiteSpace(query.PainterMobileNo) || p.Phone == query.PainterMobileNo)
                                  )
                                  orderby p.CreatedTime descending
                                  select new PainterModel
                                  {
                                      Id=p.Id,
                                      PainterName=p.PainterName,
                                      PainterNo=p.PainterNo,
                                      PainterCode=p.PainterCode,
                                      PainterImageUrl=p.PainterImageUrl,
                                      Phone=p.Phone,
                                      SaleGroupName= sginfo.Name,
                                      TerritoryName= p.Territory,
                                      ZoneName= p.Zone,
                                      DepotName= depinfo.Name1,
                                      Status =(int)p.Status
                                  }).Skip(this.SkipCount(query)).Take(query.PageSize).ToListAsync();




            var queryResult = new QueryResultModel<PainterModel>
            {
                Items = painters,
                TotalFilter = painters.Count(),
                Total = painters.Count()
            };

            return queryResult;

            //return result.ToPagedList(index, pageSize);

        }

        public async Task<int> DeleteAsync(int Id)
        {
            if (await _attachmentSvc.AnyAsync((f => f.ParentId == Id && f.TableName == nameof(Painter))))
                await _attachmentSvc.DeleteAsync(f => f.ParentId == Id && f.TableName == nameof(Painter));
            return await _painterSvc.DeleteAsync(f => f.Id == Id);
        }

        public async Task<int> DeletePainterCallAsync(int id)
        {
            if (await _painterCallAttachedDealerValueSvc.AnyAsync(x => x.PainterCallId == id)) 
                await _painterCallAttachedDealerValueSvc.DeleteAsync(x => x.PainterCallId == id);
            if (await _painterCallCompanyMTDValueSvc.AnyAsync(x => x.PainterCallId == id)) 
                await _painterCallCompanyMTDValueSvc.DeleteAsync(x => x.PainterCallId == id);
            return await _painterCallSvc.DeleteAsync(f => f.Id == id);
        }

        public async Task<bool> IsExistAsync(int Id) => await _painterSvc.IsExistAsync(f => f.Id == Id);
        public async Task<bool> IsExistPainterCallAsync(int id) => await _painterCallSvc.IsExistAsync(f => f.Id == id);

        public async Task<PainterModel> UpdateAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.UpdateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file)
        {
            var _painter = await _painterSvc.FindAsync(f => f.Id == painterId);

            if (_painter.PainterImageUrl != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImageUrl);
            if (_painter != null)
            {

                var _fileName = $"{painterId}_{_painter.PainterName}";
                var _path = await _fileUploadSvc.SaveImageAsync(file, _fileName, FileUploadCode.PainterRegistration);
                _painter.PainterImageUrl = _path;

            }
            await _painterSvc.UpdateAsync(_painter);
            return _painter.ToMap<Painter, PainterModel>();
        }
        public async Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments)
        {
            var _painter = await _painterSvc.FindIncludeAsync(f => f.Id == painterId);
            if (_painter == null) return null;

            if (_painter.PainterImageUrl != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImageUrl);
            var _fileName = $"{painterId}_{_painter.PainterName}";
            _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.PainterRegistration);

            await _painterSvc.UpdateAsync(_painter);

            var painterModel = _painter.ToMap<Painter, PainterModel>();

            var existing = await _attachmentSvc.FindAllAsync(f => f.TableName == nameof(Painter) && f.ParentId == painterModel.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);

                await _attachmentSvc.DeleteAsync(f => f.Id == item.Id);

            }
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.FileName, FileUploadCode.PainterRegistration);
                var _newAttachment = new Attachment { Path = path, Name = attach.FileName, TableName = nameof(Painter), Format = Path.GetExtension(attach.FileName), Size = 1, ParentId = _painter.Id };
                var attachment = await _attachmentSvc.CreateAsync(_newAttachment);
            }
            return painterModel;
        }
        #region App
        public async Task<IEnumerable<PainterModel>> AppGetPainterListAsync(string employeeId)
        {
            var appUser = AppIdentity.AppUser;

            var _painters = await _painterSvc.GetAllIncludeAsync(
                s => s,
                //f => f.EmployeeId == employeeId && 
                f => ((appUser.EmployeeRole == (int)EnumEmployeeRole.Admin || appUser.EmployeeRole == (int)EnumEmployeeRole.GM) ||
                            ((!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(f.Depot)) &&
                            (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(f.Territory)) &&
                            (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(f.Zone)))) && 
                        f.Status == Status.Active,
                null,
                a => a.Include(f => f.AttachedDealers).Include(f => f.Attachments).Include(f => f.PainterCat),
                false
                );

            var mapResults = _mapper.Map<List<PainterModel>>(_painters);

            #region get area mapping data
            //var depotIds = mapResults.Select(x => x.Depot).Distinct().ToList();
            //var saleGroupIds = mapResults.Select(x => x.SaleGroup).Distinct().ToList();
            //var territoryIds = mapResults.Select(x => x.Territory).Distinct().ToList();
            //var zoneIds = mapResults.Select(x => x.Zone).Distinct().ToList();

            //var depots = (await _depotSvc.FindAllAsync(x => depotIds.Contains(x.Werks)));
            //var saleGroups = (await _saleGroupSvc.FindAllAsync(x => saleGroupIds.Contains(x.Code)));
            //var territories = (await _territorySvc.FindAllAsync(x => territoryIds.Contains(x.Code)));
            //var zones = (await _zoneSvc.FindAllAsync(x => zoneIds.Contains(x.Code)));

            //foreach (var item in mapResults)
            //{
            //    item.DepotName = depots.FirstOrDefault(x => x.Werks == item.Depot)?.Name1 ?? string.Empty;
            //    item.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == item.SaleGroup)?.Name ?? string.Empty;
            //    item.TerritoryName = territories.FirstOrDefault(x => x.Code == item.Territory)?.Name ?? string.Empty;
            //    item.ZoneName = zones.FirstOrDefault(x => x.Code == item.Zone)?.Name ?? string.Empty;
            //}
            #endregion

            return mapResults;
        }

        public async Task<PainterModel> AppCreatePainterAsync(PainterModel model)
        {
            //var userId = AppIdentity.AppUser.UserId;

            var _painter = _mapper.Map<Painter>(model);
            var _painterImageFileName = $"{_painter.PainterName}_{_painter.Phone}";
            _painterImageFileName = _painterImageFileName.Replace(" ", "_");
            if (!string.IsNullOrEmpty(_painter.PainterImageUrl))
                _painter.PainterImageUrl =
                                  await _fileUploadSvc
                                  .SaveImageAsync(_painter.PainterImageUrl, _painterImageFileName, FileUploadCode.PainterRegistration);

            foreach (var attach in _painter.Attachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                    attach.Path = await _fileUploadSvc.SaveImageAsync(attach.Path, fileName, FileUploadCode.PainterRegistration);
            }

            _painter.PainterNo = GeneratePainterNo(model.EmployeeId).Result;
            _painter.Status = Status.Active;
            //TODO: need to generate code
            _painter.PainterCode = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            var result = await _painterSvc.CreateAsync(_painter);
            return _mapper.Map<PainterModel>(result);
        }

        public async Task<PainterModel> AppUpdatePainterAsync(PainterModel model)
        {
            var _painter = _mapper.Map<Painter>(model);

            var _fileName = $"{model.PainterName}_{model.Phone}";

            var _findPainter = await _painterSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.Attachments);

            if (!string.IsNullOrEmpty(_findPainter.PainterImageUrl)) await _fileUploadSvc.DeleteImageAsync(_findPainter.PainterImageUrl);

            if (!string.IsNullOrEmpty(_painter.PainterImageUrl)) _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _fileName, FileUploadCode.PainterRegistration);

            if (await _attachedDealerSvc.AnyAsync(f => f.PainterId == model.Id)) await _attachedDealerSvc.DeleteAsync(f => f.PainterId == model.Id);

            // foreach (var item in model.AttachedDealers) _painter.AttachedDealers.Add(new AttachedDealerPainter { Dealer = item });

            foreach (var item in _findPainter.Attachments)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);
                await _painterAttachmentSvc.DeleteAsync(f => f.Id == item.Id);
            }

            foreach (var attach in _painter.Attachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                    attach.Path = await _fileUploadSvc.SaveImageAsync(attach.Path, fileName, FileUploadCode.PainterRegistration);

            }

            var result = await _painterSvc.UpdateAsync(_painter);
            return _mapper.Map<PainterModel>(result);
        }

        public async Task<PainterModel> AppGetPainterByIdAsync(int Id)
        {
            var painter = (await _painterSvc.GetAllIncludeAsync(
              s => s,
              f => f.Id == Id,
              null,
              a => a.Include(f => f.AttachedDealers).Include(f => f.Attachments),
              false
              )).FirstOrDefault();
            return _mapper.Map<PainterModel>(painter);

        }

        public async Task<bool> AppDeletePainterByIdAsync(int Id) => await _painterSvc.DeleteAsync(f => f.Id == Id) == 1 ? true : false;

        public async Task<PainterModel> AppGetPainterByPhonesync(string Phone)
        {
            var result = Task.Run(() =>

                (from p in _painterSvc.GetAll().Where(f => f.Phone == Phone)
                 join sg in _saleGroupSvc.GetAll()
               on p.SaleGroup equals sg.Code into sgLeftJoin
                 from saleGroup in sgLeftJoin.DefaultIfEmpty()
                 join t in _territorySvc.GetAll()
               on p.Territory equals t.Code into terriyLeftjoin
                 from territory in terriyLeftjoin.DefaultIfEmpty()
                 join z in _zoneSvc.GetAll()
               on p.Zone equals z.Code into zLeftJoin
                 from zone in zLeftJoin.DefaultIfEmpty()
                 select new PainterModel
                 {
                     PainterName = p.PainterName,
                     Phone = p.Phone,
                     DepotName = null,
                     SaleGroup = saleGroup != null ? saleGroup.Name : null,
                     Zone = zone != null ? zone.Name : null,
                     Territory = territory != null ? territory.Name : null,
                     Loyality = p.Loyality,
                     HasDbbl = p.HasDbbl,
                 }).FirstOrDefault()

              );

            return await result;

        }

        public async Task<string> GeneratePainterNo(string employeeId)
        {
            var lastPainterNo = (await _painterSvc.AnyAsync(p => p.EmployeeId == employeeId)) ? 
                                    (await _painterSvc.FindAllAsync(p => p.EmployeeId == employeeId))
                                        .Max(p => CustomConvertExtension.ObjectToInt(p.PainterNo))
                                    : 0;

            return (lastPainterNo + 1).ToString();
        }

        public async Task<PainterUpdateModel> GetPainterForEditAsync(int id)
        {
            var find = await _painterSvc.GetFirstOrDefaultIncludeAsync(
                       p => p,
                       p => p.Id == id,
                       null,
                       p => p.Include(i => i.AttachedDealers),
                       true
                   );

            if (find == null) throw new Exception("Painter not found.");

            var result = _mapper.Map<PainterUpdateModel>(find);

            return result;
        }

        public async Task<bool> PainterUpdateAsync(PainterUpdateModel model)
        {
            var find = await _painterSvc.GetFirstOrDefaultIncludeAsync(
                       p => p,
                       p => p.Id == model.Id,
                       null,
                       null,
                       true
                   );

            if (find == null) throw new Exception("Painter not found.");

            if (!string.IsNullOrWhiteSpace(model.PainterImageBase64))
            {
                var fileName = $"{model.PainterName}_{model.Phone}";
                model.PainterImageUrl = model.PainterImageBase64.Substring(model.PainterImageBase64.LastIndexOf(',') + 1);
                find.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(model.PainterImageUrl, fileName, FileUploadCode.PainterRegistration);
            }

            find.Depot = model.Depot;
            find.Territory = model.Territory;
            find.Zone = model.Zone;
            find.PainterCatId = model.PainterCatId;
            find.PainterName = model.PainterName;
            find.Address = model.Address;
            find.Phone = model.Phone;
            find.NoOfPainterAttached = model.NoOfPainterAttached;
            find.HasDbbl = model.HasDbbl;
            find.AccDbblNumber = model.AccDbblNumber;
            find.AccDbblHolderName = model.AccDbblHolderName;
            find.PassportNo = model.PassportNo;
            find.NationalIdNo = model.NationalIdNo;
            find.BrithCertificateNo = model.BrithCertificateNo;
            find.IsAppInstalled = model.IsAppInstalled;
            find.Remark = model.Remark;
            find.AvgMonthlyVal = model.AvgMonthlyVal;
            find.Loyality = model.Loyality;
            find.EmployeeId = model.EmployeeId;

            await _painterSvc.UpdateAsync(find);


            #region
            var previousAttachDealers = (await _attachedDealerSvc.FindAllAsync(x => x.PainterId == model.Id)).ToList();
            if (previousAttachDealers.Any()) await _attachedDealerSvc.DeleteListAsync(previousAttachDealers);

            if (model.AttachedDealerIds.Any())
            {
                var newAttachDealers = model.AttachedDealerIds.Select(x => new AttachedDealerPainter() { DealerId = x, PainterId = model.Id }).ToList();
                await _attachedDealerSvc.CreateListAsync(newAttachDealers);
            }
            #endregion

            #region
            var prevoiusPainterCalls = (await _painterCallSvc.FindAllAsync(x => x.PainterId == model.Id)).ToList();
            if (prevoiusPainterCalls.Any())
            {
                foreach (var item in prevoiusPainterCalls)
                {
                    item.Territory = model.Territory;
                    item.Zone = model.Zone;
                }
                await _painterCallSvc.UpdateListAsync(prevoiusPainterCalls);
            }
            #endregion

            return true;
        }

        public async Task DeleteImage(PainterImageModel painterImageModel)
        {
            var item = await _painterSvc.FirstOrDefaultAsync(x => x.Id == painterImageModel.Id);
            var fullPath = painterImageModel.URL;

            if (item != null)
            {
                switch (painterImageModel.Type)
                {
                    case "painterImageUrl":
                        item.PainterImageUrl = null;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(fullPath))
                {
                    await _fileUploadSvc.DeleteImageAsync(fullPath);
                    await _painterSvc.UpdateAsync(item);
                }
            }
        }

        #endregion

    }
}
