import {
    Autocomplete,
    Stack,
    TextField
} from "@mui/material";

import { Controller, useFormContext } from "react-hook-form";

import { useEmployeeSearch } from "../../hooks/useEmployeeSearch.ts";
import { getEmployeeFullName } from "../../helpers/employee.ts";

import type { EmployeeResponse } from "../../types/employee.ts";
import type { ProjectFormState } from "./types.ts";

export function ManagerSelector() {

    const {
        control,
        formState: { errors }
    } = useFormContext<ProjectFormState>();

    const {
        employees,
        loading,
        setSearch
    } = useEmployeeSearch();

    return (
        <Stack spacing={3}>

            <Controller
                name="manager"
                control={control}
                rules={{
                    required: "Project manager is required"
                }}
                render={({ field }) => (

                    <Autocomplete<EmployeeResponse>

                        options={employees}

                        value={field.value}

                        loading={loading}

                        onChange={(_, employee) =>
                            field.onChange(employee)
                        }

                        onInputChange={(_, value, reason) => {

                            if (reason === "input") {
                                setSearch(value);
                            }

                        }}

                        getOptionLabel={getEmployeeFullName}

                        isOptionEqualToValue={(a, b) =>
                            a.id === b.id
                        }

                        renderInput={(params) => (

                            <TextField
                                {...params}
                                label="Project manager"
                                error={!!errors.manager}
                                helperText={errors.manager?.message}
                            />

                        )}

                    />

                )}
            />

        </Stack>
    );
}