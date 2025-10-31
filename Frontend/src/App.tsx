import React, { useEffect, useState } from 'react';
import { Task } from './types/Task';
import { TaskForm } from './components/TaskForm';
import { TaskList } from './components/TaskList';
import { taskService } from './services/taskService';

function App() {
    const [tasks, setTasks] = useState<Task[]>([]);

    useEffect(() => {
        loadTasks();
    }, []);

    const loadTasks = async () => {
        try {
            const tasks = await taskService.getAllTasks();
            setTasks(tasks);
        } catch (error) {
            console.error('Error loading tasks:', error);
        }
    };

    const handleCreateTask = async (description: string) => {
        try {
            const newTask = await taskService.createTask(description);
            setTasks([...tasks, newTask]);
        } catch (error) {
            console.error('Error creating task:', error);
        }
    };

    const handleToggleTask = async (task: Task) => {
        try {
            const updatedTask = await taskService.updateTask({
                ...task,
                isCompleted: !task.isCompleted
            });
            setTasks(tasks.map(t => t.id === updatedTask.id ? updatedTask : t));
        } catch (error) {
            console.error('Error updating task:', error);
        }
    };

    const handleDeleteTask = async (id: string) => {
        try {
            await taskService.deleteTask(id);
            setTasks(tasks.filter(t => t.id !== id));
        } catch (error) {
            console.error('Error deleting task:', error);
        }
    };

    return (
        <div className="container mx-auto px-4 py-8 max-w-2xl">
            <h1 className="text-3xl font-bold mb-6">Task Management</h1>
            <TaskForm onSubmit={handleCreateTask} />
            <TaskList
                tasks={tasks}
                onToggle={handleToggleTask}
                onDelete={handleDeleteTask}
            />
        </div>
    );
}

export default App;