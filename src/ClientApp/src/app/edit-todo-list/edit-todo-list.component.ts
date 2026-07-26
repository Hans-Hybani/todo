import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
    this.todoId = Number(this.route.snapshot.paramMap.get('id'));

    this.todoForm = this.fb.group({
      title: [''],
      dueDate: [''],
      notes: ['']
    });

    this.todoService.getTodoListById(this.todoId).subscribe(todo => {
      this.todoForm.patchValue(todo);
    });
  }

  onSubmit(): void {
    this.todoService.updateTodoList(this.todoId, this.todoForm.value).subscribe(() => {
      this.router.navigate(['/todo-list']);
    });
  }
}