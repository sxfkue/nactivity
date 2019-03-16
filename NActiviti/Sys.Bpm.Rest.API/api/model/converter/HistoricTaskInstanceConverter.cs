﻿using org.activiti.engine.history;
using org.activiti.engine.impl.persistence.entity;
using org.activiti.engine.task;
using System;
using System.Collections.Generic;
using static org.activiti.cloud.services.api.model.HistoricInstance;
using static org.activiti.cloud.services.api.model.TaskModel;

/*
 * Licensed under the Apache License, Version 2.0 (the "License");
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
 *
 */

namespace org.activiti.cloud.services.api.model.converter
{

    /// <summary>
    /// 
    /// </summary>
    public class HistoricTaskInstanceConverter : IModelConverter<IHistoricTaskInstance, TaskModel>
    {

        private readonly ListConverter listConverter;


        /// <summary>
        /// 
        /// </summary>
        public HistoricTaskInstanceConverter(ListConverter listConverter)
        {
            this.listConverter = listConverter;
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual TaskModel from(IHistoricTaskInstance source)
        {
            TaskModel task = null;
            if (source != null)
            {
                task = new TaskModel(source.Id,
                    source.Owner,
                    source.Assignee,
                    source.Name,
                    source.Description,
                    source.CreateTime,
                    source.EndTime,
                    source.ClaimTime,
                    source.DueDate,
                    source.Priority,
                    source.ProcessDefinitionId,
                    source.ProcessInstanceId,
                    source.ParentTaskId,
                    source.FormKey,
                    calculateStatus(source),
                    source.DeleteReason);
            }
            return task;
        }

        /// <summary>
        /// 
        /// </summary>

        private string calculateStatus(IHistoricTaskInstance source)
        {
            if (source is IHistoricTaskInstanceEntity hs && hs.Deleted)
            {
                return Enum.GetName(typeof(HistoricInstanceStatus), HistoricInstanceStatus.DELETED);
            }

            return Enum.GetName(typeof(HistoricInstanceStatus), HistoricInstanceStatus.COMPLETED);
        }

        /// <summary>
        /// 
        /// </summary>

        public virtual IList<TaskModel> from(IList<IHistoricTaskInstance> tasks)
        {
            return listConverter.from(tasks, this);
        }
    }

}