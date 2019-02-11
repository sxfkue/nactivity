﻿using System;

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
namespace org.activiti.engine.impl.bpmn.parser.handler
{
    using Microsoft.Extensions.Logging;
    using org.activiti.bpmn.model;
    using org.activiti.engine.impl.bpmn.behavior;

    /// 
    public class SendTaskParseHandler : AbstractActivityBpmnParseHandler<SendTask>
    {
        protected internal override Type HandledType
        {
            get
            {
                return typeof(SendTask);
            }
        }

        protected internal override void executeParse(BpmnParse bpmnParse, SendTask sendTask)
        {

            if (!string.IsNullOrWhiteSpace(sendTask.Type))
            {

                if (sendTask.Type.Equals("mail", StringComparison.CurrentCultureIgnoreCase))
                {
                    sendTask.Behavior = bpmnParse.ActivityBehaviorFactory.createMailActivityBehavior(sendTask);
                }
                else if (sendTask.Type.Equals("mule", StringComparison.CurrentCultureIgnoreCase))
                {
                    sendTask.Behavior = bpmnParse.ActivityBehaviorFactory.createMuleActivityBehavior(sendTask);
                }
                else if (sendTask.Type.Equals("camel", StringComparison.CurrentCultureIgnoreCase))
                {
                    sendTask.Behavior = bpmnParse.ActivityBehaviorFactory.createCamelActivityBehavior(sendTask);
                }

            }
            else if (ImplementationType.IMPLEMENTATION_TYPE_WEBSERVICE.Equals(sendTask.ImplementationType, StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrWhiteSpace(sendTask.OperationRef))
            {

                WebServiceActivityBehavior webServiceActivityBehavior = bpmnParse.ActivityBehaviorFactory.createWebServiceActivityBehavior(sendTask);
                sendTask.Behavior = webServiceActivityBehavior;

            }
            else
            {
                logger.LogWarning("One of the attributes 'type' or 'operation' is mandatory on sendTask " + sendTask.Id);
            }
        }

    }

}