import React, { useState, useEffect } from 'react';
import { api } from '../services/api';
import { Project } from '../types';
import { useNavigate } from 'react-router-dom';

export const Dashboard: React.FC = () => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        loadProjects();
    }, []);

    const loadProjects = async () => {
        try {
            const data = await api.getProjects();
            setProjects(data);
        } catch (err: any) {
            setError(err.response?.data || 'Error loading projects');
        }
    };

    const handleCreateProject = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await api.createProject(title, description);
            setTitle('');
            setDescription('');
            loadProjects();
        } catch (err: any) {
            setError(err.response?.data || 'Error creating project');
        }
    };

    const handleDeleteProject = async (id: string) => {
        try {
            await api.deleteProject(id);
            loadProjects();
        } catch (err: any) {
            setError(err.response?.data || 'Error deleting project');
        }
    };

    const handleLogout = () => {
        api.logout();
        navigate('/');
    };

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-4xl mx-auto">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-3xl font-bold">Projects</h1>
                    <button
                        onClick={handleLogout}
                        className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
                    >
                        Logout
                    </button>
                </div>

                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                        {error}
                    </div>
                )}

                <form onSubmit={handleCreateProject} className="mb-8">
                    <div className="grid gap-4">
                        <div>
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Title
                            </label>
                            <input
                                type="text"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                                required
                                className="w-full px-3 py-2 border rounded"
                            />
                        </div>
                        <div>
                            <label className="block text-gray-700 text-sm font-bold mb-2">
                                Description
                            </label>
                            <textarea
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                className="w-full px-3 py-2 border rounded"
                                rows={3}
                            />
                        </div>
                        <button
                            type="submit"
                            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                        >
                            Create Project
                        </button>
                    </div>
                </form>

                <div className="grid gap-4">
                    {projects.map((project) => (
                        <div
                            key={project.id}
                            className="bg-white p-6 rounded-lg shadow"
                            onClick={() => navigate(`/project/${project.id}`)}
                        >
                            <div className="flex justify-between items-start">
                                <div>
                                    <h2 className="text-xl font-bold mb-2">{project.title}</h2>
                                    {project.description && (
                                        <p className="text-gray-600 mb-4">{project.description}</p>
                                    )}
                                    <p className="text-sm text-gray-500">
                                        Created: {new Date(project.createdAt).toLocaleDateString()}
                                    </p>
                                    <p className="text-sm text-gray-500">
                                        Tasks: {project.tasks.length}
                                    </p>
                                </div>
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        handleDeleteProject(project.id);
                                    }}
                                    className="px-3 py-1 bg-red-500 text-white rounded hover:bg-red-600"
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};