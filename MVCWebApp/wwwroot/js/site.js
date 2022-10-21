// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {
    var url = window.location.protocol + "//" + window.location.host + "/home/keepalive"; 

    setInterval(httpGet, 600000);

    function httpGet() {
        console.log(url);
        fetch(url).then(function (response) {
            console.log(response);
        }).catch(function () {
            console.log("Error");
        });
    }
});

