namespace BergerMsfaApi.Models.Menus
{
    public class ActivityPermissionModel
    {
        public int MenuId { get; set; }
        public string RoleName { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string Url { get; set; }

        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanInsert { get; set; }
        public bool CanDelete { get; set; }
    }
}
