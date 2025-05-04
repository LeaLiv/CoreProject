const uri = '/users';
let users = [];
let token = '';

function getUsers() {
    token = localStorage.getItem("token");
    if (!token) {
        alert("You must log in first");
        window.location.href = "index.html";
        return;
    }

    fetch(uri, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            "Authorization": `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

const addUser = () => {
    console.log("in addUser");
    const addName = document.getElementById('add-name').value.trim();
    const addEmail = document.getElementById('add-email').value.trim();
    const adduserName = document.getElementById('add-userName').value.trim();
    const addRole = document.getElementById('add-role').value;
    const addPassword = document.getElementById('add-password').value.trim();

    const user = {
        id: users.length + 1,
        name: addName,
        email: addEmail,
        userName: adduserName,
        role: addRole,
        password: addPassword
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            document.getElementById('add-name').value = "";
            document.getElementById('add-email').value = "";
            document.getElementById('add-userName').value = "";
            document.getElementById('add-role').value = "User";
            document.getElementById('add-password').value = "";
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            "Authorization": `Bearer ${token}`
        }
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(user => user.id === id);

    document.getElementById('edit-Id').value = user.id;
    document.getElementById('edit-Name').value = user.name;
    document.getElementById('edit-Email').value = user.email;
    document.getElementById('edit-userName').value = user.userName;
    document.getElementById('edit-Role').value = user.role || "user";
    document.getElementById('edit-Password').value = user.password;

    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-Id').value;

    const user = {
        id: parseInt(userId, 10),
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
        body: JSON.stringify(user)
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to update user.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}
function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Users' : 'Types of users';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    _displayCount(data.length);
    const button = document.createElement('button');
    data.forEach(user => {
        console.log(data);
        
        let editButton = button.cloneNode(false);

        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);

        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        td1.appendChild(document.createTextNode(user.name));

        let td2 = tr.insertCell(1);
        td2.appendChild(document.createTextNode(user.email));

        let td3 = tr.insertCell(2);
        td3.appendChild(document.createTextNode(user.userName));

        let td4 = tr.insertCell(3);
        td4.appendChild(document.createTextNode(user.role));

        let td5 = tr.insertCell(4);
        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);
        td5.appendChild(deleteButton);

        let td6 = tr.insertCell(5);
        td6.appendChild(editButton);
    });

    users = data;
}

const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "index.html";
};
