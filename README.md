# punkOptimise

[![NuGet release](https://img.shields.io/nuget/v/punkOptimise.svg)](https://www.nuget.org/packages/punkOptimise/)

An action in Umbraco that allows you optimise your media using multiple providers

## Usage 

- Install the package via nuget.

- Add .AddOptimise() to your startup file: 

~~~csharp 
  services
        .AddUmbraco(_env, _config)
        .AddBackOffice()
        .AddWebsite()               
        .AddOptimise()
        .Build();
~~~

- Add the following configuration element

~~~json
 "Optimise": {
    "Quality": 60,
    "TinyPng": {
      "ApiKey": "",      
    }
  }
~~~
Update the ApiKey property with your API key from https://tinypng.com/developers


- Run Umbraco and they'll be a new action for Media to optimise. 


## Nuget

`Install-Package punkOptimise`

https://www.nuget.org/packages/punkOptimise/

## Compatibility
   
- Umbraco 10.4+

## Screenshots 

### Action
![Screenshot](https://raw.github.com/garpunkal/punkOptimise/main/context-menu.png)

### Context Menu
![Screenshot](https://raw.github.com/garpunkal/punkOptimise/main/context-menu-action.png)
  
# Contact
This project is maintained by Gareth Wright and contributors. If you have a question or issue, please get in touch on [Twitter](https://twitter.com/garpunkal), or by raising an issue on GitHub.

