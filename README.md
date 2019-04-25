# WHWebService
Sample code for pushing wav file to web service through RestSharp and HttpClient

This code sample was written to diagnose and solve issues with pushing a WAV file to a web service. The key to resolving the issue, which was causing a 500 error to be returned, was to include the proper Content-Type header on the content (not the request) of the POST call.
