using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.PainterRegistration
{
    public class PainterCallModel : IMapFrom<PainterCall>
    {
        public PainterCallModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PainterCall, PainterCallModel>();
        }
        
        public int Id { get; set; }

        public string PainterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int NoOfPainterAttached { get; set; }
        public bool IsAppInstalled { get; set; }
        public float Loyality { get; set; }
        public string AccDbblNumber { get; set; }
        public string AccDbblHolderName { get; set; }
        public string AccChangeReason { get; set; }
        public int PainterCatId { get; set; }

        public bool HasSchemeComnunaction { get; set; }
        public bool HasPremiumProtBriefing { get; set; }
        public bool HasNewProBriefing { get; set; }
        public bool HasUsageEftTools { get; set; }
        public bool HasAppUsage { get; set; }
        public decimal WorkInHandNumber { get; set; }
        public bool HasDbblIssue { get; set; }
        public string Comment { get; set; }
        public int PainterId { get; set; }
        public List<PainterCompanyMTDValueModel> PainterCompanyMTDValue { get; set; }
        public List<AttachedDealerPainterCallModel> AttachedDealers { get; set; }
    }
}
