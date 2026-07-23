import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-todo-list',
  templateUrl: `./todo-list.component.html`,
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent {
  public todos:Todo[]

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<any>(baseUrl + 'api/todos').subscribe(result => {
      this.todos = result;
    }, error => console.error(error));
  }
}

interface Todo
{
    id: number;
    text:string;
    isDone: boolean;
}
