# CSharpMiscellaneous

This repository contains test projects.

## Miscellaneous

This project uses reflection to instanciate classes and run the Execute method. It contains:
- DbCommandInterceptorTask: add interceptor to an Entity Framework Core DbContext to trace SQL commands.
- ExecutablePathTask: it was created to test the est way to get the assembly path (using single file build and/or application installed into UNC folder).
- NatStarTask: read the NatStar dictionary. Don't ask what is NatStar.
- RegistryTask: write in the Windows registry :
  - to write in the registry, you have to launch the application with administrator privilege;
  - it adds a yturi URI scheme that can open youtube video;
  - create a shorcut on your desktop using yturi:eBGIQ7ZuuiU as path or copy paste yturi:eBGIQ7ZuuiU into  your web navigator.
