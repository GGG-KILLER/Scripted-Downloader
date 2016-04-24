#Documentation

The Scripted Downloader uses JavaScript as it's language with a few additions.
Unfortunately try-catch statements do not work (so make sure your script works under all situations).

Functions such as JSON.parse and etc. are available by default.

| Globals:                      |                                               |
| ----------------------------- | --------------------------------------------- |
| `void log(String message)`    | Writes a message to the log screen            |
| `void error(String messsage)` | Writes an red error message to the log screen |

###Modules

| HTTP:                                           |                                                                            |
| ----------------------------------------------- | -------------------------------------------------------------------------- |
| `String http.getString(String URL)`             | Returns the contents of the URL as a string                                |
| `byte[] http.getData(String URL)`               | Returns the contents of the URL as a byte array                            |
| `Document http.getDoc(String URL)`              | Returns a parsed HTML document from the URL                                |
| `void http.dlFile(String URL, String FileName)` | Downloads a file from URL to the FileName inside the isolated environment. |
| `Object http.getJson(String URL)`               | Parses JSON from the informed URL                                          |
| `String http.UA`                                | Defines the User Agent to be used by the request (Chrome by default)       |

All of these functions only work inside the save path chosen by the user, otherwise they'll use the current working directory.

| IO:                                                  |                                                      |
| ---------------------------------------------------- | ---------------------------------------------------- |
| `void io.writeString(String path, String contents)`  | Writes all text to the file (overwriting everything) |
| `void io.appendString(String path, String contents)` | Appends all text to the file                         |
| `void io.writeData(String path, byte[] contents)`    | Writes all data to the file (overwriting everything) |
| `void io.appendData(String path, byte[] contents)`   | Appends all data to the file                         |
| `void io.mkdir(String folder)`                       | Creates the folder                                   |
| `Object io.readJson(String path)`                    | Reads JSON from the informed file                    |
| `void io.writeJson(String path, Object obj)`         | Writes JSON to the informed path                     |
