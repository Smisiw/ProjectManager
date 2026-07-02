import { useEffect, useState } from "react";

import { getEmployees } from "../api/employees";
import type { EmployeeResponse } from "../types/employee";

export function useEmployeeSearch() {
    const [employees, setEmployees] = useState<EmployeeResponse[]>([]);
    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState("");

    useEffect(() => {
        const timeout = setTimeout(async () => {
            setLoading(true);

            try {
                const data = await getEmployees(search);

                setEmployees(data)
            }
            finally {
                setLoading(false);
            }
        }, 300);

        return () => clearTimeout(timeout);
    }, [search]);

    return {
        employees,
        loading,
        setSearch
    };
}