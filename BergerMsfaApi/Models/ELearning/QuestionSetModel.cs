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
    public class QuestionSetModel : IMapFrom<QuestionSet>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        //public int NeededCorrectAnswer { get; set; }
        public int ELearningDocumentId { get; set; }
        public ELearningDocumentModel ELearningDocument { get; set; }
        public IList<QuestionSetCollectionModel> QuestionSetCollections { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuestionSet, QuestionSetModel>();
            profile.CreateMap<QuestionSetModel, QuestionSet>();
        }
    }

    public class SaveQuestionSetModel : IMapFrom<QuestionSet>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        //public int NeededCorrectAnswer { get; set; }
        public int ELearningDocumentId { get; set; }
        //public ELearningDocument ELearningDocument { get; set; }
        public IList<QuestionSetCollectionModel> QuestionSetCollections { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<QuestionSet, SaveQuestionSetModel>();
            profile.CreateMap<SaveQuestionSetModel, QuestionSet>();
        }
    }
}
