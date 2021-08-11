using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.KPI;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Users;
using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.KPI
{
    public class UniverseReachAnalysisModel : IMapFrom<UniverseReachAnalysis>
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public string FiscalYear { get; set; }
        public int OutletNumber { get; set; }
        public int DirectCovered { get; set; }
        public int IndirectCovered { get; set; }
        public int DirectTarget { get; set; }
        public int IndirectTarget { get; set; }
        public int IndirectManual { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UniverseReachAnalysis, UniverseReachAnalysisModel>();
            profile.CreateMap<UniverseReachAnalysisModel, UniverseReachAnalysis>();
        }
    }

    public class SaveUniverseReachAnalysisModel : IMapFrom<UniverseReachAnalysis>
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public int OutletNumber { get; set; }
        public int DirectCovered { get; set; }
        public int IndirectCovered { get; set; }
        public int DirectTarget { get; set; }
        public int IndirectTarget { get; set; }
        public int IndirectManual { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UniverseReachAnalysis, SaveUniverseReachAnalysisModel>();
            profile.CreateMap<SaveUniverseReachAnalysisModel, UniverseReachAnalysis>();
        }
    }

    public class SaveAppUniverseReachAnalysisModel : IMapFrom<UniverseReachAnalysis>
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public string FiscalYear { get; set; }
        public int DirectActual { get; set; }
        public int IndirectActual { get; set; }
        public int IndirectManual { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UniverseReachAnalysis, SaveAppUniverseReachAnalysisModel>();
            profile.CreateMap<SaveAppUniverseReachAnalysisModel, UniverseReachAnalysis>();
        }
    }

    public class UniverseReachAnalysisQueryObjectModel : QueryObjectModel
    {
        public string BusinessArea { get; set; } // Plant, Depot
        public IList<string> Territories { get; set; }

        public UniverseReachAnalysisQueryObjectModel()
        {
            this.Territories = new List<string>();
        }
    }

    public class AppUniverseReachAnalysisQueryObjectModel
    {
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
    }
}
