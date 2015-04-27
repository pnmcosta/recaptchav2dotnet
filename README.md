# Recaptcha v2 API .NET implementation

Based on the [new API version](https://developers.google.com/recaptcha/) (v2) of the [Google Recaptcha](https://www.google.com/recaptcha/) this is a Web Forms .NET implementation of the API as a validation control.

## How to use

  - Go to the [recaptcha admin](https://www.google.com/recaptcha/admin) and register your site for an API Key and Secret.
  - Compile or [get the DLL](https://github.com/pnmcosta/recaptchav2dotnet/releases/download/v1.0/recaptchav2.dll) from the latest release
  - Add reference to your project, or add it to the /bin directory, and `<add assembly="recaptchav2" namespace="recaptchav2" tagPrefix="re"/>` to your [web.config](https://github.com/pnmcosta/recaptchav2dotnet/blob/master/src/recaptchav2.test/Web.config).
  - Add the control to your page

## Planed enhancements

  * Validation group support
  * Ajax/UpdatePanel support

## Releases

  * **2015-04-25** - v1.0 First release, single validation summary support.
