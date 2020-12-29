using Berger.Common.Enumerations;
using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class Question : AuditableEntity<int>
    {
        public string Title { get; set; }
        public EnumQuestionType Type { get; set; }
        public int ELearningDocumentId { get; set; }
        public ELearningDocument ELearningDocument { get; set; }
        public IList<QuestionOption> QuestionOptions { get; set; }
    }
}
