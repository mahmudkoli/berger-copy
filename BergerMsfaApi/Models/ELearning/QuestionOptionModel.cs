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
    public class QuestionOptionModel : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuestionOption, QuestionOptionModel>();
            profile.CreateMap<QuestionOptionModel, QuestionOption>();
        }
    }
}
