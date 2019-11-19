import { FormGroup, FormBuilder, Validators } from "@angular/forms";

export class StatisticModel {

    xValues: string [];
    yValues: number [];

    public fromModel(): StatisticModel {
        this.xValues = [];
        this.yValues = [];
        return this;
    }

    buildForm(): FormGroup {
        return new FormBuilder().group({
            xValues: [{ value: this.xValues, disabled: false }, [Validators.required]],
            yValues: [{ value: this.yValues, disabled: false }, [Validators.required]]
        });
    }
}

