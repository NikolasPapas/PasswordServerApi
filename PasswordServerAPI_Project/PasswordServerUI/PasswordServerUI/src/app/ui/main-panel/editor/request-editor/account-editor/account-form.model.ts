import { FormGroup, FormBuilder, Validators, FormArray } from "@angular/forms";
import { Account } from "../../../../../core/models/account-model";
import { Guid } from "../../../../../common/types/guid";
import { Sex } from "../../../../../core/models/enums/sex-enum";
import { Password } from "../../../../../core/models/password-model";
import { PasswordForm } from '../password-editor/password-form.model';

export class AccountForm {
    accountId: Guid;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    sex: Sex;
    lastLogIn: string;

    password: string;
    role: string;
    currentToken: string;
    passwords: Password[]

    public fromModel(account: Account): AccountForm {
        if (account != null) {
            if (account.accountId) this.accountId = account.accountId;
            if (account.firstName) this.firstName = account.firstName;
            if (account.lastName) this.lastName = account.lastName;
            if (account.userName) this.userName = account.userName;
            if (account.email) this.email = account.email;
            if (account.sex) this.sex = account.sex;
            if (account.lastLogIn) this.lastLogIn = account.lastLogIn;
            if (account.passwords) this.passwords = account.passwords;
            if (account.role) this.role = account.role;
        } else {
            this.accountId = Guid.create();
            this.firstName = "";
            this.lastName = "";
            this.userName = "";
            this.email = "";
            this.sex = Sex.Male
            this.lastLogIn = "";
            this.password = "";
            this.role = "";
            this.currentToken = "";
            this.passwords = [];
        }
        return this;
    }

    buildForm(): FormGroup {
        var form = new FormBuilder().group({
            accountId: [{ value: this.accountId, disabled: false }, [Validators.required]],
            firstName: [{ value: this.firstName, disabled: false }, [Validators.required]],
            lastName: [{ value: this.lastName, disabled: false }, [Validators.required]],
            userName: [{ value: this.userName, disabled: false }, [Validators.required]],
            email: [{ value: this.email, disabled: false }, [Validators.required]],
            sex: [{ value: this.sex, disabled: false }, [Validators.required]],
            lastLogIn: [{ value: this.lastLogIn, disabled: false }, [Validators.required]],
            password: [{ value: this.password, disabled: false }, [Validators.required]],
            role: [{ value: this.role, disabled: false }, [Validators.required]],
            currentToken: [{ value: this.currentToken, disabled: false }, [Validators.required]],
        });

        let formArray: FormArray = new FormArray([]);
        if (this.passwords != null && this.passwords.length != 0)
            this.passwords.forEach(pass => {
                formArray.push(new PasswordForm().fromModel(pass).buildForm());
            })
        form.addControl('passwords', formArray);
        return form;
    }
}

