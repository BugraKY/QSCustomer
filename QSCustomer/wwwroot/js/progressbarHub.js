"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/homeHub").build();


connection.on("ReceiveProgressRate", function (data) {
    if (progressCookie == data.id) {

        console.log("Socket Progress Bar Working %" + data.rate);
        $('#theprogressbar').val(data.rate);

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
