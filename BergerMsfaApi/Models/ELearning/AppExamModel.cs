using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Mappings;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.ELearning
{
    public class AppQuestionSetModel : IMapFrom<QuestionSet>
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        public int TimeOutMinute { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IList<AppQuestionModel> Questions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuestionSet, AppQuestionSetModel>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("dd-MM-yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("dd-MM-yyyy")));
        }
    }

    public class AppQuestionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public EnumQuestionType Type { get; set; }
        public int Mark { get; set; }
        public IList<int> Answers { get; set; }
        public IList<AppQuestionOptionModel> QuestionOptions { get; set; }
    }

    public class AppQuestionOptionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public bool IsCorrectAnswer { get; set; }
    }

    public class AppQuestionAnswerResultModel
    {
        public int Id { get; set; }
        public string QuestionSetTitle { get; set; }
        public int TotalMark { get; set; }
        public int TotalCorrectAnswer { get; set; }
        public bool Passed { get; set; }
    }
}
