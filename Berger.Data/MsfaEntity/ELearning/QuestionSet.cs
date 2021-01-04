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
        public int ELearningDocumentId { get; set; }
        public ELearningDocument ELearningDocument { get; set; }
        //public int NeededCorrectAnswer { get; set; }
        public IList<QuestionSetCollection> QuestionSetCollections { get; set; }
    }
}
