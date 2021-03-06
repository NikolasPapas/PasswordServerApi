import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Guid } from "../../../../../common/types/guid";
import { PasswordForm } from '../password-editor/password-form.model';
import { Sex } from 'src/app/models/enums/sex-enum';
import { Password } from 'src/app/models/password-model';
import { Account } from 'src/app/models/account-model';

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
    //currentToken: string;
    passwords: Password[]

    public fromModel(account: Account): AccountForm {
        if (account != null) {
            if (account.accountId) this.accountId = account.accountId;
            if (account.firstName) this.firstName = account.firstName;
            if (account.lastName) this.lastName = account.lastName;
            if (account.userName) this.userName = account.userName;
            if (account.email) this.email = account.email;
            if (account.password) this.password = account.password;
            this.sex = account.sex;
            if (account.lastLogIn) this.lastLogIn = account.lastLogIn;
            if (account.passwords) this.passwords = account.passwords;
            if (account.role) this.role = account.role;
        }
        return this;
    }

    buildForm(): FormGroup {
        var form = new FormBuilder().group({
            accountId: [{ value: this.accountId, disabled: false }, [Validators.required]],
            firstName: [{ value: this.firstName, disabled: false }, [Validators.required]],
            lastName: [{ value: this.lastName, disabled: false }, [Validators.required]],
            userName: [{ value: this.userName, disabled: this.userName != null && this.userName != "" ? true : false }, [Validators.required]],
            email: [{ value: this.email, disabled: false }, [Validators.required]],
            sex: [{ value: this.sex, disabled: false }, [Validators.required]],
            lastLogIn: [{ value: this.lastLogIn, disabled: true }, [Validators.required]],
            password: [{ value: this.password, disabled: false }, [Validators.required]],
            role: [{ value: this.role, disabled: this.role !=null ? true : false }, [Validators.required]],
            //currentToken: [{ value: this.currentToken, disabled: false }, [Validators.required]],
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

