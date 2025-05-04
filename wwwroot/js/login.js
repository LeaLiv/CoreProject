

const uri = 'https://localhost:7116/Users'
const checkDetails = async (event) => {
    event.preventDefault();
    const userName = document.getElementById('userName').value;
    const password = document.getElementById('password').value;
    const loginuser = {
        "userName": userName,
        "Password": password
    }
    const response = await fetch(`${uri}/Login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginuser)
    })

    // console.log(loginuser);
    if (response.ok) {
        const token = await response.text();
        localStorage.setItem("token", token);
        window.location.href = 'show.html'
    }
    if (response.status == 401) {
        alert("wrong details try again");
        window.location.reload;
    }


}
document.addEventListener("DOMContentLoaded", function () {
    token = localStorage.getItem("token");   
    if (!token) {
        console.log("אין טוקן – מעבר לעמוד התחברות");
        return;
    }
    const payload = JSON.parse(atob(token.split('.')[1]));
    const exp = payload["exp"];
    const expirationDate = new Date(exp * 1000); 
    const now = new Date(); 

    console.log(`תאריך תפוגה: ${expirationDate}`); 
    if (now >= expirationDate) {
        console.log("⚠️ הטוקן פג – מוחקים ומעבירים לעמוד התחברות");
        localStorage.removeItem("token"); 

    } else {
        console.log("✅ הטוקן בתוקף – מעבר לעמוד הרצוי");
        window.location.href = 'show.html';
    }
});


