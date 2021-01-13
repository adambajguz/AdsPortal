namespace AdsPortal.DTO.Tests
{
    using System;
    using AutoBogus;
    using FluentAssertions;
    using Newtonsoft.Json;
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

        protected void EnsureCoherent<T0, T1, T2, T3>()
            where T0 : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            EnsureCoherent<T0, T1, T2>();
            EnsureCoherent<T2, T3>(); //If T0 is coherent with T1 and T2 then T4 must be coherent with T0 and T1 when coherence with T2 is present
        }

        protected void EnsureCoherent<T0, T1, T2>()
            where T0 : class
            where T1 : class
            where T2 : class
        {
            EnsureCoherent<T0, T1>();
            EnsureCoherent<T1, T0>();

            EnsureCoherent<T1, T2>();
            EnsureCoherent<T2, T1>();

            //If T0 is coherent with T1 then T2 must be coherent with T0 when coherence with T1 is present
        }

        protected void EnsureCoherent<T0, T1>()
            where T0 : class
            where T1 : class
        {
            EnsureCoherentOneWay<T0, T1>();
            EnsureCoherentOneWay<T1, T0>();
        }

        protected void EnsureCoherentOneWay<T0, T1>()
            where T0 : class
            where T1 : class
        {
            typeof(T1).Should().NotBe<T0>("cannot check coherence between the same DTO type");

            T0 t0 = AutoFaker.Generate<T0>();
            string value = JsonConvert.SerializeObject(t0, _settings);
            value.Should().NotBeNullOrWhiteSpace();

            _output.WriteLine($"{typeof(T0)}:");
            _output.WriteLine(value);

            T1? t1 = null;
            Action action = () =>
            {
                t1 = JsonConvert.DeserializeObject<T1>(value, _settings);
            };
            action.Should().NotThrow<JsonSerializationException>("DTO classes must match and consume all properties");
            t1.Should().NotBeNull();

            _output.WriteLine($"Coherence between '{typeof(T0)}' and '{typeof(T1)}' is ensured.{Environment.NewLine}");
        }
    }
}
