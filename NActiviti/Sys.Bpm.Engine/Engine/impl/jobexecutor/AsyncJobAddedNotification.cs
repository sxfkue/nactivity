﻿/* Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Sys.Workflow.Engine.Impl.JobExecutors
{
    using Sys.Workflow.Engine.Impl.Asyncexecutor;
    using Sys.Workflow.Engine.Impl.Cfg;
    using Sys.Workflow.Engine.Impl.Interceptor;
    using Sys.Workflow.Engine.Impl.Persistence.Entity;

    /// 
    public class AsyncJobAddedNotification : ICommandContextCloseListener
    {
        protected internal IJobEntity job;
        protected internal IAsyncExecutor asyncExecutor;

        public AsyncJobAddedNotification(IJobEntity job, IAsyncExecutor asyncExecutor)
        {
            this.job = job;
            this.asyncExecutor = asyncExecutor;
        }

        public virtual void Closed(ICommandContext commandContext)
        {
            ICommandExecutor commandExecutor = commandContext.ProcessEngineConfiguration.CommandExecutor;
            CommandConfig commandConfig = new CommandConfig(false, TransactionPropagation.REQUIRES_NEW);
            commandExecutor.Execute(commandConfig, new CommandAnonymousInnerClass(this));
        }

        private class CommandAnonymousInnerClass : ICommand<object>
        {
            private readonly AsyncJobAddedNotification outerInstance;

            public CommandAnonymousInnerClass(AsyncJobAddedNotification outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public virtual object Execute(ICommandContext commandContext)
            {
                //if (log.TraceEnabled)
                //{
                //    log.trace("notifying job executor of new job");
                //}
                outerInstance.asyncExecutor.ExecuteAsyncJob(outerInstance.job);
                return null;
            }
        }

        public virtual void Closing(ICommandContext commandContext)
        {
        }

        public virtual void AfterSessionsFlush(ICommandContext commandContext)
        {
        }

        public virtual void CloseFailure(ICommandContext commandContext)
        {
        }

    }

}