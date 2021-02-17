using Berger.Odata.Common;
using Berger.Odata.Model;
using System;
using System.Collections.Generic;

namespace Berger.Odata.Extensions
{
    public class FilterQueryOptionBuilder
    {
        public string Filter { get { return this._filter; } }
        private string _filter;

        public FilterQueryOptionBuilder()
        {
            this._filter = $"$filter=";
        }

        public FilterQueryOptionBuilder StartGroup() 
        { 
            this._filter += $"("; 
            return this; 
        }

        public FilterQueryOptionBuilder EndGroup() 
        { 
            this._filter += $")"; 
            return this; 
        }

        public FilterQueryOptionBuilder And() 
        { 
            this._filter += $" and "; 
            return this; 
        }

        public FilterQueryOptionBuilder Or() 
        { 
            this._filter += $" or "; 
            return this; 
        }

        public FilterQueryOptionBuilder Equal(string property, string value) 
        { 
            this._filter += $"{property} eq '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder NotEqual(string property, string value) 
        { 
            this._filter += $"{property} ne '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder GreaterThan(string property, string value) 
        { 
            this._filter += $"{property} gt '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder GreaterThanOrEqual(string property, string value) 
        { 
            this._filter += $"{property} ge '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder LessThan(string property, string value) 
        { 
            this._filter += $"{property} lt '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder LessThanOrEqual(string property, string value) 
        { 
            this._filter += $"{property} le '{value}'"; 
            return this; 
        }

        public FilterQueryOptionBuilder Complex(string query) 
        { 
            this._filter += $"{query}"; 
            return this; 
        }
    }

    public class SelectQueryOptionBuilder
    {
        public string Select { get { return $"{this._select}{string.Join(",", this._selectableProperties)}"; } }
        private string _select;
        private IList<string> _selectableProperties;

        public SelectQueryOptionBuilder()
        {
            this._selectableProperties = new List<string>();
            this._select = $"$select=";
        }

        public SelectQueryOptionBuilder AddProperty(string property)
        {
            this._selectableProperties.Add(property);
            return this;
        }

        public SelectQueryOptionBuilder ResetProperty()
        {
            this._selectableProperties = new List<string>();
            return this;
        }

        public SelectQueryOptionBuilder AddAllProperties()
        {
            this.ResetProperty();
            var type = typeof(SalesDataRootModel);
            foreach (var property in type.GetProperties())
            {
                this._selectableProperties.Add(property.Name);
            }
            return this;
        }
    }

    public class QueryOptionBuilder
    {
        public string Query { get { return $"{this._query}{string.Join("&", this._queries)}"; } }
        private string _query;
        private IList<string> _queries;

        public QueryOptionBuilder()
        {
            this._queries = new List<string>();
            this._query = $"&";
        }

        public QueryOptionBuilder AppendQuery(string query)
        {
            this._queries.Add(query);
            return this;
        }
    }
}
