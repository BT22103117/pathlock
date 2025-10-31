import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { api } from '../services/api';
import { Project, Task } from '../types';

export const ProjectDetails: React.FC = () => {
    const [project, setProject] = useState<Project | null>(null);
    const [taskTitle, setTaskTitle] = useState('');
    const [taskDueDate, setTaskDueDate] = useState('');
    const [error, setError] = useState('');
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    useEffect(() => {
        if (id) loadProject();
    }, [id]);

    const loadProject = async () => {
        try {
            const data = await api.getProject(id!);
            setProject(data);
        } catch (err: any) {
            setError(err.response?.data || 'Error loading project');
        }
    };

    const handleCreateTask = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await api.createTask(id!, taskTitle, taskDueDate || undefined);
            setTaskTitle('');
            setTaskDueDate('');
            loadProject();
        } catch (err: any) {
            setError(err.response?.data || 'Error creating task');
        }
    };

    const handleToggleTask = async (task: Task) => {
        try {
            await api.updateTask(task.id, task.title, !task.isCompleted, task.dueDate);
            loadProject();
        } catch (err: any) {
            setError(err.response?.data || 'Error updating task');
        }
    };

    const handleDeleteTask = async (taskId: string) => {
        try {
            await api.deleteTask(taskId);
            loadProject();
        } catch (err: any) {
            setError(err.response?.data || 'Error deleting task');
        }
    };

    if (!project) return <div>Loading...</div>;

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-4xl mx-auto">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-3xl font-bold">{project.title}</h1>
                    <button
                        onClick={() => navigate('/dashboard')}
                        className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
                    >
                        Back to Dashboard
                    </button>
                </div>

                {project.description && (
                    <p className="text-gray-600 mb-8">{project.description}</p>
                )}

                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                        {error}
                    </div>
                )}

                <form onSubmit={handleCreateTask} className="mb-8">
                    <div className="grid gap-4">
                        <div>
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Task Title
                            </label>
                            <input
                                type="text"
                                value={taskTitle}
                                onChange={(e) => setTaskTitle(e.target.value)}
                                required
                                className="w-full px-3 py-2 border rounded"
                            />
                        </div>
                        <div>
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Due Date
                            </label>
                            <input
                                type="date"
                                value={taskDueDate}
                                onChange={(e) => setTaskDueDate(e.target.value)}
                                className="w-full px-3 py-2 border rounded"
                            />
                        </div>
                        <button
                            type="submit"
                            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                        >
                            Add Task
                        </button>
                    </div>
                </form>

                <div className="grid gap-4">
                    {project.tasks.map((task) => (
                        <div
                            key={task.id}
                            className="bg-white p-4 rounded-lg shadow flex justify-between items-center"
                        >
                            <div className="flex items-center">
                                <input
                                    type="checkbox"
                                    checked={task.isCompleted}
                                    onChange={() => handleToggleTask(task)}
                                    className="mr-4"
                                />
                                <div>
                                    <p className={task.isCompleted ? 'line-through text-gray-500' : ''}>
                                        {task.title}
                                    </p>
                                    {task.dueDate && (
                                        <p className="text-sm text-gray-500">
                                            Due: {new Date(task.dueDate).toLocaleDateString()}
                                        </p>
                                    )}
                                </div>
                            </div>
                            <button
                                onClick={() => handleDeleteTask(task.id)}
                                className="px-3 py-1 bg-red-500 text-white rounded hover:bg-red-600"
                            >
                                Delete
                            </button>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};