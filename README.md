# PageUp.Fuzzy [![Build Status](https://travis-ci.org/PageUpPeopleOrg/PageUp.FuzzySerializer.svg?branch=master)](https://travis-ci.org/PageUpPeopleOrg/PageUp.FuzzySerializer)

https://www.abhayachauhan.com/2017/10/fuzzy-serializer/

This library can be plugged into the MvcPipeline which enables you to "Fuzzy" the Json response on API calls.

PageUp.FuzzySerializer is a ContractResolver for Newtonsoft.Json.

## Get Started

Set up `FuzzyObjectContractResolver` in the `Configure` method on your `Startup` file.

```
public IServiceProvider ConfigureServices(IServiceCollection services)
{
  services.AddMvc().AddJsonOptions(options =>
  {
      options.SerializerSettings.ContractResolver = new FuzzyObjectContractResolver(new FuzzyObjectContractResolverSettings());
  });
}
```

## Publishing the Package

The package is hosted in NuGet. Currently, the package version has to be incremented manually, and the package manually uploaded to NuGet gallery.


## Release 2.0
Updated packages and target frameworks to contemporary versions
- PageUp.FuzzySerializer `NetStandard2.0`
- PageUp.FuzzySerializer.Legacy `Net Framework 4.8`
- PageUp.FuzzySerializer.Net `Net Framework 4.8`
- PageUp.FuzzySerializer.NetCoreMvc `Net Core 3.1`

