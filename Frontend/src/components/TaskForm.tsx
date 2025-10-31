import React, { useState } from 'react';

interface TaskFormProps {
    onSubmit: (description: string) => void;
}

export const TaskForm: React.FC<TaskFormProps> = ({ onSubmit }) => {
    const [description, setDescription] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (description.trim()) {
            onSubmit(description);
            setDescription('');
        }
    };

    return (
        <form onSubmit={handleSubmit} className="mb-4">
            <div className="flex gap-2">
                <input
                    type="text"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Enter a new task..."
                    className="flex-1 px-3 py-2 border rounded"
                />
                <button
                    type="submit"
                    className="px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600"
                >
                    Add Task
                </button>
            </div>
        </form>
    );
};