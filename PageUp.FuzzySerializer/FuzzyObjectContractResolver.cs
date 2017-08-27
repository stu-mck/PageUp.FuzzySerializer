using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PageUp.FuzzySerializer {


    public class FuzzyObjectContractResolver : DefaultContractResolver
    {
        private readonly FuzzyObjectContractResolverSettings _settings;
        private Random _random = new Random();

        public FuzzyObjectContractResolver() : this(new FuzzyObjectContractResolverSettings()) {}
        public FuzzyObjectContractResolver(FuzzyObjectContractResolverSettings settings)
        {
            _settings = settings;
        }

        public override JsonContract ResolveContract(Type type) {
            var jsonContract = base.ResolveContract(type);

            if (!(jsonContract is JsonObjectContract))
                return jsonContract;

            return CreateContract(type);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            
            if (_settings.AddRandomPropertyToObjects) {

                var property = new JsonProperty {
                        ShouldDeserialize = (_ => false),
                        ShouldSerialize = (_ => true),
                        PropertyName = Guid.NewGuid().ToString(),
                        PropertyType = typeof(Guid),
                        ValueProvider = new RandomGuidValueProvider(),
                        Readable = true,
                        Writable = false
                    };
                    
                if (_settings.AddPropertyInRandomPosition)
                {
                    properties.Insert(_random.Next(properties.Count + 1), property);
                }
                else {
                    properties.Add(property);
                }
            }
                
            return
                _settings.ShuffleResponse ? 
                    properties.OrderBy(a => _random.Next()).ToList()
                    : properties;
        }
    }


}

