using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class QuestionSet : AuditableEntity<int>
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public int TotalMark { get; set; }
        public int PassMark { get; set; }
        public int TimeOutMinute { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ELearningDocumentId { get; set; }
        public ELearningDocument ELearningDocument { get; set; }
        //public int NeededCorrectAnswer { get; set; }
        public IList<QuestionSetCollection> QuestionSetCollections { get; set; }
        public IList<QuestionSetDepot> QuestionSetDepots { get; set; }
    }

    public class QuestionSetDepot : Entity<int>
    {
        public int QuestionSetId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public string Depot { get; set; }
    }
}
