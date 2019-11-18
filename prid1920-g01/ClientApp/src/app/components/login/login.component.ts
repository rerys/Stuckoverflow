import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AuthenticationService } from '../../services/authentication.service';
import { EditUserComponent } from '../edit-user/edit-user.component';
import { User } from 'src/app/models/user';
import { MatDialog, MatSnackBar } from '@angular/material';
import { UserService } from 'src/app/services/user.service';

@Component({

    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']

})

export class LoginComponent implements OnInit {

    loginForm: FormGroup;
    loading = false;    // utilisé en HTML pour désactiver le bouton pendant la requête de login
    submitted = false;  // retient si le formulaire a été soumis ; utilisé pour n'afficher les 

    // erreurs que dans ce cas-là (voir template)
    returnUrl: string;
    ctlPseudo: FormControl;
    ctlPassword: FormControl;


    constructor(

        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        public dialog: MatDialog,
        private userService: UserService,
        public snackBar: MatSnackBar

    ) {

        // redirect to home if already logged in
        if (this.authenticationService.currentUser) { this.router.navigate(['/']); }

    }

    ngOnInit() {

        this.ctlPseudo = this.formBuilder.control('', Validators.required);
        this.ctlPassword = this.formBuilder.control('', Validators.required);

        this.loginForm = this.formBuilder.group({
            pseudo: this.ctlPseudo,
            password: this.ctlPassword
        });

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    }

    // On définit ici un getter qui permet de simplifier les accès aux champs du formulaire dans le HTML
    get f() { return this.loginForm.controls; }

    onSubmit() {

        this.submitted = true;

        // on s'arrête si le formulaire n'est pas valide
        if (this.loginForm.invalid) return;

        this.loading = true;
        this.login(this.f.pseudo.value, this.f.password.value);


    }

    login(pseudo: string, password: string) {

        this.loading = true;

        this.authenticationService.login(pseudo, password)

            .subscribe(

                // si login est ok, on navigue vers la page demandée

                data => {

                    this.router.navigate([this.returnUrl]);

                },

                // en cas d'erreurs, on reste sur la page et on les affiche

                error => {

                    const errors = error.error.errors;

                    for (let field in errors) {

                        this.loginForm.get(field.toLowerCase()).setErrors({ custom: errors[field] })

                    }

                    this.loading = false;

                });


    }

    signUp() {
        const user = new User({});

        const dlg = this.dialog.open(EditUserComponent, { data: { user, isNew: true, signup: true } });

        dlg.beforeClose().subscribe(res => {

            if (res) {

                this.userService.addNewMember(res).subscribe(res2 => {

                    if (!res2) {

                        this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });

                    } else if (res2) {

                        this.login(res.pseudo, res.password);


                    }

                });

            }

        });
    }

}