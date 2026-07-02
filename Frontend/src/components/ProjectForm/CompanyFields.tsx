import { Controller, useFormContext } from "react-hook-form";

import {
    Stack,
    TextField
} from "@mui/material";

import type { ProjectFormState } from "./types";

interface Props {
    disabled?: boolean;
}

export function CompanyFields({
                                  disabled = false
                              }: Props) {

    const {
        control,
        formState: { errors }
    } = useFormContext<ProjectFormState>();

    return (
        <Stack spacing={3}>

            <Controller
                name="customerCompany"
                control={control}
                rules={{
                    required: "Customer company is required"
                }}
                render={({ field }) => (
                    <TextField
                        {...field}
                        label="Customer company"
                        fullWidth
                        disabled={disabled}
                        error={!!errors.customerCompany}
                        helperText={errors.customerCompany?.message}
                    />
                )}
            />

            <Controller
                name="executorCompany"
                control={control}
                rules={{
                    required: "Executor company is required"
                }}
                render={({ field }) => (
                    <TextField
                        {...field}
                        label="Executor company"
                        fullWidth
                        disabled={disabled}
                        error={!!errors.executorCompany}
                        helperText={errors.executorCompany?.message}
                    />
                )}
            />

        </Stack>
    );
}