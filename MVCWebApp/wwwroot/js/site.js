// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {
    const url = window.location.protocol + "//" + window.location.host + "/home/keepalive"; 
    const worker = new Worker('/js/worker.js');

    worker.onmessage = (e) => httpGet(e); 

    //setInterval(httpGet, 120000);

    function httpGet(e) {

        console.log('sending keep alive ping to.......');
        console.log(url);

        console.log('service worker incidental message......')
        console.log(e.data);
        fetch(url).then(function (response) {
            console.log(response);
        }).catch(function () {
            console.log("Error");
        });
    }
});

