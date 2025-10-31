import React from 'react';
import { Task } from '../types/Task';
import { TaskItem } from './TaskItem';

interface TaskListProps {
    tasks: Task[];
    onToggle: (task: Task) => void;
    onDelete: (id: string) => void;
}

export const TaskList: React.FC<TaskListProps> = ({ tasks, onToggle, onDelete }) => {
    return (
        <div>
            {tasks.map((task) => (
                <TaskItem
                    key={task.id}
                    task={task}
                    onToggle={onToggle}
                    onDelete={onDelete}
                />
            ))}
        </div>
    );
};