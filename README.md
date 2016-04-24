# Scripted-Downloader
Uses JS as the downloader logic making possible to download anything from anywhere.

# Requests
All requests should be explained in the following format:
Type of request: (functionality/script function/etc)
Request description: <description>
Reason of request: <reason>

# Script Samples
If you want to contribute by making a download script you've made public, feel free to open a pull request with your script inside the Samples folder!

# Example Script
randomdl.js:
```javascript
// ==DLScript==
// @author   You
// @homepage http://myhomepage.com
// @name     Random Downloader
// The url needs to be a valid Regex to check if the passed URL is valid
// @url      https?:\/\/example\.com\/([^\.])\.json
// @version  1.0
// ==/DLScript==
var images = http.getJson ( 'http://example.com' + url[1] + '.json' ).images;

log ( images.length + ' images found.' );

for ( var i = 0; i < images.length; i++ )
{
	log ( 'Downloading image ' + (i + 1) + ' out of ' + images.length );
	
	http.dlFile(images[i], url[1] + '/' + 'img' + i + '.png');
	
	log ( 'Downloaded image ' + (i + 1) + ' out of ' + images.length );
}
```

Then to use it, you'd input https://example.com/page1images.json, which would have the content as:
```json
{
	"images": [
		"/imgs/img1.png",
		"/imgs/img2.png",
		"/imgs/img3.png"
	]
}
```

And then it'd download img1.png, img2.png and img3.png to `<Save Path Chosen in the Settings>/randomdl/paage1images/`.

This is only a single possibility, but there are many more!

# Contributiosn
Jint - .NET JavaScript Interpreter

Jint is what runs the JavaScript, this project would be nothing without it.

Jint is Copyright of [Sebastien Ros](https://github.com/sebastienros)
