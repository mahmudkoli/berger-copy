using System;
using System.Collections.Generic;

namespace BergerMsfaApi.Core
{
    public class ApiResponse<T> where T : class
    {
        /*
    status:err
	statusCode:
	data:[]
	mgs:"Bad return"
	errors:[]
         
         
         */

        public ApiResponse()
        {
            StatusCode = 200;
            Status = string.Empty;
            Msg = string.Empty;
            Errors = new List<ValidationError>();
            //Data = GetObject();
        }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Msg { get; set; }
        public List<ValidationError> Errors { get; set; }

        public void CreateInstance()
        {
            Data = Activator.CreateInstance<T>();
        }

    }

    public class ApiResponse
    {
        public ApiResponse()
        {
            StatusCode = 200;
            Status = string.Empty;
            Msg = string.Empty;
            Errors = new List<ValidationError>();
            Data = new object();
        }
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Msg { get; set; }
        public List<ValidationError> Errors { get; set; }

    }

}
