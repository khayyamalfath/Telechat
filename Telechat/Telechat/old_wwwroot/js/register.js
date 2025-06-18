async function register() {
    const username = document.getElementById("usernameInput").value.trim();
    const password = document.getElementById("passwordInput").value.trim();
    const email = document.getElementById("emailInput").value.trim();

    const userData = {
        Username: username,
        Password: password
    };

    if (email) userData.Email = email;

    const response = await fetch("/api/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData)
    });

    if (response.ok) {
        alert("Registration successful. You can now login.");
        window.location.href = "login.html";
    } else {
        alert("Username already taken.");
    }
}
