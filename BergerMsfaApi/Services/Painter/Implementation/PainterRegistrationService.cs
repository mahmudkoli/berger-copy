using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Hirearchy;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Painter;
using BergerMsfaApi.Models.PainterRegistration;
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
        private readonly IRepository<DealerInfo> _dealerInfoSvc;
        private readonly IMapper _mapper;
        public PainterRegistrationService(
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
             IRepository<DealerInfo> dealerInfoSvc,
              IMapper mapper

            )
        {
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
            _dealerInfoSvc = dealerInfoSvc;
            _mapper = mapper;

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
                                      .Include(i => i.PainterCalls).ThenInclude(i => i.PainterCompanyMTDValue).ThenInclude(i=>i.Company),

                    true
                );

            
            var painterModel = _mapper.Map<PainterModel>(result);
            
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

            foreach (var id in painterModel.AttachedDealers)
            {
                var dealerDetails = await _dealerInfoSvc.FindAsync(dealerInfo => dealerInfo.Id == id);
                painterModel.DealerDetails.Add(new AttachedDealerDetails { CustomerName = dealerDetails?.CustomerName??string.Empty, CustomerNo = dealerDetails?.CustomerNo??string.Empty });
                
            }

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

        public async Task<IPagedList<PainterModel>> GetPainterListAsync(int index, int pageSize, string search)
        {
            var result = (await _painterSvc.GetAllAsync()).ToMap<Painter, PainterModel>();
            if (!string.IsNullOrEmpty(search))
                result = result.Search(search);

            #region get area mapping data
            var depotIds = result.Select(x => x.Depot).Distinct().ToList();
            var saleGroupIds = result.Select(x => x.SaleGroup).Distinct().ToList();
            //var territoryIds = result.Select(x => x.Territory).Distinct().ToList();
            //var zoneIds = result.Select(x => x.Zone).Distinct().ToList();

            var depots = (await _depotSvc.FindAllAsync(x => depotIds.Contains(x.Werks)));
            var saleGroups = (await _saleGroupSvc.FindAllAsync(x => saleGroupIds.Contains(x.Code)));
            //var territories = (await _territorySvc.FindAllAsync(x => territoryIds.Contains(x.Code)));
            //var zones = (await _zoneSvc.FindAllAsync(x => zoneIds.Contains(x.Code)));

            foreach (var item in result)
            {
                item.DepotName = depots.FirstOrDefault(x => x.Werks == item.Depot)?.Name1 ?? string.Empty;
                item.DepotName += $" ({item.Depot})";
                item.SaleGroupName = saleGroups.FirstOrDefault(x => x.Code == item.SaleGroup)?.Name ?? string.Empty;
                //item.TerritoryName = territories.FirstOrDefault(x => x.Code == item.Territory)?.Code ?? string.Empty;
                //item.ZoneName = zones.FirstOrDefault(x => x.Code == item.Zone)?.Code ?? string.Empty;
                item.TerritoryName = item.Territory;
                item.ZoneName = item.Zone;
            }
            #endregion

            return result.ToPagedList(index, pageSize);

        }


        public async Task<int> DeleteAsync(int Id)
        {
            if (await _attachmentSvc.AnyAsync((f => f.ParentId == Id && f.TableName == nameof(Painter))))
                await _attachmentSvc.DeleteAsync(f => f.ParentId == Id && f.TableName == nameof(Painter));
            return await _painterSvc.DeleteAsync(f => f.Id == Id);
        }


        public async Task<bool> IsExistAsync(int Id) => await _painterSvc.IsExistAsync(f => f.Id == Id);

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


            var _painters = await _painterSvc.GetAllIncludeAsync(
                s => s,
                f => f.EmployeeId == employeeId,
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
            var _painter = _mapper.Map<Painter>(model);
            var _painterImageFileName = $"{_painter.PainterName}_{_painter.Phone}";
            _painterImageFileName = _painterImageFileName.Replace(" ", "_");
            if (!string.IsNullOrEmpty(_painter.PainterImageUrl))
                _painter.PainterImageUrl =
                                  await _fileUploadSvc
                                  .SaveImageAsync(_painter.PainterImageUrl, _painterImageFileName, FileUploadCode.RegisterPainter, 300, 300);

            foreach (var attach in _painter.Attachments)
            {
                attach.Name = attach.Name.Replace(" ", "_");
                var fileName = attach.Name + "_" + Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(attach.Path))
                    attach.Path = await _fileUploadSvc.SaveImageAsync(attach.Path, fileName, FileUploadCode.RegisterPainter, 300, 300);
            }

            var result = await _painterSvc.CreateAsync(_painter);
            return _mapper.Map<PainterModel>(result);



        }

        public async Task<PainterModel> AppUpdatePainterAsync(PainterModel model)
        {
            var _painter = _mapper.Map<Painter>(model);

            var _fileName = $"{model.PainterName}_{model.Phone}";

            var _findPainter = await _painterSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.Attachments);

            if (!string.IsNullOrEmpty(_findPainter.PainterImageUrl)) await _fileUploadSvc.DeleteImageAsync(_findPainter.PainterImageUrl);

            if (!string.IsNullOrEmpty(_painter.PainterImageUrl)) _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _fileName, FileUploadCode.RegisterPainter, 300, 300);

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
                    attach.Path = await _fileUploadSvc.SaveImageAsync(attach.Path, fileName, FileUploadCode.RegisterPainter, 300, 300);

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


        #endregion

    }
}
