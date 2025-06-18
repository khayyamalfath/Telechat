"use strict";

var savedUsername = localStorage.getItem("savedUsername");
var savedUserId = localStorage.getItem("savedUserId");

if (!savedUsername || !savedUserId) {
    window.location.href = "index.html";
} else {
    document.getElementById("userInput").value = savedUsername;
}

// Creates SignalR connection 
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disables the send button until connection is established.
document.getElementById("sendButton").disabled = true;

/* Functions */
function addMessageToChat(username, messageText, sentAt) {
    // Adds message to chat

    var li = document.createElement("li");

    let sentAtUtc = new Date(sentAt);
    let sentAtLocal = sentAtUtc.toLocaleString();

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
    li.textContent = `${sentAtLocal} - ${username}: ${messageText}`;
    document.getElementById("messagesList").appendChild(li);
}

/* SignalR events */
connection.on("ReceiveMessage", function (username, messageText, sentAt) {
    // Handles messages receiving

    addMessageToChat(username, messageText, sentAt);
});

connection.on("LoadMessages", function (messages) {
    // Handles messages loading

    messages.forEach(msg => {
        addMessageToChat(msg.username, msg.messageText, msg.sentAt);
    });
});

connection.start().then(function () {
    // Starts SignalR connection

    document.getElementById("sendButton").disabled = false;
    connection.invoke("LoadPreviousMessages").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

/* User events */

document.getElementById("sendButton").addEventListener("click", function (event) {
    // Sends a message on button click

    var messageText = document.getElementById("messageInput").value;
    var userId = parseInt(savedUserId, 10)

    connection.invoke("SendMessage", userId, messageText).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("messageInput").value = "";

    event.preventDefault();
});

document.getElementById("messageInput").addEventListener("keydown", function (event) {
    // Sends a message on pressing Enter key

    if (event.key === "Enter") {
        event.preventDefault();
        document.getElementById("sendButton").click();
    }
});
