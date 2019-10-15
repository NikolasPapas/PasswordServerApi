import { FormGroup, FormBuilder, Validators } from "@angular/forms";


export class LoginModel {

    username: string;
    pass: string;

    public fromModel(): LoginModel {
       this.username="username";
       this.pass =""
        return this;
    }

    buildForm(): FormGroup {
        return new FormBuilder().group({
            username: [{ value: this.username, disabled: false }, [Validators.required]],
            pass: [{ value: this.pass, disabled: false }, [Validators.required]]
        });
    }
}

