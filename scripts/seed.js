const API_URL = 'http://localhost:3000/api';

process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

const mockEmployees = [
    { firstName: "Ivan", lastName: "Ivanov", middleName: "Ivanovich", email: "ivanov@sibers.com" },
    { firstName: "Petr", lastName: "Petrov", middleName: "Petrovich", email: "petrov@sibers.com" },
    { firstName: "Anna", lastName: "Smirnova", middleName: "Alexandrovna", email: "smirnova@sibers.com" },
    { firstName: "Dmitry", lastName: "Kuznetsov", middleName: "Sergeevich", email: "kuznetsov@sibers.com" },
    { firstName: "Elena", lastName: "Popova", middleName: "Vasilievna", email: "popova@sibers.com" }
];

const mockProjectsTemplates = [
    {
        name: "Project Management System",
        customerCompany: "Sibers Alpha",
        executorCompany: "Sibers Team",
        startDate: "2026-01-10",
        endDate: "2026-06-30",
        priority: 3
    },
    {
        name: "E-Commerce Platform Redesign",
        customerCompany: "Retail Giant Corp",
        executorCompany: "Sibers Dev",
        startDate: "2026-03-01",
        endDate: "2026-12-25",
        priority: 2
    },
    {
        name: "Mobile Analytics Dashboard",
        customerCompany: "Fintech Startups Inc",
        executorCompany: "Sibers Mobile",
        startDate: "2026-05-15",
        endDate: "2026-09-15",
        priority: 1
    }
];

async function sendRequest(endpoint, method, data) {
    const response = await fetch(`${API_URL}${endpoint}`, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: data ? JSON.stringify(data) : undefined
    });

    if (!response.ok) {
        const errorText = await response.text();
        console.error(`Ошибка бэкенда на роуте ${endpoint}:`, errorText);
        throw new Error(`Код ответа: ${response.status}. Подробности выше.`);
    }

    if (response.status === 204) return null;
    return await response.json();
}

async function runDatabaseSeeder() {
    console.log("=== Старт заполнения тестовых данных из консоли ===");
    
    try {
        const createdEmployees = [];

        for (const emp of mockEmployees) {
            console.log(`Создание сотрудника: ${emp.lastName} ${emp.firstName}...`);
            const resData = await sendRequest('/employees', 'POST', emp);
            createdEmployees.push(resData); 
        }

        console.log(`Успешно создано сотрудников: ${createdEmployees.length}\n`);

        for (let i = 0; i < mockProjectsTemplates.length; i++) {
            const template = mockProjectsTemplates[i];
            
            const mngrId = createdEmployees[i % createdEmployees.length].id;
            const perfIds = [
                createdEmployees[(i + 1) % createdEmployees.length].id,
                createdEmployees[(i + 2) % createdEmployees.length].id
            ];

            const projectData = {
                ...template,
                managerId: String(mngrId),
                employeeIds: perfIds.map(id => String(id)) 
            };

            console.log(`Создание проекта: "${projectData.name}"...`);
            await sendRequest('/projects', 'POST', projectData);
        }

        console.log("\n=== Заполнение базы данных успешно завершено! ===");
    } catch (error) {
        console.error("\nСкрипт остановлен из-за ошибки:", error.message);
    }
}

runDatabaseSeeder();