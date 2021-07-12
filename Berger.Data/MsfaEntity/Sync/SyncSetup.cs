using System;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.Sync
{
    public class SyncSetup : AuditableEntity<int>
    {
        public int SyncHourlyInterval { get; set; }
        public DateTime? LastSyncTime { get; set; }
    }
}
