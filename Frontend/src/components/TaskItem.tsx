import React from 'react';
import { Task } from '../types/Task';

interface TaskItemProps {
    task: Task;
    onToggle: (task: Task) => void;
    onDelete: (id: string) => void;
}

export const TaskItem: React.FC<TaskItemProps> = ({ task, onToggle, onDelete }) => {
    return (
        <div className="flex items-center justify-between p-4 mb-2 bg-white rounded shadow">
            <div className="flex items-center">
                <input
                    type="checkbox"
                    checked={task.isCompleted}
                    onChange={() => onToggle(task)}
                    className="mr-3"
                />
                <span className={task.isCompleted ? 'line-through text-gray-500' : ''}>
                    {task.description}
                </span>
            </div>
            <button
                onClick={() => onDelete(task.id)}
                className="px-3 py-1 text-white bg-red-500 rounded hover:bg-red-600"
            >
                Delete
            </button>
        </div>
    );
};