import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodoList } from '../models/TodoList';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {
  private apiUrl = 'https://localhost:5001/api/TodoList';

  constructor(private http: HttpClient) {}

  getTodoList(): Observable<TodoList[]> {
    return this.http.get<TodoList[]>(this.apiUrl);
  }

  getTodoListById(id: number): Observable<TodoList> {
    return this.http.get<TodoList>(`${this.apiUrl}/${id}`);
  }

  addTodoList(todo: TodoList): Observable<TodoList> {
    return this.http.post<TodoList>(this.apiUrl, todo);
  }

  updateTodoList(id: number, todo: TodoList): Observable<TodoList> {
    return this.http.put<TodoList>(`${this.apiUrl}/${id}`, todo);
  }

  deleteTodoList(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}