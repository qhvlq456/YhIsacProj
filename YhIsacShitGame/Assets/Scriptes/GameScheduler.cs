using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YhProj.Game.Play
{
    public static class GameScheduler 
    {
        private static List<Scheduler> schedulerList = new List<Scheduler>();

        public static void AddScheduler(Scheduler scheduler) 
        {
            schedulerList.Add(scheduler);
        }
        public static void RemoveScheduler(Scheduler scheduler) 
        {
            schedulerList.Remove(scheduler);
        }
        public static void UpdateSchedulers()
        {
            for (int i = 0; i < schedulerList.Count; i++)
            {
                schedulerList[i].Update();
            }
        }
    }

    public class Scheduler
    {
        private uint id;
        private List<ScheduledTask> taskList = new List<ScheduledTask>();
        private bool isRunning = false;

        public Scheduler(uint _id)
        {
            id = _id;
        }

        public void AddTask(ScheduledTask task)
        {
            taskList.Add(task);
        }

        public void RemoveTask(ScheduledTask task)
        {
            taskList.Remove(task);
        }

        public void Start()
        {
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Update()
        {
            if (!isRunning)
            {
                return;
            }

            for (int i = taskList.Count - 1; i >= 0; i--)
            {
                if (taskList[i].IsReady())
                {
                    taskList[i].Execute();
                    taskList.RemoveAt(i);
                }
            }
        }
    }

    public abstract class ScheduledTask
    {
        protected float startTime;
        protected float duration;

        public ScheduledTask(float _duration)
        {
            duration = _duration;
            startTime = Time.time;
        }

        public virtual bool IsReady()
        {
            return Time.time >= startTime + duration;
        }

        public abstract void Execute();
    }
}

