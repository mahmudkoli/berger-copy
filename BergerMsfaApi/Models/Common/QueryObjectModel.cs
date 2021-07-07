
namespace BergerMsfaApi.Models.Common
{
    public class QueryObjectModel
    {
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string GlobalSearchValue { get; set; }

        public QueryObjectModel()
        {
            this.Page = 1;
            this.PageSize = int.MaxValue;
        }
    }


    public class BrandQueryObjectModel : QueryObjectModel
    {
        public BrandQueryObjectModel()
        {
            Brands ??= new string[] { };
            MatrialCodes ??= new string[] { };
        }
        public string[] Brands { get; set; }
        public string[] MatrialCodes { get; set; }
    }
}