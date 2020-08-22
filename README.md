Steps to reproduce:

- Run sample app
- Hit https://localhost:5001/weatherforecast
- Observe the console ends up writing several Activities many of which are for the web request.  You should see the Activity for mongodb.find.

- Remove usage of .AddMongoDBInstrumentation() from Startup.cs
- Run sample app
- Hit https://localhost:5001/weatherforecast
- Only one Activity is written and it's for the web request.
