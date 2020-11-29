using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Setup.Implementation
{
    public class JourneyPlanService : IJourneyPlanService
    {
        private readonly IRepository<JourneyPlanDetail> _journeyPlanDetailSvc;
        private readonly IRepository<JourneyPlanMaster> _journeyPlanMasterSvc;
        private readonly IRepository<DealerInfo> _dealerInforSvc;
        private readonly IRepository<UserInfo> _userInfoSvc;
        private readonly IRepository<FocusDealer> _focusDealerSvc;


        public JourneyPlanService(
            IRepository<JourneyPlanDetail> journeyPlanDetailSvc,
            IRepository<JourneyPlanMaster> journeyPlanMasterSvc,
            IRepository<DealerInfo> dealerInforSvc,
            IRepository<UserInfo> userInfoSvc,
            IRepository<FocusDealer> focusDealerSvc
            )
        {
            _journeyPlanDetailSvc = journeyPlanDetailSvc;
            _journeyPlanMasterSvc = journeyPlanMasterSvc;
            _dealerInforSvc = dealerInforSvc;
            _userInfoSvc = userInfoSvc;
            _focusDealerSvc = focusDealerSvc;
        }
        public async Task<bool> ChangePlanStatus(JourneyPlanStatusChangeModel model)
        {
            var find = await _journeyPlanMasterSvc.FindAsync(f => f.Id == model.PlanId);
            if (find != null)
            {
                find.PlanStatus = (PlanStatus)model.Status;
                if (PlanStatus.Approved == (PlanStatus)model.Status)
                {
                    find.ApprovedDate = DateTime.Now;
                    find.ApprovedById = AppIdentity.AppUser.UserId;
                }
                if (PlanStatus.Rejected == (PlanStatus)model.Status)
                {
                    find.RejectedDate = DateTime.Now;
                    find.RejectedBy = AppIdentity.AppUser.UserId;
                }
                find.Comment = model.Comment;
                await _journeyPlanMasterSvc.UpdateAsync(find);
                return true;
            }
            return false;
        }

        public async Task<int> DeleteJourneyAsync(int id)
        {
            await _journeyPlanDetailSvc.DeleteAsync(s => s.PlanId == id);
            return await _journeyPlanMasterSvc.DeleteAsync(s => s.Id == id);
        }

        public async Task<bool> IsExistAsync(int id) => await _journeyPlanMasterSvc.IsExistAsync(f => f.Id == id);

        public async Task<IEnumerable<JourneyPlanDetailModel>> GetJourneyPlanDetail()
        {

            var planList = await _journeyPlanMasterSvc.FindAllAsync(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId);

            var result = planList.Select(s => new JourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                PlanDate = s.PlanDate,
                Status = s.Status,
                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id).DefaultIfEmpty()
                                    on dealer.Id equals planDetail.DealerId

                                    select new DealerInfoModel
                                    {
                                        Id = planDetail.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        VisitDate = planDetail.VisitDate
                                    }).ToList()
            }).ToList();

            return result;

        }
        //JouneryPlanDealerList By EmployeeId For App
        public async Task<IEnumerable<DealerInfoModel>> AppGetJourneyPlanDealerList(string employeeId)
        {
          
            var planList = await _journeyPlanMasterSvc.FindAllAsync(f => f.EmployeeId == employeeId && f.PlanDate.Date >=DateTime.Now.Date);
            var result = (from plan in planList
                          join planDetail in _journeyPlanDetailSvc.GetAll() on plan.Id equals planDetail.PlanId
                          join dealer in _dealerInforSvc.GetAll()
                          on planDetail.DealerId equals dealer.Id
                          join f in _focusDealerSvc.GetAll() on new { a = planDetail.DealerId, b = plan.EmployeeId } equals new { a = f.Code, b = f.EmployeeId } into leftJoin
                          from fd in leftJoin.DefaultIfEmpty()
                          select new DealerInfoModel
                          {
                              Id = dealer.Id,
                              CustomerName = dealer.CustomerName,
                              CustomerNo = dealer.CustomerNo,
                              Territory = dealer.Territory,
                              ContactNo = dealer.ContactNo,
                              Address = dealer.Address,
                              IsFocused = fd != null ? (fd.Code > 0 ? true : false) : false,
                              VisitDate = plan.PlanDate.Date
                          }).ToList().Distinct();
            return result = (from r in result select r).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList();
                          
        }

        //LineManagerJourneyPlanList
        public async Task<IPagedList<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager(int index, int pageSize, string planDate)
        {
           
            var employeeIds = _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId);

            var plans =  (from p in _journeyPlanMasterSvc.GetAll()
                               join emp in employeeIds on p.EmployeeId equals emp.EmployeeId
                               orderby p.PlanDate.Date descending
                               select p);

            if (!string.IsNullOrEmpty(planDate))
                plans = plans.Where(f => f.PlanDate.Date == Convert.ToDateTime(planDate).Date);

            var result = await plans.ToPagedListAsync(index, pageSize);

            var final= new PagedList<JourneyPlanDetailModel>(
               result,
               result.Select(s => new JourneyPlanDetailModel
               {
                   Id = s.Id,
                   EmployeeId = s.EmployeeId,
                   Comment = s.Comment,
                   PlanDate = s.PlanDate,
                   Status = s.Status,
                   PlanStatus = s.PlanStatus,
                   DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                       join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id)
                                       on dealer.Id equals planDetail.DealerId
                                       join f in _focusDealerSvc.GetAll()
                                       on planDetail.DealerId equals f.Code into focus
                                       from fd in focus.DefaultIfEmpty()
                                       select new DealerInfoModel
                                       {
                                           Id = dealer.Id,
                                           CustomerName = dealer.CustomerName,
                                           CustomerNo = dealer.CustomerNo,
                                           Territory = dealer.Territory,
                                           ContactNo = dealer.ContactNo,
                                           Address = dealer.Address,
                                           IsFocused = fd.Code > 0 ? true : false,
                                           VisitDate = planDetail.VisitDate
                                       }).ToList().Select(s => s).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList(),

                   Employee = _userInfoSvc.Where(f => f.EmployeeId == s.EmployeeId)
                            .Select(s => new EmployeeModel
                            {
                                FirstName = $"{s.FirstName} {s.LastName}",
                                Department = s.DepartMent,
                                Designation = s.Designation,
                                PhoneNumber = s.PhoneNumber

                            }).FirstOrDefault()

               }));
            return final;
        }
       
        //JourneyPlanBy Detail By PlanId
        public async Task<JourneyPlanDetailModel> GetJourneyPlanDetailById(int PlanId)
        {

            var plan = await _journeyPlanMasterSvc.FindAsync(f => f.Id == PlanId);

            var result = new JourneyPlanDetailModel
            {

                Id = plan.Id,
                EmployeeId = plan.EmployeeId,
                PlanDate = plan.PlanDate,
                Status = plan.Status,
                PlanStatus=plan.PlanStatus,
                Comment=plan.Comment,

                Employee = _userInfoSvc.Where(f => f.EmployeeId == plan.EmployeeId)
                             .Select(s => new EmployeeModel
                             {
                                 FirstName = $"{s.FirstName} {s.LastName}",
                                 Department = s.DepartMent,
                                 Designation = s.Designation,
                                 PhoneNumber = s.PhoneNumber

                             }).FirstOrDefault(),


                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == plan.Id)
                                    on dealer.Id equals planDetail.DealerId
                                  //  join f in _focusDealerSvc.GetAll() on new { a = planDetail.DealerId, b = AppIdentity.AppUser.EmployeeId } equals new { a = f.Code, b = f.EmployeeId } into leftJoin
                                    join f in _focusDealerSvc.GetAll() on planDetail.DealerId equals  f.Code into leftJoin
                                    from fd in leftJoin.DefaultIfEmpty()
                                    select new DealerInfoModel
                                    {
                                        Id = dealer.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        ContactNo = dealer.ContactNo,
                                        Address = dealer.Address,
                                        IsFocused = fd.Code > 0 ? true : false,
                                        VisitDate = planDetail.VisitDate
                                    }).ToList().Select(s => s).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList(),

            };

            return result;



        }

        public async Task<PortalPlanDetailModel> PortalCreateJourneyPlan(PortalCreateJouneryModel model)
        {
            var result = new PortalPlanDetailModel();
            var plan = await _journeyPlanMasterSvc.CreateAsync(new JourneyPlanMaster {
                EmployeeId = AppIdentity.AppUser.EmployeeId, PlanDate = model.VisitDate, Status = Status.Pending,Comment=model.Comment});
            if (plan == null) return result;
            foreach (var id in model.Dealers)
            {

                await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = plan.Id });
                var dealerInfo = await _dealerInforSvc.FindAsync(f => f.Id == id);
                result.DealerInfo.Add(new DealerInfoModel
                {
                    Id = dealerInfo.Id,
                    CustomerName = dealerInfo.CustomerName,
                    CustomerNo = dealerInfo.CustomerNo,
                    Territory = dealerInfo.Territory,
                    Address = dealerInfo.Address,
                    ContactNo = dealerInfo.ContactNo
                });
            }
            result.Id = plan.Id;
            result.PlanDate = plan.PlanDate;
            return result;

        }
        public async Task<PortalPlanDetailModel> PortalUpdateJourneyPlan(PortalCreateJouneryModel model)
        {
            var result = new PortalPlanDetailModel();
            var findPlan = await _journeyPlanMasterSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.JourneyPlanDetail);

            await _journeyPlanDetailSvc.DeleteAsync(f => f.PlanId == findPlan.Id);
            findPlan.PlanStatus = PlanStatus.Edited;
            findPlan.PlanDate = model.VisitDate;
            await _journeyPlanMasterSvc.UpdateAsync(findPlan);
            foreach (var id in model.Dealers)
            {
                await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = findPlan.Id });
                var dealerInfo = await _dealerInforSvc.FindAsync(f => f.Id == id);
                result.DealerInfo.Add(new DealerInfoModel
                {
                    Id = dealerInfo.Id,
                    CustomerName = dealerInfo.CustomerName,
                    CustomerNo = dealerInfo.CustomerNo,
                    Territory = dealerInfo.Territory
                });
            }
            result.Id = findPlan.Id;
            result.PlanDate = findPlan.PlanDate;

            return result;

        }


        public async Task<PortalCreateJouneryModel> PortalGetJourneyPlanById(int Id)
        {
            PortalCreateJouneryModel result = new PortalCreateJouneryModel();
            var find = await _journeyPlanMasterSvc.FindIncludeAsync(f => f.Id == Id && f.EmployeeId == AppIdentity.AppUser.EmployeeId);
            result.Id = find.Id;
            result.Dealers = _journeyPlanDetailSvc.FindAll(s => s.PlanId == find.Id).Select(s => s.DealerId).ToArray();
            result.VisitDate = find.PlanDate;
            return result;
        }

        public async Task<bool> CheckAlreadyTodayPlan(DateTime planDate)
        {
            return await _journeyPlanMasterSvc.AnyAsync(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId && f.PlanDate.Date == Convert.ToDateTime(planDate).Date);
        }
        public async Task<bool> AppCheckAlreadyTodayPlan(string employeeId, DateTime visitDate)
        {
            return await _journeyPlanMasterSvc.IsExistAsync(f => f.PlanDate.Date == visitDate.Date && f.EmployeeId == employeeId);
           
        }


        public async Task<IEnumerable<AppJourneyPlanDetailModel>> AppGetJourneyPlanDetailList(string employeeId)
        {
           
            var planList = await _journeyPlanMasterSvc.FindAllAsync(f => f.EmployeeId == employeeId);
         

            var result = planList.Select(s => new AppJourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                PlanDate = s.PlanDate,
                PlanStatus=s.PlanStatus,
                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id)
                                    on dealer.Id equals planDetail.DealerId
                                    
                                    select new AppDealerInfoModel
                                    {
                                        Id = dealer.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        Address=dealer.Address,
                                        ContactNo=dealer.ContactNo,
                                        PlandId=s.Id
                                    }).ToList()
            }).ToList();

            return result;
        }

        public async Task<List<AppCreateJourneyModel>> AppCreateJourneyPlan(List<AppCreateJourneyModel> model)
        {
            var result = new List<AppCreateJourneyModel>();

            foreach (var jPlan in model)
            {

                var plan = await _journeyPlanMasterSvc.CreateAsync(new JourneyPlanMaster { EmployeeId = AppIdentity.AppUser.EmployeeId, PlanDate = DateTime.Now, Status = Status.Pending });
                if (plan == null) return null;
                foreach (var id in jPlan.Dealers)
                {

                    var create = await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = plan.Id, VisitDate = jPlan.VisitDate });
                    if (create == null) jPlan.Status = "failed";

                }
                jPlan.Id = plan.Id;
                jPlan.VisitDate = jPlan.VisitDate;
                jPlan.Status = "success";
                result.Add(jPlan);
            }
            return result;
        }

        public async Task<List<AppCreateJourneyModel>> AppUpdateJourneyPlan(List<AppCreateJourneyModel> model)
        {
            var result = new List<AppCreateJourneyModel>();
            foreach (var jPlan in model)
            {
                var findPlan = await _journeyPlanMasterSvc.FindIncludeAsync(f => f.Id == jPlan.Id, f => f.JourneyPlanDetail);
                if (findPlan == null) return null;
                await _journeyPlanDetailSvc.DeleteAsync(f => f.PlanId == findPlan.Id);

                foreach (var id in jPlan.Dealers)
                {
                    var update = await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = findPlan.Id, VisitDate = jPlan.VisitDate });
                    if (update == null) jPlan.Status = "failed";
                }

                jPlan.VisitDate = jPlan.VisitDate;
                jPlan.Status = "success";
                result.Add(jPlan);
            }
            return result;
        }

        public async Task<IEnumerable<JourneyPlanDetailModel>> PortalGetJourneyPlanDetailPage(int index, int pageSize)
        {
            var _index = index == 0 ? index + 1 : index;
            var planList = await _journeyPlanMasterSvc.FindAllPagedAsync(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId, _index, pageSize);

            var result = planList.Select(s => new JourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                PlanDate = s.PlanDate,
                Status = s.Status,
                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id).DefaultIfEmpty()
                                    on dealer.Id equals planDetail.DealerId
                                    select new DealerInfoModel
                                    {
                                        Id = planDetail.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        VisitDate = planDetail.VisitDate
                                    }).ToList()
            }).ToList();

            return result;

        }

        public async Task<IPagedList<JourneyPlanDetailModel>> PortalGetJourneyPlanDeailPage(int index, int pageSize, string planDate)
        {
            var plans = await _journeyPlanMasterSvc.GetAllIncludeAsync(
                select => select,
                p => p.EmployeeId == AppIdentity.AppUser.EmployeeId,
                o => o.OrderByDescending(f => f.PlanDate.Date),
                null,
                true
                );
            var result = plans.ToPagedList<JourneyPlanMaster>(index, pageSize);

            if (!string.IsNullOrEmpty(planDate))
                result = plans.Where(f => f.PlanDate.Date == Convert.ToDateTime(planDate).Date).ToPagedList<JourneyPlanMaster>();

            var final = new PagedList<JourneyPlanDetailModel>(
               result,
               result.Select(s => new JourneyPlanDetailModel
               {
                   Id = s.Id,
                   EmployeeId = s.EmployeeId,
                   Comment = s.Comment,
                   PlanDate = s.PlanDate,
                   Status = s.Status,
                   PlanStatus = s.PlanStatus,
                   DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                       join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id)
                                       on dealer.Id equals planDetail.DealerId
                                       join f in _focusDealerSvc.GetAll()
                                       on planDetail.DealerId equals f.Code into focus
                                       from fd in focus.DefaultIfEmpty()
                                       select new DealerInfoModel
                                       {
                                           Id = dealer.Id,
                                           CustomerName = dealer.CustomerName,
                                           CustomerNo = dealer.CustomerNo,
                                           Territory = dealer.Territory,
                                           ContactNo = dealer.ContactNo,
                                           Address = dealer.Address,
                                           IsFocused = fd.Code > 0 ? true : false,
                                           VisitDate = planDetail.VisitDate
                                       }).ToList().Select(s => s).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList(),

                   Employee = _userInfoSvc.Where(f => f.EmployeeId == s.EmployeeId)
                            .Select(s => new EmployeeModel
                            {
                                FirstName = $"{s.FirstName} {s.LastName}",
                                Department = s.DepartMent,
                                Designation = s.Designation,
                                PhoneNumber = s.PhoneNumber

                            }).FirstOrDefault()



               }));

            return final;

        }
    }
}
