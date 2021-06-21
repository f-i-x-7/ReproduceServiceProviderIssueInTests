using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ReproduceServiceProviderIssueInTests
{
    public sealed class FooOptions : IOptions<FooOptions>
    {
        private static readonly Dictionary<string, string> EmptyDict = new Dictionary<string, string>(0);

        public IDictionary<string, string> KeyValuePairs { get; set; } = EmptyDict;

        FooOptions IOptions<FooOptions>.Value => this;
    }
}
