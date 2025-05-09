const uri = '/users';
let user = null;
let token = '';
let payload = '';

function getProfile() {

    const userId = payload["id"];

    fetch(`${uri}/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => response.json())
    .then(data => {
        user = data;
        console.log(data.name);
        document.getElementById('name').textContent  = data.name;
        document.getElementById('userName').textContent  = data.userName;
        document.getElementById('email').textContent  = data.email;
        document.getElementById('role').textContent  = data.role;
    })
        // document.getElementById('password').textContent  = data.password;})
        
    .catch(error => console.error('Unable to get users.', error));
}

function displayEditForm() {
    console.log(user);
   
    document.getElementById('editForm').style.display = 'block'; 
    /*
    document.getElementById('edit-username').value = user.userName;
    document.getElementById('edit-email').value = user.email;
    document.getElementById('edit-role').value = user.role;
    document.getElementById('edit-password').value = user.password;*/
     document.getElementById('edit-Id').value = user.id;
    document.getElementById('edit-Name').value = user.name;
    document.getElementById('edit-Email').value = user.email;
    document.getElementById('edit-userName').value = user.userName;
    document.getElementById('edit-Role').value = user.role || "user";
    document.getElementById('edit-Password').value = user.password;

    const role=payload["type"];
    if(role=="user")
    { 
        const editRole =document.getElementById('edit-Role');
        editRole.disabled = true;
    }

    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = user.id;

    const editUser = {
        id: userId,
        name: document.getElementById('edit-Name').value.trim(),
        email: document.getElementById('edit-Email').value.trim(),
        userName: document.getElementById('edit-userName').value.trim(),
        role: document.getElementById('edit-Role').value,
        password: document.getElementById('edit-Password').value.trim()
    };

    fetch(`${uri}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(editUser)
    })
    .then(() => {
        getProfile();
    })   
    .then(() => closeInput()) 
        .catch(error => console.error('Unable to update user.', error));

    
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "index.html";

}
document.addEventListener("DOMContentLoaded", function () {
    token = localStorage.getItem("token");
    if (!token) {
        alert("You must log in first");
        window.location.href = "index.html";
        return;
    }
    payload = JSON.parse(atob(token.split('.')[1]));
    getProfile();

});