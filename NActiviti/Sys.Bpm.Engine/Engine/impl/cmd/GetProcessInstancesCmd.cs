﻿using System;
using System.Collections.Generic;

/* Licensed under the Apache License, Version 2.0 (the "License");
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

namespace Sys.Workflow.Engine.Impl.Cmd
{

    using Sys.Workflow.Engine.Impl.Interceptor;
    using Sys.Workflow.Engine.Impl.Persistence.Entity;
    using Sys.Workflow.Engine.Query;
    using Sys.Workflow.Engine.Runtime;
    using Sys.Workflow.Engine.Tasks;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GetProcessInstancesCmd : PageQueryCmd<IProcessInstanceQuery, IProcessInstance>
    {
        private const long serialVersionUID = 1L;

        public GetProcessInstancesCmd(IQuery<IProcessInstanceQuery, IProcessInstance> query, int firstResult, int pageSize) : base(query, firstResult, pageSize)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        public override IList<IProcessInstance> Execute(ICommandContext commandContext)
        {
            IEnumerable<IProcessInstance> insts = base.Execute(commandContext) ?? new ExecutionEntityImpl[0];

            insts = ExecutionEntityImpl.EnsureStarterInitialized(insts.Cast<ExecutionEntityImpl>());

            return insts.ToList();
        }
    }
}