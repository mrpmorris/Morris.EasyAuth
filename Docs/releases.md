# Releases

## New in 0.3
 * Renamed to Morris.EasyAuth

## New in 0.2
 * Auto register so that injecting `HttpClient` or asking for an instance via
    `IHttpClientFactory` with no name returns a client that will redirect to the
    sign-in page if not authenticated.
 * Exposed `AuthorizingMessageHandler` so consuming apps can use it in
    `Services.AddHttpClient(name: "SomeName", ...).AddHttpMessageHandler<T>();`

## New in 0.1
 * The ability for WASM to call to a server and ask for credentials, passing a cookie