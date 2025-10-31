import axios from 'axios';
import { Task } from '../types/Task';

const API_URL = 'https://localhost:7088/api/task';

export const taskService = {
    getAllTasks: async (): Promise<Task[]> => {
        const response = await axios.get(API_URL);
        return response.data;
    },

    createTask: async (description: string): Promise<Task> => {
        const response = await axios.post(API_URL, {
            description,
            isCompleted: false
        });
        return response.data;
    },

    updateTask: async (task: Task): Promise<Task> => {
        const response = await axios.put(`${API_URL}/${task.id}`, task);
        return response.data;
    },

    deleteTask: async (id: string): Promise<void> => {
        await axios.delete(`${API_URL}/${id}`);
    }
};