import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { JsonPipe } from '@angular/common';
import { TextInputComponent } from "../../../shared/components/text-input/text-input.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, MatCard, MatFormField, MatLabel, MatInput, MatButton, JsonPipe, MatError, TextInputComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  private form = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);
  private snackBar = inject(SnackbarService);

  registerForm= this.form.group({
    firstName: ['',Validators.required],
    lastName: ['',Validators.required],
    email: ['',[Validators.required, Validators.email]],
    password: ['',Validators.required],
  });

  validationErrors?: string[] = [];

  validationMessages = {
    length : "Le mot de passe doit faire 6 caractères minimum",
    digit : "Le mot de passe doit avoir au minimum un chiffre compris entre 0-9",
    uppercase : "Le mot de passe doit avoir au minimum une majuscule",
    character : "Le mot de passe doit avoir au moins un caractère spécial" 
  }

  onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe({
      next: () =>{
        this.snackBar.success("Inscription effectué !");
        this.router.navigateByUrl("/account/login");
      },
      error: (errors) => {
        this.validationErrors = errors;
      } 
    })
  }

  translatePasswordErrors(error : string){
    switch(error){
      case "Passwords must be at least 6 characters.":
        return this.validationMessages.length;
        break;
      case "Passwords must have at least one digit ('0'-'9').":
        return this.validationMessages.digit;
        break;
      case "Passwords must have at least one uppercase ('A'-'Z').":
        return this.validationMessages.uppercase;
        break;
      case "Passwords must have at least one non alphanumeric character.":
        return this.validationMessages.character;
        break;
      default : 
        return;
        break;
    } 
  };

  isPasswordError(){
    return this.validationErrors?.includes("Passwords must be at least 6 characters." || "Passwords must have at least one digit ('0'-'9')."
      || "Passwords must have at least one uppercase ('A'-'Z')." || "Passwords must have at least one non alphanumeric character.");
  }

}
