// worker.js


setInterval(function () {


    var data = {

        "ID": 001,
        "test": 0.4,
        "part": "hello earthlings",
        "lastKey" : 43334

    }


    postMessage(data);
}, 120000);