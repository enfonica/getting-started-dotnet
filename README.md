## Getting Started for .NET

This repo contains sample applications written in C# that use the Enfonica cloud communications API.

See [https://enfonica.com/docs/libraries/dotnet/getting-started/](https://enfonica.com/docs/libraries/dotnet/getting-started/) for instructions on how to configure and run the samples.

The applications make use of the [Enfonica .NET Client Library].

## Configuring your local environment

To configure your local environment, you must download your Enfonica service account key and save it
to the root of this repository with the filename `service-account-key.json`.

## ApiDialler

The ApiDialler sample application starts an outgoing call on the PSTN.

This application listens on port 12964. For local development, we recommend using [ngrok] to create
a publicly accessible tunnel so the Enfonica API can connect to your application.

To create the tunnel using ngrok:

```
ngrok http localhost:12964
```

There are several constants in `Program.cs` that need to be modified to get the example working.

Once you have configured your environment and the constants, you can run the application and it will:

1. Create an outgoing call.
2. Control the call by synthesizing some speech.
3. Log all state updates to the console.

## Licensing

* See [LICENSE](LICENSE)

[Enfonica .NET Client Library]: https://github.com/enfonica/enfonica-dotnet
[ngrok]: https://ngrok.com/
