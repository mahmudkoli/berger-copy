using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BergerMsfaApi.Core
{
    public class ApplicationError
    {
        public ApplicationError()
        {
            Id = Guid.NewGuid().ToString().ToLower();
            ErrorTime = DateTime.Now;
        }
        [Key]
        [StringLength(128)]
        public string Id { get; set; }
        [StringLength(128)]
        public string FileName { get; set; }
        [StringLength(128)]
        public string MethodName { get; set; }//Or Action
        [StringLength(128)]
        public string EntityName { get; set; }//Or Controller
        [StringLength(256)]
        public string EntityFullName { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }//or Request Data
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
        public DateTime ErrorTime { get; set; }

        public static string GetAllMessage(IEnumerable<ApplicationError> exErrors)
        {
            return exErrors.Aggregate(string.Empty,
                (current, ex) => current
                                 + (DateTime.Now.ToString("MMM dd, yyyy h:mm tt")
                                  + ":: " + ex.FileName
                                  + ":: " + ex.EntityFullName
                                  + ":: " + ex.MethodName
                                  + ":: " + ex.LineNumber
                                  + ":: " + ex.Message
                                  + Environment.NewLine));
        }
        public string Get()
        {
            return (DateTime.Now.ToString("MMM dd, yyyy h:mm tt")
                    + ":: " + FileName
                    + ":: " + EntityFullName
                    + ":: " + MethodName
                    + ":: " + LineNumber
                    + ":: " + Message
                    + Environment.NewLine);
        }

    }
}