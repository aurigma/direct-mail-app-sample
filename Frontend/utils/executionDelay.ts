/* eslint-disable @typescript-eslint/no-extraneous-class */
/* eslint-disable @typescript-eslint/no-unsafe-function-type */
interface ITask {
  name: string;
  queue: Function[];
  timer: NodeJS.Timeout | null;
}

/**
 * Create a delay for the function to fire until the user completes all actions with the task
 */
export class ExecutionDelay {
  private static tasks: ITask[] = [];

  /**
   * Creates a task if there was no task with the name {taskName} and adds a new subtask {action} to the task for which {delay}ms is given.
   * @param {String} taskName task name
   * @param {Function} action new subtask function
   * @param {Number} delay (default 1500)
   */
  public static add(taskName: string, action: Function, delay: number = 1500) {
    let taskIndex: number = ExecutionDelay.tasks.findIndex(
      (task) => task.name === taskName,
    );

    const taskDoesNotExist = taskIndex === -1;
    if (taskDoesNotExist) {
      taskIndex = ExecutionDelay.createNewTask(taskName);
    }

    const currentTask: ITask = ExecutionDelay.tasks[taskIndex];
    currentTask.queue.push(action);

    if (currentTask.timer === null) {
      currentTask.timer = createTimer(delay);
    } else {
      clearTimeout(currentTask.timer);
      currentTask.timer = createTimer(delay);
    }

    function createTimer(delay: number): NodeJS.Timeout {
      return setTimeout(function () {
        const lastAction: Function =
          ExecutionDelay.getLastActionFromQueue(currentTask);
        lastAction();
        ExecutionDelay.clearTask(taskIndex);
      }, delay);
    }
  }

  private static createNewTask(name: string): number {
    const schema: ITask = {
      name,
      queue: [],
      timer: null,
    };
    const lenTasks: number = ExecutionDelay.tasks.push(schema);
    const lastIndex: number = lenTasks - 1;
    return lastIndex;
  }

  private static getQueueLastIndex(task: ITask): number {
    return task.queue.length - 1;
  }

  private static getLastActionFromQueue(task: ITask): Function {
    return task.queue[ExecutionDelay.getQueueLastIndex(task)];
  }

  private static clearTask(taskIndex: number): ITask[] {
    if (ExecutionDelay.tasks[taskIndex].timer) {
      clearTimeout(ExecutionDelay.tasks[taskIndex].timer);
    }
    ExecutionDelay.tasks.splice(taskIndex, 1);
    return ExecutionDelay.tasks;
  }
}
