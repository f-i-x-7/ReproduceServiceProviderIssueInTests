using Microsoft.Extensions.Options;
using System;

namespace ReproduceServiceProviderIssueInTests
{
    public sealed class Foo
    {
        private readonly FooOptions _options;

        public Foo(IOptions<FooOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public int OptionsCount => _options.KeyValuePairs.Count;
    }
}
