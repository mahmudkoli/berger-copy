﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Users
{
    public class AuthenticateUserModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        //public IList<KeyValuePairModel> Plants { get; set; }
        public IList<KeyValuePairModel> DealerOpeningsHierarchyList { get; set; }
        public IList<KeyValuePairModel> PainterRegistrationsHierarchyList { get; set; }
        public IList<KeyValuePairModel> LeadGenerationsHierarchyList { get; set; }
        public IList<KeyValuePairAreaModel> Plants { get; set; }
        public IList<KeyValuePairAreaModel> SalesOffices { get; set; }
        public IList<KeyValuePairAreaModel> Areas { get; set; }
        public IList<KeyValuePairAreaModel> Territories { get; set; }
        public IList<KeyValuePairAreaModel> Zones { get; set; }
        public List<string> PlantIds { get; set; }
        public string PlantId { get; set; }
        public List<string> SalesOfficeIds { get; set; }
        public string SalesOfficeId { get; set; }
        public List<string> AreaIds { get; set; }
        public string AreaId { get; set; }
        public List<string> TerritoryIds { get; set; }
        public string TerritoryId { get; set; }
        public List<string> ZoneIds { get; set; }
        public string ZoneId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string EmployeeId { get; set; }
        public int EmployeeRole { get; set; }
        public string UserCategory { get; set; }
        public List<string> UserCategoryIds { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public AuthenticateUserModel()
        {
            //this.Plants = new List<KeyValuePairModel>();
            this.DealerOpeningsHierarchyList = new List<KeyValuePairModel>();
            this.PainterRegistrationsHierarchyList = new List<KeyValuePairModel>();
            this.LeadGenerationsHierarchyList = new List<KeyValuePairModel>();
            this.Plants = new List<KeyValuePairAreaModel>();
            this.SalesOffices = new List<KeyValuePairAreaModel>();
            this.Areas = new List<KeyValuePairAreaModel>();
            this.Territories = new List<KeyValuePairAreaModel>();
            this.Zones = new List<KeyValuePairAreaModel>();
        }
    }

    public class KeyValuePairModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public IList<KeyValuePairModel> Children { get; set; }

        public KeyValuePairModel()
        {
            this.Children = new List<KeyValuePairModel>();
        }
    }

    public class KeyValuePairAreaModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
