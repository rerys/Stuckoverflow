import { FormControl, FormBuilder, Validators, FormGroup, ValidationErrors } from "@angular/forms";
import { UserService } from "../services/user.service";
import { User, Role } from "../models/user";

export class EditUSerBase {

    //longueur minimum pour [pseudo, password, firstName, lastName]
    private static MINLENGTH: number = 3;
    //longueur maximum pour [pseudo, password]
    private static MAXLENGTH: number = 10;
    //longueur maximum pour [firstName, lastName]
    private static MAXLENGTHNAMES: number = 50;
    //controle regex [pseudo]
    private static REGEXPSEUDO: string = "^[a-zA-Z][a-zA-Z0-9_]*$";

    public frm: FormGroup;
    public ctlPseudo: FormControl;
    public ctlPassword: FormControl;
    public ctlControlPassword: FormControl;
    public ctlEmail: FormControl;
    public ctlFirstName: FormControl;
    public ctlLastName: FormControl;
    public ctlBirthDate: FormControl;
    public ctlRole: FormControl;
    public ctlReputation: FormControl;


    constructor(
        public data: { user: User; isNew: boolean; signup: boolean; },
        protected userService: UserService,
        protected fb: FormBuilder) {

        this.ctlPseudo = this.validatePseudo();
        this.ctlPassword = this.validatePassword();
        this.ctlControlPassword = this.validateControlPassword();
        this.ctlEmail = this.validateEmail();
        this.ctlFirstName = this.validateFirstName();
        this.ctlLastName = this.validateLastName();
        this.ctlBirthDate = this.fb.control('', [this.validateBirthDate()]);
        this.ctlRole = this.validateRole();
        this.ctlReputation = this.fb.control('', (!this.data.signup) ? [this.validateReputation()] : []);

        this.frm = this.fb.group({

            pseudo: this.ctlPseudo,
            password: this.ctlPassword,
            email: this.ctlEmail,
            lastName: this.ctlLastName,
            firstName: this.ctlFirstName,
            birthDate: this.ctlBirthDate,
            reputation: this.ctlReputation,
            role: this.ctlRole

        }, { validator: this.crossValidations });

        this.frm.patchValue(data.user);

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle pseudo                                           //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validatePseudo() {
        return this.fb.control('', [

            Validators.required,
            Validators.minLength(EditUSerBase.MINLENGTH),
            Validators.maxLength(EditUSerBase.MAXLENGTH),
            Validators.pattern(EditUSerBase.REGEXPSEUDO)

        ], [this.pseudoUsed()]);
    }

    pseudoUsed(): any {

        let timeout: NodeJS.Timer;

        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const pseudo = ctl.value;

            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if (ctl.pristine) {
                        resolve(null);
                    } else {
                        this.userService.getByPseudo(pseudo).subscribe(user => {
                            resolve(user ? { pseudoUsed: true } : null);
                        });
                    }
                }, 300);
            });
        };
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                   méthodes de contrôle password                                          //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validatePassword() {

        return this.fb.control('', (this.data.isNew && this.emptyPassword()) ? [

            Validators.required,
            Validators.minLength(EditUSerBase.MINLENGTH),
            Validators.maxLength(EditUSerBase.MAXLENGTH),

        ] : []);
    }


    validateControlPassword() {
        return this.fb.control('', (this.data.isNew && this.emptyPassword()) ? [
            Validators.required,
            this.checkPassword()] : []);
    }

    private emptyPassword() {
        return (ctl: FormControl) => {
            if (ctl.value == null) {
                return true;
            }
            return false;
        };
    }

    checkPassword() {
        return (ctl: FormControl) => {
            if (ctl.value != null && ctl.value != this.ctlPassword.value) {
                return { badPassword: true };
            }
            return null;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                     méthodes de contrôle email                                           //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateEmail() {
        return this.fb.control('', [

            Validators.required,
            Validators.email],
            [this.emailUsed()]);
    }

    emailUsed(): any {

        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const email = ctl.value;

            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if (ctl.pristine) {
                        resolve(null);
                    } else {
                        this.userService.getByEmail(email).subscribe(user => {
                            resolve(user ? { emailUsed: true } : null);
                        });
                    }
                }, 300);
            });
        };
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                   méthodes de contrôle firstname                                         //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateFirstName() {
        return this.fb.control('',
            [Validators.minLength(EditUSerBase.MINLENGTH),
            Validators.maxLength(EditUSerBase.MAXLENGTHNAMES)]);
    }

    crossValidations(group: FormGroup): ValidationErrors {
        if (!group.value) { return null; }
        if (group.value.firstName != null && group.value.lastName == null) {
            return { missLastName: true };
        }
        if (group.value.lastName != null && group.value.firstName == null) {
            return { missFirstName: true };
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle lastname                                         //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateLastName() {
        return this.fb.control('',

            [Validators.minLength(EditUSerBase.MINLENGTH),
            Validators.maxLength(EditUSerBase.MAXLENGTHNAMES)]);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle birthdate                                        //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateBirthDate(): any {

        return (ctl: FormControl) => {
            const date = new Date(ctl.value);
            const diff = Date.now() - date.getTime();
            if (diff < 0)
                return { futureBorn: true }
            var age = new Date(diff).getUTCFullYear() - 1970;
            if (age < 18)
                return { tooYoung: true };
            return null;
        };

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle reputation                                       //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateReputation(): any {
        return (ctl: FormControl) => {
            if (ctl.value < 1) {
                return { badReputation: true };
            }
            return null;
        };
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle role                                             //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateRole() {
        return this.fb.control(Role.Member, []);
    }

}