using System;
using System.Collections.Generic;

namespace WebApplication.Helpers
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> TargetProperties { get; set; }
        public bool Revert { get; set; }
        public PropertyMappingValue(IEnumerable<string> targetProperties, bool revert = false)
        {
            TargetProperties = targetProperties ?? throw new ArgumentNullException(nameof(targetProperties)); ;
            Revert = revert;
        }
    }
}
