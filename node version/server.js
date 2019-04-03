var express = require('express')
var app = express()
var dir = require('node-dir');
var exec = require('child_process').exec;

app.use(express.static('dist'));
app.use('/images', express.static(__dirname + '/images'));

app.get('/images.json', function (req, res) {
	dir.files('images', function (err, files) {
		result = [];
		files.forEach(file => {
			if (file.toUpperCase().endsWith(".JPG")) {
				result.push(file.replace(/\\/g, "/"));
			}
			
		});
		result = shuffle(result);
		res.send(result);
	});
});

// https://stackoverflow.com/questions/2450954/how-to-randomize-shuffle-a-javascript-array
function shuffle(array) {
	var currentIndex = array.length, temporaryValue, randomIndex;

	// While there remain elements to shuffle...
	while (0 !== currentIndex) {

		// Pick a remaining element...
		randomIndex = Math.floor(Math.random() * currentIndex);
		currentIndex -= 1;

		// And swap it with the current element.
		temporaryValue = array[currentIndex];
		array[currentIndex] = array[randomIndex];
		array[randomIndex] = temporaryValue;
	}

	return array;
}

var isDisplayOff = false;

function displayOn() {
	exec("sudo vcgencmd display_power 1", function (error, stdout, stderr) {
		isDisplayOff = false;
	});
}

function displayOff() {
	exec("sudo vcgencmd display_power 0", function (error, stdout, stderr) {
		isDisplayOff = true;
	});
}

app.get('/display-off', function (req, res) {
	displayOff();
	res.send("Executed");
});
app.get('/display-on', function (req, res) {
	displayOn();
	res.send("Executed");
});

// On for 15 minutes by default
var countdown = 900;
var interval = setInterval(() => {
	if (--countdown <= 0) {
		// Turn off it is on
		if (!isDisplayOff) {
			displayOff();
		}
	}
}, 1000);

app.get('/display-on-timer', function (req, res) {
	countdown = 300;
	// Only turn on when its off
	if (isDisplayOff) {
		displayOn();
	}
	res.send("Executed");
});

app.get('/countdown.json', function (req, res) {
	res.send({ countdown });
});

app.get('/', function (req, res) {
	res.render("index.html");
});

app.listen(3000, function () {
	console.log('Node js server running on port 3000!')
});