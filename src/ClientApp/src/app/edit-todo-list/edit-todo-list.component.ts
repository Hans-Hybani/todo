import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TodoListService } from '../services/TodoList.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-todo-list',
  templateUrl: './edit-todo-list.component.html',
  styleUrls: ['./edit-todo-list.component.css']
})
export class EditTodoListComponent implements OnInit {
  todoForm!: FormGroup;
  todoId!: number;

  constructor(
    private fb: FormBuilder,
    private todoService: TodoListService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Retrieves the ID from the URL (/edit-todo/:id) to determine which todo to load.
    this.todoId = Number(this.route.snapshot.paramMap.get('id'));

    // IsDone is not present because it is not necessary to mark a task as complete when editing it.
    this.todoForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      dueDate: [''],
      notes: ['', [Validators.maxLength(2000)]]
    });

    // Pre-fills the form with the current values from the todo.
    this.todoService.getTodoListById(this.todoId).subscribe(todo => {
      this.todoForm.patchValue({
        ...todo,
        dueDate: todo.dueDate ? todo.dueDate.toString().substring(0, 10) : ''
      });
    });
  }

  onSubmit(): void {
    // Prevents submission if the form is invalid and displays error messages
    if (this.todoForm.invalid) {
      this.todoForm.markAllAsTouched();
      return;
    }
    this.todoService.updateTodoList(this.todoId, this.todoForm.value).subscribe(() => {
      this.router.navigate(['/todo-list']);
    });
  }
  
  // Discards the changes without saving and returns directly to the list. (Cancel button)
  onCancel(): void {
    this.router.navigate(['/todo-list']);
  }
}