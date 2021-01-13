namespace AdsPortal.DTO.Tests
{
    using System;
    using AutoBogus;
    using FluentAssertions;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using Newtonsoft.Json;
    using Typin;
    using Xunit.Abstractions;

    public abstract class BaseTest
    {
        private readonly ITestOutputHelper _output;
        private readonly JsonSerializerSettings _settings;

        protected BaseTest(ITestOutputHelper output)
        {
            _output = output;

            _settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                Formatting = Formatting.Indented
            };
        }

        protected void EnsureCoherent<TApi, TCli, TWeb>()
            where TApi : class
            where TCli : class
            where TWeb : class
        {
            typeof(TWeb).Should().NotBe<TApi>("WebPortal DTO cannot be WebApi DTO").And.NotBe<TCli>("WebPortal DTO cannot be CLI DTO");

            TApi api = AutoFaker.Generate<TApi>();
            string value = JsonConvert.SerializeObject(api, _settings);
            value.Should().NotBeNullOrWhiteSpace();

            _output.WriteLine($"{typeof(TApi)}:");
            _output.WriteLine(value);

            TCli? cli = null;
            Action actionCli = () =>
            {
                cli = JsonConvert.DeserializeObject<TCli>(value, _settings);
            };
            actionCli.Should().NotThrow<JsonSerializationException>("all DTO classes must match and consume all properties");
            cli.Should().NotBeNull();

            TWeb? web = null;
            Action actionWeb = () =>
            {
                web = JsonConvert.DeserializeObject<TWeb>(value, _settings);
            };
            actionWeb.Should().NotThrow<JsonSerializationException>("all DTO classes must match and consume all properties");
            web.Should().NotBeNull();
        }

        protected void EnsureCoherent<TApi, T>()
            where TApi : class
            where T : class
        {
            typeof(T).Should().NotBe<TApi>("WebPortal DTO cannot be WebApi DTO");

            TApi api = AutoFaker.Generate<TApi>();
            string value = JsonConvert.SerializeObject(api, _settings);
            value.Should().NotBeNullOrWhiteSpace();

            _output.WriteLine($"{typeof(TApi)}:");
            _output.WriteLine(value);

            T? t = null;
            Action action = () =>
            {
                t = JsonConvert.DeserializeObject<T>(value, _settings);
            };
            action.Should().NotThrow<JsonSerializationException>("all DTO classes must match and consume all properties");
            t.Should().NotBeNull();
        }
    }
}
