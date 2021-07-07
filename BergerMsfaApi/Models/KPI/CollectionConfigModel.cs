using AutoMapper;
using Berger.Data.MsfaEntity.KPI;
using Berger.Data.MsfaEntity.Scheme;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.KPI
{
    public class CollectionConfigModel : IMapFrom<CollectionConfig>
    {
        public int Id { get; set; }
        public int ChangeableMaxDateDay { get; set; }
        public DateTime ChangeableMaxDate { get; set; }
        public string ChangeableMaxDateText { get; set; }

        public void Mapping(Profile profile)
        {
            var dateNow = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);

            profile.CreateMap<CollectionConfig, CollectionConfigModel>()
                .ForMember(x => x.ChangeableMaxDate, opt => opt.MapFrom(src => 
                    src.ChangeableMaxDateDay <= daysInMonth ?
                    new DateTime(dateNow.Year, dateNow.Month, src.ChangeableMaxDateDay) : 
                    new DateTime(dateNow.Year, dateNow.Month, daysInMonth)
                ))
                .ForMember(x => x.ChangeableMaxDateText, opt => opt.MapFrom(src =>
                    (src.ChangeableMaxDateDay <= daysInMonth ?
                    new DateTime(dateNow.Year, dateNow.Month, src.ChangeableMaxDateDay) :
                    new DateTime(dateNow.Year, dateNow.Month, daysInMonth))
                    .ToString("dd-MM-yyyy")
                ));
            profile.CreateMap<CollectionConfigModel, CollectionConfig>();
        }
    }

    public class SaveCollectionConfigModel : IMapFrom<CollectionConfig>
    {
        public int Id { get; set; }
        public int ChangeableMaxDateDay { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CollectionConfig, SaveCollectionConfigModel>();
            profile.CreateMap<SaveCollectionConfigModel, CollectionConfig>();
        }
    }
}
