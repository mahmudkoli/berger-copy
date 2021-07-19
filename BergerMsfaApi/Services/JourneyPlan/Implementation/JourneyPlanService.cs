using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Master;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly IRepository<CustomerGroup> _customerGroupSvc;


        public JourneyPlanService(
            IRepository<JourneyPlanDetail> journeyPlanDetailSvc,
            IRepository<JourneyPlanMaster> journeyPlanMasterSvc,
            IRepository<DealerInfo> dealerInforSvc,
            IRepository<UserInfo> userInfoSvc,
            IRepository<FocusDealer> focusDealerSvc,
            IRepository<CustomerGroup> customerGroupSvc
            )
        {
            _journeyPlanDetailSvc = journeyPlanDetailSvc;
            _journeyPlanMasterSvc = journeyPlanMasterSvc;
            _dealerInforSvc = dealerInforSvc;
            _userInfoSvc = userInfoSvc;
            _focusDealerSvc = focusDealerSvc;
            _customerGroupSvc = customerGroupSvc;
        }



        #region Portal
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
                else if (PlanStatus.Rejected == (PlanStatus)model.Status)
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

        public async Task<bool> AppChangePlanStatus(JourneyPlanStatusChangeModel model)
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
                else if (PlanStatus.Rejected == (PlanStatus)model.Status)
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
                PlanDate = s.PlanDate.ToShortDateString(),
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
        public async Task<IPagedList<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManager(int index, int pageSize, string search)
        {

            var employeeIds = _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId);

            var plans = (from plan in _journeyPlanMasterSvc.GetAll()
                         join emp in employeeIds on plan.EmployeeId equals emp.EmployeeId
                         orderby plan.PlanDate.Date descending
                         select new { plan, emp });


            var final = plans.Select(s => new JourneyPlanDetailModel
            {
                Id = s.plan.Id,
                EmployeeId = s.emp.EmployeeId,
                Comment = s.plan.Comment,
                PlanDate = s.plan.PlanDate.ToString("yyyy-MM-dd"),
                Status = s.plan.Status,
                PlanStatus = s.plan.PlanStatus,
                PlanStatusInText = s.plan.PlanStatus.ToString(),
                EmployeeName = $"{s.emp.FullName}"
            }).ToList();

            if (!string.IsNullOrEmpty(search))
                final = final.Search(search);

            var result = await final.ToPagedListAsync(index, pageSize);
            return result;
        }

        public async Task<IList<JourneyPlanDetailModel>> GetAppJourneyPlanListForLineManager()
        {
            var currentDate = DateTime.Now;
            var employeeIds = _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId);

            var plans = (from plan in _journeyPlanMasterSvc.GetAll()
                         join emp in employeeIds on plan.EmployeeId equals emp.EmployeeId
                         where plan.PlanDate.Date >= currentDate.Date
                         orderby plan.PlanDate.Date descending
                         select new { plan, emp });


            var final = plans.Select(s => new JourneyPlanDetailModel
            {
                Id = s.plan.Id,
                EmployeeId = s.emp.EmployeeId,
                Comment = s.plan.Comment??string.Empty,
                PlanDate = s.plan.PlanDate.ToString("yyyy-MM-dd"),
                Status = s.plan.Status,
                PlanStatus = s.plan.PlanStatus,
                PlanStatusInText = s.plan.PlanStatus.ToString(),
                EmployeeName = $"{s.emp.FullName}"
            }).ToList();

            return final;
        }


        public async Task<List<JourneyPlanDetailModel>> GetJourneyPlanDetailForLineManagerForNotification()
        {

            var employeeIds = _userInfoSvc.FindAll(f => f.ManagerId == AppIdentity.AppUser.EmployeeId);

            var plans = (from plan in _journeyPlanMasterSvc.GetAll().Where(p=>p.PlanStatus==PlanStatus.Pending || p.PlanStatus==PlanStatus.Edited).ToList()
                         join emp in employeeIds on plan.EmployeeId equals emp.EmployeeId
                         orderby plan.PlanDate.Date descending
                         select new { plan, emp });


            var final = plans.Select(s => new JourneyPlanDetailModel
            {
                Id = s.plan.Id,
                EmployeeId = s.emp.EmployeeId,
                Comment = s.plan.Comment,
                PlanDate = s.plan.PlanDate.ToString("yyyy-MM-dd"),
                Status = s.plan.Status,
                PlanStatus = s.plan.PlanStatus,
                PlanStatusInText = s.plan.PlanStatus.ToString(),
                EmployeeName = $"{s.emp.FullName}"
            }).ToList();

            return final;
        }

        public async Task<JourneyPlanDetailModel> GetJourneyPlanDetailById(int PlanId)
        {

            var plan = await _journeyPlanMasterSvc.FindAsync(f => f.Id == PlanId);

            var result = new JourneyPlanDetailModel
            {

                Id = plan.Id,
                EmployeeId = plan.EmployeeId,
                PlanDate = plan.PlanDate.ToShortDateString(),
                Status = plan.Status,
                PlanStatus = plan.PlanStatus,
                Comment = plan.Comment,
                EditCount = plan.EditCount,

                Employee = _userInfoSvc.Where(f => f.EmployeeId == plan.EmployeeId)
                             .Select(s => new EmployeeModel
                             {
                                 FirstName = $"{s.FullName}",
                                 Department = s.Department,
                                 Designation = s.Designation,
                                 PhoneNumber = s.PhoneNumber

                             }).FirstOrDefault(),


                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == plan.Id)
                                    on dealer.Id equals planDetail.DealerId
                                    join f in _focusDealerSvc.GetAll() on new { a = planDetail.DealerId, b = AppIdentity.AppUser.EmployeeId } equals new { a = f.DealerId, b = f.EmployeeId } into leftJoin
                                    //join f in _focusDealerSvc.GetAll() on planDetail.DealerId equals f.Code into leftJoin
                                    from fd in leftJoin.DefaultIfEmpty()
                                    select new DealerInfoModel
                                    {
                                        Id = dealer.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        Zone = dealer.CustZone,
                                        ContactNo = dealer.ContactNo,
                                        Address = dealer.Address,
                                        IsFocused = (fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date) ? true : false,
                                        VisitDate = planDetail.VisitDate
                                    }).ToList().Select(s => s).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList(),

            };

            return result;



        }

        public async Task<JourneyPlanDetailModel> GetAppJourneyPlanDetailById(int PlanId)
        {
            var tempEmployee = (EmployeeModel)null;
            var plan = await _journeyPlanMasterSvc.FindAsync(f => f.Id == PlanId);

            var result = new JourneyPlanDetailModel
            {

                Id = plan.Id,
                EmployeeId = plan.EmployeeId,
                PlanDate = plan.PlanDate.ToString("yyyy-MM-dd"),
                Status = plan.Status,
                PlanStatus = plan.PlanStatus,
                Comment = plan.Comment ?? string.Empty,
                EditCount = plan.EditCount,

                Employee = tempEmployee = _userInfoSvc.Where(f => f.EmployeeId == plan.EmployeeId)
                             .Select(s => new EmployeeModel
                             {
                                 FirstName = $"{s.FullName}",
                                 Department = s.Department,
                                 Designation = s.Designation,
                                 PhoneNumber = s.PhoneNumber

                             }).FirstOrDefault(),

                EmployeeName = tempEmployee.FirstName ?? string.Empty,
                PlanStatusInText = plan.PlanStatus.ToString(),

                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == plan.Id)
                                    on dealer.Id equals planDetail.DealerId
                                    join f in _focusDealerSvc.GetAll() on new { a = planDetail.DealerId, b = AppIdentity.AppUser.EmployeeId } equals new { a = f.DealerId, b = f.EmployeeId } into leftJoin
                                    //join f in _focusDealerSvc.GetAll() on planDetail.DealerId equals f.Code into leftJoin
                                    from fd in leftJoin.DefaultIfEmpty()
                                    select new DealerInfoModel
                                    {
                                        Id = dealer.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        Zone = dealer.CustZone,
                                        ContactNo = dealer.ContactNo,
                                        Address = dealer.Address,
                                        IsFocused = (fd.DealerId > 0 && fd.ValidTo.Date >= DateTime.Now.Date) ? true : false,
                                        VisitDate = planDetail.VisitDate,
                                        AccountGroup = dealer.AccountGroup,
                                        PlanDate = plan.PlanDate.ToString("yyyy-MM-dd")
                                    }).ToList().Select(s => s).GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList(),

            };

            return result;
        }

        public async Task<PortalPlanDetailModel> PortalCreateJourneyPlan(PortalCreateJouneryModel model)
        {
            var result = new PortalPlanDetailModel();
            var plan = await _journeyPlanMasterSvc.CreateAsync(new JourneyPlanMaster
            {
                EmployeeId = AppIdentity.AppUser.EmployeeId,
                PlanDate = model.VisitDate,
                Status = Status.Active,
                PlanStatus = PlanStatus.Pending,
                Comment = model.Comment
            });
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
            if (findPlan.EditCount >= 2)
            {
                throw new Exception("You cant not edit jouerny plan. Maximum edit level already done");
            }
            await _journeyPlanDetailSvc.DeleteAsync(f => f.PlanId == findPlan.Id);
            findPlan.PlanStatus = PlanStatus.Edited;
            findPlan.PlanDate = findPlan.PlanDate;
            findPlan.EditCount = findPlan.EditCount + 1;
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
        public async Task<PortalCreateJouneryModel> PortalGetJourneyPlanById(string date)
        {

            PortalCreateJouneryModel result = new PortalCreateJouneryModel();
            var find = await _journeyPlanMasterSvc.FindIncludeAsync(f => f.PlanDate == DateTime.Parse(date).Date && f.EmployeeId == AppIdentity.AppUser.EmployeeId);
            if (find == null) return null;
            result.Id = find.Id;
            result.Dealers = _journeyPlanDetailSvc.FindAll(s => s.PlanId == find.Id).Select(s => s.DealerId).ToArray();
            result.VisitDate = find.PlanDate;
            return result;
        }
        public async Task<bool> CheckAlreadyTodayPlan(DateTime planDate)
        {
            return await _journeyPlanMasterSvc.AnyAsync(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId && f.PlanDate.Date == Convert.ToDateTime(planDate).Date);
        }
        public async Task<IEnumerable<JourneyPlanDetailModel>> PortalGetJourneyPlanDetailPage(int index, int pageSize)
        {
            var _index = index == 0 ? index + 1 : index;
            var planList = await _journeyPlanMasterSvc.FindAllPagedAsync(f => f.EmployeeId == AppIdentity.AppUser.EmployeeId, _index, pageSize);

            var result = planList.Select(s => new JourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                PlanDate = s.PlanDate.ToShortDateString(),
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
        public async Task<IPagedList<JourneyPlanDetailModel>> PortalGetJourneyPlanDeailPage(int index, int pageSize, string search)
        {
            var plans = await _journeyPlanMasterSvc.GetAllIncludeAsync(
                select => select,
                p => p.EmployeeId == AppIdentity.AppUser.EmployeeId,
                o => o.OrderByDescending(f => f.PlanDate.Date),
                null,
                true
                );

            var result = plans.Select(s => new JourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                Comment = s.Comment,
                PlanDate = s.PlanDate.ToString("yyyy-MM-dd"),
                Status = s.Status,
                PlanStatus = s.PlanStatus,
                EditCount = s.EditCount,
                PlanStatusInText = s.PlanStatus.ToString(),
                EmployeeName = _userInfoSvc.Where(f => f.EmployeeId == s.EmployeeId).Select(s => $"{s.FullName}").FirstOrDefault()

            }).ToList();

            if (!string.IsNullOrEmpty(search)) result = result.Search(search);

            return result.ToPagedList(index, pageSize);

        }


        #endregion




        #region App

        public async Task<IEnumerable<DealerInfoModel>> AppGetJourneyPlanDealerList(string employeeId)
        {
            var planList = await _journeyPlanMasterSvc.FindAllAsync(f => f.EmployeeId == employeeId && f.PlanDate.Date >= DateTime.Now.Date && f.PlanStatus == PlanStatus.Approved);
            var d = _customerGroupSvc.FindAll(f => f.Description.StartsWith("Subdealer"));
            var result = (from plan in planList
                          join planDetail in _journeyPlanDetailSvc.GetAll() on plan.Id equals planDetail.PlanId
                          join dealer in _dealerInforSvc.GetAll()
                          on planDetail.DealerId equals dealer.Id

                          join f in _focusDealerSvc.GetAll() on planDetail.DealerId equals f.DealerId into leftJoin
                          from fd in leftJoin.DefaultIfEmpty()

                          join cg in _customerGroupSvc.FindAll(f => f.Description.StartsWith("Subdealer"))
                          on dealer.AccountGroup equals cg.CustomerAccountGroup into custGroups
                          from custGroup in custGroups.DefaultIfEmpty()

                          select new DealerInfoModel
                          {
                              Id = dealer.Id,
                              CustomerName = dealer.CustomerName,
                              CustomerNo = dealer.CustomerNo,
                              Territory = dealer.Territory,
                              ContactNo = dealer.ContactNo,
                              Address = dealer.Address,
                              IsFocused = fd != null ? ((fd.DealerId > 0) && fd.ValidTo.Date >= DateTime.Now.Date ? true : false) : false,
                              PlanDate = plan.PlanDate.Date.ToString("yyyy-MM-dd"),
                              IsSubdealer = custGroup?.Description != null ? true : false,



                          })
                          .GroupBy(n => new { n.Id }).Select(g => g.FirstOrDefault()).ToList();



            return result.OrderBy(o => o.CustomerName).ToList();


        }
        public async Task<IEnumerable<AppJourneyPlanDetailModel>> AppGetJourneyPlanList(string employeeId)
        {

            var planList = await _journeyPlanMasterSvc.FindAllAsync(f => f.EmployeeId == employeeId && f.PlanDate.Date >= DateTime.Now.Date && f.PlanStatus == PlanStatus.Approved);


            var result = planList.Select(s => new AppJourneyPlanDetailModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                PlanDate = s.PlanDate.Date.ToString("yyyy-MM-dd"),
                PlanStatus = s.PlanStatus,
                EditCount=s.EditCount,
                DealerInfoModels = (from dealer in _dealerInforSvc.GetAll()
                                    join planDetail in _journeyPlanDetailSvc.FindAll(f => f.PlanId == s.Id)
                                    on dealer.Id equals planDetail.DealerId

                                    join f in _focusDealerSvc.GetAll().Where(f => f.ValidTo.Date >= DateTime.Now.Date) on new { a = planDetail.DealerId, b = employeeId } equals new { a = f.DealerId, b = f.EmployeeId } into leftJoin

                                    from fd in leftJoin.DefaultIfEmpty()

                                    join cg in _customerGroupSvc.GetAll().Where(f => f.Description.StartsWith("Subdealer"))
                                    on dealer.AccountGroup equals cg.CustomerAccountGroup into custGroups
                                    from custGroup in custGroups.DefaultIfEmpty()


                                    select new AppDealerInfoModel
                                    {
                                        Id = dealer.Id,
                                        CustomerName = dealer.CustomerName,
                                        CustomerNo = dealer.CustomerNo,
                                        Territory = dealer.Territory,
                                        ContactNo = dealer.ContactNo,
                                        Address = dealer.Address,
                                        IsFocused = fd != null ? ((fd.DealerId > 0) && fd.ValidTo.Date >= DateTime.Now.Date ? true : false) : false,
                                        PlanDate = s.PlanDate.Date.ToString("yyyy-MM-dd"),
                                        IsSubdealer = custGroup.Description != null ? true : false,
                                        PlanId = planDetail.PlanId
                                    }).ToList()
            }).ToList();

            return result;
        }
        public async Task<List<AppCreateJourneyModel>> AppCreateJourneyPlan(string employeeId, List<AppCreateJourneyModel> model)
        {
            var result = new List<AppCreateJourneyModel>();

            foreach (var jPlan in model)
            {
                var plan = await _journeyPlanMasterSvc.CreateAsync(new JourneyPlanMaster { EmployeeId = employeeId, PlanDate = jPlan.VisitDate, Status = Status.Active, PlanStatus = PlanStatus.Pending });
                if (plan == null) return null;
                foreach (var id in jPlan.Dealers)
                    await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = plan.Id });

                jPlan.Id = plan.Id;
                jPlan.VisitDate = jPlan.VisitDate;
                jPlan.Status = PlanStatus.Pending;
                result.Add(jPlan);
            }
            return result;
        }
        public async Task<List<AppCreateJourneyModel>> AppUpdateJourneyPlan(string employeeId, List<AppCreateJourneyModel> model)
        {
            var result = new List<AppCreateJourneyModel>();
            foreach (var jPlan in model)
            {

                var findPlan = await _journeyPlanMasterSvc.FindIncludeAsync(f => f.Id == jPlan.Id && f.EmployeeId == employeeId);
                if (findPlan == null) return null;
                if (findPlan.EditCount < 2)
                {
                    findPlan.PlanDate = jPlan.VisitDate;
                    findPlan.EditCount = findPlan.EditCount + 1;
                    await _journeyPlanMasterSvc.UpdateAsync(findPlan);
                    await _journeyPlanDetailSvc.DeleteAsync(f => f.PlanId == findPlan.Id);

                    foreach (var id in jPlan.Dealers)
                        await _journeyPlanDetailSvc.CreateAsync(new JourneyPlanDetail { DealerId = id, PlanId = findPlan.Id });
                    jPlan.VisitDate = jPlan.VisitDate;
                    jPlan.Status = PlanStatus.Edited;
                    result.Add(jPlan);

                }

            }
            return result;
        }
        public async Task<bool> AppCheckAlreadyTodayPlan(string employeeId, DateTime visitDate)
        {
            return await _journeyPlanMasterSvc.IsExistAsync(f => f.PlanDate.Date == visitDate.Date && f.EmployeeId == employeeId);

        }

        #endregion




    }
}
