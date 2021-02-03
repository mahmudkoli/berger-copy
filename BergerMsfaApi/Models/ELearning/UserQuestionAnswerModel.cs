using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.ELearning
{
    public class UserQuestionAnswerModel : IMapFrom<UserQuestionAnswer>
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        //public UserInfoModel UserInfo { get; set; }
        public string UserFullName { get; set; }
        public QuestionSetModel QuestionSet { get; set; }
        public int TotalMark { get; set; }
        public int TotalCorrectAnswer { get; set; }
        public bool Passed { get; set; }
        public IList<UserQuestionAnswerCollectionModel> QuestionAnswerCollections { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserQuestionAnswer, UserQuestionAnswerModel>()
                .ForMember(dest => dest.UserFullName, 
                    opt => opt.MapFrom(src => src.UserInfo != null ? $"{src.UserInfo.FullName}" : string.Empty));
        }
    }
}
