"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();
var connection = new signalR.HubConnectionBuilder().configureLogging(signalR.LogLevel.None).withUrl("/homeHub").build();


connection.on("ReceiveProgressRate", function (data) {
    if (progressCookie == data.id) {
        console.log("Socket Progress Bar Working %" + data.title);
        $('#theprogressbar').val(data.rate);
        $('#barTitle').html(data.title);
    }
    else {

    }

});

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    //return console.error(err.toString());
});


/*
document.getElementById("btnSend").addEventListener("click", function (event) {

    event.preventDefault();
});*/
/*
connection.invoke("SendData", dataTableVal).catch(function (err) {
    return console.error(err.toString());
});*/
