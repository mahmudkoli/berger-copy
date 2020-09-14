namespace BergerMsfaApi.Models.Menus
{
    public class MenuActivityPermissionVm
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }

        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanInsert { get; set; }
        public bool CanDelete { get; set; }
    }
}
