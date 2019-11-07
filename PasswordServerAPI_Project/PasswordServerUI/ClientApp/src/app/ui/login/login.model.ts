import { FormGroup, FormBuilder, Validators } from "@angular/forms";


export class LoginModel {

    username: string;
    password: string;

    public fromModel(): LoginModel {
       this.username="";
       this.password =""
        return this;
    }

    buildForm(): FormGroup {
        return new FormBuilder().group({
            username: [{ value: this.username, disabled: false }, [Validators.required]],
            password: [{ value: this.password, disabled: false }, [Validators.required]]
        });
    }
}

