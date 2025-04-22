async function login() {
    const username = document.getElementById("usernameInput").value.trim();
    const password = document.getElementById("passwordInput").value.trim();

    const response = await fetch("/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const userId = await response.json();
        localStorage.setItem("savedUsername", username);
        localStorage.setItem("savedUserId", userId);
        window.location.href = "Telechat.html";
    } else {
        alert("Login failed");
    }
}

async function register() {
    const username = document.getElementById("usernameInput").value.trim();
    const password = document.getElementById("passwordInput").value.trim();

    const response = await fetch("/api/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        alert("Registration successful. You can now login.");
    } else {
        alert("Username already taken.");
    }
}
