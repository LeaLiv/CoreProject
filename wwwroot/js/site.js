const uri = '/shoes';
let shoeses = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

const addItem=()=> {
    console.log("in addItem");
    // const addCode = document.getElementById('add-code');
    const addSize = document.getElementById('add-size');
    const addCompany = document.getElementById('add-company');
    const addColor = document.getElementById('add-color');


    const item = {
        code: shoeses.length + 1,    
        size: addSize.value,
        company: addCompany.value.trim(),
        color: addColor.value.trim()
    };
    console.log(item);

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addCode.value = "";
            addSize.value = "";
            addCompany.value = "";
            addColor.value = "";
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(code) {
    fetch(`${uri}/${code}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(code) {
    const item = shoeses.find(item => item.code === code);

    document.getElementById('edit-Code').value = item.code;
    document.getElementById('edit-Size').value = item.size;
    document.getElementById('edit-Company').value = item.company;
    document.getElementById('edit-Color').value = item.color;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemCode = document.getElementById('edit-Code').value;
    const item = {
        code: parseInt(itemCode, 10),
        size: document.getElementById('edit-Company').value.trim(),
        company: document.getElementById('edit-Size').value,
        color: document.getElementById('edit-Color').value
    };

    fetch(`${uri}/${itemCode}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'shoes' : 'Types of shoes';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('shoeses');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.code})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.code})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeSize= document.createTextNode(item.size);
        td1.appendChild(textNodeSize);

        let td2=tr.insertCell(1);
        let textNodeCompany= document.createTextNode(item.company);
        td2.appendChild(textNodeCompany);

        let td3=tr.insertCell(2);
        td3.style.backgroundColor = item.color;

        let td5 = tr.insertCell(3);
        td5.appendChild(editButton);

        let td6 = tr.insertCell(4);
        td6.appendChild(deleteButton);
    });

    shoeses = data;
}