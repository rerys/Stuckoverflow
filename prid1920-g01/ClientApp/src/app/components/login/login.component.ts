import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthenticationService } from '../../services/authentication.service';

@Component({ templateUrl: 'login.component.html' })

export class LoginComponent implements OnInit, AfterViewInit {

    loginForm: FormGroup;

    loading = false;    // utilisé en HTML pour désactiver le bouton pendant la requête de login

    submitted = false;  // retient si le formulaire a été soumis ; utilisé pour n'afficher les 

                        // erreurs que dans ce cas-là (voir template)

    returnUrl: string;

    error = '';

    @ViewChild('pseudo', { static: true }) pseudo: ElementRef;

    constructor(

        private formBuilder: FormBuilder,

        private route: ActivatedRoute,

        private router: Router,

        private authenticationService: AuthenticationService

    ) {

        // redirect to home if already logged in

        if (this.authenticationService.currentUser) {

            this.router.navigate(['/']);

        }

    }

    ngOnInit() {

        this.loginForm = this.formBuilder.group({

            pseudo: ['', Validators.required],

            password: ['', Validators.required]

        });

        // get return url from route parameters or default to '/'

        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    }

    ngAfterViewInit() {

        setTimeout(_ => this.pseudo && this.pseudo.nativeElement.focus());

    }

    // On définit ici un getter qui permet de simplifier les accès aux champs du formulaire dans le HTML

    get f() { return this.loginForm.controls; }


    onSubmit() {

        this.submitted = true;

        // on s'arrête si le formulaire n'est pas valide

        if (this.loginForm.invalid) return;

        this.loading = true;

        this.authenticationService.login(this.f.pseudo.value, this.f.password.value)

            .subscribe(

                // si login est ok, on navigue vers la page demandée

                data => {

                    this.router.navigate([this.returnUrl]);

                },

                // en cas d'erreurs, on reste sur la page et on les affiche

                error => {

                    console.log(error);

                    this.error = error.error.errors.Pseudo || error.error.errors.Password;

                    this.loading = false;

                });

    }

}