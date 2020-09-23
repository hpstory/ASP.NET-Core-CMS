using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Helpers
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
