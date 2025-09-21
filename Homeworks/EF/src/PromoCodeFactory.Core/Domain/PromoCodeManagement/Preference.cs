using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        : BaseEntity
    {
        public string Description { get; set; }

        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; } =
            new List<CustomerPreference>();
    }
}