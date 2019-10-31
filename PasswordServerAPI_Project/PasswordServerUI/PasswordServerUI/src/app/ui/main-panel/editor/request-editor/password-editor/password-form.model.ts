import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Guid } from "../../../../../common/types/guid";
import { Sensitivity } from "../../../../../core/models/enums/sensitivity";
import { Strength } from "../../../../../core/models/enums/strength";
import { Password } from "../../../../../core/models/password-model";

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
            this.sensitivity = password.sensitivity;
            this.strength = password.strength;
        }
        return this;
    }

    buildForm(): FormGroup {
        return new FormBuilder().group({
            passwordId: [{ value: this.passwordId, disabled: true }, [Validators.required]],
            name: [{ value: this.name, disabled: false }, [Validators.required]],
            userName: [{ value: this.userName, disabled: false }, [Validators.required]],
            password: [{ value: this.password, disabled: false }, [Validators.required]],
            logInLink: [{ value: this.logInLink, disabled: false }, [Validators.required]],
            sensitivity: [{ value: this.sensitivity, disabled: false }, [Validators.required]],
            strength: [{ value: this.strength, disabled: false }, [Validators.required]],
        });
    }
}

