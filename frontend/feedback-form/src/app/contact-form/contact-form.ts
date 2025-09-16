import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-contact-form',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.css'
})
export class ContactForm {
  feedbackForm: FormGroup;
  result: any = null;
  captchaCode = '0096'; 

  topics = [
    { id: '1', name: 'Техподдержка' },
    { id: '2', name: 'Продажи' },
    { id: '3', name: 'Другое' }
  ];

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.feedbackForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\+?\d+$/)]],
      topicId: ['', Validators.required],
      messageText: ['', Validators.required],
      captcha: ['', Validators.required]
    });
  }

  getTopicName(id: string): string {
    const topic = this.topics.find(t => t.id === id);
    return topic ? topic.name : 'Неизвестно';
  }

  onSubmit() {
    if (this.feedbackForm.invalid) {
      alert('Заполните все поля корректно');
      return;
    }

    const dto = this.feedbackForm.value;

    if (dto.captcha !== this.captchaCode) {
      alert('Неверная CAPTCHA');
      return;
    }

    const { captcha, ...payload } = dto;

    this.http.post('https://feedback-api-a8xh.onrender.com/api/feedback', payload)
      .subscribe({
        next: (response: any) => {
          console.log('Успешно отправлено', response);
          this.result = response;
          this.feedbackForm.reset();
        },
        error: (error) => {
          console.error('Ошибка при отправке', error);
          alert('Ошибка при отправке формы. Попробуйте позже.');
        }
      });
  }
}