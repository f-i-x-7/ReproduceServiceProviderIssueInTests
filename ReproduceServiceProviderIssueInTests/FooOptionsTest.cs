using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ReproduceServiceProviderIssueInTests
{
    public class FooOptionsTest
    {
        private readonly FooOptions defaultOptions;
        private readonly IConfiguration defaultConfig;

        public FooOptionsTest()
        {
            (defaultOptions, defaultConfig) = GetOptionsAndConfig("appsettings.Default.json");
        }

        [Fact]
        public void TestWithDefaultOptions()
        {
            var foo = CreateFoo();

            Assert.Equal("Value1", defaultConfig["FooOptions:KeyValuePairs:Prop1"]); // OK
            Assert.Equal("Value2", defaultConfig["FooOptions:KeyValuePairs:Prop2"]); // OK
            Assert.Null(defaultConfig["FooOptions:KeyValuePairs:Prop3"]); // OK
            Assert.Equal(2, foo.OptionsCount); // fails, count == 3
        }

        [Fact]
        public void TestWithCustomOptions()
        {
            var (customOptions, customConfig) = GetOptionsAndConfig("appsettings.Custom.json");
            var foo = CreateFoo(customOptions);

            Assert.Null(customConfig["FooOptions:KeyValuePairs:Prop1"]); // OK
            Assert.Null(customConfig["FooOptions:KeyValuePairs:Prop2"]); // OK
            Assert.Equal("Value3", customConfig["FooOptions:KeyValuePairs:Prop3"]); // OK
            Assert.Equal(1, foo.OptionsCount); // fails, count == 3
        }

        private Foo CreateFoo(FooOptions? customOptions = null) => new Foo(customOptions ?? defaultOptions);

        private static (FooOptions, IConfiguration) GetOptionsAndConfig(string configFile)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(configFile);

            var config = builder
                .Build();

            var services = new ServiceCollection();
            services.Configure<FooOptions>(config.GetSection("FooOptions"));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                return (serviceProvider.GetRequiredService<IOptions<FooOptions>>().Value, config);
            }
        }
    }
}
