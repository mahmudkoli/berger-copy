using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System.Collections.Generic;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class UserQuestionAnswer : AuditableEntity<int>
    {
        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public int QuestionSetId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public int TotalMark { get; set; }
        public int TotalCorrectAnswer { get; set; }
        public bool Passed { get; set; }
        public IList<UserQuestionAnswerCollection> QuestionAnswerCollections { get; set; }
    }
}
