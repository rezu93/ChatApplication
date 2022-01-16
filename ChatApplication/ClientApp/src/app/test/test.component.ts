import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@aspnet/signalr';

export class Message {
  content: string;
  author: string;
}

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  backendResponse: string;
  firstName: string;
  lastName: string;

  messages: Array<Message>;
  message: string

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.http.get("https://localhost:44364/" + "account" + "/getCurrentUser").subscribe(response => {
       this.firstName = (response as any).firstName
       this.lastName = (response as any).lastName

       this.refreshMessages();

       var connection = new signalR.HubConnectionBuilder()
         .configureLogging(signalR.LogLevel.Information)
         .withUrl("https://localhost:44364/message")
         .build()

       connection.start();

       connection.on("NewMessage", () => {
         this.refreshMessages();
       });

      },
        error => {

      });
  }

  refreshMessages() {
    this.http.get<Array<Message>>("https://localhost:44364/" + "controller" + "/getMessages").subscribe(response => {
      this.messages = response;
    },
      error => {
        this.backendResponse = error;
      });
  }

  sendRequestToBackend() {

    var message = new Message()
    message.content = this.message;
    message.author = this.firstName + " " + this.lastName;

    this.http.post<Message>("https://localhost:44364/" + "controller" + "/sendMessage", message).subscribe(response => {
      if (this.firstName == null) {
        return this.router.navigate(['/login']);
      }
      this.backendResponse = response.content;
    },
      error => {
        this.backendResponse = error;
      });
  }

}
