using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace PageUp.FuzzySerializer.Legacy.Tests
{
    public class FuzzyObjectContractResolverTests
    {
        private class Person {
            public string Name;
            public int Age;
        }
        private Person _person;

        public FuzzyObjectContractResolverTests()
        {
            _person = new Person { Name = "Abhaya Chauhan", Age = 21 };
        }

        [Fact]
        public void AddRandomPropertyToObjectsIsOn_Serialize_AddsNewPropertyOfTypeGuid()
        {
            var serialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, 
                new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { AddRandomPropertyToObjects = true })
                });            

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);

            Assert.Equal(deserialisedObj.Count, 3);

            var key = FindNewPropertyKey(deserialisedObj);
            Guid output;
            var result = Guid.TryParse(key, out output);
            Assert.True(result, "New property is not a Guid");
        }

        [Fact]
        public void AddRandomPropertyToObjectsIsOff_Serialize_DoesNotAddNewProperty()
        {
            var serialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, 
                new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { AddRandomPropertyToObjects = false })
                });            

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);

            Assert.Equal(deserialisedObj.Count, 2);
        }

        [Fact]
        public void AddRandomPropertyToObjectsIsOn_Serialize_NewPropertyIsDifferentEverySerialize()
        {
            var jsonSerializeSettings = new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver()
                };
            var firstSerialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, jsonSerializeSettings);

            var secondSerialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, jsonSerializeSettings);
            
            var firstDeserialisedObj = (JObject)JsonConvert.DeserializeObject(firstSerialisedObject);
            var secondDeserialisedObj = (JObject)JsonConvert.DeserializeObject(secondSerialisedObject);

            var firstKey = FindNewPropertyKey(firstDeserialisedObj);
            var secondKey = FindNewPropertyKey(secondDeserialisedObj);

            Assert.DoesNotMatch(firstKey, secondKey);
        }

        [Fact]
        public void AddPropertyInRandomPositionIsOn_Serialize_NewPropertyIsInDifferentPositionEverySerialize()
        {
            var jsonSerializeSettings = new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { AddRandomPropertyToObjects = true, AddPropertyInRandomPosition = true, ShuffleResponse = false })
                };

            List<bool> differentPositionInSerialise = new List<bool>();

            for (int i = 0; i<5; i++) {
                var firstSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);

                var secondSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);


                var firstDeserialisedObj = (JObject)JsonConvert.DeserializeObject(firstSerialisedObject);
                var secondDeserialisedObj = (JObject)JsonConvert.DeserializeObject(secondSerialisedObject);

                var firstIndex = FindIndexOfNewPropertyKey(firstDeserialisedObj);
                var secondIndex = FindIndexOfNewPropertyKey(secondDeserialisedObj);
                differentPositionInSerialise.Add(firstIndex != secondIndex);
            }

            Assert.Contains(true, differentPositionInSerialise);
        }

        [Fact]
        public void AddPropertyInRandomPositionIsOff_Serialize_NewPropertyItTheLastPositionEverySerialize()
        {
            var jsonSerializeSettings = new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { AddRandomPropertyToObjects = true, ShuffleResponse = false, AddPropertyInRandomPosition = false })
                };

            List<bool> lastPositionInSerialise = new List<bool>();

            var serialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, jsonSerializeSettings);

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);

            var index = FindIndexOfNewPropertyKey(deserialisedObj);
            Assert.Equal(index + 1, deserialisedObj.Count);
        }

        [Fact]
        public void ShuffleResponseIsOn_Serialize_EntireResponseIsShuffled()
        {
            var jsonSerializeSettings = new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { ShuffleResponse = true })
                };

            List<bool> differentPositionInSerialise = new List<bool>();

            for (int i = 0; i<3; i++) {
                var firstSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);

                var secondSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);


                var firstDeserialisedObj = (JObject)JsonConvert.DeserializeObject(firstSerialisedObject);
                var secondDeserialisedObj = (JObject)JsonConvert.DeserializeObject(secondSerialisedObject);

                var firstIndex = FindIndexOfPropertyKey(firstDeserialisedObj, "Name");
                var secondIndex = FindIndexOfPropertyKey(secondDeserialisedObj, "Name");
                differentPositionInSerialise.Add(firstIndex != secondIndex);
            }

            Assert.Contains(true, differentPositionInSerialise);
        }

        [Fact]
        public void ShuffleResponseIsOff_Serialize_EntireResponseIsShuffled()
        {
            var jsonSerializeSettings = new JsonSerializerSettings { 
                    ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { ShuffleResponse = false })
                };

            List<bool> differentPositionInSerialise = new List<bool>();

            for (int i = 0; i<3; i++) {
                var firstSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);

                var secondSerialisedObject = JsonConvert.SerializeObject(
                    _person, Formatting.None, jsonSerializeSettings);


                var firstDeserialisedObj = (JObject)JsonConvert.DeserializeObject(firstSerialisedObject);
                var secondDeserialisedObj = (JObject)JsonConvert.DeserializeObject(secondSerialisedObject);

                var firstIndex = FindIndexOfPropertyKey(firstDeserialisedObj, "Name");
                var secondIndex = FindIndexOfPropertyKey(secondDeserialisedObj, "Name");
                Assert.Equal(firstIndex, secondIndex);
            }
        }

        [Fact]
        public void UseCamelCaseNamingStrategyIsOn_Serialize_AllPropertyNamesFollowCamelCaseNamingStrategy()
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { UseCamelCaseNamingStrategy = true, AddRandomPropertyToObjects = false })
            };

            var serialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, jsonSerializeSettings);

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);
            
            Assert.Equal(2, deserialisedObj.Count);
            Assert.Equal("Abhaya Chauhan", deserialisedObj["name"]);
            Assert.Equal("21", deserialisedObj["age"]);
        }

        [Fact]
        public void UseCamelCaseNamingStrategyIsOn_Serialize_AllPropertyNamesFollowCamelCaseNamingStrategy_PropertyNameStartsWithAnAcronym()
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { UseCamelCaseNamingStrategy = true, AddRandomPropertyToObjects = false })
            };
            
            var serializableObject = 
                new
                {
                    IDDescriptor = "Abhaya Chauhan"
                };

            var serialisedObject = JsonConvert.SerializeObject(
                serializableObject, Formatting.None, jsonSerializeSettings);

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);

            Assert.Equal(1, deserialisedObj.Count);
            Assert.Equal("Abhaya Chauhan", deserialisedObj["idDescriptor"]);
        }

        [Fact]
        public void UseCamelCaseNamingStrategyIsOff_Serialize_AllPropertyNamesRemainUnchanged()
        {
            var jsonSerializeSettings = new JsonSerializerSettings
            {
                ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings { UseCamelCaseNamingStrategy = false, AddRandomPropertyToObjects = false })
            };

            var serialisedObject = JsonConvert.SerializeObject(
                _person, Formatting.None, jsonSerializeSettings);

            var deserialisedObj = (JObject)JsonConvert.DeserializeObject(serialisedObject);

            Assert.Equal(2, deserialisedObj.Count);
            Assert.Equal("Abhaya Chauhan", deserialisedObj["Name"]);
            Assert.Equal("21", deserialisedObj["Age"]);
        }

        private int FindIndexOfPropertyKey(JObject deserialisedPerson, string propertyName) {
            int index = 0;
            foreach (var keyValuePair in deserialisedPerson) 
            {
                var knownProperties = new List<string> { "Name", "Age"};
                if (knownProperties.Contains(keyValuePair.Key)) {
                    if (String.Equals(keyValuePair.Key, propertyName, StringComparison.OrdinalIgnoreCase)) {
                        return index;
                    }
                    index++;
                }
            }
            Assert.True(false, "New property not found in object");
            return -1;
        }

        private int FindIndexOfNewPropertyKey(JObject deserialisedPerson) {
            int index = 0;
            foreach (var keyValuePair in deserialisedPerson) 
            {
                var knownProperties = new List<string> { "Name", "Age"};
                if (!knownProperties.Contains(keyValuePair.Key)) {
                    return index;
                }
                index++;
            }
            Assert.True(false, "New property not found in object");
            return -1;
        }

        private string FindNewPropertyKey(JObject deserialisedPerson) {
            foreach (var keyValuePair in deserialisedPerson) 
            {
                var knownProperties = new List<string> { "Name", "Age"};
                if (!knownProperties.Contains(keyValuePair.Key)) {
                    return keyValuePair.Key;
                }
            }
            Assert.True(false, "New property not found in object");
            return null;
        }
    }
}
