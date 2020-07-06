using System;
using System.Collections.Generic;

namespace Structure.Tarefas
{
    public class TaskManager
    {
        private IList<ITask> tasks;

        public TaskManager()
        {
            tasks = new List<ITask>();
        }

        public void Add(ITask tarefa, bool startAutomatically = true)
        {
            tasks.Add(tarefa);

            if (startAutomatically)
            {
                tarefa.Start();
            }
        }

        public void Add<TTask>(bool startAutomatically = true)
            where TTask : ITask
        {
            TTask task = Activator.CreateInstance<TTask>();
            Add(task, startAutomatically);
        }

        public void Add<TTask>(bool startAutomatically = true, params object[] constructorParameters)
            where TTask : ITask
        {
            TTask task = (TTask)Activator.CreateInstance(typeof(TTask), constructorParameters);
            Add(task, startAutomatically);
        }

        public void StartAll()
        {
            foreach (var tarefa in tasks)
            {
                if (!tarefa.Running)
                {
                    tarefa.Start();
                }
            }
        }

        public void StopAll()
        {
            foreach (var tarefa in tasks)
            {
                try
                {
                    tarefa.Stop();
                }
                catch 
                {
                }
            }
        }

        public void Stop<TTask>()
        {
            foreach (var task in tasks)
            {
                if (task is TTask)
                {
                    task.Stop();
                    return;
                }
            }
        }

        public void Restart<TTask>()
        {
            ITask auxTask = null;

            foreach (var task in tasks)
            {
                if (task is TTask)
                {
                    auxTask = task;
                    break;
                }
            }

            if (auxTask != null)
            {
                auxTask.Stop();
                auxTask.Start();
            }
        }
    }
}
