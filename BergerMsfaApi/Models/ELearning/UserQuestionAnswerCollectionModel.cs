using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Users;
using System.Collections.Generic;

namespace BergerMsfaApi.Models.ELearning
{
    public class UserQuestionAnswerCollectionModel : IMapFrom<UserQuestionAnswerCollection>
    {
        public int Id { get; set; }
        //public int QuestionSetId { get; set; }
        //public QuestionSetModel QuestionSet { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public int Mark { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string Answer { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserQuestionAnswerCollection, UserQuestionAnswerCollectionModel>();
            profile.CreateMap<UserQuestionAnswerCollectionModel, UserQuestionAnswerCollection>();
        }
    }
}
