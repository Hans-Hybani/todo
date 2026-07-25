import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TodoListService } from '../services/TodoList.service';
import { TodoList } from '../models/TodoList';

@Component({
  selector: 'app-todo-list',
  templateUrl: `./todo-list.component.html`,
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent {

  todos: TodoList[];

  constructor(
    private todoService: TodoListService
  ) {}

}

