using AutoMapper;
using Berger.Data.MsfaEntity.Logs;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Logs
{
    public class MobileAppLogModel : IMapFrom<MobileAppLog>
    {
        public string EmployeeID { get; set; }
        public IList<string> ErrorMsg { get; set; }
        public string BearerToken { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Url { get; set; }
        public string LastActivity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MobileAppLog, MobileAppLogModel>()
                .ForMember(dest => dest.ErrorMsg,
                    opt => opt.MapFrom(src => src.ErrorMessage.Split(new[] { ",,," }, StringSplitOptions.None).ToList()));
            profile.CreateMap<MobileAppLogModel, MobileAppLog>()
                .ForMember(dest => dest.ErrorMessage,
                    opt => opt.MapFrom(src => string.Join(",,,", src.ErrorMsg.ToList())));
        }
    }
}
