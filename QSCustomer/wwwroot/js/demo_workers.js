/*import $ from "jquery";*/
importScripts("workerFakeDOM.js");
importScripts("jquery.js");
console.log("JQuery version:", $.fn.jquery);



//var i = 0;
var val = 0;



function timedCount(){
    $.ajax({
        url: "/progressbarval",
        type: "get",
        cache: false,
        async: true,
        /*contentType: 'application/json; charset=UTF-8',*/
        crossDomain: true,
        /*dataType: 'jsonp',*/
        timeout: 2000
    }).always(function (data) {
        console.log("done: " + data);
        val = data;
    });
    postMessage(val);
/*
i=i+1;
postMessage(val);    */               //posts a message back to the HTML page.
setTimeout("timedCount()",500);
}

timedCount();