export interface Project {
    id: string;
    title: string;
    description?: string;
    createdAt: string;
    tasks: Task[];
}

export interface Task {
    id: string;
    title: string;
    dueDate?: string;
    isCompleted: boolean;
    projectId: string;
}

export interface User {
    username: string;
    token: string;
}

export interface RegisterForm {
    username: string;
    email: string;
    password: string;
}

export interface LoginForm {
    email: string;
    password: string;
}