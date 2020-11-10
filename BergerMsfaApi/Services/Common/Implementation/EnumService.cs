using Berger.Common.Enumerations;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class EnumService : IEnumService
    {
        public IList<KeyValuePairObjectModel> GetCompetitionPresenceSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumCompetitionPresence))
                    .Cast<EnumCompetitionPresence>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetDealerSalesIssueSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumDealerSalesIssue))
                    .Cast<EnumDealerSalesIssue>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetPainterInfluenceSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumPainterInfluence))
                    .Cast<EnumPainterInfluence>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetPrioritySelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumPriority))
                    .Cast<EnumPriority>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetProductLiftingSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumProductLifting))
                    .Cast<EnumProductLifting>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetRatingsSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumRatings))
                    .Cast<EnumRatings>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetSatisfactionSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumSatisfaction))
                    .Cast<EnumSatisfaction>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }

        public IList<KeyValuePairObjectModel> GetSubDealerInfluenceSelect()
        {
            IList<KeyValuePairObjectModel> result =
                Enum.GetValues(typeof(EnumSubDealerInfluence))
                    .Cast<EnumSubDealerInfluence>()
                    .Select(p => new KeyValuePairObjectModel { Value = ((int)p), Text = p.ToString().Replace("_", " ") })
                    .ToList();

            return result;
        }
    }
}
