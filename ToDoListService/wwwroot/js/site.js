const uri = 'api/todoitems';

function initializeView() {
    fetch(uri)
        .then(response => response.json())
        .then(data => data.forEach(it => renderElement(it)))
        .catch(error => console.error('Unable to get items.', error));
}

function deleteItem(id) {
    return fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
}

function updateItem(id, name, isComplete) {
    const item = {
        id: id,
        isComplete: isComplete,
        name: name
    };

    return fetch(`${uri}/${id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
}

function addItem() {
    const itemName = document.getElementById("todoInput").value;

    const item = {
        isComplete: false,
        name: itemName.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(async response => {
            if (response.ok) {
                return response.json();
            } else {
                const body = await response.json();
                throw new Error(body);
            }
        })
        .then(response => {
            renderElement(response);
            document.getElementById("todoInput").value = "";
        })
        .catch(error => alert('Unable to add item. ' + error.message));
}

function renderElement(todoItem) {
    const todoItemId = todoItem.id;
    const todoItemName = todoItem.name;
    const todoItemIsCompleted = todoItem.isComplete;

    const li = document.createElement("li");
    li.appendChild(document.createTextNode(todoItemName));
    document.getElementById("list").appendChild(li);

    // isCompleted Logic
    if (todoItemIsCompleted) {
        li.classList.toggle('checked');
    }
    li.onclick = function () {
        const checked = li.classList.contains('checked');
        updateItem(todoItemId, todoItemName, !checked)
            .then(() => li.classList.toggle('checked'))
            .catch(error => console.error('Unable to update item.', error));
    }

    // Delete button
    const span = document.createElement("SPAN");
    const txt = document.createTextNode("\u00D7");
    span.className = "close";
    span.appendChild(txt);
    span.onclick = function () {
        deleteItem(todoItemId)
            .then(() => {
                const div = this.parentElement;
                div.style.display = "none";
            })
            .catch(error => console.error('Unable to delete item.', error));

    }
    li.appendChild(span);
}
