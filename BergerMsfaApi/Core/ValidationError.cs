namespace BergerMsfaApi.Core
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string[] ErrorList { get; set; }
    }
}
