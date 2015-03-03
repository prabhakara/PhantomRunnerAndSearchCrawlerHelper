var page = require('webpage').create(), system = require('system');

page.viewportSize = { width: 1280, height: 1024 };

if (system.args.length < 2) {
    throw ("unexpected number of arguments");
}

var url = system.args[1];

page.open(url, function (status) {
    console.log(page.content);
    phantom.exit();
});