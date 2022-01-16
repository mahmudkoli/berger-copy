using AutoMapper;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DF = Berger.Data.MsfaEntity.DealerFocus;

namespace BergerMsfaApi.Models.FocusDealer
{
    public class FocusDealerModel : IMapFrom<DF.FocusDealer>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        [JsonIgnore]
        public string CustomerName { get; set; }
        [JsonIgnore]
        public string CustomerNo { get; set; }
        [JsonIgnore]
        public string FullName { get; set; }
        [JsonIgnore]
        public DateTime CreatedTime { get; set; }

        public string ValidFromText => ValidFrom.ToString("yyyy-MM-dd");
        public string ValidToText => ValidTo.ToString("yyyy-MM-dd");
        public string DealerName => $"{CustomerName} ({CustomerNo})";
        public string UserFullName => FullName;
        public string Depot { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DF.FocusDealer, FocusDealerModel>();
            profile.CreateMap<FocusDealerModel, DF.FocusDealer>();
        }
    }

    public class SaveFocusDealerModel : IMapFrom<DF.FocusDealer>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DF.FocusDealer, SaveFocusDealerModel>();
            profile.CreateMap<SaveFocusDealerModel, DF.FocusDealer>();
        }
    }

    public class FocusDealerQueryObjectModel : QueryObjectModel
    {
        public string Depot { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }

        public FocusDealerQueryObjectModel()
        {
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }

    public class DealerOpeningQueryObjectModel : QueryObjectModel
    {
        public string Depot { get; set; }
        public IList<string> Territories { get; set; }

        public DealerOpeningQueryObjectModel()
        {
            this.Territories = new List<string>();
        }
    }

    public class LeadQueryObjectModel : QueryObjectModel
    {
        public int UserId { get; set; }
        public string Depot { get; set; }
        public IList<string> Territories { get; set; }
        public IList<string> Zones { get; set; }

        public LeadQueryObjectModel()
        {
            this.Territories = new List<string>();
            this.Zones = new List<string>();
        }
    }
}
