# Exploring .net 6

## Projects

## JSON Server with Minimal API
A simple Fake REST API for Json files which exposes GET, POST, PUT, DELETE for specified JSON files.
The Json is expected to be serialized to  ``` Dictionary<string, ICollection<Dictionary<string, object>>>  ```.

(see source.json file)

A key '__ id __' will be added to the objects. The ordinal ignore case string comparer is used for dictionary keys.





<br>
<br>

### ACKNOWLEDGEMENT

Countries data in source.json collected from calls to [RestCountries.eu](https://restcountries.eu/)
