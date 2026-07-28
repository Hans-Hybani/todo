import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TodoListService } from '../services/TodoList.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-todo-list',
  templateUrl: './create-todo-list.component.html',
  styleUrls: ['./create-todo-list.component.css']
})
export class CreateTodoListComponent implements OnInit {
  todoForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private todoService: TodoListService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.todoForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      dueDate: [''],
      notes: ['', [Validators.maxLength(2000)]]
    });
  }

  onSubmit(): void {
    // Prevents submission if the form is invalid (client-side validation).
    if (this.todoForm.invalid) {
      this.todoForm.markAllAsTouched();
      return;
    }
    this.todoService.addTodoList(this.todoForm.value).subscribe(() => {
      this.todoForm.reset({
        isDone: false
      });
      this.router.navigate(['/todo-list']);
    });
  }
}
