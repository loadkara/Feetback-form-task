import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-contact-form',
  imports: [ReactiveFormsModule],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.css'
})
export class ContactForm {
  feedbackForm: FormGroup;
  isSubmitted = false;
  submittedData: any;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.feedbackForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\+?\d+$/)]],
      topic: ['', Validators.required],
      message: ['', Validators.required],
      captcha: ['', Validators.required]
    });
  }

  onSubmit() {
  if (this.feedbackForm.valid) {
    const formData = this.feedbackForm.value;
    this.http.post('https://localhost:5001/api/feedback', formData)
      .subscribe({
        next: (response) => {
          alert('Форма успешно отправлена!');
          this.feedbackForm.reset();
        },
        error: (error) => {
          alert('Ошибка при отправке формы');
        }
      });
  } else {
    alert('Заполните все поля корректно');
  }
}
}
