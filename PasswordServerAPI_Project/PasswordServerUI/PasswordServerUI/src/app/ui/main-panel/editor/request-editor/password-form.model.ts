import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Guid } from "../../../../common/types/guid";
import { Sex } from "../../../../core/models/enums/sex-enum";
import { Password } from "../../../../core/models/password-model";
import { Sensitivity } from "../../../../core/models/enums/sensitivity";
import { Strength } from "../../../../core/models/enums/strength";

export class PasswordForm {
    passwordId: Guid;
    name: string;
    userName: string;
    password: string;
    logInLink: string;
    sensitivity: Sensitivity;
    strength: Strength;

    public fromModel(password: Password): PasswordForm {
        if (password != null) {
            if (password.passwordId) this.passwordId = password.passwordId;  
            if (password.name) this.name = password.name; 
            if (password.userName) this.userName = password.userName; 
            if (password.password) this.password = password.password; 
            if (password.logInLink) this.logInLink = password.logInLink; 
            if (password.sensitivity) this.sensitivity = password.sensitivity; 
            if (password.strength) this.strength = password.strength; 
        }else{
            this.passwordId = Guid.create();
            this.name = "";
            this.userName = "";
            this.password = "";
            this.logInLink = "";
            this.sensitivity = Sensitivity.ForEveryone;
            this.strength = Strength.VeryWeak;
        }
        return this;
    }

    buildForm(): FormGroup {
        return new FormBuilder().group({
            passwordId: [{ value: this.passwordId, disabled: true }, [Validators.required]],
            name: [{ value: this.name, disabled: true }, [Validators.required]],
            userName: [{ value: this.userName, disabled: true }, [Validators.required]],
            password: [{ value: this.password, disabled: true }, [Validators.required]],
            logInLink: [{ value: this.logInLink, disabled: true }, [Validators.required]],
            sensitivity: [{ value: this.sensitivity, disabled: true }, [Validators.required]],
            strength: [{ value: this.strength, disabled: true }, [Validators.required]],
        });
    }
}

