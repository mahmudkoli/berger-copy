﻿using AutoMapper;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Scheme
{
    public class SchemeMasterModel : IMapFrom<SchemeMaster>
    {
        public int Id { get; set; }
        public string SchemeName { get; set; }
        public string Condition { get; set; }

        //public IList<SchemeDetailModel> SchemeDetails { get; set; }

        //public SchemeMasterModel()
        //{
        //    SchemeDetails = new List<SchemeDetailModel>();
        //}

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeMaster, SchemeMasterModel>();
            profile.CreateMap<SchemeMasterModel, SchemeMaster>();
        }
    }

    public class SaveSchemeMasterModel : IMapFrom<SchemeMaster>
    {
        public int Id { get; set; }
        public string SchemeName { get; set; }
        public string Condition { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SchemeMaster, SaveSchemeMasterModel>();
            profile.CreateMap<SaveSchemeMasterModel, SchemeMaster>();
        }
    }
}
