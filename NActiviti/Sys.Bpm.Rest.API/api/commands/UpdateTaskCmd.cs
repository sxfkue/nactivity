﻿using Newtonsoft.Json;
using System;

/*
 * Copyright 2018 Alfresco, Inc. and/or its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.activiti.cloud.services.api.commands
{

    public class UpdateTaskCmd : ICommand
    {

        private readonly string id = "updateTaskCmd";
        private string name;
        private string description;
        private DateTime? dueDate;
        private int? priority;
        private string assignee;
        private string parentTaskId;

        public UpdateTaskCmd()
        {
        }

        //[JsonConstructor]
        public UpdateTaskCmd([JsonProperty("Name")] string name,
            [JsonProperty("Description")] string description,
            [JsonProperty("DueDate")] DateTime? dueDate,
            [JsonProperty("Priority")] int? priority,
            [JsonProperty("Assignee")] string assignee,
            [JsonProperty("ParentTaskId")] string parentTaskId) : this()
        {

            this.name = name;
            this.description = description;
            this.dueDate = dueDate;
            this.priority = priority;
            this.assignee = assignee;
            this.parentTaskId = parentTaskId;
        }

        public virtual string Id
        {
            get => id;
        }

        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        public virtual string Description
        {
            get
            {
                return description;
            }
        }

        public virtual DateTime? DueDate
        {
            get
            {
                return dueDate;
            }
        }

        public virtual int? Priority
        {
            get
            {
                return priority;
            }
        }

        public virtual string Assignee
        {
            get
            {
                return assignee;
            }
        }

        public virtual string ParentTaskId
        {
            get
            {
                return parentTaskId;
            }
        }
    }

}