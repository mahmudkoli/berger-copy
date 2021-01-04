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
    public class QuestionSetCollectionModel : IMapFrom<QuestionSetCollection>
    {
        public int Id { get; set; }
        public int QuestionSetId { get; set; }
        public QuestionSetModel QuestionSet { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public int Mark { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuestionSetCollection, QuestionSetCollectionModel>();
            profile.CreateMap<QuestionSetCollectionModel, QuestionSetCollection>();
        }
    }
}
