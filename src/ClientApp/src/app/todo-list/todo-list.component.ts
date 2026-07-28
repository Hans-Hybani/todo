import { Component, OnInit } from '@angular/core';
import { TodoListService } from '../services/TodoList.service';
import { TodoList } from '../models/TodoList';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {

  todos: TodoList[] = [];
  selectedTodoId: number | null = null;

  constructor(
    private todoService: TodoListService
  ) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.todoService.getTodoList().subscribe((data: TodoList[]) => {
      this.todos = data;
    });
  }

  // Mark a to-do item as “selected” to reveal the delete button
  onCheck(todo: TodoList): void {
    this.selectedTodoId = todo.id;
  }

  // Deletes a to-do item and reloads the list
  onDelete(todo: TodoList): void {
    this.todoService.deleteTodoList(todo.id).subscribe(() => {
      this.selectedTodoId = null;
      this.loadTodos();
    });
  }
}