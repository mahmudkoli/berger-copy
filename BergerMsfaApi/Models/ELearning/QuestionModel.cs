using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.ELearning
{
    public class QuestionModel : IMapFrom<Question>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public EnumQuestionType Type { get; set; }
        public int ELearningDocumentId { get; set; }
        public ELearningDocumentModel ELearningDocument { get; set; }
        public IList<QuestionOptionModel> QuestionOptions { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Question, QuestionModel>();
            profile.CreateMap<QuestionModel, Question>();
        }
    }

    public class SaveQuestionModel : IMapFrom<Question>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public EnumQuestionType Type { get; set; }
        public int ELearningDocumentId { get; set; }
        public IList<QuestionOptionModel> QuestionOptions { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Question, SaveQuestionModel>();
            profile.CreateMap<SaveQuestionModel, Question>();
        }
    }
}
