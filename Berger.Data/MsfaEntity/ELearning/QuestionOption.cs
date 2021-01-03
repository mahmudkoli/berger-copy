using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class QuestionOption : AuditableEntity<int>
    {
        public string Title { get; set; }
        public int Sequence { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
