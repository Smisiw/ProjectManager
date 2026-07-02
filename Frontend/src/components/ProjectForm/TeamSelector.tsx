import {
    Autocomplete,
    Chip,
    Stack,
    TextField
} from "@mui/material";

import { Controller, useFormContext } from "react-hook-form";
import { useState } from "react";

import { useEmployeeSearch } from "../../hooks/useEmployeeSearch.ts";
import { getEmployeeFullName } from "../../helpers/employee.ts";

import type { EmployeeResponse } from "../../types/employee.ts";
import type { ProjectFormState } from "./types";

export function TeamSelector() {

    const { control } =
        useFormContext<ProjectFormState>();

    const {
        employees,
        loading,
        setSearch
    } = useEmployeeSearch();

    const [inputValue, setInputValue] = useState("");

    return (

        <Controller

            name="selectedEmployees"

            control={control}

            render={({ field }) => (

                <Stack spacing={3}>

                    <Autocomplete<EmployeeResponse>

                        options={employees}

                        value={null}

                        inputValue={inputValue}

                        loading={loading}

                        getOptionLabel={getEmployeeFullName}

                        isOptionEqualToValue={(a, b) =>
                            a.id === b.id
                        }

                        filterOptions={options => options}

                        onInputChange={(_, value, reason) => {

                            if (reason === "input") {
                                setInputValue(value);
                                setSearch(value);
                            }

                        }}

                        onChange={(_, employee) => {

                            if (!employee) {
                                return;
                            }

                            if (field.value?.some(x => x.id === employee.id)) {
                                setInputValue("");
                                return;
                            }

                            field.onChange([
                                ...field.value ?? [],
                                employee
                            ]);

                            setInputValue("");
                            setSearch("");
                        }}

                        renderInput={(params) => (

                            <TextField
                                {...params}
                                label="Search employee"
                            />

                        )}

                    />

                    <Stack
                        sx={{
                            direction: "row",
                            spacing: 1,
                            gap: 2,
                            flexWrap: "wrap"
                        }}

                    >

                        {field.value?.map(employee => (

                            <Chip

                                key={employee.id}

                                label={getEmployeeFullName(employee)}

                                onDelete={() =>
                                    field.onChange(
                                        field.value?.filter(
                                            x => x.id !== employee.id
                                        )
                                    )
                                }

                            />

                        ))}

                    </Stack>

                </Stack>

            )}

        />

    );
}