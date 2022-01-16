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
        //public int UserInfoId { get; set; }
        //public UserInfoModel UserInfo { get; set; }
        public string UserFullName { get; set; }
        public string EmployeeId { get; set; }
        //public QuestionSetModel QuestionSet { get; set; }
        public string QuestionSetTitle { get; set; }
        public int QuestionSetLevel { get; set; }
        public string ExamDate { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        public int UserMark { get; set; }
        //public int TotalCorrectAnswer { get; set; }
        //public bool Passed { get; set; }
        public string PassStatus { get; set; }
        //public IList<UserQuestionAnswerCollectionModel> QuestionAnswerCollections { get; set; }
        //public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserQuestionAnswer, UserQuestionAnswerModel>()
                .ForMember(dest => dest.UserFullName, 
                    opt => opt.MapFrom(src => src.UserInfo != null ? $"{src.UserInfo.FullName}" : string.Empty))
                .ForMember(dest => dest.EmployeeId, 
                    opt => opt.MapFrom(src => src.UserInfo != null ? $"{src.UserInfo.EmployeeId}" : string.Empty))
                .ForMember(dest => dest.ExamDate, 
                    opt => opt.MapFrom(src => src.CreatedTime.ToString("dd-MM-yyyy")))
                .ForMember(dest => dest.QuestionSetTitle,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.Title}" : string.Empty))
                .ForMember(dest => dest.QuestionSetLevel,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.Level}" : string.Empty))
                .ForMember(dest => dest.TotalMark,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.TotalMark}" : string.Empty))
                .ForMember(dest => dest.PassMark,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.PassMark}" : string.Empty))
                .ForMember(dest => dest.UserMark,
                    opt => opt.MapFrom(src => src.TotalMark))
                .ForMember(dest => dest.PassStatus,
                    opt => opt.MapFrom(src => src.Passed ? "Pass" : "Fail"));
        }
    }

    public class AppUserQuestionAnswerModel : IMapFrom<UserQuestionAnswer>
    {
        public int Id { get; set; }
        public string QuestionSetTitle { get; set; }
        public int QuestionSetLevel { get; set; }
        public string ExamDate { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        public int UserMark { get; set; }
        //public bool Passed { get; set; }
        public string PassStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserQuestionAnswer, AppUserQuestionAnswerModel>()
                .ForMember(dest => dest.QuestionSetTitle,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.Title}" : string.Empty))
                .ForMember(dest => dest.QuestionSetLevel,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.Level}" : string.Empty))
                .ForMember(dest => dest.TotalMark,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.TotalMark}" : string.Empty))
                .ForMember(dest => dest.PassMark,
                    opt => opt.MapFrom(src => src.QuestionSet != null ? $"{src.QuestionSet.PassMark}" : string.Empty))
                .ForMember(dest => dest.ExamDate,
                    opt => opt.MapFrom(src => src.CreatedTime.ToString("dd-MM-yyyyy")))
                .ForMember(dest => dest.UserMark,
                    opt => opt.MapFrom(src => src.TotalMark))
                .ForMember(dest => dest.PassStatus,
                    opt => opt.MapFrom(src => src.Passed ? "Pass" : "Fail"));
        }
    }
}
