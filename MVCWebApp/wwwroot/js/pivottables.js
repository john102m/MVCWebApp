
//date must be YYYY-MM-DD  =>  DD/MM/YYYY

const transposeDate = function (date) {
    if (!date.includes("-")) return "28/08/1964";
    let res = date.split('-');
    if (res.length !== 3) return "28/08/1964";
    return res[2] + "/" + res[1] + "/" + res[0];
}
//top answer for JS groupBy
//https://stackoverflow.com/questions/14446511/most-efficient-method-to-groupby-on-an-array-of-objects
var groupBy = function (xs, key) {
    return xs.reduce(function (rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
    }, {});
};
//console.log(groupBy(['one', 'two', 'three'], 'length'));

var dataArray = [];
$.getJSON("/mps.json", function (data) {
    var items = groupBy(data, 'Province')
    //$.each( data, function( key, val ) {
    //    items.push(val['Province']);
    //});
    dataArray = Object.getOwnPropertyNames(items);
    //console.log(dataArray);
});

$.getJSON("/testdata.json", function (data) {
    var items = data;//groupBy(data, 'profile')
    //$.each( data, function( key, val ) {
    //    items.push(val['Province']);
    //});
    dataArray = Object.getOwnPropertyNames(items);
    items.forEach(function (item) {
        //console.log(transposeDate(item['profile']['dob']));
    });
});

// This example shows custom aggregators and sorters using
// the "Canadian Parliament 2012" dataset.
$(function () {
    var tpl = $.pivotUtilities.aggregatorTemplates;

    $.getJSON("/mps.json", function (mps) {
        $("#output").pivotUI(mps, {
            rows: ["Province"], cols: ["Party"],
            aggregators: {
                "Number of MPs": function () { return tpl.count()() },
                "Average Age of MPs": function () { return tpl.average()(["Age"]) }
                //"Testy test test": function() { return tpl.sum()(["Age"])}
            },
            sorters: {
                Province: $.pivotUtilities.sortAs(dataArray.sort()),

                //Province: $.pivotUtilities.sortAs(
                //        ["British Columbia", "Alberta",
                //        "Saskatchewan", "Manitoba",
                //        "Territories", "Ontario", "Quebec",
                //        "New Brunswick", "Prince Edward Island",
                //        "Nova Scotia", "Newfoundland and Labrador"]),
                Age: function (a, b) { return b - a; } //sort backwards
            }
        });

        restoreSettings = function () {
            //console.log('System User Name: ' + SystemUser['Name']);
            let cookieName = "pivotConfig" + SystemUser.ID;  //Must be .NET logged in
            //console.log(cookieName);
            let cookie = $.cookie(cookieName);
            //console.log(cookie);
            if (cookie !== undefined) {
                var options = JSON.parse(cookie);
                options['aggregators'] = {
                    "Number of MPs": function () { return tpl.count()() },
                    "Average Age of MPs": function () { return tpl.average()(["Age"]) }
                    //"Testy test test": function() { return tpl.sum()(["Age"])}
                };

                $("#output").pivotUI(mps, options, true);
                return true;

            } else return false;

        }


        if (SystemUser.ID) restoreSettings();

        $("#save").on("click", function () {
            var config = $("#output").data("pivotUIOptions");
            var config_copy = JSON.parse(JSON.stringify(config));

            //delete some values which will not serialize to JSON
            delete config_copy["aggregators"];
            delete config_copy["renderers"];


            var cookie = $.cookie("siteCookie");
            //console.log(cookie);           
            if (cookie !== undefined ) {
                var options = JSON.parse(cookie);
                if (options['Value'] === "Accept") {
                    let cookieName = "pivotConfig" + SystemUser.ID;  //Must be .NET logged in
                    //console.log(cookieName);
                    $.cookie(cookieName, JSON.stringify(config_copy));
                    myCustomAlert("Configuration settings saved.");
                } else myCustomAlert("Your cookie settings prevent this action.");

            } else myCustomAlert("You must accept cookies to save the settings.");

        });

        $("#restore").on("click", function () {
            if (restoreSettings()) myCustomAlert("Configuration settings are restored for " + SystemUser.Name);
            else myCustomAlert("No config settings found.");

        });


    });
});


myAlert = function (msg) {
    bootbox.alert({
        message: msg,
        //centerVertical: true,
        closeButton: false,
        animate: true,
        buttons: {}
    });
    window.setTimeout(function () { bootbox.hideAll() }, 2000);
}

myCustomAlert = function (msg) {
    bootbox.dialog({
        title: 'Confuguration Settings Information',
        message: `<p>${msg}</p>`,
        //centerVertical: true,         
        //size: 'small',
        onEscape: true,
        backdrop: false,
        closeButton: false,
        buttons: {
            ok: {
                label: 'OK',
                className: 'btn-primary',
                callback: function () {

                }
            },
        }
    });
    window.setTimeout(function () { bootbox.hideAll() }, 2000);
}