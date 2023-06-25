"use strict";
class TodoApp {
    constructor() {
        this.tasks = [];
        this.filter = 'all';

        this.taskInput = document.getElementById('taskInput');
        this.addButton = document.getElementById('addButton');
        this.todoList = document.getElementById('todoList');
        this.doneList = document.getElementById('doneList');
        this.allButton = document.getElementById('allButton');
        this.todoButton = document.getElementById('todoButton');
        this.doneButton = document.getElementById('doneButton');

        this.addButton.addEventListener('click', () => this.addTask());
        this.allButton.addEventListener('click', () => this.setFilter('all'));
        this.todoButton.addEventListener('click', () => this.setFilter('todo'));
        this.doneButton.addEventListener('click', () => this.setFilter('done'));

        this.renderTasks();
    }

    addTask() {
        const task = this.taskInput.value.trim();
        if (task !== '') {
            this.tasks.push({ text: task, done: false });
            this.renderTasks();
            this.taskInput.value = '';
        }
    }

    removeTask(index) {
        this.tasks.splice(index, 1);
        this.renderTasks();
    }

    toggleTaskDone(index) {
        this.tasks[index].done = !this.tasks[index].done;
        this.renderTasks();
    }

    setFilter(filter) {
        this.filter = filter;
        this.renderTasks();
    }

    renderTasks() {
        this.clearList(this.todoList);
        this.clearList(this.doneList);

        const filteredTasks = this.filterTasks();

        filteredTasks.forEach((task, index) => {
            const listItem = document.createElement('li');
            listItem.textContent = task.text;
            listItem.classList.add('flex', 'items-center', 'justify-between', 'border-b', 'py-2', 'px-4');

            const buttonContainer = document.createElement('div');

            if (!task.done) {
                const doneButton = document.createElement('button');
                doneButton.textContent = 'Done';
                doneButton.classList.add('text-black', 'px-4', 'py-2');
                doneButton.addEventListener('click', () => this.toggleTaskDone(index));
                buttonContainer.appendChild(doneButton);
            }

            const removeButton = document.createElement('button');
            removeButton.textContent = 'Remove';
            removeButton.classList.add('text-black', 'px-4', 'py-2');
            removeButton.addEventListener('click', () => this.removeTask(index));
            buttonContainer.appendChild(removeButton);

            listItem.appendChild(buttonContainer);

            if (task.done) {
                listItem.classList.add('line-through', 'text-black');
                this.doneList.appendChild(listItem);
            } else {
                this.todoList.appendChild(listItem);
            }
        });
    }

    clearList(list) {
        while (list.firstChild) {
            list.firstChild.remove();
        }
    }

    filterTasks() {
        if (this.filter === 'all') {
            return this.tasks;
        } else if (this.filter === 'todo') {
            return this.tasks.filter(task => !task.done);
        } else {
            return this.tasks.filter(task => task.done);
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new TodoApp();
});