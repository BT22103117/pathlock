import axios from 'axios';
import { Project, Task, User, RegisterForm, LoginForm } from '../types';

const API_URL = 'https://localhost:7088/api';

axios.interceptors.request.use((config) => {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if (user.token) {
        config.headers.Authorization = `Bearer ${user.token}`;
    }
    return config;
});

export const api = {
    // Auth
    register: async (data: RegisterForm): Promise<User> => {
        const response = await axios.post(`${API_URL}/auth/register`, data);
        localStorage.setItem('user', JSON.stringify(response.data));
        return response.data;
    },

    login: async (data: LoginForm): Promise<User> => {
        const response = await axios.post(`${API_URL}/auth/login`, data);
        localStorage.setItem('user', JSON.stringify(response.data));
        return response.data;
    },

    logout: () => {
        localStorage.removeItem('user');
    },

    // Projects
    getProjects: async (): Promise<Project[]> => {
        const response = await axios.get(`${API_URL}/projects`);
        return response.data;
    },

    getProject: async (id: string): Promise<Project> => {
        const response = await axios.get(`${API_URL}/projects/${id}`);
        return response.data;
    },

    createProject: async (title: string, description?: string): Promise<Project> => {
        const response = await axios.post(`${API_URL}/projects`, { title, description });
        return response.data;
    },

    updateProject: async (id: string, title: string, description?: string): Promise<Project> => {
        const response = await axios.put(`${API_URL}/projects/${id}`, { title, description });
        return response.data;
    },

    deleteProject: async (id: string): Promise<void> => {
        await axios.delete(`${API_URL}/projects/${id}`);
    },

    // Tasks
    getProjectTasks: async (projectId: string): Promise<Task[]> => {
        const response = await axios.get(`${API_URL}/tasks/project/${projectId}`);
        return response.data;
    },

    createTask: async (projectId: string, title: string, dueDate?: string): Promise<Task> => {
        const response = await axios.post(`${API_URL}/tasks/project/${projectId}`, { title, dueDate });
        return response.data;
    },

    updateTask: async (id: string, title: string, isCompleted: boolean, dueDate?: string): Promise<Task> => {
        const response = await axios.put(`${API_URL}/tasks/${id}`, { title, isCompleted, dueDate });
        return response.data;
    },

    deleteTask: async (id: string): Promise<void> => {
        await axios.delete(`${API_URL}/tasks/${id}`);
    }
};