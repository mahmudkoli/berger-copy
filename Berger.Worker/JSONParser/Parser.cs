
using System.Collections.Generic;
using System.IO;
using Berger.Worker.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Berger.Worker.JSONParser
{
    public static  class Parser<TEntity> where TEntity : class
    {
        public class Data
        {
            public List<TEntity> results { get; set; }
        }
          
        public static Data ParseJson(string json)
        {


            try
            {
                Data dataObj = new Data();

                JObject data = JObject.Parse(json);
                if (data.HasValues)
                {
                    if (data.First != null)
                    {
                        if (data.First.First != null)
                        {
                            dataObj = JsonConvert.DeserializeObject<Data>(data.First.First.ToString());
                            
                        }
                    }
                }
                else
                {
                    return new Data();
                }

                return dataObj;

            }
            catch (System.Exception ex)
            {

                throw;
            }
            
        }

    }
    
}
