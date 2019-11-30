import { FormControl, FormBuilder, Validators, FormGroup, ValidationErrors } from "@angular/forms";
import { Post } from "../models/post";
import { PostService } from "../services/post.service";

export class EditPostBase {

    //longueur minimum pour [title, body]
    private static MINLENGTH: number = 5;
    //longueur maximum pour [title,body]
    private static MAXLENGTH: number = 500;
    //controle regex [pseudo]
    private static REGEXPSEUDO: string = "^[a-zA-Z][a-zA-Z0-9_]*$";

    public frm: FormGroup;
    public ctlTitle: FormControl;
    public ctlBody: FormControl;

    constructor(
        public data: { post: Post; isNew: boolean; add: boolean; },
        protected PostService: PostService,
        protected fb: FormBuilder) {

        this.ctlTitle = this.validateTitle();
        this.ctlBody = this.validateBody();

        this.frm = this.fb.group({

            title: this.ctlTitle,
            body: this.ctlBody,

        });

        this.frm.patchValue(data.post);

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle title                                           //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateTitle() {
        return this.fb.control('', [

            Validators.required,
            Validators.minLength(EditPostBase.MINLENGTH),
            Validators.maxLength(EditPostBase.MAXLENGTH),
            Validators.pattern(EditPostBase.REGEXPSEUDO)

        ]);
    }

    // titleUsed(): any {

    //     let timeout: NodeJS.Timer;

    //     return (ctl: FormControl) => {
    //         clearTimeout(timeout);
    //         const title = ctl.value;

    //         return new Promise(resolve => {
    //             timeout = setTimeout(() => {
    //                 if (ctl.pristine) {
    //                     resolve(null);
    //                 } else {
    //                     this.PostService.getByTitle(title).subscribe(user => {
    //                         resolve(user ? { pseudoUsed: true } : null);
    //                     });
    //                 }
    //             }, 300);
    //         });
    //     };
    // }




    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //                                                                                                          //
    //                                    méthodes de contrôle body                                             //
    //                                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    validateBody() {
        return this.fb.control('', [

            Validators.required,
            Validators.minLength(EditPostBase.MINLENGTH),
            Validators.maxLength(EditPostBase.MAXLENGTH),
            Validators.pattern(EditPostBase.REGEXPSEUDO)

        ]);
    }
}
