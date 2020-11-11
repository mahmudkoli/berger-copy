using BergerMsfaApi.Models.Common;
using System.Collections.Generic;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface IEnumService
    {
        IList<KeyValuePairObjectModel> GetRatingsSelect();
        IList<KeyValuePairObjectModel> GetPrioritySelect();
        IList<KeyValuePairObjectModel> GetSatisfactionSelect();
        IList<KeyValuePairObjectModel> GetProductLiftingSelect();
        IList<KeyValuePairObjectModel> GetDealerSalesIssueSelect();
        IList<KeyValuePairObjectModel> GetPainterInfluenceSelect();
        IList<KeyValuePairObjectModel> GetSubDealerInfluenceSelect();
        IList<KeyValuePairObjectModel> GetCompetitionPresenceSelect();
    }
}
