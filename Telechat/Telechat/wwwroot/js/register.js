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
        window.location.href = "login.html";  // Redirect to login page after successful registration
    } else {
        alert("Username already taken.");
    }
}
